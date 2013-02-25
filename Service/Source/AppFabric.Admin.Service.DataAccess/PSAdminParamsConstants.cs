// -----------------------------------------------------------------------
// <copyright file="PSAdminParamsConstants.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Service.DataAccess
{
    /// <summary>
    /// Contains common constants for PowerShell command parameters.
    /// </summary>
    internal static class PSAdminParamsConstants
    {
        /// <summary>
        /// HostName parameter.
        /// </summary>
        internal const string HostName = "HostName";

        /// <summary>
        /// CachePort parameter.
        /// </summary>
        internal const string PortNo = "CachePort";

        /// <summary>
        /// Account parameter.
        /// </summary>
        internal const string ClientAccount = "Account";

        /// <summary>
        /// Force parameter.
        /// </summary>
        internal const string ForceCommand = "Force";

        /// <summary>
        /// Minimum cache port number.
        /// </summary>
        internal const int MinCachePortNumber = 1024;

        /// <summary>
        /// Minimum cache port number.
        /// </summary>
        internal const int DefaultCachePort = 22233;

        /// <summary>
        /// File parameter.
        /// </summary>
        internal const string FilePath = "File";

        /// <summary>
        /// ArbitratorPortNumber parameter.
        /// </summary>
        internal const string ArbitrationPortNumber = "ArbitrationPortNumber";

        /// <summary>
        /// CacheSize parameter.
        /// </summary>
        internal const string CacheSize = "CacheSize";

        /// <summary>
        /// ClusterPortNumber parameter.
        /// </summary>
        internal const string ClusterPortNumber = "ClusterPortNumber";

        /// <summary>
        /// HighWatermark parameter.
        /// </summary>
        internal const string HighWatermark = "HighWatermark";

        /// <summary>
        /// LowWatermark parameter.
        /// </summary>
        internal const string LowWatermark = "LowWatermark";

        /// <summary>
        /// ReplicationPortNumber parameter.
        /// </summary>
        internal const string ReplicationPortNumber = "ReplicationPortNumber";

        /// <summary>
        /// CacheName parameter.
        /// </summary>
        internal const string CacheName = "CacheName";

        /// <summary>
        /// Eviction parameter.
        /// </summary>
        internal const string Eviction = "Eviction";

        /// <summary>
        /// TimeToLive parameter.
        /// </summary>
        internal const string TimeToLive = "TimeToLive";

        /// <summary>
        /// Expirable parameter.
        /// </summary>
        internal const string Expirable = "Expirable";

        /// <summary>
        /// Secondaries parameter.
        /// </summary>
        internal const string Secondaries = "Secondaries";

        /// <summary>
        /// NotificationsEnabled parameter.
        /// </summary>
        internal const string NotificationsEnabled = "NotificationsEnabled";

        /// <summary>
        /// HostTimeout parameter.
        /// </summary>
        internal const string HostTimeout = "HostTimeout";

        /// <summary>
        /// Provider parameter.
        /// </summary>
        internal const string Provider = "Provider";

        /// <summary>
        /// ConnectionString parameter.
        /// </summary>
        internal const string ConnectionString = "ConnectionString";
    }
}
