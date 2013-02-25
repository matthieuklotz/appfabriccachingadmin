// -----------------------------------------------------------------------
// <copyright file="CacheHostHelper.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Clients.Common.Helpers
{
    using System;
    using System.Diagnostics.Contracts;
    using Omu.ValueInjecter;
    using AdminCacheHost = AppFabric.Admin.Service.DataContracts.Administration.CacheHost;
    using AdminCacheHostVersionInfo = AppFabric.Admin.Service.DataContracts.Administration.CacheHostVersionInfo;
    using AdminCacheServiceStatus = AppFabric.Admin.Service.DataContracts.Administration.CacheServiceStatus;

    internal static class CacheHostHelper
    {
        internal static Data.CacheHost ToDataCacheHost(this AdminCacheHost adminCacheHost)
        {
            Contract.Requires<ArgumentNullException>(adminCacheHost != null);
            Data.CacheHost cacheHost = new Data.CacheHost();
            cacheHost.InjectFrom(adminCacheHost);
            if (adminCacheHost.VersionInformation != null)
            {
                cacheHost.VersionInformation = adminCacheHost.VersionInformation.ToDataVersionInformation();
            }

            cacheHost.ServiceStatus = (Data.CacheServiceStatus)Enum.ToObject(typeof(Data.CacheServiceStatus), adminCacheHost.ServiceStatus);
            return cacheHost;
        }

        internal static AdminCacheHost ToDataContractsCacheHost(this Data.CacheHost cacheHost)
        {
            Contract.Requires<ArgumentNullException>(cacheHost != null);
            AdminCacheHost adminCacheHost = new AdminCacheHost();
            adminCacheHost.InjectFrom(cacheHost);
            if (cacheHost.VersionInformation != null)
            {
                adminCacheHost.VersionInformation = cacheHost.VersionInformation.ToDataContractsVersionInformation();
            }

            adminCacheHost.ServiceStatus = (AdminCacheServiceStatus)Enum.ToObject(typeof(AdminCacheServiceStatus), cacheHost.ServiceStatus);
            return adminCacheHost;
        }

        internal static Data.CacheHostVersionInfo ToDataVersionInformation(this AdminCacheHostVersionInfo adminVersionInfo)
        {
            Contract.Requires<ArgumentNullException>(adminVersionInfo != null);
            Data.CacheHostVersionInfo versionInfo = new Data.CacheHostVersionInfo();
            versionInfo.InjectFrom(adminVersionInfo);
            return versionInfo;
        }

        internal static AdminCacheHostVersionInfo ToDataContractsVersionInformation(this Data.CacheHostVersionInfo versionInfo)
        {
            Contract.Requires<ArgumentNullException>(versionInfo != null);
            AdminCacheHostVersionInfo adminVersionInfo = new AdminCacheHostVersionInfo();
            adminVersionInfo.InjectFrom(versionInfo);
            return adminVersionInfo;
        }
    }
}
