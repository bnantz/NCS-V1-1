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
    /// <para>
    /// Indicates the base class or interface that must be assignable from the type specified in the property that this attribute decorates.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public sealed class BaseTypeAttribute : Attribute
    {
        private readonly Type baseType;
        private readonly TypeSelectorIncludeFlags flags;

        /// <summary>
        /// <para>Initializes a new instance of the  <see cref="BaseTypeAttribute"/> class with the specified <see cref="Type"/> object.</para>
        /// </summary>
        /// <param name="baseType">
        /// <para>The <see cref="Type"/> to filter selections.</para>
        /// </param>
        public BaseTypeAttribute(Type baseType) : this(baseType, TypeSelectorIncludeFlags.Default)
        {
        }

        /// <summary>
        /// <para>
        /// Initializes a new instance of the <see cref="BaseTypeAttribute"/> class with the specified <see cref="Type"/> object and <see cref="TypeSelectorIncludeFlags"/>.
        /// </para>
        /// </summary>
        /// <param name="baseType">
        /// <para>The <see cref="Type"/> to filter selections.</para>
        /// </param>
        /// <param name="flags">
        /// <para>One of the <see cref="TypeSelectorIncludeFlags"/> values.</para>
        /// </param>
        public BaseTypeAttribute(Type baseType, TypeSelectorIncludeFlags flags) : base()
        {
            this.baseType = baseType;
            this.flags = flags;
        }

        /// <summary>
        /// <para>Gets the <see cref="Type"/> to filter selections.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="Type"/> to filter selections.</para>
        /// </value>
        public Type BaseType
        {
            get { return this.baseType; }
        }

        /// <summary>
        /// <para>Gets the <see cref="TypeSelectorIncludeFlags"/> set for the filter.</para>
        /// </summary>
        /// <value>
        /// <para>One of the <see cref="TypeSelectorIncludeFlags"/> values.</para>
        /// </value>
        public TypeSelectorIncludeFlags IncludeFlags
        {
            get { return this.flags; }
        }
    }
}