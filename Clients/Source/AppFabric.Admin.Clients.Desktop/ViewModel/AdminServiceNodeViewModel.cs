﻿// -----------------------------------------------------------------------
// <copyright file="AdminServiceNodeViewModel.cs" company="Matthieu Klotz">
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
    using AppFabric.Admin.Clients.Common;

    public abstract class AdminServiceNodeViewModel : NodeViewModel, IAdminServiceViewModel
    {
        private IAdministrationServiceClient service;

        private object syncService = new object();

        protected AdminServiceNodeViewModel(NodeViewModel parent)
            : base(parent)
        {
        }

        /// <inheritdoc />
        public virtual IAdministrationServiceClient Service
        {
            get
            {
                if (this.service == null)
                {
                    lock (this.syncService)
                    {
                        if (this.service == null)
                        {
                            this.service = this.InitializeService();
                        }
                    }
                }

                return this.service;
            }

            set
            {
                if (this.service != value)
                {
                    this.service = value;
                }
            }
        }

        /// <inheritdoc />
        public virtual IAdministrationServiceClient InitializeService()
        {
            IAdministrationServiceClient adminService = null;
            NodeViewModel nodeVm = this.Parent;
            if (nodeVm != null)
            {
                IAdminServiceViewModel adminServiceViewModel = nodeVm as IAdminServiceViewModel;
                while (nodeVm.Parent != null && adminServiceViewModel == null)
                {
                    nodeVm = nodeVm.Parent;
                    adminServiceViewModel = nodeVm as IAdminServiceViewModel;
                }

                if (adminServiceViewModel != null)
                {
                    adminService = adminServiceViewModel.Service;
                }
            }

            return adminService;
        }
    }
}
