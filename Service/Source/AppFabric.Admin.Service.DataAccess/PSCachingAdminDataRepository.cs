// -----------------------------------------------------------------------
// <copyright file="PSCachingAdminDataRepository.cs" company="Matthieu Klotz">
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
    using System.Diagnostics.Contracts;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using AppFabric.Admin.Common.Configuration;
    using AppFabric.Admin.Common.Injection;
    using AppFabric.Admin.Common.Logging;
    using AppFabric.Admin.Service.DataAccess.Datas;
    using AppFabric.Admin.Service.DataAccess.Exceptions;
    using AppFabric.Admin.Service.DataAccess.Helpers;
    using AppFabric.Admin.Service.DataAccess.Interfaces;
    using AppFabric.Admin.Service.DataAccess.Resources;
    using Microsoft.ApplicationServer.Caching.AdminApi;
    using Microsoft.ApplicationServer.Caching.Commands;

    /// <summary>
    /// Provides the base class for administrate AppFabric Caching Service.
    /// </summary>
    [ObjectDisposedValidatorAspect]
    public sealed class PSCachingAdminDataRepository : ICachingAdminDataRepository, IDisposable
    {
        /// <summary>
        /// PowerShell runspace to use for executing commands.
        /// </summary>
        private RunspacePool powerShellRunspacePool;

        /// <summary>
        /// Initializes a new instance of the <see cref="PSCachingAdminDataRepository"/> class.
        /// </summary>
        public PSCachingAdminDataRepository()
        {
            this.Initialize();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="PSCachingAdminDataRepository"/> class.
        /// </summary>
        ~PSCachingAdminDataRepository()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Indicates if the current instance is disposed or not.
        /// </summary>
        private bool Disposed { get; set; }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public IList<CacheHost> StartCacheCluster()
        {
            return this.PsCommandToCacheHosts(PSAdminCommandsConstants.StartCacheCluster);
        }

        /// <inheritdoc />
        public IList<CacheHost> RestartCacheCluster()
        {
            return this.PsCommandToCacheHosts(PSAdminCommandsConstants.RestartCacheCluster);
        }

        /// <inheritdoc />
        public IList<CacheHost> StopCacheCluster()
        {
            return this.PsCommandToCacheHosts(PSAdminCommandsConstants.StopCacheCluster);
        }

        /// <inheritdoc />
        public CacheClusterHealth GetCacheClusterHealth()
        {
            IList<PSObject> results = null;
            try
            {
                results = this.powerShellRunspacePool.InvokeCommand(PSAdminCommandsConstants.GetCacheClusterHealth);
            }
            catch (CmdletInvocationException)
            {
                throw new ClusterAdminException(Resources.ErrorMessages.NoClusterStatistics);
            }

            if (results == null || results.Count < 1)
            {
                throw new ClusterAdminException(Resources.ErrorMessages.NoClusterStatistics);
            }

            ClusterHealth apiClusterHealth = null;
            PSObject result = results[0];
            if (result != null)
            {
                apiClusterHealth = result.BaseObject as ClusterHealth;
            }

            if (apiClusterHealth == null)
            {
                throw new ClusterAdminException(Resources.ErrorMessages.NoClusterStatistics);
            }

            CacheClusterHealth cacheClusterHealth = new CacheClusterHealth();
            if (apiClusterHealth.Hosts != null && apiClusterHealth.Hosts.Count > 0)
            {
                cacheClusterHealth.Hosts = new List<CacheHostHealth>(apiClusterHealth.Hosts.Count);
                foreach (HostHealth hostHealth in apiClusterHealth.Hosts)
                {
                    if (hostHealth == null)
                    {
                        continue;
                    }

                    CacheHostHealth cacheHostHealth = new CacheHostHealth
                    {
                        HostName = hostHealth.HostName
                    };
                    if (hostHealth.NamedCaches != null && hostHealth.NamedCaches.Count > 0)
                    {
                        cacheHostHealth.NamedCaches = new List<NamedCacheHealth>(hostHealth.NamedCaches.Count);
                        foreach (NamedCacheHealthPerHost namedCachePerHost in hostHealth.NamedCaches)
                        {
                            if (namedCachePerHost == null)
                            {
                                continue;
                            }

                            NamedCacheHealth namedCacheHealth = namedCachePerHost.ToNamedCacheHealth();
                            cacheHostHealth.NamedCaches.Add(namedCacheHealth);
                        }
                    }

                    cacheClusterHealth.Hosts.Add(cacheHostHealth);
                }
            }

            if (apiClusterHealth.UnallocatedNamedCaches != null && apiClusterHealth.UnallocatedNamedCaches.NamedCaches != null && apiClusterHealth.UnallocatedNamedCaches.NamedCaches.Count > 1)
            {
                cacheClusterHealth.UnallocatedNamedCaches = new List<Datas.UnallocatedNamedCache>(apiClusterHealth.UnallocatedNamedCaches.NamedCaches.Count);
                foreach (var unallocatedNamedCache in apiClusterHealth.UnallocatedNamedCaches.NamedCaches)
                {
                    if (unallocatedNamedCache != null)
                    {
                        Datas.UnallocatedNamedCache unallocatedCache = unallocatedNamedCache.ToUnallocatedNamedCache();
                        cacheClusterHealth.UnallocatedNamedCaches.Add(unallocatedCache);
                    }
                }
            }

            return cacheClusterHealth;
        }

        /// <inheritdoc />
        public IList<string> GetCacheAllowedClientAccounts()
        {
            IList<PSObject> accounts = this.powerShellRunspacePool.InvokeCommand(PSAdminCommandsConstants.GetCacheAllowedClientAccount);
            IList<string> results = null;
            if (accounts != null && accounts.Count > 0)
            {
                results = new List<string>(accounts.Count);
                foreach (var account in accounts)
                {
                    string result = account.BaseObject as string;
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        results.Add(result);
                    }
                }
            }

            return results;
        }

        /// <inheritdoc />
        public void GrantCacheAllowedClientAccount(string clientAccount, bool force)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { PSAdminParamsConstants.ClientAccount, clientAccount },
                { PSAdminParamsConstants.ForceCommand, force }
            };

            try
            {
                this.powerShellRunspacePool.InvokeCommand(PSAdminCommandsConstants.GrantCacheAllowedClientAccount, parameters);
            }
            catch (CmdletInvocationException cmdLetException)
            {
                throw new ClusterAdminException(cmdLetException.Message);
            }
        }

        /// <inheritdoc />
        public void RevokeCacheAllowedClientAccount(string clientAccount)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { PSAdminParamsConstants.ClientAccount, clientAccount }
            };

            try
            {
                this.powerShellRunspacePool.InvokeCommand(PSAdminCommandsConstants.RevokeCacheAllowedClientAccount, parameters);
            }
            catch (CmdletInvocationException cmdLetException)
            {
                throw new ClusterAdminException(cmdLetException.Message);
            }
        }

        /// <inheritdoc />
        public void ExportCacheClusterConfig(string fileName)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { PSAdminParamsConstants.FilePath, fileName }
            };

            this.powerShellRunspacePool.InvokeCommand(PSAdminCommandsConstants.ExportCacheClusterConfig, parameters);
        }

        /// <inheritdoc />
        public void ImportCacheClusterConfig(string fileName)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { PSAdminParamsConstants.FilePath, fileName },
                { PSAdminParamsConstants.ForceCommand, true }
            };

            try
            {
                this.powerShellRunspacePool.InvokeCommand(PSAdminCommandsConstants.ImportCacheClusterConfig, parameters);
            }
            catch (CmdletInvocationException cmdLetException)
            {
                throw new ClusterAdminException(cmdLetException.Message);
            }
        }

        /// <inheritdoc />
        public IList<CacheHost> GetCacheHost(string hostName, int portNo)
        {
            Dictionary<string, object> commandParameters = null;
            if (!string.IsNullOrWhiteSpace(hostName))
            {
                commandParameters = new Dictionary<string, object>
                                    {
                                        { PSAdminParamsConstants.HostName, hostName },
                                        { PSAdminParamsConstants.PortNo, portNo }
                                    };
            }

            return this.PsCommandToCacheHosts(PSAdminCommandsConstants.GetCacheHost, commandParameters);
        }

        /// <inheritdoc />
        public CacheHostConfiguration GetCacheHostConfig(string hostName, int portNo)
        {
            Dictionary<string, object> commandParameters = new Dictionary<string, object>
            {
                { PSAdminParamsConstants.HostName, hostName },
                { PSAdminParamsConstants.PortNo, portNo }
            };

            CacheHostConfiguration cacheHostConfig = null;
            IList<PSObject> results = this.powerShellRunspacePool.InvokeCommand(PSAdminCommandsConstants.GetCacheHostConfig, commandParameters);
            if (results != null && results.Count > 0)
            {
                PSObject result = results[0];
                if (result != null)
                {
                    HostConfig hostConfig = result.BaseObject as HostConfig;
                    if (hostConfig != null)
                    {
                        cacheHostConfig = hostConfig.ToCacheHostConfiguration();
                    }
                }
            }

            return cacheHostConfig;
        }

        /// <inheritdoc />
        public void SetCacheHostConfig(CacheHostConfiguration cacheHostConfiguration)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { PSAdminParamsConstants.HostName, cacheHostConfiguration.HostName },
                { PSAdminParamsConstants.PortNo, cacheHostConfiguration.CachePort },
                { PSAdminParamsConstants.ArbitrationPortNumber, cacheHostConfiguration.ArbitrationPort },
                { PSAdminParamsConstants.CacheSize, cacheHostConfiguration.Size },
                { PSAdminParamsConstants.ClusterPortNumber, cacheHostConfiguration.ClusterPort },
                { PSAdminParamsConstants.HighWatermark, cacheHostConfiguration.HighWatermark },
                { PSAdminParamsConstants.LowWatermark, cacheHostConfiguration.LowWatermark },
                { PSAdminParamsConstants.ReplicationPortNumber, cacheHostConfiguration.ReplicationPort }
            };

            this.powerShellRunspacePool.InvokeCommand(PSAdminCommandsConstants.SetCacheHostConfig, parameters);
        }

        /// <inheritdoc />
        public CacheHost StartCacheHost(string hostName, int portNo, int? hostTimeout)
        {
            return this.StartStopCacheHost(PSAdminCommandsConstants.StartCacheHost, hostName, portNo, hostTimeout);
        }

        /// <inheritdoc />
        public CacheHost StopCacheHost(string hostName, int portNo, int? hostTimeout)
        {
            return this.StartStopCacheHost(PSAdminCommandsConstants.StopCacheHost, hostName, portNo, hostTimeout);
        }

        /// <inheritdoc />
        public CacheHost RestartCacheHost(string hostName, int portNo, int? hostTimeout)
        {
            return this.StartStopCacheHost(PSAdminCommandsConstants.RestartCacheHost, hostName, portNo, hostTimeout);
        }

        /// <inheritdoc />
        public IList<NamedCache> GetCache(string hostName, int? portNo)
        {
            Dictionary<string, object> parameters = null;
            if (!string.IsNullOrWhiteSpace(hostName))
            {
                if (!portNo.HasValue)
                {
                    throw new ArgumentException(ErrorMessages.InvalidHostPort, "portNo");
                }

                parameters = new Dictionary<string, object>
                {
                    { PSAdminParamsConstants.HostName, hostName },
                    { PSAdminParamsConstants.PortNo, portNo.Value }
                };
            }

            IList<PSObject> results = this.powerShellRunspacePool.InvokeCommand(PSAdminCommandsConstants.GetCache, parameters);
            List<NamedCache> namedCaches = null;
            if (results != null && results.Count > 0)
            {
                namedCaches = new List<NamedCache>(results.Count);
                foreach (PSObject psobject in results)
                {
                    if (psobject != null)
                    {
                        CacheInfo cacheInfo = psobject.BaseObject as CacheInfo;
                        if (cacheInfo != null)
                        {
                            NamedCache namedCache = new NamedCache
                            {
                                CacheName = cacheInfo.CacheName,
                                HostRegionMap = cacheInfo.HostRegionMap
                            };

                            namedCaches.Add(namedCache);
                        }
                    }
                }
            }

            return namedCaches;
        }

        /// <inheritdoc />
        public void NewCache(CacheConfiguration cacheConfiguration)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { PSAdminParamsConstants.CacheName, cacheConfiguration.CacheName },
                { PSAdminParamsConstants.Eviction, cacheConfiguration.EvictionType.ToString() },
                { PSAdminParamsConstants.Expirable, cacheConfiguration.IsExpirable },
                { PSAdminParamsConstants.NotificationsEnabled, cacheConfiguration.NotificationsEnabled },
                { PSAdminParamsConstants.TimeToLive, cacheConfiguration.TimeToLive },
                { PSAdminParamsConstants.Secondaries, cacheConfiguration.Secondaries },
                { PSAdminParamsConstants.ForceCommand, null }
            };

            this.powerShellRunspacePool.InvokeCommand(PSAdminCommandsConstants.NewCache, parameters);
        }

        /// <inheritdoc />
        public void RemoveCache(string cacheName)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { PSAdminParamsConstants.CacheName, cacheName }
            };

            this.powerShellRunspacePool.InvokeCommand(PSAdminCommandsConstants.RemoveCache, parameters);
        }

        /// <inheritdoc />
        public CacheConfiguration GetCacheConfig(string cacheName)
        {
            CacheConfiguration configuration = null;
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { PSAdminParamsConstants.CacheName, cacheName }
            };

            IList<PSObject> results = this.powerShellRunspacePool.InvokeCommand(PSAdminCommandsConstants.GetCacheConfig, parameters);
            if (results != null && results.Count > 0)
            {
                PSObject result = results[0];
                if (result != null)
                {
                    CacheConfig cacheConfig = result.BaseObject as CacheConfig;
                    if (cacheConfig != null)
                    {
                        configuration = cacheConfig.ToCacheConfiguration();
                    }
                }
            }

            return configuration;
        }

        /// <inheritdoc />
        public void SetCacheConfig(CacheConfiguration cacheConfiguration)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { PSAdminParamsConstants.CacheName, cacheConfiguration.CacheName },
                { PSAdminParamsConstants.Eviction, cacheConfiguration.EvictionType },
                { PSAdminParamsConstants.Expirable, cacheConfiguration.IsExpirable },
                { PSAdminParamsConstants.NotificationsEnabled, cacheConfiguration.NotificationsEnabled },
                { PSAdminParamsConstants.TimeToLive, cacheConfiguration.TimeToLive },
                { PSAdminParamsConstants.Secondaries, cacheConfiguration.Secondaries },
                { PSAdminParamsConstants.ForceCommand, null }
            };

            this.powerShellRunspacePool.InvokeCommand(PSAdminCommandsConstants.SetCacheConfig, parameters);
        }

        /// <inheritdoc />
        public IList<Region> GetCacheRegions(string cacheName)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                 { PSAdminParamsConstants.CacheName, cacheName }
            };

            IList<Region> regions = null;
            IList<PSObject> result = this.powerShellRunspacePool.InvokeCommand(PSAdminCommandsConstants.GetCacheRegion, parameters);
            if (result != null && result.Count > 0)
            {
                regions = new List<Region>(result.Count);
                foreach (PSObject powerShellObject in result)
                {
                    if (powerShellObject == null)
                    {
                        continue;
                    }

                    RegionInfo regionInfo = powerShellObject.BaseObject as RegionInfo;
                    if (regionInfo != null)
                    {
                        Region region = regionInfo.ToDataRegion();
                        if (region != null)
                        {
                            regions.Add(region);
                        }
                    }
                }
            }

            return regions;
        }

        /// <summary>
        /// Starts / Stops or Restarts the cache host.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="portNo">The port number.</param>
        /// <param name="hostTimeout">The host timeout.</param>
        /// <returns>The restarted cache host.</returns>
        private CacheHost StartStopCacheHost(string command, string hostName, int portNo, int? hostTimeout)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(command), Resources.ErrorMessages.CommandNullOrEmpty);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(hostName), Resources.ErrorMessages.HostNameNullOrEmpty);
            Contract.Requires<ArgumentException>(portNo > PSAdminParamsConstants.MinCachePortNumber, Resources.ErrorMessages.InvalidCachePort);

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { PSAdminParamsConstants.HostName, hostName },
                { PSAdminParamsConstants.PortNo, portNo },
            };

            if (hostTimeout.HasValue)
            {
                parameters.Add(PSAdminParamsConstants.HostTimeout, hostTimeout.Value);
            }

            CacheHost cacheHost = null;
            IList<CacheHost> result = this.PsCommandToCacheHosts(command, parameters);
            if (result != null && result.Count == 1)
            {
                cacheHost = result[0];
            }

            return cacheHost;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                    if (this.powerShellRunspacePool != null)
                    {
                        this.powerShellRunspacePool.Close();
                        this.powerShellRunspacePool.Dispose();
                    }
                }

                this.powerShellRunspacePool = null;
                this.Disposed = true;
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            if (this.powerShellRunspacePool == null)
            {
                string[] powerShellModules = new string[1] { PSAdminModulesConstants.CacheAdminModule };
                this.powerShellRunspacePool = PowerShellHelper.GetRunSpacePool(powerShellModules);
                if (this.powerShellRunspacePool != null)
                {
                    if (this.powerShellRunspacePool.RunspacePoolStateInfo.State != RunspacePoolState.Opened)
                    {
                        this.powerShellRunspacePool.Open();
                    }

                    try
                    {
                        Dictionary<string, object> parameters = null;
                        string provider = ConfigurationManager.AppSettings[Constants.CacheConfigurationProviderAppSettingKey];
                        if (!string.IsNullOrWhiteSpace(provider))
                        {
                            string connectionString = ConfigurationManager.AppSettings[Constants.ProviderConnectionStringAppSettingKey];
                            //// TO-DO : throw exception if provider defined but connectionstring is undefined;

                            parameters = new Dictionary<string, object>
                            {
                                { PSAdminParamsConstants.Provider, provider },
                                { PSAdminParamsConstants.ConnectionString, connectionString }
                            };
                        }

                        this.powerShellRunspacePool.InvokeCommand(PSAdminCommandsConstants.UseCacheCluster, parameters);
                    }
                    catch (CmdletInvocationException cmdLetException)
                    {
                        Logger.Fatal(Constants.PowerShellLoggingCategory, Resources.ErrorMessages.InvalidCluster, cmdLetException);
                        throw new ClusterAdminException(Resources.ErrorMessages.InvalidCluster, cmdLetException);
                    }
                }
            }
        }

        /// <summary>
        /// Executes a PowerShell command that returns a collection of CacheHost.
        /// </summary>
        /// <param name="command">The PowerShell command to invoke.</param>
        /// <param name="parameters">PowerShell command parameters.</param>
        /// <returns>CacheHosts returns by the PowerShell command.</returns>
        private IList<CacheHost> PsCommandToCacheHosts(string command, Dictionary<string, object> parameters = null)
        {
            Contract.Requires(this.powerShellRunspacePool != null);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(command), Resources.ErrorMessages.CommandNullOrEmpty);
            ICollection<PSObject> results = null;
            try
            {
                results = this.powerShellRunspacePool.InvokeCommand(command, parameters);
            }
            catch (CmdletInvocationException cmdLetException)
            {
                throw new ClusterAdminException(cmdLetException.Message);
            }

            if (results == null || results.Count < 1)
            {
                throw new ClusterAdminException(Resources.ErrorMessages.InvalidCluster);
            }

            List<CacheHost> cacheHosts = new List<CacheHost>(results.Count);
            foreach (var result in results)
            {
                HostInfo hostInfo = result.BaseObject as HostInfo;
                if (hostInfo != null)
                {
                    cacheHosts.Add(hostInfo.ToCacheHost());
                }
            }

            return cacheHosts;
        }
    }
}
