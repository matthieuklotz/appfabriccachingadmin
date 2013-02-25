// -----------------------------------------------------------------------
// <copyright file="ICachingSearchBusiness.cs" company="Matthieu Klotz">
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

    /// <summary>
    /// Defines methods for searching items in the cache cluster.
    /// </summary>
    [ContractClass(typeof(Contracts.CachingSearchBusinessContracts))]
    public interface ICachingSearchBusiness
    {
        /// <summary>
        /// Searches cache items in a specified named cache, optionally in a region, and that where cache key respect the specified pattern.
        /// </summary>
        /// <param name="cacheKeyPattern">The cache key pattern.</param>
        /// <param name="namedCache">The named cache.</param>
        /// <param name="region">The region.</param>
        /// <returns>Found cache keys.</returns>
        IList<string> SearchCacheItems(string cacheKeyPattern, string namedCache, string region);

        /// <summary>
        /// Removes a cache item.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="namedCache">The named cache.</param>
        /// <returns><c>true</c> if the cache item is removed, <c>false</c> otherwise.</returns>
        bool RemoveCacheItem(string cacheKey, string namedCache);
    }
}
