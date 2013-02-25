// -----------------------------------------------------------------------
// <copyright file="ConvertersExtensions.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Service.BusinessLogic.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using AppFabric.Admin.Service.DataAccess.Datas;
    using Omu.ValueInjecter;

    /// <summary>
    /// Contains static methods for data conversion.
    /// </summary>
    internal static class ConvertersExtensions
    {
        /// <summary>
        /// Converts a collection of <see cref="CacheHost"/> to a collection of <see cref="BusinessEntities.CacheHost"/>.
        /// </summary>
        /// <param name="cluster">The collection of <see cref="CacheHost"/> to convert.</param>
        /// <returns>Collection of <see cref="BusinessEntities.CacheHost"/>.</returns>
        internal static IList<BusinessEntities.CacheHost> ToBusinessCacheHosts(this IList<CacheHost> cluster)
        {
            Contract.Requires<ArgumentNullException>(cluster != null, "cluster");
            List<BusinessEntities.CacheHost> cacheHosts = new List<BusinessEntities.CacheHost>(cluster.Count);
            foreach (CacheHost dataCacheHost in cluster)
            {
                if (dataCacheHost != null)
                {
                    BusinessEntities.CacheHost cacheHost = dataCacheHost.ToBusinessCacheHost();
                    cacheHosts.Add(cacheHost);
                }
            }

            return cacheHosts;
        }

        /// <summary>
        /// Converts an instance of <see cref="CacheHost"/> to an instance of <see cref="BusinessEntities.CacheHost"/>
        /// </summary>
        /// <param name="dataCacheHost">The instance of <see cref="CacheHost"/> to convert.</param>
        /// <returns><see cref="BusinessEntities.CacheHost"/> instance converted from the specified dataCacheHost.</returns>
        internal static BusinessEntities.CacheHost ToBusinessCacheHost(this CacheHost dataCacheHost)
        {
            Contract.Requires<ArgumentNullException>(dataCacheHost != null, "dataCacheHost");
            BusinessEntities.CacheHost cacheHost = new BusinessEntities.CacheHost();
            cacheHost.InjectFrom(dataCacheHost);
            cacheHost.VersionInfo = new BusinessEntities.CacheHostVersionInfo();
            if (dataCacheHost.VersionInfo != null)
            {
                cacheHost.VersionInfo.InjectFrom(dataCacheHost.VersionInfo);
            }

            cacheHost.Status = (BusinessEntities.CacheServiceStatus)Enum.ToObject(typeof(BusinessEntities.CacheServiceStatus), dataCacheHost.Status);
            return cacheHost;
        }

        /// <summary>
        /// Converts an instance of <see cref="CacheHostConfiguration"/> to an instance of <see cref="BusinessEntities.CacheHostConfiguration"/>
        /// </summary>
        /// <param name="configuration">The instance of <see cref="CacheHostConfiguration"/> to convert.</param>
        /// <returns><see cref="BusinessEntities.CacheHostConfiguration"/> instance converted from the specified configuration.</returns>
        internal static BusinessEntities.CacheHostConfiguration ToBusinessCacheHostConfiguration(this CacheHostConfiguration configuration)
        {
            Contract.Requires<ArgumentNullException>(configuration != null, "configuration");
            BusinessEntities.CacheHostConfiguration cacheHostConfiguration = new BusinessEntities.CacheHostConfiguration();
            cacheHostConfiguration.InjectFrom(configuration);
            return cacheHostConfiguration;
        }

        /// <summary>
        /// Converts an instance of <see cref="BusinessEntities.CacheHostConfiguration"/> to an instance of <see cref="CacheHostConfiguration"/>
        /// </summary>
        /// <param name="cacheHostConfiguration">The instance of <see cref="BusinessEntities.CacheHostConfiguration"/> to convert.</param>
        /// <returns><see cref="CacheHostConfiguration"/> instance converted from the specified cacheHostConfiguration.</returns>
        internal static CacheHostConfiguration ToDatasCacheHostConfiguration(this BusinessEntities.CacheHostConfiguration cacheHostConfiguration)
        {
            Contract.Requires<ArgumentNullException>(cacheHostConfiguration != null, "cacheHostConfiguration");
            CacheHostConfiguration configuration = new CacheHostConfiguration();
            configuration.InjectFrom(cacheHostConfiguration);
            return configuration;
        }

        /// <summary>
        /// Converts an instance of <see cref="CacheConfiguration"/> to an instance of <see cref="BusinessEntities.CacheConfiguration"/>
        /// </summary>
        /// <param name="cacheConfiguration">The instance of <see cref="CacheConfiguration"/> to convert.</param>
        /// <returns><see cref="BusinessEntities.CacheConfiguration"/> instance converted from the specified cacheConfiguration.</returns>
        internal static CacheConfiguration ToDataCacheConfiguration(this BusinessEntities.CacheConfiguration cacheConfiguration)
        {
            Contract.Requires<ArgumentNullException>(cacheConfiguration != null, "cacheConfiguration");
            CacheConfiguration configuration = new CacheConfiguration();
            configuration.InjectFrom(cacheConfiguration);
            configuration.EvictionType = (EvictionType)Enum.ToObject(typeof(EvictionType), cacheConfiguration.EvictionType);
            return configuration;
        }

        /// <summary>
        /// Converts an instance of <see cref="CacheConfiguration"/> to an instance of <see cref="BusinessEntities.CacheConfiguration"/>
        /// </summary>
        /// <param name="dataCacheConfiguration">The instance of <see cref="CacheConfiguration"/> to convert.</param>
        /// <returns><see cref="BusinessEntities.CacheConfiguration"/> instance converted from the specified dataCacheConfiguration.</returns>
        internal static BusinessEntities.CacheConfiguration ToBusinessCacheConfiguration(this CacheConfiguration dataCacheConfiguration)
        {
            Contract.Requires<ArgumentNullException>(dataCacheConfiguration != null, "dataCacheConfiguration");
            BusinessEntities.CacheConfiguration cacheConfiguration = new BusinessEntities.CacheConfiguration();
            cacheConfiguration.InjectFrom(dataCacheConfiguration);
            cacheConfiguration.EvictionType = (BusinessEntities.EvictionType)Enum.ToObject(typeof(BusinessEntities.EvictionType), dataCacheConfiguration.EvictionType);
            return cacheConfiguration;
        }

        /// <summary>
        /// Converts an instance of <see cref="CacheHostHealth"/> to an instance of <see cref="BusinessEntities.CacheHostHealth"/>
        /// </summary>
        /// <param name="dataCacheHostHealth">The instance of <see cref="CacheHostHealth"/> to convert.</param>
        /// <param name="serviceStatus">State of the cache host.</param>
        /// <returns><see cref="BusinessEntities.CacheHostHealth"/> instance converted from the specified dataCacheHostHealth.</returns>
        internal static BusinessEntities.CacheHostHealth ToBusinessCacheHostHealth(this CacheHostHealth dataCacheHostHealth, CacheServiceStatus serviceStatus)
        {
            Contract.Requires<ArgumentNullException>(dataCacheHostHealth != null, "dataCacheHostHealth");
            BusinessEntities.CacheHostHealth cacheHostHealth = new BusinessEntities.CacheHostHealth();
            cacheHostHealth.InjectFrom(dataCacheHostHealth);
            cacheHostHealth.Status = (BusinessEntities.CacheServiceStatus)Enum.ToObject(typeof(BusinessEntities.CacheServiceStatus), serviceStatus);
            IList<NamedCacheHealth> dataNamedCachesHealth = dataCacheHostHealth.NamedCaches;
            if (dataNamedCachesHealth != null && dataNamedCachesHealth.Count > 0)
            {
                cacheHostHealth.NamedCaches = new List<BusinessEntities.NamedCacheHealth>(dataNamedCachesHealth.Count);
                foreach (NamedCacheHealth dataNamedCacheHealth in dataNamedCachesHealth)
                {
                    if (dataNamedCacheHealth != null)
                    {
                        BusinessEntities.NamedCacheHealth cacheHealth = dataNamedCacheHealth.ToNamedCacheHealth();
                        if (cacheHealth != null)
                        {
                            cacheHostHealth.NamedCaches.Add(cacheHealth);
                        }
                    }
                }
            }

            return cacheHostHealth;
        }

        /// <summary>
        /// Converts an instance of <see cref="NamedCacheHealth"/> to an instance of <see cref="BusinessEntities.NamedCacheHealth"/>
        /// </summary>
        /// <param name="dataNamedCacheHealth">The instance of <see cref="NamedCacheHealth"/> to convert.</param>
        /// <returns><see cref="BusinessEntities.NamedCacheHealth"/> instance converted from the specified dataNamedCacheHealth.</returns>
        internal static BusinessEntities.NamedCacheHealth ToNamedCacheHealth(this NamedCacheHealth dataNamedCacheHealth)
        {
            Contract.Requires<ArgumentNullException>(dataNamedCacheHealth != null, "dataNamedCacheHealth");
            BusinessEntities.NamedCacheHealth cacheHealth = new BusinessEntities.NamedCacheHealth();
            cacheHealth.InjectFrom(dataNamedCacheHealth);
            if (dataNamedCacheHealth.Healthy > 0)
            {
                cacheHealth.State = BusinessEntities.NamedCacheHealthStatus.Healthy;
            }
            else if (dataNamedCacheHealth.NotPrimary > 0)
            {
                cacheHealth.State = BusinessEntities.NamedCacheHealthStatus.NotPrimary;
            }
            else if (dataNamedCacheHealth.InadequateSecondaries > 0)
            {
                cacheHealth.State = BusinessEntities.NamedCacheHealthStatus.InadequateSecondaries;
            }
            else if (dataNamedCacheHealth.Throttled > 0)
            {
                cacheHealth.State = BusinessEntities.NamedCacheHealthStatus.Throttled;
            }
            else if (dataNamedCacheHealth.UnderReconfiguration > 0)
            {
                cacheHealth.State = BusinessEntities.NamedCacheHealthStatus.UnderReconfiguration;
            }

            return cacheHealth;
        }

        /// <summary>
        /// Converts an instance of <see cref="UnallocatedNamedCache"/> to an instance of <see cref="BusinessEntities.UnallocatedNamedCache"/>
        /// </summary>
        /// <param name="dataUnallocatedNamedCache">The instance of <see cref="UnallocatedNamedCache"/> to convert.</param>
        /// <returns><see cref="BusinessEntities.UnallocatedNamedCache"/> instance converted from the specified dataUnallocatedNamedCache.</returns>
        internal static BusinessEntities.UnallocatedNamedCache ToBusinessUnallocatedNamedCache(this UnallocatedNamedCache dataUnallocatedNamedCache)
        {
            Contract.Requires<ArgumentNullException>(dataUnallocatedNamedCache != null, "dataUnallocatedNamedCache");
            BusinessEntities.UnallocatedNamedCache unallocatedNamedCache = new BusinessEntities.UnallocatedNamedCache();
            unallocatedNamedCache.InjectFrom(dataUnallocatedNamedCache);
            return unallocatedNamedCache;
        }

        internal static List<BusinessEntities.Counter> ToBusinessCounters(this IEnumerable<StatisticCounter> counters)
        {
            Contract.Requires<ArgumentNullException>(counters != null, "counters");
            List<BusinessEntities.Counter> businessCounters = new List<BusinessEntities.Counter>();
            foreach (StatisticCounter counter in counters)
            {
                if (counter != null)
                {
                    BusinessEntities.Counter businessCounter = new BusinessEntities.Counter
                    {
                        CounterName = counter.CounterName,
                        Value = counter.Value
                    };

                    businessCounters.Add(businessCounter);
                }
            }

            return businessCounters;
        }
    }
}
