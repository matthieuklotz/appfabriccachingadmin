// -----------------------------------------------------------------------
// <copyright file="ClusterHealthViewModel.cs" company="Matthieu Klotz">
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
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Windows.Threading;
    using AppFabric.Admin.Clients.Common.Data;
    using GalaSoft.MvvmLight;

    public class ClusterHealthViewModel : ViewModelBase, INamedViewModel
    {
        private DispatcherTimer dispatcherTimer;

        private ClusterViewModel clusterViewModel;

        private BackgroundWorker backgroundWorker;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterHealthViewModel" /> class.
        /// </summary>
        /// <param name="clusterViewModel">The cluster view model.</param>
        public ClusterHealthViewModel(ClusterViewModel clusterViewModel)
        {
            Contract.Requires<ArgumentNullException>(clusterViewModel != null);
            this.clusterViewModel = clusterViewModel;

            this.UnallocatedNamedCaches = new ObservableCollection<UnallocatedNamedCache>();
            this.CacheHosts = new ObservableCollection<CacheHostHealth>();

            this.backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.DoWork += this.DoGetCacheClusterHealth;
            this.backgroundWorker.RunWorkerCompleted += this.GetCacheClusterHealthCompleted;
            this.backgroundWorker.RunWorkerAsync();

            this.dispatcherTimer = new DispatcherTimer();
            this.dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            this.dispatcherTimer.Tick += this.GetCacheClusterHealthTick;
            this.dispatcherTimer.Start();
        }

        /// <inheritdoc />
        public string Name
        {
            get
            {
                string name = null;
                if (this.clusterViewModel != null)
                {
                    name = string.Format(CultureInfo.CurrentUICulture, Resources.UIResources.HealthTabItemHeader, this.clusterViewModel.Name);
                }

                return name;
            }

            set
            {
            }
        }

        public ObservableCollection<UnallocatedNamedCache> UnallocatedNamedCaches { get; private set; }

        public ObservableCollection<CacheHostHealth> CacheHosts { get; private set; }

        /// <inheritdoc />
        public override void Cleanup()
        {
            base.Cleanup();

            if (this.backgroundWorker.IsBusy)
            {
                this.backgroundWorker.CancelAsync();
            }

            this.dispatcherTimer.Stop();
            this.dispatcherTimer.Tick -= this.GetCacheClusterHealthTick;
            this.dispatcherTimer = null;
            this.backgroundWorker.DoWork -= this.DoGetCacheClusterHealth;
            this.backgroundWorker.RunWorkerCompleted -= this.GetCacheClusterHealthCompleted;
            this.backgroundWorker.Dispose();
            this.backgroundWorker = null;
            this.clusterViewModel = null;
        }

        private void GetCacheClusterHealthTick(object sender, EventArgs e)
        {
            if (!this.backgroundWorker.IsBusy)
            {
                this.backgroundWorker.RunWorkerAsync();
            }
        }

        private void DoGetCacheClusterHealth(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker == null || worker.CancellationPending)
            {
                return;
            }

            ClusterHealth health = this.clusterViewModel.Service.GetCacheClusterHealth();
            e.Result = health;
        }

        private void GetCacheClusterHealthCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker backgroundWorker = sender as BackgroundWorker;
            if (e.Cancelled || e.Error != null || backgroundWorker == null || backgroundWorker.CancellationPending)
            {
                return;
            }

            ClusterHealth health = e.Result as ClusterHealth;
            if (health != null)
            {
                if (this.CacheHosts.Count > 0)
                {
                    this.CacheHosts.Clear();
                }

                if (health.CacheHosts != null && health.CacheHosts.Count > 0)
                {
                    foreach (CacheHostHealth cacheHostHealth in health.CacheHosts)
                    {
                        if (cacheHostHealth != null)
                        {
                            this.CacheHosts.Add(cacheHostHealth);
                        }
                    }
                }

                if (this.UnallocatedNamedCaches.Count > 0)
                {
                    this.UnallocatedNamedCaches.Clear();
                }

                if (health.UnallocatedNamedCaches != null && health.UnallocatedNamedCaches.Count > 0)
                {
                    foreach (UnallocatedNamedCache unallocatedNamedCache in health.UnallocatedNamedCaches)
                    {
                        if (unallocatedNamedCache != null)
                        {
                            this.UnallocatedNamedCaches.Add(unallocatedNamedCache);
                        }
                    }
                }
            }
        }
    }
}
