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
    public class ExceptionConstructorStorageProviderData : StorageProviderData
    {
        private NameValueItemCollection extensions;

        public ExceptionConstructorStorageProviderData() : this(string.Empty)
        {
        }

        public ExceptionConstructorStorageProviderData(string name) : base(name)
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
            return new NotRealTransformerData(Name);
        }

        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(ExceptionConstructorStorageProvider).AssemblyQualifiedName; }
            set
            {
            }
        }

    }
}

#endif