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
    /// <summary>
    /// <para>Represents a custom storage provider to read and write data in configuration.</para>
    /// </summary>
    /// <remarks>
    /// <para>The class maps to the <c>storageProvider</c> element in configuration.</para>
    /// </remarks>
    [XmlRoot("storageProvider", Namespace=ConfigurationSettings.ConfigurationNamespace)]
    public class MyCustomStorageProviderData : CustomStorageProviderData
    {
        public MyCustomStorageProviderData() : base()
        {
        }
    }
}

#endif
