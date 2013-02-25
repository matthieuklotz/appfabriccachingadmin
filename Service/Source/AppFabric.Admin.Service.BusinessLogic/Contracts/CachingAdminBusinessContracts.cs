// -----------------------------------------------------------------------
// <copyright file="CachingAdminBusinessContracts.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Service.BusinessLogic.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using AppFabric.Admin.Service.BusinessEntities;
    using AppFabric.Admin.Service.BusinessLogic.Interfaces;

    /// <summary>
    /// Code contract for <see cref="ICachingAdminBusiness"/>.
    /// </summary>
    [ContractClassFor(typeof(ICachingAdminBusiness))]
    internal abstract class CachingAdminBusinessContracts : ICachingAdminBusiness
    {
        /// <inheritdoc />
        public abstract IList<CacheHost> GetCacheCluster();

        /// <inheritdoc />
        public abstract IList<CacheHost> StartCacheCluster();

        /// <inheritdoc />
        public abstract IList<CacheHost> StopCacheCluster();

        /// <inheritdoc />
        public abstract IList<CacheHost> RestartCacheCluster();

        /// <inheritdoc />
        public abstract IList<string> GetCacheAllowedClientAccounts();

        /// <inheritdoc />
        public IList<string> GrantCacheAllowedClientAccounts(IList<string> clientAccounts)
        {
            Contract.Requires<ArgumentException>(clientAccounts != null && clientAccounts.Count > 0, "clientAccounts");
            Contract.Requires<ArgumentException>(Contract.ForAll<string>(clientAccounts, clientAccount => !string.IsNullOrWhiteSpace(clientAccount)), Resources.ErrorMessages.InvalidClientAccounts);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IList<string> RevokeCacheAllowedClientAccounts(IList<string> clientAccounts)
        {
            Contract.Requires<ArgumentException>(clientAccounts != null && clientAccounts.Count > 0, "clientAccounts");
            Contract.Requires<ArgumentException>(Contract.ForAll<string>(clientAccounts, clientAccount => !string.IsNullOrWhiteSpace(clientAccount)), Resources.ErrorMessages.InvalidClientAccounts);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void ExportCacheClusterConfig(string filePath)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(filePath), Resources.ErrorMessages.InvalidFilePath);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void ImportCacheClusterConfig(string filePath)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(filePath), Resources.ErrorMessages.InvalidFilePath);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CacheHostConfiguration GetCacheHostConfig(CacheHost host)
        {
            Contract.Requires<ArgumentNullException>(host != null, "host");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(host.HostName), Resources.ErrorMessages.InvalidHostName);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CacheHostConfiguration SetCacheHostConfig(CacheHostConfiguration cacheHostConfiguration)
        {
            Contract.Requires<ArgumentNullException>(cacheHostConfiguration != null, "cacheHostConfiguration");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(cacheHostConfiguration.HostName), Resources.ErrorMessages.InvalidHostName);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CacheHost StartCacheHost(CacheHost host)
        {
            Contract.Requires<ArgumentNullException>(host != null, "host");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(host.HostName), Resources.ErrorMessages.InvalidHostName);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CacheHost StopCacheHost(CacheHost host)
        {
            Contract.Requires<ArgumentNullException>(host != null, "host");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(host.HostName), Resources.ErrorMessages.InvalidHostName);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CacheHost RestartCacheHost(CacheHost host)
        {
            Contract.Requires<ArgumentNullException>(host != null, "host");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(host.HostName), Resources.ErrorMessages.InvalidHostName);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public NamedCache NewCache(CacheConfiguration cacheConfiguration)
        {
            Contract.Requires<ArgumentNullException>(cacheConfiguration != null, "cacheConfiguration");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(cacheConfiguration.CacheName), Resources.ErrorMessages.InvalidCacheName);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void RemoveCache(string cacheName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(cacheName), Resources.ErrorMessages.InvalidCacheName);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CacheConfiguration GetCacheConfig(string cacheName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(cacheName), Resources.ErrorMessages.InvalidCacheName);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public NamedCache SetCacheConfig(CacheConfiguration cacheConfiguration)
        {
            Contract.Requires<ArgumentNullException>(cacheConfiguration != null, "cacheConfiguration");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(cacheConfiguration.CacheName), Resources.ErrorMessages.InvalidCacheName);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public abstract IList<NamedCache> GetNamedCaches();
    }
}
