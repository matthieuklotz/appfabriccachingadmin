// -----------------------------------------------------------------------
// <copyright file="ConfigurationManager.cs" company="Matthieu Klotz">
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
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using AppFabric.Admin.Common.Injection;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// A manager for application configuration.
    /// </summary>
    public static class ConfigurationManager
    {
        /// <summary>
        /// Base configuration manager to use.
        /// </summary>
        private static List<ConfigurationManagerBase> configurationManagers;

        /// <summary>
        /// Merged application settings.
        /// </summary>
        private static NameValueCollection appSettings = null;

        /// <summary>
        /// Merge connection strings.
        /// </summary>
        private static ConnectionStringSettingsCollection connectionStrings = null;

        /// <summary>
        /// Found configuration section.
        /// </summary>
        private static ConcurrentDictionary<string, ConfigurationSection> sections = new ConcurrentDictionary<string, ConfigurationSection>();

        /// <summary>
        /// Prevents multi-threading access on properties of the static instance of the <see cref="ConfigurationManager"/>.
        /// </summary>
        private static object syncLock = new object();

        /// <summary>
        /// Initializes static members of the <see cref="ConfigurationManager"/> class.
        /// </summary>
        static ConfigurationManager()
        {
            Initialize();
        }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        public static NameValueCollection AppSettings
        {
            get
            {
                if (appSettings == null)
                {
                    lock (syncLock)
                    {
                        if (appSettings == null)
                        {
                            appSettings = GetAppSettings();
                        }
                    }
                }

                return appSettings;
            }
        }

        /// <summary>
        /// Gets the connection strings.
        /// </summary>
        public static ConnectionStringSettingsCollection ConnectionStrings
        {
            get
            {
                if (connectionStrings == null)
                {
                    lock (syncLock)
                    {
                        if (connectionStrings == null)
                        {
                            connectionStrings = GetConnectionStrings();
                        }
                    }
                }

                return connectionStrings;
            }
        }

        /// <summary>
        /// Creates the specified section name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        public static TSection CreateSection<TSection>(string sectionName, ConfigurationAllowExeDefinition allowExeDefinition, bool protectSection) where TSection : ConfigurationSection, new()
        {
            TSection section = new TSection();
            section.SectionInformation.AllowExeDefinition = allowExeDefinition;
            ConfigurationManagerBase configurationManagerBase = configurationManagers.FirstOrDefault();
            if (configurationManagerBase != null)
            {
                configurationManagerBase.AddSection(sectionName, section);
            }

            if (protectSection)
            {
                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
            }

            return section;
        }

        /// <summary>
        /// Gets the configuration section.
        /// </summary>
        /// <typeparam name="TSection">The type of the section.</typeparam>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns>The found configuration section.</returns>
        public static TSection GetSection<TSection>(string sectionName) where TSection : ConfigurationSection
        {
            return sections.GetOrAdd(sectionName, GetSection) as TSection;
        }

        public static void Save()
        {
            configurationManagers.ForEach(confManager => confManager.Save());
        }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        /// <returns>Merged Application Settings as a <see cref="NameValueCollection"/>.</returns>
        private static NameValueCollection GetAppSettings()
        {
            NameValueCollection appSettings = new NameValueCollection();
            foreach (ConfigurationManagerBase configManager in configurationManagers)
            {
                NameValueCollection confManagerAppSettings = configManager.AppSettings;
                if (confManagerAppSettings == null || confManagerAppSettings.Count < 1)
                {
                    continue;
                }

                int count = confManagerAppSettings.Count;
                for (int i = 0; i < count; ++i)
                {
                    string name = confManagerAppSettings.GetKey(i);
                    if (appSettings.Get(name) == null)
                    {
                        string value = confManagerAppSettings.Get(name);
                        if (value != null)
                        {
                            appSettings.Set(name, value);
                        }
                    }
                }
            }

            return appSettings;
        }

        /// <summary>
        /// Gets the connection strings.
        /// </summary>
        /// <returns>Merged ConnectionStrings for the current application.</returns>
        private static ConnectionStringSettingsCollection GetConnectionStrings()
        {
            ConnectionStringSettingsCollection connectionStrings = new ConnectionStringSettingsCollection();
            foreach (ConfigurationManagerBase configManager in configurationManagers)
            {
                ConnectionStringSettingsCollection configManagerConnectionStrings = configManager.ConnectionStrings;
                if (configManagerConnectionStrings == null || configManagerConnectionStrings.Count < 1)
                {
                    continue;
                }

                foreach (ConnectionStringSettings connectionString in configManagerConnectionStrings)
                {
                    if (connectionStrings[connectionString.Name] == null)
                    {
                        connectionStrings.Add(connectionString);
                    }
                }
            }

            return connectionStrings;
        }

        /// <summary>
        /// Gets the configuration section with the specified name.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns>The found configuration section.</returns>
        private static ConfigurationSection GetSection(string sectionName)
        {
            ConfigurationSection section = null;
            foreach (ConfigurationManagerBase configManager in configurationManagers)
            {
                object sectionObject = configManager.GetSection(sectionName);
                if (sectionObject != null)
                {
                    section = sectionObject as ConfigurationSection;
                    break;
                }
            }

            return section;
        }

        /// <summary>
        /// Invalidates the configuration.
        /// </summary>
        /// <param name="source">The source that fired the event.</param>
        private static void InvalidateConfiguration(ConfigurationManagerBase source)
        {
            configurationManagers = null;
            appSettings = null;
            connectionStrings = null;
            sections.Clear();
            Initialize();
        }

        private static void Initialize()
        {
            IEnumerable<ConfigurationManagerBase> configManagers = IoCManager.Container.ResolveAll<ConfigurationManagerBase>();
            if (configManagers != null)
            {
                configurationManagers = configManagers.ToList<ConfigurationManagerBase>();
                foreach (ConfigurationManagerBase configManager in configurationManagers)
                {
                    configManager.ConfigurationChanged += InvalidateConfiguration;
                }
            }
        }
    }
}
