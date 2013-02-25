// -----------------------------------------------------------------------
// <copyright file="CompositeTypeFormatter.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Common.Formatters
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;
    using AppFabric.Admin.Common.Injection;
    using AppFabric.Admin.Common.Logging;

    /// <summary>
    /// Formatter for all registered <see cref="ICompositeTypeFormatter"/>.
    /// </summary>
    public class CompositeTypeFormatter : IFormatProvider, ICustomFormatter
    {
        /// <summary>
        /// Global instance for current Application Domain.
        /// </summary>
        private static Lazy<CompositeTypeFormatter> current = new Lazy<CompositeTypeFormatter>(CreateInstance, LazyThreadSafetyMode.None);

        /// <summary>
        /// Found formatters by MEF.
        /// </summary>
        private IEnumerable<Lazy<ICustomFormatter, ICompositeTypeFormatter>> registredFormatters;

        /// <summary>
        /// Declared formatters.
        /// </summary>
        private ConcurrentDictionary<Type, ICustomFormatter> formatters;

        /// <summary>
        /// Gets the current format provider.
        /// </summary>
        /// <value>The current format provider.</value>
        public static IFormatProvider Current
        {
            get
            {
                return current.Value;
            }
        }

        /// <inheritdoc />
        public object GetFormat(Type formatType)
        {
            return typeof(ICustomFormatter).IsAssignableFrom(formatType) ? this : null;
        }

        /// <inheritdoc />
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            string formattingObject = null;
            ICustomFormatter formatter = this.formatters.GetOrAdd(arg.GetType(), this.GetFormatter);
            if (formatter != null)
            {
                formattingObject = formatter.Format(format, arg, formatProvider);
            }

            if (formattingObject == null && format != null)
            {
                formattingObject = string.Format(CultureInfo.InvariantCulture, format, arg);
            }

            return formattingObject == null ? string.Empty : formattingObject;
        }

        /// <summary>
        /// Creates an instance of <see cref="CompositeTypeFormatter"/>.
        /// </summary>
        /// <returns>An instance of <see cref="CompositeTypeFormatter"/>.</returns>
        private static CompositeTypeFormatter CreateInstance()
        {
            CompositeTypeFormatter compositeFormatter = new CompositeTypeFormatter();
            compositeFormatter.formatters = new ConcurrentDictionary<Type, ICustomFormatter>();
            try
            {
                compositeFormatter.registredFormatters = CompositionManager.Container.GetExports<ICustomFormatter, ICompositeTypeFormatter>();
            }
            catch (ReflectionTypeLoadException ex)
            {
                Logger.Error(string.Empty, string.Empty, ex);
                foreach (Exception inEx in ex.LoaderExceptions)
                {
                    Logger.Error(string.Empty, string.Empty, inEx);
                }
            }

            return compositeFormatter;
        }

        /// <summary>
        /// Gets the formatter for the specified type.
        /// </summary>
        /// <param name="type">The type to search.</param>
        /// <returns>The formatter for the specified type. Null if not found.</returns>
        private ICustomFormatter GetFormatter(Type type)
        {
            Contract.Requires(this.registredFormatters != null);
            ICustomFormatter formatter = null;
            foreach (Lazy<ICustomFormatter, ICompositeTypeFormatter> registredFormatter in this.registredFormatters)
            {
                if (registredFormatter != null)
                {
                    ICompositeTypeFormatter metadata = registredFormatter.Metadata;
                    if (metadata != null)
                    {
                        Type formatType = metadata.CanFormatType;
                        if (formatType.IsAssignableFrom(type))
                        {
                            formatter = registredFormatter.Value;
                            break;
                        }
                    }
                }
            }

            return formatter;
        }
    }
}
