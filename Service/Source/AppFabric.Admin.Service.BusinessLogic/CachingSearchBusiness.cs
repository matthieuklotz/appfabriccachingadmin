// -----------------------------------------------------------------------
// <copyright file="CachingSearchBusiness.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Service.BusinessLogic
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using AppFabric.Admin.Common.Injection;
    using AppFabric.Admin.Service.BusinessLogic.Interfaces;
    using AppFabric.Admin.Service.DataAccess.Interfaces;
    using Microsoft.ApplicationServer.Caching;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Caching Search business class.
    /// </summary>
    public class CachingSearchBusiness : ICachingSearchBusiness
    {
        /// <summary>
        /// Default datacache factory.
        /// </summary>
        private Lazy<DataCacheFactory> dataCacheFactory = new Lazy<DataCacheFactory>(InitializeFactory, LazyThreadSafetyMode.ExecutionAndPublication);

        private ConcurrentDictionary<string, DataCache> namedCaches = new ConcurrentDictionary<string, DataCache>();

        private ICachingAdminDataRepository repository;

        public CachingSearchBusiness()
        {
            this.dataCacheFactory = new Lazy<DataCacheFactory>(InitializeFactory, LazyThreadSafetyMode.ExecutionAndPublication);
            this.namedCaches = new ConcurrentDictionary<string, DataCache>();
            this.repository = IoCManager.Container.Resolve<ICachingAdminDataRepository>();
        }

        /// <inheritdoc />
        public IList<string> SearchCacheItems(string cacheKeyPattern, string namedCache, string region)
        {
            List<string> results = new List<string>();
            DataCache cache = this.namedCaches.GetOrAdd(namedCache, this.dataCacheFactory.Value.GetCache);
            if (cache != null)
            {
                IList<DataAccess.Datas.Region> dataRegions = null;
                if (!string.IsNullOrWhiteSpace(region))
                {
                    dataRegions = new List<DataAccess.Datas.Region>();
                    DataAccess.Datas.Region dataRegion = new DataAccess.Datas.Region { RegionName = region };
                    dataRegions.Add(dataRegion);
                }

                if (dataRegions == null)
                {
                    dataRegions = this.repository.GetCacheRegions(namedCache);
                }

                if (dataRegions != null && dataRegions.Count > 0)
                {
                    Regex regex = new Regex(cacheKeyPattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    foreach (DataAccess.Datas.Region dataRegion in dataRegions)
                    {
                        if (dataRegion == null)
                        {
                            continue;
                        }

                        IEnumerable<KeyValuePair<string, object>> objects = cache.GetObjectsInRegion(dataRegion.RegionName);
                        if (objects != null && objects.Any())
                        {
                            foreach (KeyValuePair<string, object> cacheItem in objects)
                            {
                                if (regex.IsMatch(cacheItem.Key))
                                {
                                    results.Add(cacheItem.Key);
                                }
                            }
                        }
                    }
                }
            }

            return results;
        }

        /// <inheritdoc />
        public bool RemoveCacheItem(string cacheKey, string namedCache)
        {
            bool cacheItemRemoved = false;
            DataCache cache = this.namedCaches.GetOrAdd(namedCache, this.dataCacheFactory.Value.GetCache);
            if (cache != null)
            {
                cacheItemRemoved = cache.Remove(cacheKey);
            }

            return cacheItemRemoved;
        }

        private static DataCacheFactory InitializeFactory()
        {
            DataCacheFactory factory = new DataCacheFactory();
            return factory;
        }
    }
}
