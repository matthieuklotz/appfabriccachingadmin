// -----------------------------------------------------------------------
// <copyright file="CachingAdministrationService.cs" company="Matthieu Klotz">
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
    using AppFabric.Admin.Common.Injection;
    using AppFabric.Admin.Common.Security;
    using AppFabric.Admin.Service.BusinessLogic.Interfaces;
    using AppFabric.Admin.Service.DataContracts.Administration;
    using AppFabric.Admin.Service.MessageContracts;
    using AppFabric.Admin.Service.ServiceContracts;
    using AppFabric.Admin.Service.ServiceImplementation.Helpers;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Implementation of all caching administration methods.
    /// </summary>
    [ServiceBehavior(Namespace = "http://appcacheadminservice.codeplex.com/Services/", InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class CachingAdministrationService : IAdministrationService
    {
        /// <summary>
        /// Gets the caching administration business layer.
        /// </summary>
        private ICachingAdminBusiness cachingAdminBusiness;

        /// <summary>
        /// The caching business layer.
        /// </summary>
        private ICachingSearchBusiness cachingBusiness;

        /// <summary>
        /// The administration business layer.
        /// </summary>
        private ICachingReportingBusiness reportingBusiness;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingAdministrationService"/> class.
        /// </summary>
        public CachingAdministrationService()
        {
            this.cachingAdminBusiness = IoCManager.Container.Resolve<ICachingAdminBusiness>();
            this.cachingBusiness = IoCManager.Container.Resolve<ICachingSearchBusiness>();
            this.reportingBusiness = IoCManager.Container.Resolve<ICachingReportingBusiness>();
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Users)]
        public CacheClusterMessage GetCacheCluster()
        {
            CacheClusterMessage message = new CacheClusterMessage();
            IList<BusinessEntities.CacheHost> cluster = this.cachingAdminBusiness.GetCacheCluster();
            if (cluster != null && cluster.Count > 0)
            {
                message.CacheHosts = cluster.ToAdminCacheHosts();
            }

            message.NamedCaches = this.GetNamedCaches();
            return message;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Administrators)]
        public CacheClusterMessage StartCacheCluster()
        {
            CacheClusterMessage message = new CacheClusterMessage();
            IList<BusinessEntities.CacheHost> cluster = this.cachingAdminBusiness.StartCacheCluster();
            if (cluster != null && cluster.Count > 0)
            {
                message.CacheHosts = cluster.ToAdminCacheHosts();
            }

            message.NamedCaches = this.GetNamedCaches();
            return message;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Administrators)]
        public CacheClusterMessage StopCacheCluster()
        {
            CacheClusterMessage message = new CacheClusterMessage();
            IList<BusinessEntities.CacheHost> cluster = this.cachingAdminBusiness.StopCacheCluster();
            if (cluster != null && cluster.Count > 0)
            {
                message.CacheHosts = cluster.ToAdminCacheHosts();
            }

            message.NamedCaches = this.GetNamedCaches();
            return message;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Administrators)]
        public CacheClusterMessage RestartCacheCluster()
        {
            CacheClusterMessage message = new CacheClusterMessage();
            IList<BusinessEntities.CacheHost> cluster = this.cachingAdminBusiness.RestartCacheCluster();
            if (cluster != null && cluster.Count > 0)
            {
                message.CacheHosts = cluster.ToAdminCacheHosts();
            }

            message.NamedCaches = this.GetNamedCaches();
            return message;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Administrators)]
        public CacheHostMessage StartCacheHost(CacheHostMessage hostMessage)
        {
            CacheHostMessage message = new CacheHostMessage();
            CacheHost contractCacheHost = hostMessage.CacheHosts[0];
            BusinessEntities.CacheHost cacheHost = contractCacheHost.ToBusinessCacheHost();
            if (cacheHost != null)
            {
                BusinessEntities.CacheHost businessCacheHost = this.cachingAdminBusiness.StartCacheHost(cacheHost);
                if (businessCacheHost != null)
                {
                    message.CacheHosts = new List<CacheHost>(1);
                    message.CacheHosts.Add(businessCacheHost.ToAdminCacheHost());
                }
            }

            return message;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Administrators)]
        public CacheHostMessage StopCacheHost(CacheHostMessage hostMessage)
        {
            CacheHostMessage message = new CacheHostMessage();
            CacheHost contractCacheHost = hostMessage.CacheHosts[0];
            BusinessEntities.CacheHost cacheHost = contractCacheHost.ToBusinessCacheHost();
            if (cacheHost != null)
            {
                BusinessEntities.CacheHost businessCacheHost = this.cachingAdminBusiness.StopCacheHost(cacheHost);
                if (businessCacheHost != null)
                {
                    message.CacheHosts = new List<CacheHost>(1);
                    message.CacheHosts.Add(businessCacheHost.ToAdminCacheHost());
                }
            }

            return message;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Administrators)]
        public CacheHostMessage RestartCacheHost(CacheHostMessage hostMessage)
        {
            CacheHostMessage message = new CacheHostMessage();
            CacheHost contractCacheHost = hostMessage.CacheHosts[0];
            BusinessEntities.CacheHost cacheHost = contractCacheHost.ToBusinessCacheHost();
            if (cacheHost != null)
            {
                BusinessEntities.CacheHost businessCacheHost = this.cachingAdminBusiness.RestartCacheHost(cacheHost);
                if (businessCacheHost != null)
                {
                    message.CacheHosts = new List<CacheHost>(1);
                    message.CacheHosts.Add(businessCacheHost.ToAdminCacheHost());
                }
            }

            return message;
        }

        private List<DataContracts.Administration.NamedCache> GetNamedCaches()
        {
            List<DataContracts.Administration.NamedCache> namedCaches = null;
            IList<BusinessEntities.NamedCache> adminNamedCaches = this.cachingAdminBusiness.GetNamedCaches();
            if (adminNamedCaches != null && adminNamedCaches.Count > 0)
            {
                namedCaches = new List<DataContracts.Administration.NamedCache>(adminNamedCaches.Count);
                foreach (BusinessEntities.NamedCache businessNamedCache in adminNamedCaches)
                {
                    DataContracts.Administration.NamedCache namedCache = new DataContracts.Administration.NamedCache
                    {
                        CacheName = businessNamedCache.CacheName
                    };

                    namedCaches.Add(namedCache);
                }
            }

            return namedCaches;
        }
    }
}
