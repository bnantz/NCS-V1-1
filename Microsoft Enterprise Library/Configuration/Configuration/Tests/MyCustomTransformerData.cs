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
    [XmlRoot("dataTransformer", Namespace=ConfigurationSettings.ConfigurationNamespace)]
    public class MyCustomTransformerData : CustomTransformerData
    {
        public MyCustomTransformerData() : base()
        {
        }
    }
}

#endif
