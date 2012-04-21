//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Specifies the <see cref="Type"/> that a node references.</para>
    /// </summary>	
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public sealed class ReferenceTypeAttribute : Attribute
    {

        private Type referenceType;

        /// <summary>
        /// <para>Initialzie a new instance of the <see cref="ReferenceTypeAttribute"/> class with type to reference.</para>
        /// </summary>
        /// <param name="referenceType">
        /// <para>The <see cref="Type"/> of the reference.</para>
        /// </param>
        public ReferenceTypeAttribute(Type referenceType) : base()
        {
            this.referenceType = referenceType;
        }

        /// <summary>
        /// <para>Gets the <see cref="Type"/> to reference.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="Type"/> to reference.</para>
        /// </value>
        public Type ReferenceType
        {
            get { return referenceType; }
        }
    }
}