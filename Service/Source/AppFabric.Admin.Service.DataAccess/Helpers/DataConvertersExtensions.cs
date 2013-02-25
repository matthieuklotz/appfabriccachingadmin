// -----------------------------------------------------------------------
// <copyright file="DataConvertersExtensions.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Service.DataAccess.Helpers
{
    using System;
    using System.Diagnostics.Contracts;
    using AppFabric.Admin.Service.DataAccess.Datas;
    using Microsoft.ApplicationServer.Caching.AdminApi;
    using Microsoft.ApplicationServer.Caching.Commands;
    using Omu.ValueInjecter;

    /// <summary>
    /// Contains static methods for data conversion.
    /// </summary>
    internal static class DataConvertersExtensions
    {
        /// <summary>
        /// Converts an instance of <see cref="HostInfo"/> to an instance of <see cref="CacheHost"/>.
        /// </summary>
        /// <param name="hostInfo">The instance of <see cref="HostInfo"/> to convert.</param>
        /// <returns><see cref="CacheHost"/> instance converted from the specified HostInfo.</returns>
        internal static CacheHost ToCacheHost(this HostInfo hostInfo)
        {
            Contract.Requires<ArgumentNullException>(hostInfo != null, "hostInfo");
            CacheHost cacheHost = new CacheHost();
            cacheHost.InjectFrom(hostInfo);
            if (hostInfo.VersionInfo != null)
            {
                cacheHost.VersionInfo = new CacheHostVersionInfo();
                cacheHost.VersionInfo.InjectFrom(hostInfo.VersionInfo);
            }

            cacheHost.Status = (CacheServiceStatus)Enum.ToObject(typeof(CacheServiceStatus), hostInfo.Status);
            return cacheHost;
        }

        /// <summary>
        /// Converts an instance of <see cref="NamedCacheHealthPerHost"/> to an instance of <see cref="NamedCacheHealth"/>.
        /// </summary>
        /// <param name="namedCacheHealthPerHost">The instance of <see cref="NamedCacheHealthPerHost"/> to convert.</param>
        /// <returns><see cref="NamedCacheHealth"/> instance converted from the specified NamedCacheHealthPerHost.</returns>
        internal static NamedCacheHealth ToNamedCacheHealth(this NamedCacheHealthPerHost namedCacheHealthPerHost)
        {
            Contract.Requires<ArgumentNullException>(namedCacheHealthPerHost != null, "namedCacheHealthPerHost");
            NamedCacheHealth namedCacheHealth = new NamedCacheHealth();
            namedCacheHealth.InjectFrom(namedCacheHealthPerHost);
            namedCacheHealth.Healthy = namedCacheHealthPerHost[NamedCacheHealthPerHost.Healthy];
            namedCacheHealth.NotPrimary = namedCacheHealthPerHost[NamedCacheHealthPerHost.NotPrimary];
            namedCacheHealth.InadequateSecondaries = namedCacheHealthPerHost[NamedCacheHealthPerHost.InadequateSecondaries];
            namedCacheHealth.Throttled = namedCacheHealthPerHost[NamedCacheHealthPerHost.Throttled];
            namedCacheHealth.UnderReconfiguration = namedCacheHealthPerHost[NamedCacheHealthPerHost.UnderReconfiguration];
            return namedCacheHealth;
        }

        /// <summary>
        /// Converts an instance of <see cref="Microsoft.ApplicationServer.Caching.Commands.UnallocatedNamedCache"/> to an instance of <see cref="Datas.UnallocatedNamedCache"/>.
        /// </summary>
        /// <param name="unallocatedNamedCache">The instance of <see cref="Microsoft.ApplicationServer.Caching.Commands.UnallocatedNamedCache"/> to convert.</param>
        /// <returns><see cref="Datas.UnallocatedNamedCache"/> instance converted from the specified NamedCacheHealthPerHost.</returns>
        internal static Datas.UnallocatedNamedCache ToUnallocatedNamedCache(this Microsoft.ApplicationServer.Caching.Commands.UnallocatedNamedCache unallocatedNamedCache)
        {
            Contract.Requires<ArgumentNullException>(unallocatedNamedCache != null, "unallocatedNamedCache");
            Datas.UnallocatedNamedCache dataUnallocatedNamedCache = new Datas.UnallocatedNamedCache();
            dataUnallocatedNamedCache.InjectFrom(unallocatedNamedCache);
            return dataUnallocatedNamedCache;
        }

        /// <summary>
        /// Converts an instance of <see cref="Microsoft.ApplicationServer.Caching.Commands.HostConfig"/> to an instance of <see cref="Datas.CacheHostConfiguration"/>.
        /// </summary>
        /// <param name="hostConfig">The instance of <see cref="Microsoft.ApplicationServer.Caching.Commands.HostConfig"/> to convert.</param>
        /// <returns><see cref="Datas.CacheHostConfiguration"/> instance converted from the specified hostConfig.</returns>
        internal static CacheHostConfiguration ToCacheHostConfiguration(this HostConfig hostConfig)
        {
            Contract.Requires<ArgumentNullException>(hostConfig != null, "hostConfig");
            CacheHostConfiguration cacheHostConfig = new CacheHostConfiguration();
            cacheHostConfig.InjectFrom(hostConfig);
            return cacheHostConfig;
        }

        /// <summary>
        /// Converts an instance of <see cref="Microsoft.ApplicationServer.Caching.Commands.CacheConfig"/> to an instance of <see cref="Datas.CacheConfiguration"/>.
        /// </summary>
        /// <param name="cacheConfig">The instance of <see cref="Microsoft.ApplicationServer.Caching.Commands.CacheConfig"/> to convert.</param>
        /// <returns><see cref="Datas.CacheConfiguration"/> instance converted from the specified cacheConfig.</returns>
        internal static CacheConfiguration ToCacheConfiguration(this CacheConfig cacheConfig)
        {
            Contract.Requires<ArgumentNullException>(cacheConfig != null);
            CacheConfiguration configuration = new CacheConfiguration();
            configuration.InjectFrom(cacheConfig);
            if (!string.IsNullOrWhiteSpace(cacheConfig.EvictionType))
            {
                Datas.EvictionType evictionType;
                if (Enum.TryParse<Datas.EvictionType>(cacheConfig.EvictionType, true, out evictionType))
                {
                    configuration.EvictionType = evictionType;
                }
            }

            return configuration;
        }

        /// <summary>
        /// Converts an instance of <see cref="Microsoft.ApplicationServer.Caching.AdminApi.RegionInfo"/> to an instance of <see cref="Datas.Region"/>.
        /// </summary>
        /// <param name="regionInfo">The instance of <see cref="Microsoft.ApplicationServer.Caching.AdminApi.RegionInfo"/> to convert.</param>
        /// <returns><see cref="Datas.Region"/> instance converted from the specified regionInfo.</returns>
        internal static Region ToDataRegion(this RegionInfo regionInfo)
        {
            Contract.Requires<ArgumentNullException>(regionInfo != null, "regionInfo");
            Region region = new Region();
            region.InjectFrom(regionInfo);
            return region;
        }
    }
}
