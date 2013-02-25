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

namespace AppFabric.Admin.Service.ServiceImplementation.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Omu.ValueInjecter;

    /// <summary>
    /// Contains static methods for data conversion.
    /// </summary>
    internal static class ConvertersExtensions
    {
        /// <summary>
        /// Converts a collection of <see cref="BusinessEntities.CacheHost"/> to a collection of <see cref="DataContracts.Administration.CacheHost"/>.
        /// </summary>
        /// <param name="cluster">The collection of <see cref="BusinessEntities.CacheHost"/> to convert.</param>
        /// <returns>Collection of <see cref="DataContracts.Administration.CacheHost"/>.</returns>
        internal static IList<DataContracts.Administration.CacheHost> ToAdminCacheHosts(this IList<BusinessEntities.CacheHost> cluster)
        {
            Contract.Requires<ArgumentNullException>(cluster != null, "cluster");
            List<DataContracts.Administration.CacheHost> cacheHosts = new List<DataContracts.Administration.CacheHost>(cluster.Count);
            foreach (BusinessEntities.CacheHost businessCacheHost in cluster)
            {
                if (businessCacheHost != null)
                {
                    DataContracts.Administration.CacheHost cacheHost = businessCacheHost.ToAdminCacheHost();
                    cacheHosts.Add(cacheHost);
                }
            }

            return cacheHosts;
        }

        /// <summary>
        /// Converts an instance of <see cref="BusinessEntities.CacheHost"/> to an instance of <see cref="DataContracts.Administration.CacheHost"/>
        /// </summary>
        /// <param name="businessCacheHost">The instance of <see cref="BusinessEntities.CacheHost"/> to convert.</param>
        /// <returns><see cref="DataContracts.Administration.CacheHost"/> instance converted from the specified businessCacheHost.</returns>
        internal static DataContracts.Administration.CacheHost ToAdminCacheHost(this BusinessEntities.CacheHost businessCacheHost)
        {
            Contract.Requires<ArgumentNullException>(businessCacheHost != null, "businessCacheHost");
            DataContracts.Administration.CacheHost cacheHost = new DataContracts.Administration.CacheHost();
            cacheHost.InjectFrom(businessCacheHost);
            cacheHost.VersionInformation = new DataContracts.Administration.CacheHostVersionInfo();
            if (businessCacheHost.VersionInfo != null)
            {
                cacheHost.VersionInformation.InjectFrom(businessCacheHost.VersionInfo);
            }

            cacheHost.ServiceStatus = (DataContracts.Administration.CacheServiceStatus)Enum.ToObject(typeof(DataContracts.Administration.CacheServiceStatus), businessCacheHost.Status);
            return cacheHost;
        }

        /// <summary>
        /// Converts an instance of <see cref="DataContracts.Administration.CacheHost"/> to an instance of <see cref="BusinessEntities.CacheHost"/>
        /// </summary>
        /// <param name="cacheHost">The instance of <see cref="DataContracts.Administration.CacheHost"/> to convert.</param>
        /// <returns><see cref="BusinessEntities.CacheHost"/> instance converted from the specified cacheHost.</returns>
        internal static BusinessEntities.CacheHost ToBusinessCacheHost(this DataContracts.Administration.CacheHost cacheHost)
        {
            Contract.Requires<ArgumentNullException>(cacheHost != null, "cacheHost");
            BusinessEntities.CacheHost businessCacheHost = new BusinessEntities.CacheHost();
            businessCacheHost.InjectFrom(cacheHost);
            businessCacheHost.VersionInfo = new BusinessEntities.CacheHostVersionInfo();
            if (cacheHost.VersionInformation != null)
            {
                businessCacheHost.VersionInfo.InjectFrom(cacheHost.VersionInformation);
            }

            businessCacheHost.Status = (BusinessEntities.CacheServiceStatus)Enum.ToObject(typeof(BusinessEntities.CacheServiceStatus), cacheHost.ServiceStatus);
            return businessCacheHost;
        }

        /// <summary>
        /// Converts an instance of <see cref="BusinessEntities.CacheHostConfiguration"/> to an instance of <see cref="DataContracts.Administration.CacheHostConfiguration"/>
        /// </summary>
        /// <param name="businessCacheHostConfiguration">The instance of <see cref="BusinessEntities.CacheHostConfiguration"/> to convert.</param>
        /// <returns><see cref="DataContracts.Administration.CacheHostConfiguration"/> instance converted from the specified businessCacheHostConfiguration.</returns>
        internal static DataContracts.Administration.CacheHostConfiguration ToAdminCacheHostConfiguration(this BusinessEntities.CacheHostConfiguration businessCacheHostConfiguration)
        {
            Contract.Requires<ArgumentNullException>(businessCacheHostConfiguration != null, "businessCacheHostConfiguration");
            DataContracts.Administration.CacheHostConfiguration cacheHostConfig = new DataContracts.Administration.CacheHostConfiguration();
            cacheHostConfig.InjectFrom(businessCacheHostConfiguration);
            return cacheHostConfig;
        }

        /// <summary>
        /// Converts an instance of <see cref="BusinessEntities.CacheHostConfiguration"/> to an instance of <see cref="BusinessEntities.CacheHostConfiguration"/>
        /// </summary>
        /// <param name="adminCacheHostConfig">The instance of <see cref="DataContracts.Administration.CacheHostConfiguration"/> to convert.</param>
        /// <returns><see cref="BusinessEntities.CacheHostConfiguration"/> instance converted from the specified adminCacheHostConfig.</returns>
        internal static BusinessEntities.CacheHostConfiguration ToBusinessCacheHostConfiguration(this DataContracts.Administration.CacheHostConfiguration adminCacheHostConfig)
        {
            Contract.Requires<ArgumentNullException>(adminCacheHostConfig != null, "adminCacheHostConfig");
            BusinessEntities.CacheHostConfiguration cacheHostConfig = new BusinessEntities.CacheHostConfiguration();
            cacheHostConfig.InjectFrom(adminCacheHostConfig);
            return cacheHostConfig;
        }

        /// <summary>
        /// Converts an instance of <see cref="DataContracts.Administration.CacheConfiguration"/> to an instance of <see cref="BusinessEntities.CacheConfiguration"/>
        /// </summary>
        /// <param name="adminCacheConfig">The instance of <see cref="DataContracts.Administration.CacheConfiguration"/> to convert.</param>
        /// <returns><see cref="BusinessEntities.CacheConfiguration"/> instance converted from the specified adminCacheConfig.</returns>
        internal static BusinessEntities.CacheConfiguration ToBusinessCacheConfiguration(this DataContracts.Administration.CacheConfiguration adminCacheConfig)
        {
            Contract.Requires<ArgumentNullException>(adminCacheConfig != null, "adminCacheConfig");
            BusinessEntities.CacheConfiguration cacheConfig = new BusinessEntities.CacheConfiguration();
            cacheConfig.InjectFrom(adminCacheConfig);
            cacheConfig.EvictionType = (BusinessEntities.EvictionType)Enum.ToObject(typeof(BusinessEntities.EvictionType), adminCacheConfig.EvictionType);
            return cacheConfig;
        }

        /// <summary>
        /// Converts an instance of <see cref="BusinessEntities.CacheConfiguration"/> to an instance of <see cref="DataContracts.Administration.CacheConfiguration"/>
        /// </summary>
        /// <param name="cacheConfig">The instance of <see cref="BusinessEntities.CacheConfiguration"/> to convert.</param>
        /// <returns><see cref=" DataContracts.Administration.CacheConfiguration"/> instance converted from the specified cacheConfig.</returns>
        internal static DataContracts.Administration.CacheConfiguration FromBusinessCacheConfiguration(this BusinessEntities.CacheConfiguration cacheConfig)
        {
            Contract.Requires<ArgumentNullException>(cacheConfig != null, "cacheConfig");
            DataContracts.Administration.CacheConfiguration businessCacheConfig = new DataContracts.Administration.CacheConfiguration();
            businessCacheConfig.InjectFrom(cacheConfig);
            businessCacheConfig.EvictionType = (DataContracts.Administration.EvictionType)Enum.ToObject(typeof(DataContracts.Administration.EvictionType), cacheConfig.EvictionType);
            return businessCacheConfig;
        }

        /// <summary>
        /// Converts an instance of <see cref="BusinessEntities.CacheHostHealth"/> to an instance of <see cref="DataContracts.Reporting.CacheHostHealth"/>
        /// </summary>
        /// <param name="hostHealth">The instance of <see cref="BusinessEntities.CacheHostHealth"/> to convert.</param>
        /// <returns><see cref=" DataContracts.Reporting.CacheHostHealth"/> instance converted from the specified hostHealth.</returns>
        internal static DataContracts.Reporting.CacheHostHealth FromBusinessCacheHostHealth(this BusinessEntities.CacheHostHealth hostHealth)
        {
            Contract.Requires<ArgumentNullException>(hostHealth != null, "hostHealth");
            DataContracts.Reporting.CacheHostHealth host = new DataContracts.Reporting.CacheHostHealth();
            host.InjectFrom(hostHealth);
            host.Status = (DataContracts.Administration.CacheServiceStatus)Enum.ToObject(typeof(DataContracts.Administration.CacheServiceStatus), hostHealth.Status);
            if (hostHealth.NamedCaches != null && hostHealth.NamedCaches.Count > 0)
            {
                host.NamedCaches = new List<DataContracts.Reporting.NamedCacheHealth>(hostHealth.NamedCaches.Count);
                foreach (BusinessEntities.NamedCacheHealth namedCacheHealth in hostHealth.NamedCaches)
                {
                    if (namedCacheHealth != null)
                    {
                        DataContracts.Reporting.NamedCacheHealth dataNamedCache = namedCacheHealth.FromBusinessNamedCacheHealth();
                        if (dataNamedCache != null)
                        {
                            host.NamedCaches.Add(dataNamedCache);
                        }
                    }
                }
            }

            return host;
        }

        /// <summary>
        /// Converts an instance of <see cref="BusinessEntities.NamedCacheHealth"/> to an instance of <see cref="DataContracts.Reporting.NamedCacheHealth"/>
        /// </summary>
        /// <param name="cacheHealth">The instance of <see cref="BusinessEntities.NamedCacheHealth"/> to convert.</param>
        /// <returns><see cref=" DataContracts.Reporting.NamedCacheHealth"/> instance converted from the specified cacheHealth.</returns>
        internal static DataContracts.Reporting.NamedCacheHealth FromBusinessNamedCacheHealth(this BusinessEntities.NamedCacheHealth cacheHealth)
        {
            Contract.Requires<ArgumentNullException>(cacheHealth != null, "cacheHealth");
            DataContracts.Reporting.NamedCacheHealth cache = new DataContracts.Reporting.NamedCacheHealth();
            cache.InjectFrom(cacheHealth);
            cache.State = (DataContracts.Reporting.NamedCacheHealthStatus)Enum.ToObject(typeof(DataContracts.Reporting.NamedCacheHealthStatus), cacheHealth.State);
            return cache;
        }

        /// <summary>
        /// Converts an instance of <see cref="BusinessEntities.UnallocatedNamedCache"/> to an instance of <see cref="DataContracts.Reporting.UnallocatedNamedCache"/>
        /// </summary>
        /// <param name="unallacotedNamedCache">The instance of <see cref="BusinessEntities.UnallocatedNamedCache"/> to convert.</param>
        /// <returns><see cref=" DataContracts.Reporting.UnallocatedNamedCache"/> instance converted from the specified unallacotedNamedCache.</returns>
        internal static DataContracts.Reporting.UnallocatedNamedCache FromBusinessNamedCacheHealth(this BusinessEntities.UnallocatedNamedCache unallacotedNamedCache)
        {
            Contract.Requires<ArgumentNullException>(unallacotedNamedCache != null, "unallacotedNamedCache");
            DataContracts.Reporting.UnallocatedNamedCache dataUnallocatedNamedCache = new DataContracts.Reporting.UnallocatedNamedCache();
            dataUnallocatedNamedCache.InjectFrom(unallacotedNamedCache);
            return dataUnallocatedNamedCache;
        }

        internal static DataContracts.Reporting.CacheStatistics FromBusinessCacheStatistics(this BusinessEntities.CacheStatistics cacheStatistics)
        {
            Contract.Requires<ArgumentNullException>(cacheStatistics != null, "cacheStatistics");
            DataContracts.Reporting.CacheStatistics dataCacheStatistics = new DataContracts.Reporting.CacheStatistics();
            dataCacheStatistics.InstanceName = cacheStatistics.InstanceName;
            if (cacheStatistics.CountersByHost != null && cacheStatistics.CountersByHost.Count > 0)
            {
                Dictionary<string, IList<DataContracts.Reporting.Counter>> countersByHost = new Dictionary<string, IList<DataContracts.Reporting.Counter>>(cacheStatistics.CountersByHost.Count);
                foreach (KeyValuePair<string, IList<BusinessEntities.Counter>> kv in cacheStatistics.CountersByHost)
                {
                    if (kv.Value != null)
                    {
                        countersByHost.Add(kv.Key, kv.Value.FromBusinessCounters());
                    }
                }

                dataCacheStatistics.CountersByHost = countersByHost;
            }

            if (cacheStatistics.Counters != null && cacheStatistics.Counters.Count > 0)
            {
                dataCacheStatistics.Counters = cacheStatistics.Counters.FromBusinessCounters();
            }

            return dataCacheStatistics;
        }

        internal static List<DataContracts.Reporting.Counter> FromBusinessCounters(this IEnumerable<BusinessEntities.Counter> businessCounters)
        {
            Contract.Requires<ArgumentNullException>(businessCounters != null, "businessCounters");
            List<DataContracts.Reporting.Counter> counters = new List<DataContracts.Reporting.Counter>();
            foreach (BusinessEntities.Counter counter in businessCounters)
            {
                if (counter != null)
                {
                    DataContracts.Reporting.Counter dataCounter = new DataContracts.Reporting.Counter();
                    dataCounter.InjectFrom(counter);
                    counters.Add(dataCounter);
                }
            }

            return counters;
        }
    }
}
