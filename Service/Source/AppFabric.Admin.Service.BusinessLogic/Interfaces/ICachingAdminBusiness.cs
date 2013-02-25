// -----------------------------------------------------------------------
// <copyright file="ICachingAdminBusiness.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Service.BusinessLogic.Interfaces
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using AppFabric.Admin.Service.BusinessEntities;

    /// <summary>
    /// Defines methods to administrate AppFabric Caching Service.
    /// </summary>
    [ContractClass(typeof(Contracts.CachingAdminBusinessContracts))]
    public interface ICachingAdminBusiness
    {
        /// <summary>
        /// Gets the cache cluster.
        /// </summary>
        /// <returns>Cache Hosts present in the cluster.</returns>
        IList<CacheHost> GetCacheCluster();

        /// <summary>
        /// Starts the cache cluster.
        /// </summary>
        /// <returns>Cache Hosts present in the cluster.</returns>
        IList<CacheHost> StartCacheCluster();

        /// <summary>
        /// Stops the cache cluster.
        /// </summary>
        /// <returns>Cache Hosts present in the cluster.</returns>
        IList<CacheHost> StopCacheCluster();

        /// <summary>
        /// Restarts the cache cluster.
        /// </summary>
        /// <returns>Cache Hosts present in the cluster.</returns>
        IList<CacheHost> RestartCacheCluster();

        /// <summary>
        /// Gets the cache allowed client accounts.
        /// </summary>
        /// <returns>Allowed client account.</returns>
        IList<string> GetCacheAllowedClientAccounts();

        /// <summary>
        /// Grants clients account to access to the cache.
        /// </summary>
        /// <param name="clientAccounts">The client accounts.</param>
        /// <returns>Allowed client account.</returns>
        IList<string> GrantCacheAllowedClientAccounts(IList<string> clientAccounts);

        /// <summary>
        /// Revokes access to clients.
        /// </summary>
        /// <param name="clientAccounts">The client accounts.</param>
        /// <returns>Allowed client account.</returns>
        IList<string> RevokeCacheAllowedClientAccounts(IList<string> clientAccounts);

        /// <summary>
        /// Exports the cache cluster configuration.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        void ExportCacheClusterConfig(string filePath);

        /// <summary>
        /// Imports the cache cluster configuration.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        void ImportCacheClusterConfig(string filePath);

        /// <summary>
        /// Gets the configuration details of the specified Cache Host.
        /// </summary>
        /// <param name="host">The cache host.</param>
        /// <returns>Cache Host configuration.</returns>
        CacheHostConfiguration GetCacheHostConfig(CacheHost host);

        /// <summary>
        /// Updates the configuration settings for a Cache host to the specified values.
        /// </summary>
        /// <param name="cacheHostConfiguration">The cache host configuration.</param>
        /// <returns>The new cache host configuration.</returns>
        CacheHostConfiguration SetCacheHostConfig(CacheHostConfiguration cacheHostConfiguration);

        /// <summary>
        /// Starts the cache host.
        /// </summary>
        /// <param name="host">The cache host to start.</param>
        /// <returns>The started cache host.</returns>
        CacheHost StartCacheHost(CacheHost host);

        /// <summary>
        /// Stops the cache host.
        /// </summary>
        /// <param name="host">The cache host to start.</param>
        /// <returns>The stopped cache host.</returns>
        CacheHost StopCacheHost(CacheHost host);

        /// <summary>
        /// Restarts the cache host.
        /// </summary>
        /// <param name="host">The cache host to start.</param>
        /// <returns>The restarted cache host.</returns>
        CacheHost RestartCacheHost(CacheHost host);

        /// <summary>
        /// Creates a new named cache when the cluster is running.
        /// </summary>
        /// <param name="cacheConfiguration">The cache configuration.</param>
        /// <returns>New cache created.</returns>
        NamedCache NewCache(CacheConfiguration cacheConfiguration);

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
        /// <returns>The modified named cache.</returns>
        NamedCache SetCacheConfig(CacheConfiguration cacheConfiguration);

        /// <summary>
        /// Gets all named caches.
        /// </summary>
        /// <returns>Named Caches.</returns>
        IList<NamedCache> GetNamedCaches();
    }
}
