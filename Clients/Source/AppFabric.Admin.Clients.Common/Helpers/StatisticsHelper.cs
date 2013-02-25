// -----------------------------------------------------------------------
// <copyright file="StatisticsHelper.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Clients.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Omu.ValueInjecter;
    using AdminCacheStats = AppFabric.Admin.Service.DataContracts.Reporting.CacheStatistics;
    using AdminCounter = AppFabric.Admin.Service.DataContracts.Reporting.Counter;
    using AdminStats = AppFabric.Admin.Service.DataContracts.Reporting.Statistics;

    internal static class StatisticsHelper
    {
        internal static Data.Statistics ToDataStatistics(this AdminStats adminStats)
        {
            Contract.Requires<ArgumentNullException>(adminStats != null);
            Data.Statistics stats = new Data.Statistics();
            stats.InjectFrom(adminStats);
            if (adminStats.Counters != null && adminStats.Counters.Count > 0)
            {
                stats.Counters = adminStats.Counters.ToDataCounters();
            }

            return stats;
        }

        internal static Data.CacheStatistics ToDataCacheStatistics(this AdminCacheStats adminStats)
        {
            Contract.Requires<ArgumentNullException>(adminStats != null);
            Data.CacheStatistics stats = new Data.CacheStatistics();
            stats.InjectFrom(adminStats);
            if (adminStats.CountersByHost != null && adminStats.CountersByHost.Count > 0)
            {
                stats.CountersByHost = new Dictionary<string, IList<Data.Counter>>(adminStats.CountersByHost.Count);
                foreach (KeyValuePair<string, IList<AdminCounter>> adminCounters in adminStats.CountersByHost)
                {
                    if (adminCounters.Value != null)
                    {
                        IList<Data.Counter> counters = adminCounters.Value.ToDataCounters();
                        if (counters != null && counters.Count > 0)
                        {
                            stats.CountersByHost.Add(adminCounters.Key, counters);
                        }
                    }
                }
            }

            return stats;
        }

        internal static IList<Data.Counter> ToDataCounters(this IList<AdminCounter> adminCounters)
        {
            Contract.Requires<ArgumentNullException>(adminCounters != null);
            Contract.Requires<InvalidOperationException>(adminCounters.Count > 0);
            List<Data.Counter> counters = new List<Data.Counter>(adminCounters.Count);
            foreach (AdminCounter adminCounter in adminCounters)
            {
                if (adminCounter != null)
                {
                    Data.Counter counter = new Data.Counter();
                    counter.InjectFrom(adminCounter);
                    counters.Add(counter);
                }
            }

            return counters;
        }
    }
}
