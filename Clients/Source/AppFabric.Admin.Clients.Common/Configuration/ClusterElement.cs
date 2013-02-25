// -----------------------------------------------------------------------
// <copyright file="ClusterElement.cs" company="Matthieu Klotz">
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
    using System;
    using System.Configuration;
    using System.Globalization;
    using AppFabric.Admin.Common.Configuration;

    internal class ClusterElement : NamedConfigurationElement
    {
        private const string PropertyUri = "uri";

        private const string PropertyUserName = "userName";

        private const string PropertyUserPassword = "userPassword";

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description of the configuration element.
        /// </value>
        [ConfigurationProperty(PropertyUri, IsKey = false, IsRequired = true)]
        public virtual Uri Uri
        {
            get
            {
                return base[PropertyUri] as Uri;
            }

            set
            {
                base[PropertyUri] = value;
            }
        }

        [ConfigurationProperty(PropertyUserName, IsKey = false, IsRequired = false, DefaultValue = "")]
        public string UserName
        {
            get
            {
                return Convert.ToString(base[PropertyUserName], CultureInfo.InvariantCulture);
            }

            set
            {
                base[PropertyUserName] = value;
            }
        }

        [ConfigurationProperty(PropertyUserPassword, IsKey = false, IsRequired = false, DefaultValue = "")]
        public string UserPassword
        {
            get
            {
                return Convert.ToString(base[PropertyUserPassword], CultureInfo.InvariantCulture);
            }

            set
            {
                base[PropertyUserPassword] = value;
            }
        }
    }
}
