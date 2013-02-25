// -----------------------------------------------------------------------
// <copyright file="AdministrationServiceClient.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Clients.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.ServiceModel.Security;
    using AppFabric.Admin.Clients.Common.Helpers;
    using AppFabric.Admin.Common.Configuration;
    using AppFabric.Admin.Service.DataContracts.Administration;
    using AppFabric.Admin.Service.DataContracts.Reporting;
    using AppFabric.Admin.Service.MessageContracts;
    using AppFabric.Admin.Service.MessageContracts.Reporting;
    using AppFabric.Admin.Service.ServiceContracts;
    using AdminCacheHost = AppFabric.Admin.Service.DataContracts.Administration.CacheHost;

    public sealed class AdministrationServiceClient : IAdministrationServiceClient
    {
        private IAdministrationService service;

        private ChannelFactory<IAdministrationService> factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdministrationServiceClient" /> class.
        /// </summary>
        /// <param name="serviceUri">The service URI.</param>
        public AdministrationServiceClient(Uri serviceUri, string userName, string password)
        {
            Contract.Requires<ArgumentNullException>(serviceUri != null);
            Contract.Requires<ArgumentException>(serviceUri.IsAbsoluteUri);
            Contract.Requires<ArgumentException>(string.Equals(Uri.UriSchemeHttp, serviceUri.Scheme, StringComparison.OrdinalIgnoreCase) || string.Equals(Uri.UriSchemeHttps, serviceUri.Scheme, StringComparison.OrdinalIgnoreCase));
            this.InitializeService(serviceUri, userName, password);
        }

        /// <inheritdoc />
        public Data.Cluster GetCacheCluster()
        {
            CacheClusterMessage message = this.service.GetCacheCluster();
            Data.Cluster cluster = message.ToDataCluster();
            return cluster;
        }

        /// <inheritdoc />
        public Data.Cluster StartCacheCluster()
        {
            CacheClusterMessage message = this.service.StartCacheCluster();
            Data.Cluster cluster = message.ToDataCluster();
            return cluster;
        }

        /// <inheritdoc />
        public Data.Cluster StopCacheCluster()
        {
            CacheClusterMessage message = this.service.StopCacheCluster();
            Data.Cluster cluster = message.ToDataCluster();
            return cluster;
        }

        /// <inheritdoc />
        public Data.Cluster RestartCacheCluster()
        {
            CacheClusterMessage message = this.service.RestartCacheCluster();
            Data.Cluster cluster = message.ToDataCluster();
            return cluster;
        }

        /// <inheritdoc />
        public Data.CacheHost StartCacheHost(Data.CacheHost host)
        {
            Data.CacheHost cacheHost = null;
            CacheHostMessage messageToSend = new CacheHostMessage
            {
                CacheHosts = new List<AdminCacheHost> { host.ToDataContractsCacheHost() }
            };

            CacheHostMessage message = this.service.StartCacheHost(messageToSend);
            if (message != null && message.CacheHosts != null && message.CacheHosts.Count > 0)
            {
                cacheHost = message.CacheHosts[0].ToDataCacheHost();
            }

            return cacheHost;
        }

        /// <inheritdoc />
        public Data.CacheHost StopCacheHost(Data.CacheHost host)
        {
            Data.CacheHost cacheHost = null;
            CacheHostMessage messageToSend = new CacheHostMessage
            {
                CacheHosts = new List<AdminCacheHost> { host.ToDataContractsCacheHost() }
            };

            CacheHostMessage message = this.service.StopCacheHost(messageToSend);
            if (message != null && message.CacheHosts != null && message.CacheHosts.Count > 0)
            {
                cacheHost = message.CacheHosts[0].ToDataCacheHost();
            }

            return cacheHost;
        }

        /// <inheritdoc />
        public Data.CacheHost RestartCacheHost(Data.CacheHost host)
        {
            Data.CacheHost cacheHost = null;
            CacheHostMessage messageToSend = new CacheHostMessage
            {
                CacheHosts = new List<AdminCacheHost> { host.ToDataContractsCacheHost() }
            };

            CacheHostMessage message = this.service.RestartCacheHost(messageToSend);
            if (message != null && message.CacheHosts != null && message.CacheHosts.Count > 0)
            {
                cacheHost = message.CacheHosts[0].ToDataCacheHost();
            }

            return cacheHost;
        }

        /// <inheritdoc />
        public Data.ClusterHealth GetCacheClusterHealth()
        {
            Data.ClusterHealth clusterHealth = null;
            CacheClusterHealthMessage message = this.service.GetCacheClusterHealth();
            if (message != null)
            {
                clusterHealth = new Data.ClusterHealth();
                if (message.Hosts != null & message.Hosts.Count > 0)
                {
                    clusterHealth.CacheHosts = new List<Data.CacheHostHealth>(message.Hosts.Count);
                    foreach (CacheHostHealth serviceCacheHostHealth in message.Hosts)
                    {
                        if (serviceCacheHostHealth != null)
                        {
                            Data.CacheHostHealth cacheHostHealth = new Data.CacheHostHealth
                            {
                                HostName = serviceCacheHostHealth.HostName,
                                ServiceStatus = (Data.CacheServiceStatus)Enum.ToObject(typeof(CacheServiceStatus), serviceCacheHostHealth.Status)
                            };

                            if (serviceCacheHostHealth.NamedCaches != null && serviceCacheHostHealth.NamedCaches.Count > 0)
                            {
                                cacheHostHealth.NamedCaches = new List<Data.NamedCacheHealth>(serviceCacheHostHealth.NamedCaches.Count);
                                foreach (NamedCacheHealth serviceNamedCacheHealth in serviceCacheHostHealth.NamedCaches)
                                {
                                    if (serviceNamedCacheHealth != null)
                                    {
                                        Data.NamedCacheHealth namedCacheHealth = new Data.NamedCacheHealth
                                        {
                                            Name = serviceNamedCacheHealth.Name,
                                            State = (Data.NamedCacheHealthStatus)Enum.ToObject(typeof(Data.NamedCacheHealthStatus), serviceNamedCacheHealth.State)
                                        };

                                        cacheHostHealth.NamedCaches.Add(namedCacheHealth);
                                    }
                                }
                            }

                            clusterHealth.CacheHosts.Add(cacheHostHealth);
                        }
                    }
                }

                if (message.UnallocatedNamedCaches != null && message.UnallocatedNamedCaches.Count > 0)
                {
                    clusterHealth.UnallocatedNamedCaches = new List<Data.UnallocatedNamedCache>(message.UnallocatedNamedCaches.Count);
                    foreach (UnallocatedNamedCache serviceUnallocatedNamedCache in message.UnallocatedNamedCaches)
                    {
                        if (serviceUnallocatedNamedCache != null)
                        {
                            Data.UnallocatedNamedCache unalloctedNamedCache = new Data.UnallocatedNamedCache
                            {
                                Name = serviceUnallocatedNamedCache.Name,
                                Fraction = serviceUnallocatedNamedCache.Fraction
                            };

                            clusterHealth.UnallocatedNamedCaches.Add(unalloctedNamedCache);
                        }
                    }
                }
            }

            return clusterHealth;
        }

        /// <inheritdoc />
        public ICollection<Data.DataCacheItem> SearchCacheItems(ICollection<Data.SearchItemRequest> itemsToSearch)
        {
            List<Data.DataCacheItem> results = null;
            if (itemsToSearch != null && itemsToSearch.Count > 0)
            {
                SearchMessage searchMessage = new SearchMessage();
                searchMessage.SearchItems = new List<DataCacheItemSearch>(itemsToSearch.Count);
                foreach (Data.SearchItemRequest item in itemsToSearch)
                {
                    if (item != null)
                    {
                        DataCacheItemSearch dataCacheItemSearch = new DataCacheItemSearch
                        {
                            SearchPattern = item.SearchPattern,
                            NamedCache = item.NamedCache,
                            Region = item.Region
                        };

                        searchMessage.SearchItems.Add(dataCacheItemSearch);
                    }
                }

                SearchResultMessage serviceResults = this.service.SearchDataCacheItems(searchMessage);
                if (serviceResults != null)
                {
                    IList<DataCacheItem> datacacheItems = serviceResults.CacheItems;
                    if (datacacheItems != null && datacacheItems.Count > 0)
                    {
                        results = new List<Data.DataCacheItem>(datacacheItems.Count);
                        foreach (DataCacheItem dataCacheItem in datacacheItems)
                        {
                            if (dataCacheItem != null)
                            {
                                Data.DataCacheItem searchItemResult = new Data.DataCacheItem
                                {
                                    Key = dataCacheItem.Key,
                                    NamedCache = dataCacheItem.NamedCache,
                                    Region = dataCacheItem.Region
                                };

                                results.Add(searchItemResult);
                            }
                        }
                    }
                }
            }

            return results;
        }

        /// <inheritdoc />
        public ICollection<Data.DataCacheItem> RemoveCacheItems(ICollection<Data.DataCacheItem> itemsToRemove)
        {
            List<Data.DataCacheItem> removedItems = null;
            if (itemsToRemove != null && itemsToRemove.Count > 0)
            {
                RemoveItemsMessage message = new RemoveItemsMessage();
                message.Items = new List<DataCacheItem>(itemsToRemove.Count);
                foreach (Data.DataCacheItem item in itemsToRemove)
                {
                    if (item != null)
                    {
                        DataCacheItem dataCacheItem = new DataCacheItem
                        {
                            Key = item.Key,
                            NamedCache = item.NamedCache,
                            Region = item.Region
                        };

                        message.Items.Add(dataCacheItem);
                    }
                }

                RemoveItemsMessage removedItemsResult = this.service.RemoveDataCacheItems(message);
                if (removedItemsResult != null)
                {
                    IList<DataCacheItem> dataCacheItems = removedItemsResult.Items;
                    if (dataCacheItems != null && dataCacheItems.Count > 0)
                    {
                        removedItems = new List<Data.DataCacheItem>(dataCacheItems.Count);
                        foreach (DataCacheItem item in dataCacheItems)
                        {
                            if (item != null)
                            {
                                Data.DataCacheItem removedItem = new Data.DataCacheItem
                                {
                                    Key = item.Key,
                                    NamedCache = item.NamedCache,
                                    Region = item.Region
                                };

                                removedItems.Add(removedItem);
                            }
                        }
                    }
                }
            }

            return removedItems;
        }

        /// <inheritdoc />
        public ICollection<Data.CacheStatistics> GetCachesStatistics(ICollection<Data.NamedCache> namedCaches)
        {
            List<Data.CacheStatistics> statsByNamedCache = null;
            NamedCacheMessage message = new NamedCacheMessage();
            List<NamedCache> adminNamedCaches = new List<NamedCache>(namedCaches.Count);
            foreach (Data.NamedCache namedCache in namedCaches)
            {
                if (namedCache != null)
                {
                    NamedCache adminNamedCache = namedCache.ToDataContractsNamedCache();
                    if (adminNamedCache != null)
                    {
                        adminNamedCaches.Add(adminNamedCache);
                    }
                }
            }

            message.NamedCaches = adminNamedCaches;
            NamedCacheStatsMessage response = this.service.GetCacheStatistics(message);
            if (response != null)
            {
                IList<CacheStatistics> stats = response.CachesStatistics;
                if (stats != null && stats.Count > 0)
                {
                    statsByNamedCache = new List<Data.CacheStatistics>(stats.Count);
                    foreach (CacheStatistics stat in stats)
                    {
                        if (stat != null)
                        {
                            Data.CacheStatistics cacheStats = stat.ToDataCacheStatistics();
                            statsByNamedCache.Add(cacheStats);
                        }
                    }
                }
            }

            return statsByNamedCache;
        }

        /// <inheritdoc />
        public ICollection<Data.Statistics> GetHostsStatistics(ICollection<Data.CacheHost> hosts)
        {
            List<Data.Statistics> stats = null;
            CacheHostMessage message = new CacheHostMessage();
            List<AdminCacheHost> cacheHosts = new List<AdminCacheHost>(hosts.Count);
            foreach (Data.CacheHost host in hosts)
            {
                if (host != null)
                {
                    AdminCacheHost adminCacheHost = host.ToDataContractsCacheHost();
                    if (adminCacheHost != null)
                    {
                        cacheHosts.Add(adminCacheHost);
                    }
                }
            }

            message.CacheHosts = cacheHosts;
            CacheHostStatsMessage response = this.service.GetHostStatistics(message);
            if (response != null)
            {
                IList<Statistics> statistics = response.HostsStatistics;
                if (statistics != null && statistics.Count > 0)
                {
                    stats = new List<Data.Statistics>(statistics.Count);
                    foreach (Statistics hostStat in statistics)
                    {
                        if (hostStat != null)
                        {
                            Data.Statistics stat = hostStat.ToDataStatistics();
                            if (stat != null)
                            {
                                stats.Add(stat);
                            }
                        }
                    }
                }
            }

            return stats;
        }

        private void FactoryFaulted(object sender, EventArgs e)
        {
            ChannelFactory<IAdministrationService> factorySender = sender as ChannelFactory<IAdministrationService>;
            if (factorySender == null)
            {
                return;
            }

            factorySender.Faulted -= this.FactoryFaulted;
            if (factorySender != null)
            {
                try
                {
                    factorySender.Abort();
                }
                finally
                {
                    this.service = this.factory.CreateChannel();
                    factorySender.Faulted += this.FactoryFaulted;
                }
            }
        }

        private void InitializeService(Uri serviceUri, string userName, string password)
        {
            string dnsIdentity = ConfigurationManager.AppSettings[Constants.WcfDnsEndpointIdentityAppSetting];
            EndpointIdentity identity = EndpointIdentity.CreateDnsIdentity(dnsIdentity);
            EndpointAddress endpointAddress = new EndpointAddress(serviceUri, identity);
            WSHttpBinding binding = null;
            string bindingName = ConfigurationManager.AppSettings[Constants.WcfClientBindingNameAppSetting];
            if (string.IsNullOrWhiteSpace(bindingName))
            {
                binding = new WSHttpBinding();
            }
            else
            {
                binding = new WSHttpBinding(bindingName);
            }

            this.factory = new ChannelFactory<IAdministrationService>(binding, endpointAddress);
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                ClientCredentials defaultCredentials = this.factory.Endpoint.Behaviors.Find<ClientCredentials>();
                this.factory.Endpoint.Behaviors.Remove(defaultCredentials);
                ClientCredentials loginCredentials = new ClientCredentials();
                loginCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
                loginCredentials.UserName.UserName = userName;
                loginCredentials.UserName.Password = password;
                this.factory.Endpoint.Behaviors.Add(loginCredentials);
            }

            this.factory.Faulted += this.FactoryFaulted;
            this.service = this.factory.CreateChannel();
        }
    }
}
