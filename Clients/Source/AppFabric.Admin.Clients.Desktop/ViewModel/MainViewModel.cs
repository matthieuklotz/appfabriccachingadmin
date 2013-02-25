// -----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Matthieu Klotz">
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
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using AppFabric.Admin.Clients.Common;
    using AppFabric.Admin.Common.Injection;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using Microsoft.Practices.Unity;
    using Environment = AppFabric.Admin.Clients.Common.Data.Environment;

    /// <summary>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IEnvironmentService environmentService;

        private NodeViewModel selectedValue;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            this.TabItems = new ObservableCollection<INamedViewModel>();
            this.Environments = new ObservableCollection<EnvironmentViewModel>();
            this.CloseCommand = new RelayCommand(Application.Current.Shutdown);
            this.WindowLoadedCommand = new RelayCommand(this.LoadEnvironments);
            this.AddEnvironmentCommand = new RelayCommand(this.AddEnvironment);
            this.RemoveEnvironmentCommand = new RelayCommand<int>(this.RemoveEnvironment);
            this.SaveConfigurationCommand = new RelayCommand(this.SaveConfigurationAsync);
            this.UpdateSelectedValueCommand = new RelayCommand<NodeViewModel>((NodeViewModel model) => this.Set<NodeViewModel>("SelectedValue", ref this.selectedValue, model));
            this.SearchInClusterCommand = new RelayCommand<NodeViewModel>(this.AddClusterSearchTab);
            this.CloseTabItemCommand = new RelayCommand<int>(this.CloseTabItem);
            this.GetClusterHealthCommand = new RelayCommand<NodeViewModel>(this.AddClusterHealthTab);
            this.GetStatisticsCommand = new RelayCommand<NodeViewModel>(this.AddStatisticsTab);

            if (!this.IsInDesignMode)
            {
                this.environmentService = IoCManager.Container.Resolve<IEnvironmentService>();
            }
        }

        public ObservableCollection<EnvironmentViewModel> Environments { get; private set; }

        public ObservableCollection<INamedViewModel> TabItems { get; private set; }

        /// <summary>
        /// Gets the close command.
        /// </summary>
        /// <value>The command for closing application.</value>
        public ICommand CloseCommand { get; private set; }

        public ICommand WindowLoadedCommand { get; private set; }

        public ICommand AddEnvironmentCommand { get; private set; }

        public ICommand RemoveEnvironmentCommand { get; private set; }

        public ICommand SaveConfigurationCommand { get; private set; }

        public ICommand UpdateSelectedValueCommand { get; private set; }

        public ICommand SearchInClusterCommand { get; private set; }

        public ICommand GetClusterHealthCommand { get; private set; }

        public ICommand CloseTabItemCommand { get; private set; }

        public ICommand GetStatisticsCommand { get; private set; }

        public NodeViewModel SelectedValue
        {
            get
            {
                return this.selectedValue;
            }
        }

        private async void LoadEnvironments()
        {
            ICollection<Environment> environments = await Task<ICollection<Environment>>.Factory.StartNew(this.environmentService.GetAllEnvironments);
            if (environments != null && environments.Count > 0)
            {
                foreach (Environment environment in environments)
                {
                    if (environment != null)
                    {
                        EnvironmentViewModel environmentViewModel = new EnvironmentViewModel(environment);
                        this.Environments.Add(environmentViewModel);
                    }
                }
            }
        }

        private void AddEnvironment()
        {
            EnvironmentViewModel environment = new EnvironmentViewModel(new Environment());
            this.Environments.Add(environment);
        }

        private async void RemoveEnvironment(int indexToRemove)
        {
            if (indexToRemove > -1 && this.Environments != null && indexToRemove < this.Environments.Count)
            {
                EnvironmentViewModel environment = this.Environments[indexToRemove];
                this.Environments.RemoveAt(indexToRemove);
                await Task.Factory.StartNew(() =>
                {
                    string environmentName = environment.Name;
                    environment.Cleanup();
                    this.environmentService.RemoveEnvironment(environment.Name);
                });
            }
        }

        private void AddClusterSearchTab(NodeViewModel nodeViewModel)
        {
            ClusterViewModel clusterViewModel = nodeViewModel as ClusterViewModel;
            if (clusterViewModel != null)
            {
                SearchViewModel search = new SearchViewModel(clusterViewModel);
                this.TabItems.Add(search);
            }
        }

        private void AddStatisticsTab(NodeViewModel nodeViewModel)
        {
            AdminServiceNodeViewModel adminViewModel = nodeViewModel as AdminServiceNodeViewModel;
            if (adminViewModel != null)
            {
                StatisticsViewModel search = new StatisticsViewModel(adminViewModel);
                this.TabItems.Add(search);
            }
        }

        private void AddClusterHealthTab(NodeViewModel nodeViewModel)
        {
            ClusterViewModel clusterViewModel = nodeViewModel as ClusterViewModel;
            if (clusterViewModel != null)
            {
                ClusterHealthViewModel search = new ClusterHealthViewModel(clusterViewModel);
                this.TabItems.Add(search);
            }
        }

        private async void CloseTabItem(int indexToClose)
        {
            if (indexToClose > -1 && this.TabItems != null && indexToClose < this.TabItems.Count)
            {
                INamedViewModel namedViewModel = this.TabItems[indexToClose];
                this.TabItems.RemoveAt(indexToClose);
                ViewModelBase viewModelBase = namedViewModel as ViewModelBase;
                if (viewModelBase != null)
                {
                    await Task.Factory.StartNew(viewModelBase.Cleanup);
                }
            }
        }

        private async void SaveConfigurationAsync()
        {
            await Task.Factory.StartNew(this.SaveConfiguration);
        }

        private void SaveConfiguration()
        {
            IEnumerable<Environment> environments = this.Environments.Select(envViewModel => envViewModel.Environment);
            if (environments != null)
            {
                foreach (Environment environment in environments)
                {
                    this.environmentService.AddOrUpdateEnvironment(environment);
                }
            }
        }
    }
}