// -----------------------------------------------------------------------
// <copyright file="EnvironmentHelper.cs" company="Matthieu Klotz">
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
    using System.Diagnostics.Contracts;
    using AppFabric.Admin.Clients.Common.Configuration;
    using AppFabric.Admin.Common.Configuration;
    using Omu.ValueInjecter;

    internal static class EnvironmentHelper
    {
        internal static Data.Environment ToDataEnvironment(this EnvironmentElement environmentElement)
        {
            Contract.Requires<ArgumentNullException>(environmentElement != null);
            Data.Environment environment = new Data.Environment();
            environment.InjectFrom<FlatLoopValueInjection>(environmentElement);
            environment.Description = environmentElement.Description;
            if (environmentElement.Clusters != null && environmentElement.Clusters.Count > 0)
            {
                foreach (ClusterElement clusterElement in environmentElement.Clusters)
                {
                    Data.Cluster cluster = new Data.Cluster();
                    cluster.Name = clusterElement.Name;
                    cluster.Uri = clusterElement.Uri;
                    cluster.UserName = clusterElement.UserName;
                    cluster.UserPassword = clusterElement.UserPassword;
                    environment.Clusters.Add(cluster);
                }
            }

            return environment;
        }

        internal static EnvironmentElement ToEnvironmentElement(this Data.Environment environment)
        {
            Contract.Requires<ArgumentNullException>(environment != null);
            EnvironmentElement environmentElement = new EnvironmentElement();
            environmentElement.InjectFrom<FlatLoopValueInjection>(environment);
            environmentElement.Description = environment.Description;
            if (environment.Clusters != null && environment.Clusters.Count > 0)
            {
                environmentElement.Clusters = new NamedElementCollection<ClusterElement>();
                foreach (Data.Cluster cluster in environment.Clusters)
                {
                    ClusterElement clusterElement = new ClusterElement();
                    clusterElement.Name = cluster.Name;
                    clusterElement.Uri = cluster.Uri;
                    clusterElement.UserName = cluster.UserName;
                    clusterElement.UserPassword = cluster.UserPassword;
                    environmentElement.Clusters.Add(clusterElement);
                }
            }

            return environmentElement;
        }
    }
}
