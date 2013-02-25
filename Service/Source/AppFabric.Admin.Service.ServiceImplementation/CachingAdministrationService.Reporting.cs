// -----------------------------------------------------------------------
// <copyright file="CachingAdministrationService.Reporting.cs" company="Matthieu Klotz">
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
    using AppFabric.Admin.Common.Security;
    using AppFabric.Admin.Service.DataContracts.Administration;
    using AppFabric.Admin.Service.DataContracts.Reporting;
    using AppFabric.Admin.Service.MessageContracts.Reporting;
    using AppFabric.Admin.Service.ServiceImplementation.Helpers;

    /// <summary>
    /// Implementation of all caching reporting methods.
    /// </summary>
    public partial class CachingAdministrationService
    {
        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Users)]
        public CacheClusterHealthMessage GetCacheClusterHealth()
        {
            CacheClusterHealthMessage message = new CacheClusterHealthMessage();
            BusinessEntities.CacheClusterHealth cacheClusterHealth = this.reportingBusiness.GetCacheClusterHealth();
            if (cacheClusterHealth != null)
            {
                IList<BusinessEntities.CacheHostHealth> hostsHealth = cacheClusterHealth.Hosts;
                if (hostsHealth != null && hostsHealth.Count > 0)
                {
                    message.Hosts = new List<CacheHostHealth>(hostsHealth.Count);
                    foreach (BusinessEntities.CacheHostHealth host in hostsHealth)
                    {
                        if (host != null)
                        {
                            CacheHostHealth hostHealth = host.FromBusinessCacheHostHealth();
                            if (hostHealth != null)
                            {
                                message.Hosts.Add(hostHealth);
                            }
                        }
                    }
                }

                IList<BusinessEntities.UnallocatedNamedCache> unallocatedNameCaches = cacheClusterHealth.UnallocatedNameCaches;
                if (unallocatedNameCaches != null && unallocatedNameCaches.Count > 0)
                {
                    message.UnallocatedNamedCaches = new List<UnallocatedNamedCache>(unallocatedNameCaches.Count);
                    foreach (BusinessEntities.UnallocatedNamedCache businessUnallocated in unallocatedNameCaches)
                    {
                        if (businessUnallocated != null)
                        {
                            UnallocatedNamedCache namedCache = businessUnallocated.FromBusinessNamedCacheHealth();
                            if (namedCache != null)
                            {
                                message.UnallocatedNamedCaches.Add(namedCache);
                            }
                        }
                    }
                }
            }

            return message;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Users)]
        public SearchResultMessage SearchDataCacheItems(SearchMessage searchMessage)
        {
            SearchResultMessage result = new SearchResultMessage();
            result.CacheItems = new List<DataCacheItem>();
            foreach (DataCacheItemSearch dciSearch in searchMessage.SearchItems)
            {
                if (dciSearch != null && !string.IsNullOrWhiteSpace(dciSearch.SearchPattern) && !string.IsNullOrWhiteSpace(dciSearch.NamedCache))
                {
                    IList<string> items = this.cachingBusiness.SearchCacheItems(dciSearch.SearchPattern, dciSearch.NamedCache, dciSearch.Region);
                    if (items != null && items.Count > 0)
                    {
                        foreach (string item in items)
                        {
                            DataCacheItem cacheItem = new DataCacheItem { Key = item, NamedCache = dciSearch.NamedCache, Region = dciSearch.Region };
                            result.CacheItems.Add(cacheItem);
                        }
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Administrators)]
        public RemoveItemsMessage RemoveDataCacheItems(RemoveItemsMessage removeMessage)
        {
            RemoveItemsMessage result = new RemoveItemsMessage();
            result.Items = new List<DataCacheItem>();
            foreach (DataCacheItem cacheItem in removeMessage.Items)
            {
                if (cacheItem != null && !string.IsNullOrWhiteSpace(cacheItem.Key) && !string.IsNullOrWhiteSpace(cacheItem.NamedCache))
                {
                    if (this.cachingBusiness.RemoveCacheItem(cacheItem.Key, cacheItem.NamedCache))
                    {
                        result.Items.Add(cacheItem);
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Users)]
        public NamedCacheStatsMessage GetCacheStatistics(MessageContracts.NamedCacheMessage namedCacheMessage)
        {
            NamedCacheStatsMessage message = new NamedCacheStatsMessage();
            IList<NamedCache> namedCaches = namedCacheMessage.NamedCaches;
            if (namedCaches != null && namedCaches.Count > 0)
            {
                message.CachesStatistics = new List<CacheStatistics>(namedCaches.Count);
                foreach (NamedCache namedCache in namedCaches)
                {
                    if (namedCache != null && !string.IsNullOrWhiteSpace(namedCache.CacheName))
                    {
                        BusinessEntities.CacheStatistics businessStats = this.reportingBusiness.GetCacheStatistics(namedCache.CacheName);
                        if (businessStats != null)
                        {
                            CacheStatistics stats = businessStats.FromBusinessCacheStatistics();
                            if (stats != null)
                            {
                                message.CachesStatistics.Add(stats);
                            }
                        }
                    }
                }
            }

            return message;
        }

        /// <inheritdoc />
        [PrincipalPermission(SecurityAction.Demand, Role = ApplicationRoles.Users)]
        public CacheHostStatsMessage GetHostStatistics(MessageContracts.CacheHostMessage hostMessage)
        {
            CacheHostStatsMessage message = new CacheHostStatsMessage();
            IList<CacheHost> cacheHosts = hostMessage.CacheHosts;
            if (cacheHosts != null && cacheHosts.Count > 0)
            {
                message.HostsStatistics = new List<Statistics>(cacheHosts.Count);
                foreach (CacheHost cacheHost in cacheHosts)
                {
                    if (cacheHost != null && !string.IsNullOrWhiteSpace(cacheHost.HostName))
                    {
                        BusinessEntities.CacheHost businessCacheHost = cacheHost.ToBusinessCacheHost();
                        if (businessCacheHost != null)
                        {
                            BusinessEntities.Statistics businessStats = this.reportingBusiness.GetHostStatistics(businessCacheHost);
                            if (businessStats != null)
                            {
                                Statistics stats = new Statistics
                                {
                                    InstanceName = businessStats.InstanceName,
                                    Counters = businessStats.Counters.FromBusinessCounters()
                                };

                                message.HostsStatistics.Add(stats);
                            }
                        }
                    }
                }
            }

            return message;
        }
    }
}
