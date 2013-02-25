// -----------------------------------------------------------------------
// <copyright file="UnHandledErrorHandler.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Common.Logging
{
    using System;
    using System.Globalization;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using WcfContrib.Errors;
    using WcfContrib.Extensions;

    /// <summary>
    /// A WCF Error handler that handle exceptions if the service doesn't handle it.
    /// </summary>
    [CLSCompliant(false)]
    public class UnhandledErrorHandler : IErrorHandler, IErrorContextHandler
    {
        /// <inheritdoc />
        public MessageAccessMode RequiredMessageAccess
        {
            get
            {
                return MessageAccessMode.Read;
            }
        }

        /// <inheritdoc />
        public bool HandleError(Exception error)
        {
            return true;
        }

        /// <inheritdoc />
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
        }

        /// <inheritdoc />
        public void HandleError(Exception error, ActivationErrorContext errorContext)
        {
            if (error == null)
            {
                return;
            }

            string message = null;
            if (!string.IsNullOrWhiteSpace(errorContext.Action))
            {
                message = string.Format(CultureInfo.InvariantCulture, Resources.ErrorMessages.UnhandledWcfWithActionError, errorContext.ServiceType, errorContext.Action);
            }
            else
            {
                message = string.Format(CultureInfo.InvariantCulture, Resources.ErrorMessages.UnhandledWcfError, errorContext.ServiceType);
            }

            Logger.Error("ServiceUnHandledError", message, error);
        }
    }
}
