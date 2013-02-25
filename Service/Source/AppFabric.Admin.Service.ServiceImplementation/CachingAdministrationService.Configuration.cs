// -----------------------------------------------------------------------
// <copyright file="CachingAdministrationService.Configuration.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Service.ServiceImplementation
{
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.ServiceModel;
    using AppFabric.Admin.Common.Security;
    using AppFabric.Admin.Service.DataContracts.Administration;
    using AppFabric.Admin.Service.MessageContracts;
    using AppFabric.Admin.Service.MessageContracts.Configuration;
    using AppFabric.Admin.Service.ServiceImplementation.Helpers;

    /// <summary>
    /// Implements all caching configuration operations.
    /// </summary>
    public partial class CachingAdministrationService
    {
        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Users)]
        public ClientAccountMessage GetCacheAllowedClientAccounts()
        {
            ClientAccountMessage message = new ClientAccountMessage();
            IList<string> clientAccounts = this.cachingAdminBusiness.GetCacheAllowedClientAccounts();
            if (clientAccounts != null && clientAccounts.Count > 0)
            {
                message.Accounts = new List<string>(clientAccounts);
            }

            return message;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Administrators)]
        public ClientAccountMessage GrantCacheAllowedClientAccounts(ClientAccountMessage accountMessage)
        {
            ClientAccountMessage message = new ClientAccountMessage();
            if (accountMessage != null && accountMessage.Accounts != null && accountMessage.Accounts.Count > 0)
            {
                message.Accounts = this.cachingAdminBusiness.GrantCacheAllowedClientAccounts(accountMessage.Accounts);
            }

            return message;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Administrators)]
        public ClientAccountMessage RevokeCacheAllowedClientAccounts(ClientAccountMessage accountMessage)
        {
            ClientAccountMessage message = new ClientAccountMessage();
            if (accountMessage != null && accountMessage.Accounts != null && accountMessage.Accounts.Count > 0)
            {
                message.Accounts = this.cachingAdminBusiness.RevokeCacheAllowedClientAccounts(accountMessage.Accounts);
            }

            return message;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Users)]
        public CacheHostConfigurationMessage GetCacheHostConfig(CacheHostMessage hostMessage)
        {
            CacheHostConfigurationMessage message = new CacheHostConfigurationMessage();
            CacheHost contractCacheHost = hostMessage.CacheHosts[0];
            BusinessEntities.CacheHost businnessCacheHost = contractCacheHost.ToBusinessCacheHost();
            if (businnessCacheHost != null)
            {
                BusinessEntities.CacheHostConfiguration cacheHostConfiguration = this.cachingAdminBusiness.GetCacheHostConfig(businnessCacheHost);
                if (cacheHostConfiguration != null)
                {
                    message.CacheHostConfiguration = cacheHostConfiguration.ToAdminCacheHostConfiguration();
                }
            }

            return message;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Administrators)]
        public CacheHostConfigurationMessage SetCacheHostConfig(CacheHostConfigurationMessage cacheHostConfigurationMessage)
        {
            CacheHostConfigurationMessage message = new CacheHostConfigurationMessage();
            BusinessEntities.CacheHostConfiguration config = cacheHostConfigurationMessage.CacheHostConfiguration.ToBusinessCacheHostConfiguration();
            if (config != null)
            {
                BusinessEntities.CacheHostConfiguration hostConfig = this.cachingAdminBusiness.SetCacheHostConfig(config);
                if (hostConfig != null)
                {
                    message.CacheHostConfiguration = hostConfig.ToAdminCacheHostConfiguration();
                }
            }

            return message;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Administrators)]
        public NamedCacheMessage NewCache(CacheConfigurationMessage cacheConfigurationMessage)
        {
            NamedCacheMessage message = new NamedCacheMessage();
            BusinessEntities.CacheConfiguration cacheConfiguration = cacheConfigurationMessage.CacheConfiguration.ToBusinessCacheConfiguration();
            if (cacheConfigurationMessage != null)
            {
                BusinessEntities.NamedCache businessNamedCache = this.cachingAdminBusiness.NewCache(cacheConfiguration);
                if (businessNamedCache != null)
                {
                    message.NamedCaches = new List<DataContracts.Administration.NamedCache>();
                    DataContracts.Administration.NamedCache namedCache = new DataContracts.Administration.NamedCache
                    {
                        CacheName = businessNamedCache.CacheName,
                        HostRegionMap = businessNamedCache.HostRegionMap
                    };
                    message.NamedCaches.Add(namedCache);
                }
            }

            return message;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Administrators)]
        public void RemoveCaches(NamedCacheMessage cacheNameMessage)
        {
            if (cacheNameMessage.NamedCaches != null && cacheNameMessage.NamedCaches.Count > 0)
            {
                foreach (DataContracts.Administration.NamedCache namedCache in cacheNameMessage.NamedCaches)
                {
                    this.cachingAdminBusiness.RemoveCache(namedCache.CacheName);
                }
            }
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Users)]
        public CacheConfigurationMessage GetCacheConfig(NamedCacheMessage cacheNameMessage)
        {
            CacheConfigurationMessage message = new CacheConfigurationMessage();
            if (cacheNameMessage.NamedCaches != null && cacheNameMessage.NamedCaches.Count > 0)
            {
                DataContracts.Administration.NamedCache namedCache = cacheNameMessage.NamedCaches[0];
                if (namedCache != null && !string.IsNullOrWhiteSpace(namedCache.CacheName))
                {
                    BusinessEntities.CacheConfiguration cacheConfig = this.cachingAdminBusiness.GetCacheConfig(namedCache.CacheName);
                    if (cacheConfig != null)
                    {
                        message.CacheConfiguration = cacheConfig.FromBusinessCacheConfiguration();
                    }
                }
            }

            return message;
        }

        /// <inheritdoc />
        [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
        public NamedCacheMessage SetCacheConfig(CacheConfigurationMessage cacheConfigurationMessage)
        {
            NamedCacheMessage message = new NamedCacheMessage();
            BusinessEntities.CacheConfiguration cacheConfiguration = cacheConfigurationMessage.CacheConfiguration.ToBusinessCacheConfiguration();
            if (cacheConfigurationMessage != null)
            {
                BusinessEntities.NamedCache businessNamedCache = this.cachingAdminBusiness.SetCacheConfig(cacheConfiguration);
                if (businessNamedCache != null)
                {
                    message.NamedCaches = new List<DataContracts.Administration.NamedCache>();
                    DataContracts.Administration.NamedCache namedCache = new DataContracts.Administration.NamedCache
                    {
                        CacheName = businessNamedCache.CacheName,
                        HostRegionMap = businessNamedCache.HostRegionMap
                    };
                    message.NamedCaches.Add(namedCache);
                }
            }

            return message;
        }
    }
}
