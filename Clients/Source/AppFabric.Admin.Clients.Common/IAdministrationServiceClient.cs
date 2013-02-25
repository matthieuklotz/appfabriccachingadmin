// -----------------------------------------------------------------------
// <copyright file="IAdministrationServiceClient.cs" company="Matthieu Klotz">
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
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using AppFabric.Admin.Clients.Common.Data;

    [ContractClass(typeof(Contracts.AdministrationServiceClientContracts))]
    public interface IAdministrationServiceClient
    {
        Cluster GetCacheCluster();

        Cluster StartCacheCluster();

        Cluster StopCacheCluster();

        Cluster RestartCacheCluster();

        CacheHost StartCacheHost(CacheHost host);

        CacheHost StopCacheHost(CacheHost host);

        CacheHost RestartCacheHost(CacheHost host);

        ICollection<Data.DataCacheItem> SearchCacheItems(ICollection<Data.SearchItemRequest> itemsToSearch);

        ICollection<Data.DataCacheItem> RemoveCacheItems(ICollection<Data.DataCacheItem> itemsToRemove);

        ClusterHealth GetCacheClusterHealth();

        ICollection<Data.CacheStatistics> GetCachesStatistics(ICollection<Data.NamedCache> namedCaches);

        ICollection<Data.Statistics> GetHostsStatistics(ICollection<Data.CacheHost> hosts);
    }
}
