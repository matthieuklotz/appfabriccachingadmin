// -----------------------------------------------------------------------
// <copyright file="SearchViewModel.cs" company="Matthieu Klotz">
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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Input;
    using AppFabric.Admin.Clients.Common.Data;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using DataCluster = AppFabric.Admin.Clients.Common.Data.Cluster;
    using DataNamedCache = AppFabric.Admin.Clients.Common.Data.NamedCache;

    public class SearchViewModel : ViewModelBase, INamedViewModel
    {
        private AdminServiceNodeViewModel nodeViewModel;

        private BackgroundWorker searchWorker;

        private BackgroundWorker removeWorker;

        public SearchViewModel(AdminServiceNodeViewModel nodeViewModel)
        {
            this.SearchResults = new ObservableCollection<DataCacheItem>();
            this.NamedCaches = new ObservableCollection<string>();
            this.SearchCommand = new RelayCommand(this.Search);
            this.RemoveItemCommand = new RelayCommand<DataCacheItem>(this.RemoveItem);
            this.searchWorker = new BackgroundWorker();
            this.searchWorker.DoWork += this.DoSearch;
            this.searchWorker.RunWorkerCompleted += this.SearchCompleted;
            this.searchWorker.WorkerSupportsCancellation = true;

            this.removeWorker = new BackgroundWorker();
            this.removeWorker.DoWork += this.DoRemove;
            this.removeWorker.WorkerSupportsCancellation = true;
            this.UpdateViewModel(nodeViewModel);
        }

        public ICommand SearchCommand { get; private set; }

        public ICommand RemoveItemCommand { get; private set; }

        public string SearchPattern { get; set; }

        public string SelectedNamedCache { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Cannot be read-only because a binding is set with a datagrid")]
        public ObservableCollection<DataCacheItem> SearchResults { get; set; }

        public ObservableCollection<string> NamedCaches { get; private set; }

        public string Name
        {
            get
            {
                string name = null;
                if (this.nodeViewModel != null)
                {
                    name = string.Format(CultureInfo.CurrentUICulture, Resources.UIResources.SearchTabItemHeader, this.nodeViewModel.Name);
                }

                return name;
            }

            set
            {
            }
        }

        public void UpdateViewModel(AdminServiceNodeViewModel source)
        {
            if (this.NamedCaches.Count > 0)
            {
                this.NamedCaches.Clear();
            }

            this.nodeViewModel = source;
            if (this.nodeViewModel != null)
            {
                ClusterViewModel clustervm = this.nodeViewModel as ClusterViewModel;
                if (clustervm != null)
                {
                    DataCluster cluster = clustervm.Cluster;
                    if (cluster != null)
                    {
                        List<DataNamedCache> namedCaches = cluster.NamedCaches;
                        if (namedCaches != null && namedCaches.Count > 0)
                        {
                            IEnumerable<string> namedCachesName = namedCaches.Select(namedCache => namedCache.Name);
                            foreach (string namedCacheName in namedCachesName)
                            {
                                this.NamedCaches.Add(namedCacheName);
                            }
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public override void Cleanup()
        {
            base.Cleanup();
            if (this.SearchResults.Count > 0)
            {
                this.SearchResults.Clear();
            }

            this.searchWorker.DoWork -= this.DoSearch;
            this.searchWorker.RunWorkerCompleted -= this.SearchCompleted;
            this.searchWorker.Dispose();
            this.searchWorker = null;

            this.removeWorker.DoWork -= this.DoRemove;
            this.removeWorker.Dispose();
            this.removeWorker = null;

            this.nodeViewModel = null;
        }

        private void Search()
        {
            if (this.SearchResults.Count > 0)
            {
                this.SearchResults.Clear();
            }

            if (!this.searchWorker.IsBusy)
            {
                this.searchWorker.RunWorkerAsync(null);
            }
        }

        private void RemoveItem(DataCacheItem item)
        {
            if (!this.removeWorker.IsBusy && item != null)
            {
                this.removeWorker.RunWorkerAsync(item);
            }
        }

        private void DoRemove(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker backgroundWorker = sender as BackgroundWorker;
            if (backgroundWorker == null || backgroundWorker.CancellationPending)
            {
                return;
            }

            DataCacheItem item = e.Argument as DataCacheItem;
            if (item != null)
            {
                ClusterViewModel clusterViewModel = this.nodeViewModel as ClusterViewModel;
                if (clusterViewModel != null)
                {
                    List<DataCacheItem> itemsToRemove = new List<DataCacheItem> { item };
                    clusterViewModel.Service.RemoveCacheItems(itemsToRemove);
                }
            }
        }

        private void DoSearch(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker backgroundWorker = sender as BackgroundWorker;
            if (backgroundWorker.CancellationPending)
            {
                return;
            }

            ClusterViewModel clusterViewModel = this.nodeViewModel as ClusterViewModel;
            if (clusterViewModel != null)
            {
                SearchItemRequest request = new SearchItemRequest
                {
                    SearchPattern = this.SearchPattern,
                    NamedCache = this.SelectedNamedCache
                };

                e.Result = clusterViewModel.Service.SearchCacheItems(new List<SearchItemRequest> { request });
            }
        }

        private void SearchCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null)
            {
                return;
            }

            ICollection<DataCacheItem> foundDataCacheItems = e.Result as ICollection<DataCacheItem>;
            if (foundDataCacheItems != null && foundDataCacheItems.Count > 0)
            {
                foreach (DataCacheItem item in foundDataCacheItems)
                {
                    this.SearchResults.Add(item);
                }
            }
        }
    }
}
