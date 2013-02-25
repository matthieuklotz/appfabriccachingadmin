// -----------------------------------------------------------------------
// <copyright file="ActiveDirectoryMapRoleProvider.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Common.Security
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.DirectoryServices.AccountManagement;
    using System.Web.Security;
    using AppFabric.Admin.Common.Configuration;
    using AppFabric.Admin.Common.Security.Configuration;
    using ConfigurationManager = AppFabric.Admin.Common.Configuration.ConfigurationManager;

    internal class ActiveDirectoryMapRoleProvider : RoleProvider
    {
        private PrincipalContext principalContext;

        private Dictionary<string, string[]> roleMap;

        private string[] roles;

        private object syncLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveDirectoryMapRoleProvider" /> class.
        /// </summary>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">Role Provider named WindowsTokenRoleProvider is missing.</exception>
        public ActiveDirectoryMapRoleProvider()
        {
            this.principalContext = new PrincipalContext(ContextType.Domain);
            RoleMapConfigurationSection section = ConfigurationManager.GetSection<RoleMapConfigurationSection>(RoleMapConfigurationSection.SectionName);
            if (section == null)
            {
                throw new ConfigurationErrorsException("RoleMapConfigurationSection is missing in the configuration.");
            }

            NamedElementCollection<RoleMapConfigurationElement> roleElts = section.Roles;
            if (roleElts == null || roleElts.Count < 1)
            {
                throw new ConfigurationErrorsException("RoleMapConfigurationSection does not contains any role.");
            }

            this.roles = new string[roleElts.Count];
            this.roleMap = new Dictionary<string, string[]>(roleElts.Count);
            int index = 0;
            foreach (RoleMapConfigurationElement roleElt in roleElts)
            {
                if (roleElt != null && index < this.roles.Length)
                {
                    this.roles[index] = roleElt.Name;
                    string[] adRoles = null;
                    NamedElementCollection<RoleConfigurationElement> activeDirectoryRoles = roleElt.MapTo;
                    if (activeDirectoryRoles != null && activeDirectoryRoles.Count > 0)
                    {
                        adRoles = new string[activeDirectoryRoles.Count];
                        int indexAdRoles = 0;
                        foreach (RoleConfigurationElement role in activeDirectoryRoles)
                        {
                            if (role != null && indexAdRoles < adRoles.Length)
                            {
                                adRoles[indexAdRoles] = role.Name;
                                indexAdRoles++;
                            }
                        }
                    }

                    adRoles = adRoles ?? new string[0];
                    this.roleMap.Add(roleElt.Name, adRoles);
                    index++;
                }
            }
        }

        /// <inheritdoc />
        public override string ApplicationName
        {
            get
            {
                return null;
            }

            set
            {
            }
        }

        /// <inheritdoc />
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new ProviderException("ActiveDirectoryMapRoleProvider does not support AddUsersToRoles operation");
        }

        /// <inheritdoc />
        public override void CreateRole(string roleName)
        {
            throw new ProviderException("ActiveDirectoryMapRoleProvider does not support CreateRole operation");
        }

        /// <inheritdoc />
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new ProviderException("ActiveDirectoryMapRoleProvider does not support DeleteRole operation");
        }

        /// <inheritdoc />
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new ProviderException("ActiveDirectoryMapRoleProvider does not support FindUsersInRole operation");
        }

        /// <inheritdoc />
        public override string[] GetAllRoles()
        {
            return this.roles;
        }

        /// <inheritdoc />
        public override string[] GetRolesForUser(string username)
        {
            List<string> rolesForUser = new List<string>(this.roles.Length);
            for (int i = 0; i < this.roles.Length; ++i)
            {
                string role = this.roles[i];
                if (this.IsUserInRole(username, role))
                {
                    rolesForUser.Add(role);
                }
            }

            return rolesForUser != null ? rolesForUser.ToArray() : new string[0];
        }

        /// <inheritdoc />
        public override string[] GetUsersInRole(string roleName)
        {
            throw new ProviderException("ActiveDirectoryMapRoleProvider does not support GetUsersInRole operation");
        }

        /// <inheritdoc />
        public override bool IsUserInRole(string username, string roleName)
        {
            bool userInRole = false;
            string[] adRoles = null;

            using (UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(this.principalContext, username))
            {
                if (this.roleMap.TryGetValue(roleName, out adRoles))
                {
                    for (int i = 0; i < adRoles.Length; ++i)
                    {
                        string role = adRoles[i];
                        try
                        {
                            userInRole = userPrincipal.IsMemberOf(this.principalContext, IdentityType.Name, role);
                        }
                        catch (Exception ex)
                        {
                            Logging.Logger.Error(string.Empty, string.Empty, ex);
                        }

                        if (userInRole)
                        {
                            break;
                        }
                    }
                }
            }

            return userInRole;
        }

        /// <inheritdoc />
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new ProviderException("ActiveDirectoryMapRoleProvider does not support RemoveUsersFromRoles operation");
        }

        /// <inheritdoc />
        public override bool RoleExists(string roleName)
        {
            return Array.IndexOf<string>(this.roles, roleName) > -1;
        }
    }
}
