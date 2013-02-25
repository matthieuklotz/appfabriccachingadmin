// -----------------------------------------------------------------------
// <copyright file="CachingReportingBusiness.cs" company="Matthieu Klotz">
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
    using System.Collections.Generic;
    using System.Linq;
    using AppFabric.Admin.Common.Injection;
    using AppFabric.Admin.Service.BusinessLogic.Helpers;
    using AppFabric.Admin.Service.BusinessLogic.Interfaces;
    using AppFabric.Admin.Service.DataAccess.Datas;
    using AppFabric.Admin.Service.DataAccess.Interfaces;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Caching Reporting business class.
    /// </summary>
    public class CachingReportingBusiness : ICachingReportingBusiness
    {
        /// <summary>
        /// The administration repository.
        /// </summary>
        private ICachingAdminDataRepository adminRepository;

        /// <summary>
        /// The performance monitor repository.
        /// </summary>
        private ICachingStatisticsRepository statsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingReportingBusiness"/> class.
        /// </summary>
        public CachingReportingBusiness()
        {
            this.adminRepository = IoCManager.Container.Resolve<ICachingAdminDataRepository>();
            this.statsRepository = IoCManager.Container.Resolve<ICachingStatisticsRepository>();
        }

        /// <inheritdoc />
        public BusinessEntities.CacheClusterHealth GetCacheClusterHealth()
        {
            BusinessEntities.CacheClusterHealth cacheClusterHealth = null;

            CacheClusterHealth health = null;
            try
            {
                health = this.adminRepository.GetCacheClusterHealth();
            }
            catch
            {
                IList<CacheHost> hosts = this.adminRepository.GetCacheHost(null, Constants.DefaultCachePort);
                if (hosts != null && hosts.Count > 0)
                {
                    health = new CacheClusterHealth();
                    health.Hosts = new List<CacheHostHealth>(hosts.Count);
                    foreach (CacheHost host in hosts)
                    {
                        CacheHostHealth cacheHostHealth = new CacheHostHealth { HostName = host.HostName };
                        health.Hosts.Add(cacheHostHealth);
                    }
                }
            }

            if (health == null)
            {
                return cacheClusterHealth;
            }

            cacheClusterHealth = new BusinessEntities.CacheClusterHealth();
            IList<CacheHostHealth> hostHealth = health.Hosts;
            if (hostHealth != null && hostHealth.Count > 0)
            {
                cacheClusterHealth.Hosts = new List<BusinessEntities.CacheHostHealth>(hostHealth.Count);
                foreach (CacheHostHealth cacheHost in hostHealth)
                {
                    if (cacheHost == null)
                    {
                        continue;
                    }

                    IList<CacheHost> hosts = this.adminRepository.GetCacheHost(cacheHost.HostName, Constants.DefaultCachePort);
                    CacheHost host = null;
                    if (hosts != null && hosts.Count > 0)
                    {
                        host = hosts.FirstOrDefault();
                    }

                    CacheServiceStatus status = host == null ? CacheServiceStatus.Unknown : host.Status;
                    BusinessEntities.CacheHostHealth cacheHostHealth = cacheHost.ToBusinessCacheHostHealth(status);
                    if (cacheHostHealth != null)
                    {
                        cacheClusterHealth.Hosts.Add(cacheHostHealth);
                    }
                }
            }

            IList<UnallocatedNamedCache> dataUnallocatedNamedCaches = health.UnallocatedNamedCaches;
            if (dataUnallocatedNamedCaches != null && dataUnallocatedNamedCaches.Count > 0)
            {
                cacheClusterHealth.UnallocatedNameCaches = new List<BusinessEntities.UnallocatedNamedCache>();
                foreach (UnallocatedNamedCache dataUnallocatedNamedCache in dataUnallocatedNamedCaches)
                {
                    if (dataUnallocatedNamedCache != null)
                    {
                        BusinessEntities.UnallocatedNamedCache unallocatedNamedCache = dataUnallocatedNamedCache.ToBusinessUnallocatedNamedCache();
                        if (unallocatedNamedCache != null)
                        {
                            cacheClusterHealth.UnallocatedNameCaches.Add(unallocatedNamedCache);
                        }
                    }
                }
            }

            return cacheClusterHealth;
        }

        /// <inheritdoc />
        public BusinessEntities.CacheStatistics GetCacheStatistics(string namedCache)
        {
            BusinessEntities.CacheStatistics stats = null;
            ICollection<StatisticCounter> counters = this.statsRepository.GetCacheStatistics(namedCache);
            if (counters != null && counters.Count > 0)
            {
                stats = new BusinessEntities.CacheStatistics { InstanceName = namedCache };
                IEnumerable<IGrouping<string, StatisticCounter>> groupedByHostCounters = counters.GroupBy(counter => counter.MachineName);
                Dictionary<string, IList<BusinessEntities.Counter>> countersByHost = new Dictionary<string, IList<BusinessEntities.Counter>>();
                foreach (IGrouping<string, StatisticCounter> groupedCounter in groupedByHostCounters)
                {
                    List<BusinessEntities.Counter> hostCounters = groupedCounter.ToBusinessCounters();
                    if (hostCounters != null && hostCounters.Count > 0)
                    {
                        countersByHost.Add(groupedCounter.Key, hostCounters);
                    }
                }

                stats.CountersByHost = countersByHost;

                IEnumerable<IGrouping<string, StatisticCounter>> groupedByCounter = counters.GroupBy(counter => counter.CounterName);
                List<BusinessEntities.Counter> sumCounters = new List<BusinessEntities.Counter>();
                foreach (IGrouping<string, StatisticCounter> groupedCounter in groupedByCounter)
                {
                    float value = groupedCounter.Sum(counter => counter.Value);
                    float rawValue = groupedCounter.Sum(counter => counter.Value);
                    BusinessEntities.Counter businessCounter = new BusinessEntities.Counter
                    {
                        CounterName = groupedCounter.Key,
                        Value = value,
                    };

                    sumCounters.Add(businessCounter);
                }

                stats.Counters = sumCounters;
            }

            return stats;
        }

        /// <inheritdoc />
        public BusinessEntities.Statistics GetHostStatistics(BusinessEntities.CacheHost cacheHost)
        {
            BusinessEntities.Statistics stats = null;
            ICollection<StatisticCounter> counters = this.statsRepository.GetHostStatistics(cacheHost.HostName, cacheHost.PortNo);
            if (counters != null && counters.Count > 0)
            {
                stats = new BusinessEntities.Statistics
                {
                    InstanceName = cacheHost.HostName,
                    Counters = counters.ToBusinessCounters()
                };
            }

            return stats;
        }
    }
}
