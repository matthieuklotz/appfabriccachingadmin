// -----------------------------------------------------------------------
// <copyright file="ObjectDisposedValidatorAspect.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Common.Injection
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using PostSharp.Aspects;
    using PostSharp.Aspects.Advices;
    using PostSharp.Extensibility;

    /// <summary>
    /// A class attribute for throwing an <see cref="ObjectDisposedException"/> if the object has been disposed.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    [MulticastAttributeUsage(MulticastTargets.Class, Inheritance = MulticastInheritance.Multicast)]
    public class ObjectDisposedValidatorAspect : InstanceLevelAspect
    {
        /// <summary>
        /// Imported disposed property.
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Required by Postsharp")]
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Required by Postsharp")]
        [ImportMember("Disposed", IsRequired = true)]
        public Property<bool> disposed;

        /// <summary>
        /// Called for all method of the class that uses this attribute.
        /// </summary>
        /// <param name="args">The method call arguments.</param>
        [OnMethodEntryAdvice, MulticastPointcut(Targets = MulticastTargets.Method, Attributes = MulticastAttributes.Instance | MulticastAttributes.Private | MulticastAttributes.Public)]
        public void OnEntry(MethodExecutionArgs args)
        {
            Contract.Requires<ArgumentNullException>(args != null, "args");
            Contract.Requires<ArgumentException>(args.Instance != null, "args.Instance is null");
            Contract.Requires<ArgumentException>(args.Method != null, "args.Method is null");
            MethodBase method = args.Method;
            if (string.Equals(method.Name, "get_Disposed", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            bool value = this.disposed.Get();
            if (value)
            {
                Type instanceType = args.Instance.GetType();
                throw new ObjectDisposedException(instanceType.FullName);
            }
        }
    }
}
