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

using System.Collections;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Tests
{
    [TestFixture]
    public class ManagedSecurityContextInformationProviderFixture
    {
        private Hashtable dictionary;
        private ManagedSecurityContextInformationProvider provider;

        [SetUp]
        public void SetUp()
        {
            dictionary = new Hashtable();
        }

        [Test]
        public void PopulateDictionaryFilledCorrectly()
        {
            provider = new ManagedSecurityContextInformationProvider();
            provider.PopulateDictionary(dictionary);

            Assert.IsTrue(dictionary.Count > 0, "Dictionary contains no items");
            AssertUtilities.AssertStringDoesNotContain(dictionary[SR.ManagedSecurity_AuthenticationType] as string, SR.ExtendedPropertyError(""), "Authentication Type");
            AssertUtilities.AssertStringDoesNotContain(dictionary[SR.ManagedSecurity_IdentityName] as string, SR.ExtendedPropertyError(""), "Identity Name");
            AssertUtilities.AssertStringDoesNotContain(dictionary[SR.ManagedSecurity_IsAuthenticated] as string, SR.ExtendedPropertyError(""), "Is Authenticated");
        }
    }
}

#endif