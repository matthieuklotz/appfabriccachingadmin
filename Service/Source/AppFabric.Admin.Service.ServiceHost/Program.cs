// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Service.ServiceHost
{
    using System;
    using System.ComponentModel.Composition.Hosting;
    using System.ServiceProcess;
    using AppFabric.Admin.Common.Injection;
    using AppFabric.Admin.Common.Logging;
    using AppFabric.Admin.Service.ServiceImplementation;

    /// <summary>
    /// Program entry.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        internal static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;
            CompositionManager.SetContainer(GetContainer);
            ServiceBase[] servicesToRun = new ServiceBase[] 
            { 
                new AppFabricCachingBaseService(Constants.AdministrationServiceName, typeof(CachingAdministrationService)),
            };

            ServiceBase.Run(servicesToRun);
        }

        /// <summary>
        /// Initializes the composition container.
        /// </summary>
        /// <returns>A new instance of <see cref="CompositionContainer"/>.</returns>
        private static CompositionContainer GetContainer()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new InvalidOperationException();
            }

            DirectoryCatalog catalog = new DirectoryCatalog(directory);
            return new CompositionContainer(catalog);
        }

        /// <summary>
        /// Logs the first chance exceptions.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs"/> instance containing the event data.</param>
        private static void LogUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e != null && e.ExceptionObject != null)
            {
                Exception exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    Logger.Fatal("UnHandledException", "A un handled exception occured", exception);
                }
            }
        }
    }
}
