﻿// -----------------------------------------------------------------------
// <copyright file="INamedCacheConfigurationService.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Service.ServiceContracts.Configuration
{
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using AppFabric.Admin.Service.MessageContracts;
    using AppFabric.Admin.Service.MessageContracts.Configuration;
    using AppFabric.Admin.Service.ServiceContracts.Contracts;

    /// <summary>
    /// Defines methods for configure named cache.
    /// </summary>
    [ServiceContract(Namespace = "http://appcacheadminservice.codeplex.com/Services/Configuration")]
    [ContractClass(typeof(NamedCacheConfigurationServiceContracts))]
    public interface INamedCacheConfigurationService
    {
        /// <summary>
        /// Creates a new named cache when the cluster is running.
        /// </summary>
        /// <param name="cacheConfigurationMessage">The cache configuration.</param>
        /// <returns>New cache created.</returns>
        [OperationContract]
        NamedCacheMessage NewCache(CacheConfigurationMessage cacheConfigurationMessage);

        /// <summary>
        /// Removes the specified named caches.
        /// </summary>
        /// <param name="cacheNameMessage">The Named cache to remove.</param>
        [OperationContract]
        void RemoveCaches(NamedCacheMessage cacheNameMessage);

        /// <summary>
        /// Gets the configuration details of the specified Cache.
        /// </summary>
        /// <param name="cacheNameMessage">Named cache to retrieve configuration.</param>
        /// <returns>The cache configuration.</returns>
        [OperationContract]
        CacheConfigurationMessage GetCacheConfig(NamedCacheMessage cacheNameMessage);

        /// <summary>
        /// Updates the configuration settings for a cache.
        /// </summary>
        /// <param name="cacheConfigurationMessage">The cache configuration.</param>
        /// <returns>The modified named cache.</returns>
        [OperationContract]
        NamedCacheMessage SetCacheConfig(CacheConfigurationMessage cacheConfigurationMessage);
    }
}
