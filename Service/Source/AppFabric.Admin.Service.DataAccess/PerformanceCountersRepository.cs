// -----------------------------------------------------------------------
// <copyright file="PerformanceCountersRepository.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Service.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using AppFabric.Admin.Common.Configuration;
    using AppFabric.Admin.Common.Injection;
    using AppFabric.Admin.Common.Logging;
    using AppFabric.Admin.Service.DataAccess.Configuration;
    using AppFabric.Admin.Service.DataAccess.Datas;
    using AppFabric.Admin.Service.DataAccess.Interfaces;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Implements <see cref="ICachingStatisticsRepository"/> by using Windows performance counters.
    /// </summary>
    public sealed class PerformanceCountersRepository : ICachingStatisticsRepository
    {
        /// <summary>
        /// Named caches performance counters.
        /// </summary>
        private Dictionary<string, List<PerformanceCounter>> cachesCounters = new Dictionary<string, List<PerformanceCounter>>();

        /// <summary>
        /// Cache hosts performance counters.
        /// </summary>
        private Dictionary<string, List<PerformanceCounter>> hostsCounters = new Dictionary<string, List<PerformanceCounter>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCountersRepository"/> class.
        /// </summary>
        public PerformanceCountersRepository()
        {
            this.InitializeCounters();
        }

        /// <inheritdoc />
        public ICollection<StatisticCounter> GetCacheStatistics(string cacheName)
        {
            List<StatisticCounter> statcounters = null;
            List<PerformanceCounter> counters;
            if (this.cachesCounters.TryGetValue(cacheName, out counters))
            {
                if (counters != null && counters.Count > 0)
                {
                    statcounters = RetrieveStatisticsCounters(counters);
                }
            }

            return statcounters;
        }

        /// <inheritdoc />
        public ICollection<StatisticCounter> GetHostStatistics(string hostName, int portNo)
        {
            List<StatisticCounter> statcounters = null;
            List<PerformanceCounter> counters;
            if (this.hostsCounters.TryGetValue(hostName, out counters))
            {
                if (counters != null && counters.Count > 0)
                {
                    statcounters = RetrieveStatisticsCounters(counters);
                }
            }

            return statcounters;
        }

        private static List<StatisticCounter> RetrieveStatisticsCounters(List<PerformanceCounter> counters)
        {
            Contract.Requires<ArgumentNullException>(counters != null);
            Contract.Requires<ArgumentException>(counters.Count > 0);
            List<StatisticCounter> statcounters = new List<StatisticCounter>(counters.Count);
            foreach (PerformanceCounter counter in counters)
            {
                if (counter != null)
                {
                    StatisticCounter statcounter = new StatisticCounter
                    {
                        CounterName = counter.CounterName,
                        MachineName = counter.MachineName,
                        Value = counter.RawValue
                    };

                    statcounters.Add(statcounter);
                }
            }

            return statcounters;
        }

        private static List<PerformanceCounter> GetCacheCounters(string cacheName, IList<string> hostnames, NamedElementCollection<PerformanceCounterConfigurationElement> counterNames)
        {
            Contract.Requires<ArgumentException>(string.IsNullOrWhiteSpace(cacheName));
            Contract.Requires<ArgumentNullException>(hostnames != null);
            Contract.Requires<ArgumentException>(hostnames.Count > 0);
            Contract.Requires<ArgumentNullException>(counterNames != null);

            List<PerformanceCounter> counters = new List<PerformanceCounter>();
            if (hostnames != null)
            {
                foreach (string hostname in hostnames)
                {
                    PerformanceCounterCategory[] categories = PerformanceCounterCategory.GetCategories(hostname);
                    PerformanceCounterCategory cacheCategory = categories.FirstOrDefault(category => string.Equals(category.CategoryName, PerformanceCountersConstants.CacheCounterCategory, StringComparison.OrdinalIgnoreCase));
                    if (cacheCategory != null)
                    {
                        PerformanceCounter[] perfCounters = null;
                        try
                        {
                            perfCounters = cacheCategory.GetCounters(cacheName);
                        }
                        catch (Exception exception)
                        {
                            Logger.Error("PerformanceCounters", exception.Message, exception);
                        }

                        if (perfCounters != null && perfCounters.Length > 0)
                        {
                            counters.AddRange(perfCounters.Where(counter => counterNames.Get(counter.CounterName) != null));
                        }
                    }
                }
            }

            return counters;
        }

        private static List<PerformanceCounter> GetHostCounters(string hostName, NamedElementCollection<PerformanceCounterConfigurationElement> hostCounters)
        {
            Contract.Requires<ArgumentException>(string.IsNullOrWhiteSpace(hostName));
            Contract.Requires<ArgumentNullException>(hostCounters != null);
            Contract.Requires<ArgumentException>(hostCounters.Count > 0);

            List<PerformanceCounter> counters = new List<PerformanceCounter>();
            PerformanceCounterCategory[] categories = PerformanceCounterCategory.GetCategories(hostName);
            foreach (PerformanceCounterConfigurationElement elt in hostCounters)
            {
                PerformanceCounterCategory category = categories.FirstOrDefault(cat => string.Equals(cat.CategoryName, elt.Category, StringComparison.OrdinalIgnoreCase));
                if (category != null)
                {
                    PerformanceCounter[] countersArray = null;
                    string instanceName = elt.Instance;
                    if (!string.IsNullOrWhiteSpace(instanceName))
                    {
                        try
                        {
                            countersArray = category.GetCounters(instanceName);
                        }
                        catch (Exception exception)
                        {
                            Logger.Error("PerformanceCounters", exception.Message, exception);
                        }
                    }
                    else
                    {
                        countersArray = category.GetCounters();
                    }

                    if (countersArray != null)
                    {
                        PerformanceCounter counter = countersArray.FirstOrDefault(c => string.Equals(c.CounterName, elt.Name, StringComparison.OrdinalIgnoreCase));
                        if (counter != null)
                        {
                            counters.Add(counter);
                        }
                    }
                }
            }

            return counters;
        }

        private void InitializeCounters()
        {
            PerformanceCountersSection section = ConfigurationManager.GetSection<PerformanceCountersSection>(PerformanceCountersSection.ConfigurationSectionName);
            if (section != null)
            {
                ICachingAdminDataRepository adminRepository = IoCManager.Container.Resolve<ICachingAdminDataRepository>();
                if (adminRepository != null)
                {
                    IList<CacheHost> hosts = adminRepository.GetCacheHost(null, PSAdminParamsConstants.DefaultCachePort);
                    if (hosts != null && hosts.Count > 0)
                    {
                        List<string> hostNames = hosts.Select(host => host.HostName).ToList();
                        NamedElementCollection<PerformanceCounterConfigurationElement> counters = section.NamedCacheCounters;
                        if (counters != null && counters.Count > 0)
                        {
                            IList<NamedCache> namedCaches = adminRepository.GetCache(null, null);
                            if (namedCaches != null && namedCaches.Count > 0)
                            {
                                foreach (NamedCache namedCache in namedCaches)
                                {
                                    List<PerformanceCounter> cacheCounters = GetCacheCounters(namedCache.CacheName, hostNames, counters);
                                    if (cacheCounters != null && cacheCounters.Count > 0)
                                    {
                                        this.cachesCounters.Add(namedCache.CacheName, cacheCounters);
                                    }
                                }
                            }
                        }

                        counters = section.HostsCounters;
                        if (counters != null && counters.Count > 0)
                        {
                            foreach (string hostname in hostNames)
                            {
                                List<PerformanceCounter> hostcounters = GetHostCounters(hostname, counters);
                                if (hostcounters != null && hostcounters.Count > 0)
                                {
                                    this.hostsCounters.Add(hostname, hostcounters);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
