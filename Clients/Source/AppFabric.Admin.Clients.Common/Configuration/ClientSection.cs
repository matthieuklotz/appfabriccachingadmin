// -----------------------------------------------------------------------
// <copyright file="ClientSection.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Clients.Common.Configuration
{
    using System.Configuration;
    using AppFabric.Admin.Common.Configuration;

    internal class ClientSection : ConfigurationSection
    {
        internal const string ConfigurationSectionName = "appfabricAdmin.client";

        /// <summary>
        /// Properties for this configuration section.
        /// </summary>
        private static readonly ConfigurationPropertyCollection PropertyCollection;

        /// <summary>
        /// Environments property.
        /// </summary>
        private static readonly ConfigurationProperty EnvironmentsProperty;

        /// <summary>
        /// Initializes static members of the <see cref="ClientSection" /> class.
        /// </summary>
        static ClientSection()
        {
            EnvironmentsProperty = new ConfigurationProperty(null, typeof(NamedElementCollection<EnvironmentElement>), null, ConfigurationPropertyOptions.IsDefaultCollection);
            PropertyCollection = new ConfigurationPropertyCollection();
            PropertyCollection.Add(EnvironmentsProperty);
        }

        /// <summary>
        /// Gets the environments.
        /// </summary>
        /// <value>Environments declared in the configuration.</value>
        [ConfigurationProperty("environments", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(NamedElementCollection<EnvironmentElement>), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
        internal NamedElementCollection<EnvironmentElement> Environments
        {
            get
            {
                return this[EnvironmentsProperty] as NamedElementCollection<EnvironmentElement>;
            }

            set
            {
                this[EnvironmentsProperty] = value;
            }
        }

        /// <inheritdoc />
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return PropertyCollection;
            }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}
