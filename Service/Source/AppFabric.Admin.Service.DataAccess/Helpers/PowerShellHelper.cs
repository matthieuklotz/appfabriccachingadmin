// -----------------------------------------------------------------------
// <copyright file="PowerShellHelper.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Service.DataAccess.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using AppFabric.Admin.Common.Formatters;
    using AppFabric.Admin.Common.Logging;
    using AppFabric.Admin.Service.DataAccess.Resources;

    /// <summary>
    /// Contains static method for PowerShell execution.
    /// </summary>
    internal static class PowerShellHelper
    {
        /// <summary>
        /// Gets a PowerShell RunspacePool with specified module imported.
        /// </summary>
        /// <param name="importModules">The modules to import.</param>
        /// <returns>PowerShell RunspacePool.</returns>
        internal static RunspacePool GetRunSpacePool(string[] importModules)
        {
            InitialSessionState state = InitialSessionState.CreateDefault();
            state.ImportPSModule(importModules);
            state.ThrowOnRunspaceOpenError = true;
            RunspacePool runspacePool = RunspaceFactory.CreateRunspacePool(state);
            runspacePool.Open();
            return runspacePool;
        }

        /// <summary>
        /// Invokes a PowerShell command.
        /// </summary>
        /// <param name="runspacePool">The RunspacePool to use for command execution.</param>
        /// <param name="commandName">Command to invoke.</param>
        /// <param name="parameters">Command's parameters.</param>
        /// <returns>Command's results.</returns>
        internal static IList<PSObject> InvokeCommand(this RunspacePool runspacePool, string commandName, Dictionary<string, object> parameters = null)
        {
            Contract.Requires<ArgumentNullException>(runspacePool != null, "runspacePool");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(commandName), ErrorMessages.CommandNullOrEmpty);

            Command command = new Command(commandName);
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param.Key, param.Value);
                }
            }

            Logger.Debug(Constants.PowerShellLoggingCategory, CompositeTypeFormatter.Current, DebugMessages.ExecPSCommand, command);
            IList<PSObject> results = null;
            PowerShell powershell = null;
            try
            {
                powershell = PowerShell.Create();
                powershell.RunspacePool = runspacePool;
                powershell.Commands.AddCommand(command);
                IAsyncResult asynResult = powershell.BeginInvoke();
                results = powershell.EndInvoke(asynResult);
                LogPowerShellStreams(powershell.Streams);
            }
            finally
            {
                if (powershell != null)
                {
                    powershell.Dispose();
                    powershell = null;
                }
            }

            return results;
        }

        /// <summary>
        /// Logs the PowerShell streams.
        /// </summary>
        /// <param name="powerShellStreams">The power shell streams.</param>
        private static void LogPowerShellStreams(PSDataStreams powerShellStreams)
        {
            if (powerShellStreams == null)
            {
                return;
            }

            LogPowerShellDebugOutput(powerShellStreams.Debug);

            PSDataCollection<WarningRecord> warnings = powerShellStreams.Warning;
            if (warnings != null && warnings.Count > 0)
            {
                foreach (WarningRecord warningRecord in warnings)
                {
                    Logger.Warn(Constants.PowerShellLoggingCategory, CompositeTypeFormatter.Current, ErrorMessages.WarnPsWarnings, warningRecord);
                }
            }

            PSDataCollection<ErrorRecord> errors = powerShellStreams.Error;
            if (errors != null && errors.Count > 0)
            {
                foreach (ErrorRecord errorRecord in errors)
                {
                    Logger.Error(Constants.PowerShellLoggingCategory, CompositeTypeFormatter.Current, ErrorMessages.WarnPsErrors, errorRecord);
                }
            }
        }

        /// <summary>
        /// Logs the PowerShell debug output.
        /// </summary>
        /// <param name="debugRecords">The debug records.</param>
        [Conditional("DEBUG")]
        private static void LogPowerShellDebugOutput(PSDataCollection<DebugRecord> debugRecords)
        {
            if (debugRecords == null || debugRecords.Count < 1)
            {
                return;
            }

            foreach (DebugRecord debugRecord in debugRecords)
            {
                Logger.Debug(Constants.PowerShellLoggingCategory, CompositeTypeFormatter.Current, DebugMessages.DebugOutput, debugRecord);
            }
        }
    }
}
