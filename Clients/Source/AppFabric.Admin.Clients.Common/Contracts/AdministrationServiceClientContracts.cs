// -----------------------------------------------------------------------
// <copyright file="AdministrationServiceClientContracts.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Clients.Common.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IAdministrationServiceClient))]
    internal abstract class AdministrationServiceClientContracts : IAdministrationServiceClient
    {
        /// <inheritdoc />
        public abstract Data.Cluster GetCacheCluster();

        /// <inheritdoc />
        public abstract Data.Cluster StartCacheCluster();

        /// <inheritdoc />
        public abstract Data.Cluster StopCacheCluster();

        /// <inheritdoc />
        public abstract Data.Cluster RestartCacheCluster();

        /// <inheritdoc />
        public Data.CacheHost StartCacheHost(Data.CacheHost host)
        {
            Contract.Requires<ArgumentNullException>(host != null);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Data.CacheHost StopCacheHost(Data.CacheHost host)
        {
            Contract.Requires<ArgumentNullException>(host != null);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Data.CacheHost RestartCacheHost(Data.CacheHost host)
        {
            Contract.Requires<ArgumentNullException>(host != null);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public abstract Data.ClusterHealth GetCacheClusterHealth();

        /// <inheritdoc />
        public ICollection<Data.DataCacheItem> SearchCacheItems(ICollection<Data.SearchItemRequest> itemsToSearch)
        {
            Contract.Requires<ArgumentNullException>(itemsToSearch != null);
            Contract.Requires<ArgumentException>(itemsToSearch.Count > 0);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ICollection<Data.DataCacheItem> RemoveCacheItems(ICollection<Data.DataCacheItem> itemsToRemove)
        {
            Contract.Requires<ArgumentNullException>(itemsToRemove != null);
            Contract.Requires<ArgumentException>(itemsToRemove.Count > 0);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ICollection<Data.CacheStatistics> GetCachesStatistics(ICollection<Data.NamedCache> namedCaches)
        {
            Contract.Requires<ArgumentNullException>(namedCaches != null);
            Contract.Requires<ArgumentException>(namedCaches.Count > 0);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ICollection<Data.Statistics> GetHostsStatistics(ICollection<Data.CacheHost> hosts)
        {
            Contract.Requires<ArgumentNullException>(hosts != null);
            Contract.Requires<ArgumentException>(hosts.Count > 0);
            throw new NotImplementedException();
        }
    }
}
