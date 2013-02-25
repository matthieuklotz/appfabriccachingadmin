// -----------------------------------------------------------------------
// <copyright file="ConfigurationManagerBase.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Common.Configuration
{
    using System.Collections.Specialized;
    using System.Configuration;

    /// <summary>
    /// Base configuration manager.
    /// </summary>
    public abstract class ConfigurationManagerBase
    {
        /// <summary>
        /// Object for prevent multi-threading access on event configurationChanged.
        /// </summary>
        private object syncEvent = new object();

        /// <summary>
        /// Event that occurs when configuration changed.
        /// </summary>
        private ConfigurationChangedEventHandler configurationChanged;

        /// <summary>
        /// Handle event when configuration has changed.
        /// </summary>
        /// <param name="source">The configuration manager that fires the event.</param>
        public delegate void ConfigurationChangedEventHandler(ConfigurationManagerBase source);

        /// <summary>
        /// Occurs when the configuration changed.
        /// </summary>
        public event ConfigurationChangedEventHandler ConfigurationChanged
        {
            add
            {
                lock (this.syncEvent)
                {
                    this.configurationChanged += value;
                }
            }

            remove
            {
                lock (this.syncEvent)
                {
                    this.configurationChanged -= value;
                }
            }
        }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        public NameValueCollection AppSettings { get; protected set; }

        /// <summary>
        /// Gets the connection strings.
        /// </summary>
        public ConnectionStringSettingsCollection ConnectionStrings { get; protected set; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public Configuration Configuration { get; protected set; }

        /// <summary>
        /// Gets the configuration section.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns>The found configuration section.</returns>
        public abstract object GetSection(string sectionName);

        public abstract void AddSection(string sectionName, ConfigurationSection section);

        /// <summary>
        /// Saves the specified user level.
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// Called when the configuration has changed.
        /// </summary>
        protected virtual void OnChanged()
        {
            if (this.configurationChanged != null)
            {
                this.configurationChanged(this);
            }
        }
    }
}
