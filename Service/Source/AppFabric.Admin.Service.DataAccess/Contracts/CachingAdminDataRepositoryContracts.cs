// -----------------------------------------------------------------------
// <copyright file="CachingAdminDataRepositoryContracts.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <summary>This is the Contract class for ICachingAdminDataRepository.</summary>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Service.DataAccess.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using AppFabric.Admin.Service.DataAccess.Datas;
    using AppFabric.Admin.Service.DataAccess.Interfaces;

    /// <summary>
    /// Contract class for <see cref="ICachingAdminDataRepository"/>.
    /// </summary>
    [ContractClassFor(typeof(ICachingAdminDataRepository))]
    internal abstract class CachingAdminDataRepositoryContracts : ICachingAdminDataRepository
    {
        /// <inheritdoc />
        public abstract IList<CacheHost> StartCacheCluster();

        /// <inheritdoc />
        public abstract IList<CacheHost> StopCacheCluster();

        /// <inheritdoc />
        public abstract IList<CacheHost> RestartCacheCluster();

        /// <inheritdoc />
        public abstract CacheClusterHealth GetCacheClusterHealth();

        /// <inheritdoc />
        public abstract IList<string> GetCacheAllowedClientAccounts();

        /// <inheritdoc />
        public void GrantCacheAllowedClientAccount(string clientAccount, bool force)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(clientAccount), Resources.ErrorMessages.InvalidClientAccount);
        }

        /// <inheritdoc />
        public void RevokeCacheAllowedClientAccount(string clientAccount)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(clientAccount), Resources.ErrorMessages.InvalidClientAccount);
        }

        /// <inheritdoc />
        public void ExportCacheClusterConfig(string fileName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(fileName), Resources.ErrorMessages.InvalidFileName);
        }

        /// <inheritdoc />
        public void ImportCacheClusterConfig(string fileName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(fileName), Resources.ErrorMessages.InvalidFileName);
        }

        /// <inheritdoc />
        public IList<CacheHost> GetCacheHost(string hostName, int portNo)
        {
            Contract.Requires<ArgumentException>(portNo > PSAdminParamsConstants.MinCachePortNumber, Resources.ErrorMessages.InvalidCachePort);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CacheHostConfiguration GetCacheHostConfig(string hostName, int portNo)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(hostName), Resources.ErrorMessages.HostNameNullOrEmpty);
            Contract.Requires<ArgumentException>(portNo > PSAdminParamsConstants.MinCachePortNumber, Resources.ErrorMessages.InvalidCachePort);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void SetCacheHostConfig(CacheHostConfiguration cacheHostConfiguration)
        {
            Contract.Requires<ArgumentNullException>(cacheHostConfiguration != null, "cacheHostConfiguration");
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CacheHost StartCacheHost(string hostName, int portNo, int? hostTimeout)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(hostName), Resources.ErrorMessages.HostNameNullOrEmpty);
            Contract.Requires<ArgumentException>(portNo > PSAdminParamsConstants.MinCachePortNumber, Resources.ErrorMessages.InvalidCachePort);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CacheHost StopCacheHost(string hostName, int portNo, int? hostTimeout)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(hostName), Resources.ErrorMessages.HostNameNullOrEmpty);
            Contract.Requires<ArgumentException>(portNo > PSAdminParamsConstants.MinCachePortNumber, Resources.ErrorMessages.InvalidCachePort);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public CacheHost RestartCacheHost(string hostName, int portNo, int? hostTimeout)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(hostName), Resources.ErrorMessages.HostNameNullOrEmpty);
            Contract.Requires<ArgumentException>(portNo > PSAdminParamsConstants.MinCachePortNumber, Resources.ErrorMessages.InvalidCachePort);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public abstract IList<NamedCache> GetCache(string hostName, int? portNo);

        /// <inheritdoc />
        public void NewCache(CacheConfiguration cacheConfiguration)
        {
            Contract.Requires<ArgumentNullException>(cacheConfiguration != null, "cacheConfiguration");
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
        public void SetCacheConfig(CacheConfiguration cacheConfiguration)
        {
            Contract.Requires<ArgumentNullException>(cacheConfiguration != null, "cacheConfiguration");
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IList<Region> GetCacheRegions(string cacheName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(cacheName), Resources.ErrorMessages.InvalidCacheName);
            throw new NotImplementedException();
        }
    }
}
