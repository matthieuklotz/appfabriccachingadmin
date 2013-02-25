// -----------------------------------------------------------------------
// <copyright file="ClusterViewModel.cs" company="Matthieu Klotz">
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
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using AppFabric.Admin.Clients.Common;
    using AppFabric.Admin.Clients.Common.Data;
    using AppFabric.Admin.Common.Injection;
    using GalaSoft.MvvmLight.Command;
    using Microsoft.Practices.Unity;

    public class ClusterViewModel : AdminServiceNodeViewModel
    {
        private readonly Cluster cluster;

        public ClusterViewModel(NodeViewModel parent, Cluster cluster)
            : base(parent)
        {
            Contract.Requires<ArgumentNullException>(cluster != null);
            this.StartClusterCommand = new RelayCommand(this.StartCluster);
            this.StopClusterCommand = new RelayCommand(this.StopCluster);
            this.RestartClusterCommand = new RelayCommand(this.RestartCluster);
            this.cluster = cluster;
            this.GetCacheCluster();
        }

        public ICommand StartClusterCommand { get; private set; }

        public ICommand StopClusterCommand { get; private set; }

        public ICommand RestartClusterCommand { get; private set; }

        public Cluster Cluster
        {
            get
            {
                return this.cluster;
            }
        }

        /// <inheritdoc />
        public override string Name
        {
            get
            {
                return this.cluster.Name;
            }

            set
            {
                if (!string.Equals(this.cluster.Name, value, StringComparison.OrdinalIgnoreCase))
                {
                    this.RaisePropertyChanging("Name");
                    this.cluster.Name = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        public Uri Uri
        {
            get
            {
                return this.cluster.Uri;
            }

            set
            {
                if (System.Uri.Compare(this.cluster.Uri, value, UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    this.RaisePropertyChanging("Uri");
                    this.cluster.Uri = value;
                    this.Service = null;
                    this.RaisePropertyChanged("Uri");
                    this.GetCacheCluster();
                }
            }
        }

        public string UserName
        {
            get
            {
                return this.cluster.UserName;
            }

            set
            {
                if (!string.Equals(this.cluster.UserName, value, StringComparison.OrdinalIgnoreCase))
                {
                    this.RaisePropertyChanging("UserName");
                    this.cluster.UserName = value;
                    this.Service = null;
                    this.RaisePropertyChanged("UserName");
                    this.GetCacheCluster();
                }
            }
        }

        public string UserPassword
        {
            get
            {
                return this.cluster.UserPassword;
            }

            set
            {
                if (!string.Equals(this.cluster.UserPassword, value, StringComparison.OrdinalIgnoreCase))
                {
                    this.RaisePropertyChanging("UserPassword");
                    this.cluster.UserPassword = value;
                    this.Service = null;
                    this.RaisePropertyChanged("UserPassword");
                    this.GetCacheCluster();
                }
            }
        }

        /// <inheritdoc />
        public override IAdministrationServiceClient InitializeService()
        {
            IAdministrationServiceClient service = null;
            if (this.Uri != null && !string.IsNullOrWhiteSpace(this.UserName) && !string.IsNullOrWhiteSpace(this.UserPassword))
            {
                ResolverOverride[] resolverOverrides = new ResolverOverride[3]
                {
                    new ParameterOverride("serviceUri", this.Uri),
                    new ParameterOverride("userName", this.UserName),
                    new ParameterOverride("password", this.UserPassword)  
                };

                service = IoCManager.Container.Resolve<IAdministrationServiceClient>(resolverOverrides);
            }

            return service;
        }

        /// <inheritdoc />
        public override void Cleanup()
        {
            base.Cleanup();
        }

        public async void GetCacheCluster()
        {
            if (this.Service == null)
            {
                return;
            }

            try
            {
                Cluster cluster = await Task.Factory.StartNew<Cluster>(this.Service.GetCacheCluster);
                if (cluster != null)
                {
                    this.UpdateCluster(cluster);
                }
            }
            catch
            {
                MessageBox.Show("An exception was thrown when retrieving cluster information.");
            }
        }

        private async void StartCluster()
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to perform this action ?", "Start Cluster", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    Cluster cluster = await Task.Factory.StartNew<Cluster>(this.Service.StartCacheCluster);
                    if (cluster != null)
                    {
                        this.UpdateCluster(cluster);
                    }
                }
                catch
                {
                    MessageBox.Show("An exception was thrown during the execution of this operation");
                }
            }
        }

        private async void StopCluster()
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to perform this action ?", "Stop Cluster", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    Cluster cluster = await Task.Factory.StartNew<Cluster>(this.Service.StopCacheCluster);
                    if (cluster != null)
                    {
                        this.UpdateCluster(cluster);
                    }
                }
                catch
                {
                    MessageBox.Show("An exception was thrown during the execution of this operation");
                }
            }
        }

        private async void RestartCluster()
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to perform this action ?", "Restart Cluster", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    Cluster cluster = await Task.Factory.StartNew<Cluster>(this.Service.RestartCacheCluster);
                    if (cluster != null)
                    {
                        this.UpdateCluster(cluster);
                    }
                }
                catch
                {
                    MessageBox.Show("An exception was thrown during the execution of this operation");
                }
            }
        }

        private void UpdateCluster(Cluster source)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            if (this.Children != null && this.Children.Count > 0)
            {
                this.CleanupChildren();
            }

            List<CacheHost> cacheHosts = source.CacheHosts;
            if (cacheHosts != null && cacheHosts.Count > 0)
            {
                FolderViewModel cacheHostFolder = new FolderViewModel(Resources.UIResources.CacheHostsFolderTitle, this);
                this.cluster.CacheHosts = cacheHosts;
                foreach (CacheHost cacheHost in cacheHosts)
                {
                    CacheHostViewModel cacheHostViewModel = new CacheHostViewModel(cacheHost, this);
                    cacheHostFolder.Children.Add(cacheHostViewModel);
                }

                this.Children.Add(cacheHostFolder);
            }

            List<NamedCache> namedCaches = source.NamedCaches;
            if (namedCaches != null && namedCaches.Count > 0)
            {
                FolderViewModel namedcachesFolder = new FolderViewModel(Resources.UIResources.NamedCachesFolderTitle, this);
                this.cluster.NamedCaches = namedCaches;
                foreach (NamedCache namedCache in namedCaches)
                {
                    NamedCacheViewModel namedCacheViewModel = new NamedCacheViewModel(namedCache, this);
                    namedcachesFolder.Children.Add(namedCacheViewModel);
                }

                this.Children.Add(namedcachesFolder);
            }
        }
    }
}
