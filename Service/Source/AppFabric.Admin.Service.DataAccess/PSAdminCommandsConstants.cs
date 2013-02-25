// -----------------------------------------------------------------------
// <copyright file="PSAdminCommandsConstants.cs" company="Matthieu Klotz">
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
    /// Contains command constants for powershell commands.
    /// </summary>
    internal static class PSAdminCommandsConstants
    {
        /// <summary>
        /// Use-CacheCluster command.
        /// </summary>
        internal const string UseCacheCluster = "Use-CacheCluster";

        /// <summary>
        /// Start-CacheCluster command.
        /// </summary>
        internal const string StartCacheCluster = "Start-CacheCluster";

        /// <summary>
        /// Stop-CacheCluster command.
        /// </summary>
        internal const string StopCacheCluster = "Stop-CacheCluster";

        /// <summary>
        /// Start-CacheCluster command.
        /// </summary>
        internal const string RestartCacheCluster = "Start-CacheCluster";

        /// <summary>
        /// Get-CacheClusterHealth command.
        /// </summary>
        internal const string GetCacheClusterHealth = "Get-CacheClusterHealth";

        /// <summary>
        /// Grant-CacheAllowedClientAccount command.
        /// </summary>
        internal const string GrantCacheAllowedClientAccount = "Grant-CacheAllowedClientAccount";

        /// <summary>
        /// Revoke-CacheAllowedClientAccount command.
        /// </summary>
        internal const string RevokeCacheAllowedClientAccount = "Revoke-CacheAllowedClientAccount";

        /// <summary>
        /// Get-CacheAllowedClientAccounts command.
        /// </summary>
        internal const string GetCacheAllowedClientAccount = "Get-CacheAllowedClientAccounts";

        /// <summary>
        /// Export-CacheClusterConfig command.
        /// </summary>
        internal const string ExportCacheClusterConfig = "Export-CacheClusterConfig";

        /// <summary>
        /// Import-CacheClusterConfig command.
        /// </summary>
        internal const string ImportCacheClusterConfig = "Import-CacheClusterConfig";

        /// <summary>
        /// Start-CacheHost command.
        /// </summary>
        internal const string StartCacheHost = "Start-CacheHost";

        /// <summary>
        /// Stop-CacheHost command.
        /// </summary>
        internal const string StopCacheHost = "Stop-CacheHost";

        /// <summary>
        /// Restart-CacheHost command.
        /// </summary>
        internal const string RestartCacheHost = "Restart-CacheHost";

        /// <summary>
        /// Get-CacheHost command.
        /// </summary>
        internal const string GetCacheHost = "Get-CacheHost";

        /// <summary>
        /// Get-CacheHostConfig command.
        /// </summary>
        internal const string GetCacheHostConfig = "Get-CacheHostConfig";

        /// <summary>
        /// Set-CacheHostConfig command.
        /// </summary>
        internal const string SetCacheHostConfig = "Set-CacheHostConfig";

        /// <summary>
        /// Get-Cache command.
        /// </summary>
        internal const string GetCache = "Get-Cache";

        /// <summary>
        /// Get-CacheConfig command.
        /// </summary>
        internal const string GetCacheConfig = "Get-CacheConfig";

        /// <summary>
        /// Set-CacheConfig command.
        /// </summary>
        internal const string SetCacheConfig = "Set-CacheConfig";

        /// <summary>
        /// Remove-Cache command.
        /// </summary>
        internal const string RemoveCache = "Remove-Cache";

        /// <summary>
        /// New-Cache command.
        /// </summary>
        internal const string NewCache = "New-Cache";

        /// <summary>
        /// Get-Region command.
        /// </summary>
        internal const string GetCacheRegion = "Get-CacheRegion";

        /// <summary>
        /// Get-CacheStatistics command.
        /// </summary>
        internal const string GetCacheStatistics = "Get-CacheStatistics";
    }
}
