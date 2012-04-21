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

#if UNIT_TESTS
using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [XmlRoot("storageProvider", Namespace=ConfigurationSettings.ConfigurationNamespace)]
    public class ReadOnlyStorageProviderData : StorageProviderData
    {
        private NameValueItemCollection extensions;

        public ReadOnlyStorageProviderData() : this(string.Empty)
        {
        }

        public ReadOnlyStorageProviderData(string name) : base(name)
        {
            this.extensions = new NameValueItemCollection();
        }

        [XmlArray(ElementName="extensions")]
        [XmlArrayItem(ElementName="extension", Type=typeof(NameValueItem))]
        public NameValueItemCollection Extensions
        {
            get { return this.extensions; }
        }

        public override object Clone()
        {
            return new ReadOnlyStorageProviderData(Name);
        }

        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(ReadOnlyStorageProvider).AssemblyQualifiedName; }
            set
            {
            }
        }

    }
}

#endif