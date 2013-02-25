// -----------------------------------------------------------------------
// <copyright file="Logger.cs" company="Matthieu Klotz">
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
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using NLog;
    using NLogger = NLog.Logger;

    /// <summary>
    /// Log events, exceptions, or messages.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Contains all loggers for current application instance.
        /// </summary>
        private static ConcurrentDictionary<string, NLogger> loggers = new ConcurrentDictionary<string, NLogger>();

        /// <summary>
        /// Logs debug messages in the specified logging category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        [Conditional("DEBUG")]
        public static void Debug(string category, IFormatProvider formatProvider, string format, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(format != null, "format");
            Contract.Requires<ArgumentNullException>(formatProvider != null, "formatProvider");

            NLogger logger = GetLogger(category);
            if (logger != null)
            {
                logger.Debug(formatProvider, format, args);
            }
        }

        /// <summary>
        /// Logs warning messages in the specified logging category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void Warn(string category, IFormatProvider formatProvider, string format, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(format != null, "format");
            Contract.Requires<ArgumentNullException>(formatProvider != null, "formatProvider");

            NLogger logger = GetLogger(category);
            if (logger != null)
            {
                logger.Warn(formatProvider, format, args);
            }
        }

        /// <summary>
        /// Logs error messages in the specified logging category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void Error(string category, IFormatProvider formatProvider, string format, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(format != null, "format");
            Contract.Requires<ArgumentNullException>(formatProvider != null, "formatProvider");

            NLogger logger = GetLogger(category);
            if (logger != null)
            {
                logger.Error(formatProvider, format, args);
            }
        }

        /// <summary>
        /// Logs error messages and exception in the specified logging category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Error(string category, string message, Exception exception)
        {
            Contract.Requires<ArgumentNullException>(message != null, "message");
            Contract.Requires<ArgumentNullException>(exception != null, "exception");

            NLogger logger = GetLogger(category);
            if (logger != null)
            {
                logger.ErrorException(message, exception);
            }
        }

        /// <summary>
        /// Logs fatal error messages and exception in the specified logging category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Fatal(string category, string message, Exception exception)
        {
            Contract.Requires<ArgumentNullException>(message != null, "message");
            Contract.Requires<ArgumentNullException>(exception != null, "exception");

            NLogger logger = GetLogger(category);
            if (logger != null)
            {
                logger.FatalException(message, exception);
            }
        }

        /// <summary>
        /// Gets the logger with the specified <paramref name="loggerName"/>.
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        /// <returns>The logger instance, default logger if the specified logger does not exist.</returns>
        private static NLogger GetLogger(string loggerName)
        {
            return loggers.GetOrAdd(loggerName, LogManager.GetLogger);
        }
    }
}
