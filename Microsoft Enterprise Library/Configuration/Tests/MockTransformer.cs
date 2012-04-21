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

#if   UNIT_TESTS
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    public class MockTransformer : TransformerProvider
    {
        private XmlSerializer serializer;

        public MockTransformer()
        {
        }

        public override object Deserialize(object configSection)
        {
            XmlNode node = configSection as XmlNode;
            return serializer.Deserialize(new StringReader(node.OuterXml));

        }

        public override object Serialize(object value)
        {
            StringWriter sw = new StringWriter(CultureInfo.CurrentUICulture);
            serializer.Serialize(sw, value);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sw.ToString());
            return doc.DocumentElement;
        }

        public override void Initialize(ConfigurationView configurationView)
        {
            serializer = new XmlSerializer(typeof(MockConfigurationData));
        }
    }
}

#endif