// -----------------------------------------------------------------------
// <copyright file="StatisticsViewModel.cs" company="Matthieu Klotz">
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
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Threading;
    using AppFabric.Admin.Clients.Common;
    using AppFabric.Admin.Clients.Common.Data;
    using GalaSoft.MvvmLight;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [CLSCompliant(false)]
    public class StatisticsViewModel : ViewModelBase, INamedViewModel
    {
        private const int TimerPeriod = 10;

        private DispatcherTimer dispatcherTimer;

        private BackgroundWorker backgroundWorker;

        private AdminServiceNodeViewModel nodeViewModel;

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validation by code contracts.")]
        public StatisticsViewModel(AdminServiceNodeViewModel nodeViewModel)
        {
            Contract.Requires<ArgumentNullException>(nodeViewModel != null);
            this.Statistics = new ObservableCollection<PlotModel>();
            this.nodeViewModel = nodeViewModel;
            this.backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.WorkerSupportsCancellation = true;
            Type nodeViewModelType = nodeViewModel.GetType();
            if (nodeViewModelType == typeof(NamedCacheViewModel))
            {
                this.backgroundWorker.DoWork += this.DoGetCacheStatistics;
            }
            else if (nodeViewModelType == typeof(CacheHostViewModel))
            {
                this.backgroundWorker.DoWork += this.DoGetHostStatistics;
            }
            else
            {
                this.backgroundWorker.DoWork += this.DoGetClusterStatistics;
            }

            this.backgroundWorker.RunWorkerCompleted += this.GetStatisticsCompleted;
            this.backgroundWorker.RunWorkerAsync();
            this.dispatcherTimer = new DispatcherTimer();
            this.dispatcherTimer.Tick += this.TimerStatisticsCallback;
            this.dispatcherTimer.Interval = new TimeSpan(0, 0, TimerPeriod);
            this.dispatcherTimer.IsEnabled = true;
        }

        /// <inheritdoc />
        public string Name
        {
            get
            {
                string name = null;
                if (this.nodeViewModel != null)
                {
                    name = string.Format(CultureInfo.CurrentUICulture, Resources.UIResources.StatsTabItemHeader, this.nodeViewModel.Name);
                }

                return name;
            }

            set
            {
            }
        }

        public ObservableCollection<PlotModel> Statistics { get; private set; }

        /// <inheritdoc />
        public override void Cleanup()
        {
            this.dispatcherTimer.IsEnabled = false;
            if (this.backgroundWorker.IsBusy)
            {
                this.backgroundWorker.CancelAsync();
            }

            this.dispatcherTimer.Tick -= this.TimerStatisticsCallback;
            this.dispatcherTimer = null;

            Type nodeViewModelType = this.nodeViewModel.GetType();
            if (nodeViewModelType == typeof(NamedCacheViewModel))
            {
                this.backgroundWorker.DoWork -= this.DoGetCacheStatistics;
            }
            else if (nodeViewModelType == typeof(CacheHostViewModel))
            {
                this.backgroundWorker.DoWork -= this.DoGetHostStatistics;
            }
            else
            {
                this.backgroundWorker.DoWork -= this.DoGetClusterStatistics;
            }

            this.backgroundWorker.RunWorkerCompleted -= this.GetStatisticsCompleted;
            this.backgroundWorker.Dispose();
            this.backgroundWorker = null;
            this.nodeViewModel = null;
            base.Cleanup();
        }

        private static bool WorkerIsCancelled(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            bool cancellationPending = worker != null ? worker.CancellationPending : true;
            return cancellationPending || e.Cancel;
        }

        private void TimerStatisticsCallback(object sender, EventArgs e)
        {
            if (this.backgroundWorker != null && !this.backgroundWorker.IsBusy)
            {
                this.backgroundWorker.RunWorkerAsync();
            }
        }

        private void DoGetClusterStatistics(object sender, DoWorkEventArgs e)
        {
            if (WorkerIsCancelled(sender, e))
            {
                return;
            }

            ClusterViewModel clusterViewModel = this.nodeViewModel as ClusterViewModel;
            if (clusterViewModel != null && clusterViewModel.Service != null)
            {
                Cluster cluster = clusterViewModel.Cluster;
                if (cluster != null)
                {
                    if (cluster.CacheHosts != null && cluster.CacheHosts.Count > 0)
                    {
                        this.GetHostsStatistics(clusterViewModel.Service, cluster.CacheHosts);
                    }

                    if (cluster.NamedCaches != null && cluster.NamedCaches.Count > 0)
                    {
                        this.GetCachesStatistics(clusterViewModel.Service, cluster.NamedCaches);
                    }
                }
            }
        }

        private void DoGetHostStatistics(object sender, DoWorkEventArgs e)
        {
            if (WorkerIsCancelled(sender, e))
            {
                return;
            }

            CacheHostViewModel hostViewModel = this.nodeViewModel as CacheHostViewModel;
            if (hostViewModel != null)
            {
                CacheHost cacheHost = hostViewModel.CacheHost;
                if (cacheHost != null)
                {
                    IAdministrationServiceClient client = hostViewModel.Service;
                    if (client != null)
                    {
                        List<CacheHost> hosts = new List<CacheHost>(1) { cacheHost };
                        this.GetHostsStatistics(client, hosts);
                    }
                }
            }
        }

        private void DoGetCacheStatistics(object sender, DoWorkEventArgs e)
        {
            if (WorkerIsCancelled(sender, e))
            {
                return;
            }

            NamedCacheViewModel cacheViewModel = this.nodeViewModel as NamedCacheViewModel;
            if (cacheViewModel != null)
            {
                NamedCache namedCache = cacheViewModel.NamedCache;
                if (namedCache != null)
                {
                    IAdministrationServiceClient client = cacheViewModel.Service;
                    if (client != null)
                    {
                        List<NamedCache> namedCaches = new List<NamedCache>(1) { namedCache };
                        this.GetCachesStatistics(client, namedCaches);
                    }
                }
            }
        }

        private void GetStatisticsCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e != null && !e.Cancelled)
            {
                this.RaisePropertyChanged("Statistics");
            }
        }

        private void GetHostsStatistics(IAdministrationServiceClient service, List<CacheHost> hosts)
        {
            ICollection<Statistics> stats = null;
            if (hosts != null && hosts.Count > 0 && service != null)
            {
                stats = service.GetHostsStatistics(hosts);
            }

            if (stats != null && stats.Count > 0)
            {
                foreach (Statistics hostStats in stats)
                {
                    if (hostStats == null)
                    {
                        continue;
                    }

                    IList<Counter> counters = hostStats.Counters;
                    if (counters != null && counters.Count > 0)
                    {
                        foreach (Counter counter in counters)
                        {
                            bool modelFound = true;
                            PlotModel model = this.Statistics.FirstOrDefault(plotModel => string.Equals(plotModel.Title, counter.CounterName, StringComparison.OrdinalIgnoreCase));
                            if (model == null)
                            {
                                modelFound = false;
                                model = new PlotModel(counter.CounterName);
                                model.Axes.Add(new DateTimeAxis(AxisPosition.Bottom, DateTime.Now, DateTime.Now.AddMinutes(30), title: "Time", intervalType: DateTimeIntervalType.Minutes));
                                model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 1));
                            }

                            Axis ax = model.Axes.FirstOrDefault(axis => axis.Position == AxisPosition.Left);
                            double max = ax.Maximum;
                            LineSeries lineSeries = model.Series.FirstOrDefault(serie => string.Equals(serie.Title, hostStats.InstanceName, StringComparison.OrdinalIgnoreCase)) as LineSeries;
                            if (lineSeries == null)
                            {
                                lineSeries = new LineSeries(hostStats.InstanceName);
                                model.Series.Add(lineSeries);
                            }

                            if (counter.Value > max)
                            {
                                max = counter.Value + (counter.Value * 10d / 100d);
                            }

                            lineSeries.Points.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, counter.Value));
                            ax.Maximum = (int)max;
                            model.AttachPlotControl(null);
                            model.Update();

                            if (!modelFound)
                            {
                                this.Statistics.Add(model);
                            }
                        }
                    }
                }
            }
        }

        private void GetCachesStatistics(IAdministrationServiceClient service, List<NamedCache> namedCaches)
        {
            ICollection<CacheStatistics> stats = null;
            if (namedCaches != null && namedCaches.Count > 0 && service != null)
            {
                stats = service.GetCachesStatistics(namedCaches);
            }

            if (stats != null && stats.Count > 0)
            {
                foreach (CacheStatistics cacheStat in stats)
                {
                    if (cacheStat == null)
                    {
                        continue;
                    }

                    IDictionary<string, IList<Counter>> countersByHosts = new Dictionary<string, IList<Counter>>(cacheStat.CountersByHost);
                    if (cacheStat.Counters != null)
                    {
                        countersByHosts.Add("Total", cacheStat.Counters);
                    }

                    Dictionary<string, List<HostCounter>> hostCounters = new Dictionary<string, List<HostCounter>>();
                    foreach (KeyValuePair<string, IList<Counter>> counterByHost in countersByHosts)
                    {
                        foreach (Counter counter in counterByHost.Value)
                        {
                            List<HostCounter> hostCounter = null;
                            if (!hostCounters.TryGetValue(counter.CounterName, out hostCounter))
                            {
                                hostCounter = new List<HostCounter>();
                                hostCounters.Add(counter.CounterName, hostCounter);
                            }

                            hostCounter.Add(new HostCounter
                                            {
                                                MachineName = counterByHost.Key,
                                                CounterName = counter.CounterName,
                                                Value = counter.Value
                                            });
                        }
                    }

                    foreach (KeyValuePair<string, List<HostCounter>> hostCounter in hostCounters)
                    {
                        foreach (HostCounter counter in hostCounter.Value)
                        {
                            bool modelFound = true;
                            PlotModel model = this.Statistics.FirstOrDefault(plotModel => string.Equals(plotModel.Title, counter.CounterName, StringComparison.OrdinalIgnoreCase));
                            if (model == null)
                            {
                                modelFound = false;
                                model = new PlotModel(counter.CounterName);
                                model.Axes.Add(new DateTimeAxis(AxisPosition.Bottom, DateTime.Now, DateTime.Now.AddMinutes(30), title: "Time", intervalType: DateTimeIntervalType.Minutes));
                                model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 1));
                            }

                            Axis ax = model.Axes.FirstOrDefault(axis => axis.Position == AxisPosition.Left);
                            double max = ax.Maximum;
                            LineSeries lineSeries = model.Series.FirstOrDefault(serie => string.Equals(serie.Title, counter.MachineName, StringComparison.OrdinalIgnoreCase)) as LineSeries;
                            if (lineSeries == null)
                            {
                                lineSeries = new LineSeries(counter.MachineName);
                                model.Series.Add(lineSeries);
                            }

                            if (counter.Value > max)
                            {
                                max = counter.Value + (counter.Value * 10d / 100d);
                            }

                            lineSeries.Points.Add(DateTimeAxis.CreateDataPoint(DateTime.Now, counter.Value));
                            ax.Maximum = (int)max;
                            model.AttachPlotControl(null);
                            model.Update();

                            if (!modelFound)
                            {
                                this.Statistics.Add(model);
                            }
                        }
                    }
                }
            }
        }

        private class HostCounter : Counter
        {
            internal string MachineName { get; set; }
        }
    }
}
