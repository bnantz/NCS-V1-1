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
    public class UnmanagedSecurityContextInformationProviderFixture
    {
        private Hashtable dictionary;
        private UnmanagedSecurityContextInformationProvider provider;

        [SetUp]
        public void SetUp()
        {
            dictionary = new Hashtable();
        }

        [Test]
        public void PopulateDictionaryFilledCorrectly()
        {
            provider = new UnmanagedSecurityContextInformationProvider();
            provider.PopulateDictionary(dictionary);

            Assert.AreEqual(2, dictionary.Count);
            AssertUtilities.AssertStringDoesNotContain(dictionary[SR.UnmanagedSecurity_CurrentUser] as string, SR.ExtendedPropertyError(""), "CurrentUser");
            AssertUtilities.AssertStringDoesNotContain(dictionary[SR.UnmanagedSecurity_ProcessAccountName] as string, SR.ExtendedPropertyError(""), "ProcessAccountName");
        }
    }
}

#endif