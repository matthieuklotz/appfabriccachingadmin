// -----------------------------------------------------------------------
// <copyright file="CacheConfiguration.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Service.DataAccess.Datas
{
    /// <summary>
    /// Represents the configuration of a named cache.
    /// </summary>
    public class CacheConfiguration
    {
        /// <summary>
        /// Gets or sets the name of the cache.
        /// </summary>
        /// <value>
        /// The name of the cache.
        /// </value>
        public string CacheName { get; set; }

        /// <summary>
        /// Gets or sets the objects time to live.
        /// </summary>
        /// <value>
        /// The objects time to live.
        /// </value>
        public long TimeToLive { get; set; }

        /// <summary>
        /// Gets or sets the type of the cache.
        /// </summary>
        /// <value>
        /// The type of the cache.
        /// </value>
        public string CacheType { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates if high availability feature is enable or not.
        /// </summary>
        /// <value>
        /// 1 if high availability is enable, 0 if not.
        /// </value>
        public int Secondaries { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this cache is expirable.
        /// </summary>
        /// <value><c>true</c> if this instance is expirable; otherwise, <c>false</c>.</value>
        public bool IsExpirable { get; set; }

        /// <summary>
        /// Gets or sets the type of the eviction.
        /// </summary>
        /// <value>
        /// The type of the eviction.
        /// </value>
        public EvictionType EvictionType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether notifications are enabled or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if notifications are enabled; otherwise, <c>false</c>.
        /// </value>
        public bool NotificationsEnabled { get; set; }
    }
}
