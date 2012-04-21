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
    public class MockStorageProviderData : StorageProviderData
    {
        private NameValueItemCollection attributes;
        private string typeName;

        public MockStorageProviderData() : this(string.Empty)
        {
        }

        public MockStorageProviderData(string name) : this(name, string.Empty)
        {
        }

        public MockStorageProviderData(string name, string typeName) : base(name, typeName)
        {
            this.typeName = typeName;
            this.attributes = new NameValueItemCollection();
        }

        [XmlArray("attributes")]
        [XmlArrayItem("attribute", typeof(NameValueItem))]
        public NameValueItemCollection Attributes
        {
            get { return this.attributes; }
        }

        public override object Clone()
        {
            return new MockStorageProviderData(Name, TypeName);
        }

        public override string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

    }
}

#endif