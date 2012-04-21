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
using System;
using System.ComponentModel;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class NodeNameCreationServiceFixture
    {
        private NodeNameCreationService nameCreationService;

        [SetUp]
        public void Setup()
        {
            nameCreationService = new NodeNameCreationService();
        }

        [Test]
        public void CreateNameTest()
        {
            string name = nameCreationService.CreateName(new Container(), typeof(ConfigurationNode));
            Assert.AreEqual(name, String.Concat(typeof(ConfigurationNode).Name, "1"));
        }

        [Test]
        public void IsValidNameWithUnderScoreTest()
        {
            string name = "_test";
            Assert.IsFalse(nameCreationService.IsValidName(name));
        }

        [Test]
        public void IsValidNameWithoutAllCharsOrDigitsTest()
        {
            string name = "te@st";
            Assert.IsFalse(nameCreationService.IsValidName(name));
        }

        [Test]
        public void IsValidNameTest()
        {
            string name = "te1st";
            Assert.IsTrue(nameCreationService.IsValidName(name));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValiddateNameWithUnderScoreTest()
        {
            string name = "_test";
            nameCreationService.ValidateName(name);
            Assert.Fail("Should have thrown exception because the name is invalid.");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValidateNameWithoutAllCharsOrDigitsTest()
        {
            string name = "te@st";
            nameCreationService.ValidateName(name);
        }

        [Test]
        public void ValidateNameTest()
        {
            string name = "te1st";
            nameCreationService.ValidateName(name);
        }

    }
}

#endif