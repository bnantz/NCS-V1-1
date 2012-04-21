//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Tests
{
    [TestFixture]
    public class AzManAuthProviderDataFixture
    {
        private static readonly string applicationName = "MyAzManApp";
        private static readonly string nodeName = "AzManProvider";
        private static readonly string nodeType = typeof(AzManAuthorizationProvider).AssemblyQualifiedName;
        private static readonly string storeLocation = "msxml://c:/test/test/test.xml";
        private static readonly string auditIdPrefix = "AzMan:";

        //		private static readonly string xmlString =  
        //			"<authorizationProvider " + 
        //				"xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
        //				"xsi:type=\"AzManAuthorizationProviderData\" " + 
        //				"name=\"" + nodeName + "\" " + 
        //				"type=\"" + nodeType + "\" " + 
        //				"storeLocation=\"" + storeLocation + "\" " + 
        //				"application=\"" + applicationName + "\"/>";
        //
        //		private AzManAuthorizationProviderData provider; 
        //
        //		[TestFixtureSetUp]
        //		public void Init() 
        //		{
        //			XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
        //			XmlSerializer xmlSerializer = new XmlSerializer(typeof(AuthorizationProviderData));			
        //			this.provider = xmlSerializer.Deserialize(xmlReader) as AzManAuthorizationProviderData;
        //		}
        //
        //		[Test]
        //		public void DeserializeTest() 
        //		{
        //			Assert.IsNotNull(this.provider);
        //		}

        [Test]
        public void Properties()
        {
            AzManAuthorizationProviderData data = new AzManAuthorizationProviderData();
            data.Name = nodeName;
            data.TypeName = nodeType;
            data.Application = applicationName;
            data.StoreLocation = storeLocation;
            data.AuditIdentifierPrefix = auditIdPrefix;

            Assert.AreEqual(nodeName, data.Name);
            Assert.AreEqual(nodeType, data.TypeName);
            Assert.AreEqual(applicationName, data.Application);
            Assert.AreEqual(storeLocation, data.StoreLocation);
            Assert.AreEqual(auditIdPrefix, data.AuditIdentifierPrefix);
        }
    }
}

#endif