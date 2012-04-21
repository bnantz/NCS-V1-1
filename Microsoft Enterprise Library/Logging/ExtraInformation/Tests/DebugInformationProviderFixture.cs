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
    public class DebugInformationProviderFixture
    {
        private Hashtable dictionary;
        private DebugInformationProvider provider;

        [SetUp]
        public void SetUp()
        {
            dictionary = new Hashtable();
        }

        [Test]
        public void PopulateDictionaryFilledCorrectly()
        {
            provider = new DebugInformationProvider();
            provider.PopulateDictionary(dictionary);

            Assert.IsTrue(dictionary.Count > 0, "Dictionary contains no items");
            AssertUtilities.AssertStringDoesNotContain(dictionary[SR.DebugInfo_StackTrace] as string, SR.ExtendedPropertyError(""), "Stack trace");
        }

        [Test]
        public void PopulateDictionaryFilledWithSecurityException()
        {
            provider = new DebugInformationProvider(new MockDebugUtilsThrowsSecurityException());
            provider.PopulateDictionary(dictionary);

            Assert.IsTrue(dictionary.Count > 0, "Dictionary contains no items");
            Assert.AreEqual(dictionary[SR.DebugInfo_StackTrace], SR.ExtendedPropertyError(SR.DebugInfo_StackTraceSecurityException));
        }

        [Test]
        public void PopulateDictionaryFilledWithGenericException()
        {
            provider = new DebugInformationProvider(new MockDebugUtilsThrowsNonSecurityException());
            provider.PopulateDictionary(dictionary);

            Assert.IsTrue(dictionary.Count > 0, "Dictionary contains no items");
            Assert.AreEqual(dictionary[SR.DebugInfo_StackTrace], SR.ExtendedPropertyError(SR.DebugInfo_StackTraceException));
        }

    }
}

#endif