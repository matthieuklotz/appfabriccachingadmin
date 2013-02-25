// -----------------------------------------------------------------------
// <copyright file="ConfigurationEnvironmentService.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Clients.Common
{
    using System.Collections.Generic;
    using System.Configuration;
    using AppFabric.Admin.Clients.Common.Configuration;
    using AppFabric.Admin.Clients.Common.Helpers;
    using AppFabric.Admin.Common.Configuration;
    using ConfigurationManager = AppFabric.Admin.Common.Configuration.ConfigurationManager;

    public class ConfigurationEnvironmentService : IEnvironmentService
    {
        private object lockClientConfig = new object();

        /// <inheritdoc />
        public ICollection<Data.Environment> GetAllEnvironments()
        {
            List<Data.Environment> environments = null;
            ClientSection client = this.GetClientSection();
            NamedElementCollection<EnvironmentElement> confEnvironments = client.Environments;
            if (confEnvironments != null && confEnvironments.Count > 0)
            {
                environments = new List<Data.Environment>(confEnvironments.Count);
                foreach (EnvironmentElement environmentElt in confEnvironments)
                {
                    if (environmentElt != null)
                    {
                        Data.Environment environment = environmentElt.ToDataEnvironment();
                        if (environment != null)
                        {
                            environments.Add(environment);
                        }
                    }
                }
            }

            return environments;
        }

        /// <inheritdoc />
        public void RemoveEnvironment(string environmentName)
        {
            ClientSection client = this.GetClientSection();
            if (client != null)
            {
                client.Environments.Remove(environmentName);
                ConfigurationManager.Save();
            }
        }

        /// <inheritdoc />
        public void AddOrUpdateEnvironment(Data.Environment environment)
        {
            this.RemoveEnvironment(environment.Name);
            ClientSection client = this.GetClientSection();
            if (client != null)
            {
                EnvironmentElement envElt = environment.ToEnvironmentElement();
                client.Environments.Add(envElt);
                ConfigurationManager.Save();
            }
        }

        /// <summary>
        /// Gets the client section.
        /// </summary>
        /// <returns>Client configuration section</returns>
        private ClientSection GetClientSection()
        {
            ClientSection client = ConfigurationManager.GetSection<ClientSection>(ClientSection.ConfigurationSectionName);
            if (client == null)
            {
                lock (this.lockClientConfig)
                {
                    client = ConfigurationManager.GetSection<ClientSection>(ClientSection.ConfigurationSectionName);
                    if (client == null)
                    {
                        ClientSection clientToCreate = ConfigurationManager.CreateSection<ClientSection>(ClientSection.ConfigurationSectionName, ConfigurationAllowExeDefinition.MachineToLocalUser, true);
                        clientToCreate.Environments = new NamedElementCollection<EnvironmentElement>();
                        ConfigurationManager.Save();
                        client = clientToCreate;
                    }
                }
            }

            return client;
        }
    }
}
