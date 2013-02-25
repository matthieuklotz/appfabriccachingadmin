// -----------------------------------------------------------------------
// <copyright file="ICachingAdminDataRepository.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Service.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using AppFabric.Admin.Service.DataAccess.Datas;

    /// <summary>
    /// Defines methods to administrate AppFabric Caching Service.
    /// </summary>
    [ContractClass(typeof(Contracts.CachingAdminDataRepositoryContracts))]
    public interface ICachingAdminDataRepository
    {
        /// <summary>
        /// Starts the cache cluster.
        /// </summary>
        /// <returns>Cache Hosts which are registred in current the cluster.</returns>
        IList<CacheHost> StartCacheCluster();

        /// <summary>
        /// Stops the cache cluster.
        /// </summary>
        /// <returns>Cache Hosts which are registred in current the cluster.</returns>
        IList<CacheHost> StopCacheCluster();

        /// <summary>
        /// Restarts the cache cluster.
        /// </summary>
        /// <returns>Cache Hosts which are registred in current the cluster.</returns>
        IList<CacheHost> RestartCacheCluster();

        /// <summary>
        /// Gets statistics on the health of the cache cluster.
        /// </summary>
        /// <returns>Statistics on the health of the cache cluster.</returns>
        CacheClusterHealth GetCacheClusterHealth();

        /// <summary>
        /// Gets allowed client accounts.
        /// </summary>
        /// <returns>Allowed client accounts.</returns>
        IList<string> GetCacheAllowedClientAccounts();

        /// <summary>
        /// Grants a Windows account access to the cache cluster.
        /// </summary>
        /// <param name="clientAccount">The Windows account to grant access to the cache cluster.</param>
        /// <param name="force">If set to <c>true</c> adds the specified client account without validating that it exists.</param>
        void GrantCacheAllowedClientAccount(string clientAccount, bool force);

        /// <summary>
        /// Revokes a Windows account access to the cache cluster.
        /// </summary>
        /// <param name="clientAccount">The Windows account to revoke access to the cache cluster.</param>
        void RevokeCacheAllowedClientAccount(string clientAccount);

        /// <summary>
        /// Exports the cache cluster configuration.
        /// </summary>
        /// <param name="fileName">Fully qualified name of the XML-based configuration file to create.</param>
        void ExportCacheClusterConfig(string fileName);

        /// <summary>
        /// Imports a cache cluster configuration.
        /// </summary>
        /// <param name="fileName">The fully qualified path and name of the XML-based configuration file that describes the cache cluster configuration settings to be applied to the cluster.</param>
        void ImportCacheClusterConfig(string fileName);

        /// <summary>
        /// Gets informations about the specified cache host.
        /// If <paramref name="hostName"/> is null, this method will return all cache hosts registred in the cluster.
        /// </summary>
        /// <param name="hostName">The name of the cache host.</param>
        /// <param name="portNo">The cache port number of the cache host.</param>
        /// <returns>Cache Host informations.</returns>
        IList<CacheHost> GetCacheHost(string hostName, int portNo);

        /// <summary>
        /// Gets the configuration details of the specified Cache Host.
        /// </summary>
        /// <param name="hostName">The name of the cache host.</param>
        /// <param name="portNo">The cache port number of the cache host.</param>
        /// <returns>Cache Host configuration.</returns>
        CacheHostConfiguration GetCacheHostConfig(string hostName, int portNo);

        /// <summary>
        /// Updates the configuration settings for a Cache host to the specified values.
        /// </summary>
        /// <param name="cacheHostConfiguration">The cache host configuration.</param>
        void SetCacheHostConfig(CacheHostConfiguration cacheHostConfiguration);

        /// <summary>
        /// Starts the cache host.
        /// </summary>
        /// <param name="hostName">The name of the cache host.</param>
        /// <param name="portNo">The cache port number of the cache host.</param>
        /// <param name="hostTimeout">Timeout value in seconds for the cache host to start. By default, this is 60 seconds.</param>
        /// <returns>The started cache host.</returns>
        CacheHost StartCacheHost(string hostName, int portNo, int? hostTimeout);

        /// <summary>
        /// Stops the cache host.
        /// </summary>
        /// <param name="hostName">The name of the cache host.</param>
        /// <param name="portNo">The cache port number of the cache host.</param>
        /// <param name="hostTimeout">Timeout value in seconds for the cache host to stop. By default, this is 60 seconds.</param>
        /// <returns>The stopped cache host.</returns>
        CacheHost StopCacheHost(string hostName, int portNo, int? hostTimeout);

        /// <summary>
        /// Restarts the cache host.
        /// </summary>
        /// <param name="hostName">The name of the cache host.</param>
        /// <param name="portNo">The cache port number of the cache host.</param>
        /// <param name="hostTimeout">Timeout value in seconds for the cache host to restart. By default, this is 60 seconds.</param>
        /// <returns>The restarted cache host.</returns>
        CacheHost RestartCacheHost(string hostName, int portNo, int? hostTimeout);

        /// <summary>
        /// Gets all caches for a specified cache host. If <paramref name="hostName"/> is null. It will return all caches presents in the cluster.
        /// </summary>
        /// <param name="hostName">Name of the cache host.</param>
        /// <param name="portNo">The cache port.</param>
        /// <returns>Collection of Named Cache.</returns>
        IList<NamedCache> GetCache(string hostName, int? portNo);

        /// <summary>
        /// Creates a new named cache when the cluster is running.
        /// </summary>
        /// <param name="cacheConfiguration">The cache configuration.</param>
        void NewCache(CacheConfiguration cacheConfiguration);

        /// <summary>
        /// Removes the named cache specified.
        /// </summary>
        /// <param name="cacheName">Name of the cache.</param>
        void RemoveCache(string cacheName);

        /// <summary>
        /// Gets the configuration details of the specified Cache.
        /// </summary>
        /// <param name="cacheName">Name of the cache.</param>
        /// <returns>The cache configuration.</returns>
        CacheConfiguration GetCacheConfig(string cacheName);

        /// <summary>
        /// Updates the configuration settings for a cache.
        /// </summary>
        /// <param name="cacheConfiguration">The cache configuration.</param>
        void SetCacheConfig(CacheConfiguration cacheConfiguration);

        /// <summary>
        /// Gets the cache regions that are defined in a specified named cache.
        /// </summary>
        /// <param name="cacheName">Name of the cache.</param>
        /// <returns>Regions present in the cache.</returns>
        IList<Region> GetCacheRegions(string cacheName);
    }
}
