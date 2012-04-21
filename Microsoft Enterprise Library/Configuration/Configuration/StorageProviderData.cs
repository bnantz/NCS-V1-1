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
using System.Xml.Serialization;
#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents a storage provider to read and write data in configuration.</para>
    /// </summary>
    /// <remarks>
    /// <para>The class maps to the <c>storageProvider</c> element in configuration.</para>
    /// </remarks>
    [XmlRoot("storageProvider", Namespace=ConfigurationSettings.ConfigurationNamespace)]
    [XmlInclude(typeof(XmlFileStorageProviderData))]
    [XmlInclude(typeof(CustomStorageProviderData))]
#if UNIT_TESTS
    [XmlInclude(typeof(ReadOnlyStorageProviderData))]
    [XmlInclude(typeof(MockStorageProviderData))]
    [XmlInclude(typeof(NotRealStorageProviderData))]
    [XmlInclude(typeof(ExceptionConstructorStorageProviderData))]
    [XmlInclude(typeof(PrivateConstructorStorageProviderData))]
#endif
        public abstract class StorageProviderData : ProviderData, ICloneable
    {
        private string sectionName;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="StorageProviderData"/> class..</para>
        /// </summary>
        protected StorageProviderData() : this(string.Empty)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="StorageProviderData"/> class with a name and fully qualified type name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the storage provider.</para>
        /// </param>
        protected StorageProviderData(string name) : this(name, string.Empty)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="StorageProviderData"/> class with a name and fully qualified type name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the storage provider.</para>
        /// </param>
        /// <param name="sectionName">
        /// <para>The section name for the configuration.</para>
        /// </param>
        protected StorageProviderData(string name, string sectionName) : base(name)
        {
            this.sectionName = sectionName;
        }

        /// <summary>
        /// <para>Gets or sets the section name for the configuration.</para>
        /// </summary>
        /// <value>
        /// <para>The section name for the configuration.</para>
        /// </value>
        [XmlIgnore]
        public string SectionName
        {
            get { return this.sectionName; }
            set { this.sectionName = value; }
        }

        /// <summary>
        /// <para>Creates a new object that is a copy of the current instance.</para>
        /// </summary>
        /// <returns>
        /// <para>A new object that is a copy of this instance.</para>
        /// </returns>
        public abstract object Clone();

    }
}