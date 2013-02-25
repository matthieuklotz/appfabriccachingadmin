// -----------------------------------------------------------------------
// <copyright file="CompositionManager.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Common.Injection
{
    using System;
    using System.ComponentModel.Composition.Hosting;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Manage injection with MEF.
    /// </summary>
    public static class CompositionManager
    {
        /// <summary>
        /// CompositionContainer builder.
        /// </summary>
        private static Func<CompositionContainer> builder;

        /// <summary>
        /// Current composition container.
        /// </summary>
        private static CompositionContainer container;

        /// <summary>
        /// Prevents multi-threading access when building the container.
        /// </summary>
        private static object syncLockContainer = new object();

        /// <summary>
        /// Gets the current container.
        /// </summary>
        public static CompositionContainer Container
        {
            get
            {
                if (container == null)
                {
                    lock (syncLockContainer)
                    {
                        if (container == null)
                        {
                            if (builder == null)
                            {
                                throw new InvalidOperationException();
                            }

                            container = builder();
                        }
                    }
                }

                return container;
            }
        }

        /// <summary>
        /// Sets the current container.
        /// </summary>
        /// <param name="compositionContainerBuilder">The composition container builder.</param>
        public static void SetContainer(Func<CompositionContainer> compositionContainerBuilder)
        {
            Contract.Requires<ArgumentNullException>(compositionContainerBuilder != null);
            builder = compositionContainerBuilder;
        }

        /// <summary>
        /// Releases the current container.
        /// </summary>
        public static void ReleaseContainer()
        {
            if (container != null)
            {
                container.Dispose();
                container = null;
            }

            builder = null;
        }
    }
}
