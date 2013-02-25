// -----------------------------------------------------------------------
// <copyright file="CachingAdminBusiness.cs" company="Matthieu Klotz">
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
    using System.Collections.Generic;
    using System.Linq;
    using AppFabric.Admin.Common.Injection;
    using AppFabric.Admin.Service.BusinessEntities;
    using AppFabric.Admin.Service.BusinessLogic.Helpers;
    using AppFabric.Admin.Service.BusinessLogic.Interfaces;
    using AppFabric.Admin.Service.DataAccess.Interfaces;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Caching Administration business class.
    /// </summary>
    public class CachingAdminBusiness : ICachingAdminBusiness
    {
        /// <summary>
        /// The administration repository.
        /// </summary>
        private ICachingAdminDataRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingAdminBusiness"/> class.
        /// </summary>
        public CachingAdminBusiness()
        {
            this.repository = IoCManager.Container.Resolve<ICachingAdminDataRepository>();
        }

        /// <inheritdoc />
        public IList<CacheHost> GetCacheCluster()
        {
            IList<BusinessEntities.CacheHost> businessCacheHosts = null;
            IList<DataAccess.Datas.CacheHost> cacheHosts = this.repository.GetCacheHost(null, Constants.DefaultCachePort);
            if (cacheHosts != null && cacheHosts.Count > 0)
            {
                businessCacheHosts = cacheHosts.ToBusinessCacheHosts();
            }

            return businessCacheHosts;
        }

        /// <inheritdoc />
        public IList<CacheHost> StartCacheCluster()
        {
            IList<BusinessEntities.CacheHost> businessCacheHosts = null;
            IList<DataAccess.Datas.CacheHost> cacheHosts = this.repository.StartCacheCluster();
            if (cacheHosts != null && cacheHosts.Count > 0)
            {
                businessCacheHosts = cacheHosts.ToBusinessCacheHosts();
            }

            return businessCacheHosts;
        }

        /// <inheritdoc />
        public IList<CacheHost> StopCacheCluster()
        {
            IList<BusinessEntities.CacheHost> businessCacheHosts = null;
            IList<DataAccess.Datas.CacheHost> cacheHosts = this.repository.StopCacheCluster();
            if (cacheHosts != null && cacheHosts.Count > 0)
            {
                businessCacheHosts = cacheHosts.ToBusinessCacheHosts();
            }

            return businessCacheHosts;
        }

        /// <inheritdoc />
        public IList<CacheHost> RestartCacheCluster()
        {
            IList<BusinessEntities.CacheHost> businessCacheHosts = null;
            IList<DataAccess.Datas.CacheHost> cacheHosts = this.repository.RestartCacheCluster();
            if (cacheHosts != null && cacheHosts.Count > 0)
            {
                businessCacheHosts = cacheHosts.ToBusinessCacheHosts();
            }

            return businessCacheHosts;
        }

        /// <inheritdoc />
        public IList<string> GetCacheAllowedClientAccounts()
        {
            return this.repository.GetCacheAllowedClientAccounts();
        }

        /// <inheritdoc />
        public IList<string> GrantCacheAllowedClientAccounts(IList<string> clientAccounts)
        {
            if (clientAccounts != null && clientAccounts.Count > 0)
            {
                foreach (string clientAccount in clientAccounts)
                {
                    this.repository.GrantCacheAllowedClientAccount(clientAccount, true);
                }
            }

            return this.GetCacheAllowedClientAccounts();
        }

        /// <inheritdoc />
        public IList<string> RevokeCacheAllowedClientAccounts(IList<string> clientAccounts)
        {
            if (clientAccounts != null && clientAccounts.Count > 0)
            {
                foreach (string clientAccount in clientAccounts)
                {
                    this.repository.RevokeCacheAllowedClientAccount(clientAccount);
                }
            }

            return this.GetCacheAllowedClientAccounts();
        }

        /// <inheritdoc />
        public void ExportCacheClusterConfig(string filePath)
        {
            this.repository.ExportCacheClusterConfig(filePath);
        }

        /// <inheritdoc />
        public void ImportCacheClusterConfig(string filePath)
        {
            this.repository.ImportCacheClusterConfig(filePath);
        }

        /// <inheritdoc />
        public CacheHostConfiguration GetCacheHostConfig(CacheHost host)
        {
            DataAccess.Datas.CacheHostConfiguration configuration = this.repository.GetCacheHostConfig(host.HostName, host.PortNo);
            CacheHostConfiguration cacheHostConfiguration = null;
            if (configuration != null)
            {
                cacheHostConfiguration = configuration.ToBusinessCacheHostConfiguration();
            }

            return cacheHostConfiguration;
        }

        /// <inheritdoc />
        public CacheHostConfiguration SetCacheHostConfig(CacheHostConfiguration cacheHostConfiguration)
        {
            DataAccess.Datas.CacheHostConfiguration dataCacheHostConfiguration = cacheHostConfiguration.ToDatasCacheHostConfiguration();
            this.repository.SetCacheHostConfig(dataCacheHostConfiguration);
            CacheHost host = new CacheHost { HostName = cacheHostConfiguration.HostName, PortNo = cacheHostConfiguration.CachePort };
            if (host == null || string.IsNullOrWhiteSpace(host.HostName))
            {
                return null;
            }

            return this.GetCacheHostConfig(host);
        }

        /// <inheritdoc />
        public CacheHost StartCacheHost(CacheHost host)
        {
            CacheHost cacheHost = null;
            DataAccess.Datas.CacheHost dataCacheHost = this.repository.StartCacheHost(host.HostName, host.PortNo, null);
            if (dataCacheHost != null)
            {
                cacheHost = dataCacheHost.ToBusinessCacheHost();
            }

            return cacheHost;
        }

        /// <inheritdoc />
        public CacheHost StopCacheHost(CacheHost host)
        {
            CacheHost cacheHost = null;
            DataAccess.Datas.CacheHost dataCacheHost = this.repository.StopCacheHost(host.HostName, host.PortNo, null);
            if (dataCacheHost != null)
            {
                cacheHost = dataCacheHost.ToBusinessCacheHost();
            }

            return cacheHost;
        }

        /// <inheritdoc />
        public CacheHost RestartCacheHost(CacheHost host)
        {
            CacheHost cacheHost = null;
            DataAccess.Datas.CacheHost dataCacheHost = this.repository.RestartCacheHost(host.HostName, host.PortNo, null);
            if (dataCacheHost != null)
            {
                cacheHost = dataCacheHost.ToBusinessCacheHost();
            }

            return cacheHost;
        }

        /// <inheritdoc />
        public NamedCache NewCache(CacheConfiguration cacheConfiguration)
        {
            NamedCache businessNamedCache = null;
            DataAccess.Datas.CacheConfiguration dataCacheConfiguration = cacheConfiguration.ToDataCacheConfiguration();
            if (dataCacheConfiguration != null)
            {
                this.repository.NewCache(dataCacheConfiguration);
                businessNamedCache = this.GetCache(cacheConfiguration.CacheName);
            }

            return businessNamedCache;
        }

        /// <inheritdoc />
        public void RemoveCache(string cacheName)
        {
            this.repository.RemoveCache(cacheName);
        }

        /// <inheritdoc />
        public CacheConfiguration GetCacheConfig(string cacheName)
        {
            CacheConfiguration cacheConfiguration = null;
            DataAccess.Datas.CacheConfiguration dataCacheConfiguration = this.repository.GetCacheConfig(cacheName);
            if (dataCacheConfiguration != null)
            {
                cacheConfiguration = dataCacheConfiguration.ToBusinessCacheConfiguration();
            }

            return cacheConfiguration;
        }

        /// <inheritdoc />
        public NamedCache SetCacheConfig(CacheConfiguration cacheConfiguration)
        {
            NamedCache businessNamedCache = null;
            DataAccess.Datas.CacheConfiguration dataCacheConfiguration = cacheConfiguration.ToDataCacheConfiguration();
            if (dataCacheConfiguration != null)
            {
                this.repository.SetCacheConfig(dataCacheConfiguration);
                businessNamedCache = this.GetCache(cacheConfiguration.CacheName);
            }

            return businessNamedCache;
        }

        /// <inheritdoc />
        public IList<NamedCache> GetNamedCaches()
        {
            IList<NamedCache> namedCaches = null;
            IList<DataAccess.Datas.NamedCache> dataNamedCaches = this.repository.GetCache(null, null);
            if (dataNamedCaches != null && dataNamedCaches.Count > 0)
            {
                namedCaches = new List<NamedCache>(dataNamedCaches.Count);
                foreach (DataAccess.Datas.NamedCache dataNamedcache in dataNamedCaches)
                {
                    if (dataNamedcache != null)
                    {
                        NamedCache businessNamedCache = new NamedCache
                        {
                            CacheName = dataNamedcache.CacheName,
                            HostRegionMap = dataNamedcache.HostRegionMap
                        };

                        namedCaches.Add(businessNamedCache);
                    }
                }
            }

            return namedCaches;
        }

        /// <summary>
        /// Gets a specified named cache.
        /// </summary>
        /// <param name="cacheName">Name of the cache.</param>
        /// <returns>Specified named cache.</returns>
        private NamedCache GetCache(string cacheName)
        {
            NamedCache businessNamedCache = null;
            IList<NamedCache> namedCaches = this.GetNamedCaches();
            if (namedCaches != null && namedCaches.Count > 0)
            {
                businessNamedCache = namedCaches.FirstOrDefault(cache => string.Equals(cacheName, cache.CacheName, StringComparison.OrdinalIgnoreCase));
            }

            return businessNamedCache;
        }
    }
}
