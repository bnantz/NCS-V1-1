//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests
{
    [XmlRoot("formatter", Namespace=DistributorSettings.ConfigurationNamespace)]
    public class CustomTextFormatterData : TextFormatterData
    {
        public CustomTextFormatterData()
        {
        }

        public override string TypeName
        {
            get { return typeof(CustomTextFormatter).AssemblyQualifiedName; }
        }
    }
}

#endif