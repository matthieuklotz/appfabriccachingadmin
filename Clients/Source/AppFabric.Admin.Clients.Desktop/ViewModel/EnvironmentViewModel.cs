// -----------------------------------------------------------------------
// <copyright file="EnvironmentViewModel.cs" company="Matthieu Klotz">
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
    using System.Windows.Input;
    using AppFabric.Admin.Clients.Common.Data;
    using GalaSoft.MvvmLight.Command;
    using DataEnvironment = AppFabric.Admin.Clients.Common.Data.Environment;

    public class EnvironmentViewModel : NodeViewModel
    {
        public EnvironmentViewModel(DataEnvironment environment)
            : base()
        {
            Contract.Requires<ArgumentNullException>(environment != null);
            this.AddClusterCommand = new RelayCommand(this.AddCluster);
            this.RemoveClusterCommand = new RelayCommand<int>(this.RemoveCluster);

            this.Environment = environment;
            List<Cluster> clusters = this.Environment.Clusters;
            if (clusters != null && clusters.Count > 0)
            {
                foreach (Cluster cluster in clusters)
                {
                    ClusterViewModel clusterViewModel = new ClusterViewModel(this, cluster);
                    this.Children.Add(clusterViewModel);
                }
            }
        }

        public DataEnvironment Environment { get; private set; }

        /// <inheritdoc />
        public override string Name
        {
            get
            {
                return this.Environment.Name;
            }

            set
            {
                if (!string.Equals(this.Environment.Name, value, StringComparison.OrdinalIgnoreCase))
                {
                    this.RaisePropertyChanging("Name");
                    this.Environment.Name = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        public string Description
        {
            get
            {
                return this.Environment.Description;
            }

            set
            {
                if (!string.Equals(this.Environment.Description, value, StringComparison.OrdinalIgnoreCase))
                {
                    this.RaisePropertyChanging("Description");
                    this.Environment.Description = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }

        public ICommand AddClusterCommand { get; private set; }

        public ICommand RemoveClusterCommand { get; private set; }

        private void AddCluster()
        {
            ClusterViewModel clusterViewModel = new ClusterViewModel(this, new Cluster());
            this.Children.Add(clusterViewModel);
            this.Environment.Clusters.Add(clusterViewModel.Cluster);
        }

        private void RemoveCluster(int indexToRemove)
        {
            if (indexToRemove > -1 && this.Children != null && indexToRemove < this.Children.Count)
            {
                ClusterViewModel cluster = this.Children[indexToRemove] as ClusterViewModel;
                if (cluster != null)
                {
                    this.Children.RemoveAt(indexToRemove);
                    this.Environment.Clusters.RemoveAt(indexToRemove);
                    cluster.Cleanup();
                }
            }
        }
    }
}
