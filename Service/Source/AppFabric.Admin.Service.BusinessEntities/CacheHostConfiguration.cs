// -----------------------------------------------------------------------
// <copyright file="CacheHostConfiguration.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Service.BusinessEntities
{
    /// <summary>
    /// Represents the configuration of a cache host.
    /// </summary>
    public class CacheHostConfiguration
    {
        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>
        /// The name of the host.
        /// </value>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the cluster port.
        /// </summary>
        /// <value>
        /// The cluster port.
        /// </value>
        public int ClusterPort { get; set; }

        /// <summary>
        /// Gets or sets the cache port.
        /// </summary>
        /// <value>
        /// The cache port.
        /// </value>
        public int CachePort { get; set; }

        /// <summary>
        /// Gets or sets the arbitration port.
        /// </summary>
        /// <value>
        /// The arbitration port.
        /// </value>
        public int ArbitrationPort { get; set; }

        /// <summary>
        /// Gets or sets the replication port.
        /// </summary>
        /// <value>
        /// The replication port.
        /// </value>
        public int ReplicationPort { get; set; }

        /// <summary>
        /// Gets or sets the cache host size.
        /// </summary>
        /// <value>
        /// The the cache host size.
        /// </value>
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the high watermark.
        /// </summary>
        /// <value>
        /// The high watermark.
        /// </value>
        public long HighWatermark { get; set; }

        /// <summary>
        /// Gets or sets the low watermark.
        /// </summary>
        /// <value>
        /// The low watermark.
        /// </value>
        public long LowWatermark { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this cache host is lead host.
        /// </summary>
        /// <value><c>true</c> if this cache host is lead host; otherwise, <c>false</c>.</value>
        public bool IsLeadHost { get; set; }
    }
}
