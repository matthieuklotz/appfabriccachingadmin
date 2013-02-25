// -----------------------------------------------------------------------
// <copyright file="CacheHostViewModel.cs" company="Matthieu Klotz">
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
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using AppFabric.Admin.Clients.Common.Data;
    using GalaSoft.MvvmLight.Command;

    public class CacheHostViewModel : AdminServiceNodeViewModel, IAdminServiceViewModel
    {
        private readonly CacheHost cacheHost;

        public CacheHostViewModel(CacheHost cacheHost, ClusterViewModel parent)
            : base(parent)
        {
            Contract.Requires<ArgumentNullException>(cacheHost != null);
            this.StartHostCommand = new RelayCommand(this.StartHost);
            this.StopHostCommand = new RelayCommand(this.StopHost);
            this.RestartHostCommand = new RelayCommand(this.RestartHost);
            this.cacheHost = cacheHost;
        }

        public ICommand StartHostCommand { get; private set; }

        public ICommand StopHostCommand { get; private set; }

        public ICommand RestartHostCommand { get; private set; }

        /// <inheritdoc />
        public override string Name
        {
            get
            {
                return this.cacheHost.HostName;
            }

            set
            {
                if (!string.Equals(this.cacheHost.HostName, value, StringComparison.OrdinalIgnoreCase))
                {
                    this.RaisePropertyChanging("Name");
                    this.cacheHost.HostName = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        public CacheHost CacheHost
        {
            get
            {
                return this.cacheHost;
            }
        }

        private async void StartHost()
        {
            if (this.Service == null || this.cacheHost == null)
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show("Do you really want to perform this action ?", "Start Host", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    CacheHost host = await Task.Factory.StartNew<CacheHost>(() => this.Service.StartCacheHost(this.cacheHost));
                    this.UpdateHost(host);
                }
                catch
                {
                    MessageBox.Show("An exception was thrown during the execution of this operation");
                }
            }
        }

        private async void StopHost()
        {
            if (this.Service == null || this.cacheHost == null)
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show("Do you really want to perform this action ?", "Stop Host", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    CacheHost host = await Task.Factory.StartNew<CacheHost>(() => this.Service.StopCacheHost(this.cacheHost));
                    this.UpdateHost(host);
                }
                catch
                {
                    MessageBox.Show("An exception was thrown during the execution of this operation");
                }
            }
        }

        private async void RestartHost()
        {
            if (this.Service == null || this.cacheHost == null)
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show("Do you really want to perform this action ?", "Restart Host", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    CacheHost host = await Task.Factory.StartNew<CacheHost>(() => this.Service.RestartCacheHost(this.cacheHost));
                    this.UpdateHost(host);
                }
                catch
                {
                    MessageBox.Show("An exception was thrown during the execution of this operation");
                }
            }
        }

        private void UpdateHost(CacheHost host)
        {
            if (host == null)
            {
                return;
            }

            this.Name = host.HostName;
            this.cacheHost.PortNo = host.PortNo;
            this.cacheHost.ServiceName = host.ServiceName;
            this.cacheHost.ServiceStatus = host.ServiceStatus;
            this.cacheHost.VersionInformation = host.VersionInformation;
        }
    }
}
