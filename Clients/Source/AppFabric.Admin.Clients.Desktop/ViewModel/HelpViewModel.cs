// -----------------------------------------------------------------------
// <copyright file="HelpViewModel.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Clients.Desktop.ViewModel
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class HelpViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpViewModel" /> class.
        /// </summary>
        public HelpViewModel()
        {
            if (this.IsInDesignMode)
            {
                this.ProductVersion = "1.0.0.0";
            }
            else
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                if (assembly != null)
                {
                    AssemblyName nameAssembly = assembly.GetName();
                    if (nameAssembly != null)
                    {
                        this.ProductVersion = nameAssembly.Version.ToString();
                    }
                }
            }

            this.OpenTrackerCommand = new RelayCommand(() => OpenUriAsync("http://appcacheadminservice.codeplex.com/workitem/list/basic"));
            this.OpenForumCommand = new RelayCommand(() => OpenUriAsync("http://appcacheadminservice.codeplex.com/discussions"));
            this.OpenDocumentationCommand = new RelayCommand(() => OpenUriAsync("http://appcacheadminservice.codeplex.com/documentation"));
            this.OpenHomeCommand = new RelayCommand(() => OpenUriAsync("http://appcacheadminservice.codeplex.com/"));
        }

        /// <summary>
        /// Gets the product version.
        /// </summary>
        /// <value>
        /// The product version.
        /// </value>
        public string ProductVersion { get; private set; }

        /// <summary>
        /// Gets the open tracker command.
        /// </summary>
        /// <value>The command that open the link to the bug tracker.</value>
        public ICommand OpenTrackerCommand { get; private set; }

        /// <summary>
        /// Gets the open forum command.
        /// </summary>
        /// <value>The command that open the link to the codeplex forum.</value>
        public ICommand OpenForumCommand { get; private set; }

        /// <summary>
        /// Gets the open license command.
        /// </summary>
        /// <value>The command that open the link to the codeplex license of the project.</value>
        public ICommand OpenHomeCommand { get; private set; }

        /// <summary>
        /// Gets the open documentation command.
        /// </summary>
        /// <value>The command that open the link to the project's documentation.</value>
        public ICommand OpenDocumentationCommand { get; private set; }

        /// <summary>
        /// Opens the URI.
        /// </summary>
        /// <param name="uriToOpen">The URI to open.</param>
        private static async void OpenUriAsync(string uriToOpen)
        {
            await Task.Factory.StartNew(() => Process.Start(uriToOpen));
        }
    }
}
