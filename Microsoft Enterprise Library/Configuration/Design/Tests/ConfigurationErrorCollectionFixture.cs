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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class ConfigurationErrorCollectionFixture
    {
        private ConfigurationErrorCollection errors;

        [SetUp]
        public void SetUp()
        {
            errors = new ConfigurationErrorCollection();
        }

        [Test]
        public void AddTest()
        {
            ConfigurationError error = new ConfigurationError(new MyNode(), "Argument Exception");
            errors.Add(error);
            Assert.AreEqual(1, errors.Count);
            Assert.AreSame(error, errors[0]);
        }

        [Test]
        public void AddRangeTest()
        {
            errors.Add(new ConfigurationError(new MyNode(), "Argument Exception"));
            errors.Add(new ConfigurationError(new MyNode(), "Argument Exception"));
            ConfigurationErrorCollection errorCollection = new ConfigurationErrorCollection();
            errorCollection.AddRange(errors);
            Assert.AreEqual(2, errorCollection.Count);
        }

        [Test]
        public void AddNullTest()
        {
            errors.Add(null);
        }

        [Test]
        public void AddCreateTest()
        {
            errors.Add(new MyNode(), "Argument Exception");
            Assert.AreEqual(1, errors.Count);
            ConfigurationError error = errors[0];
            Assert.AreEqual("Argument Exception", error.Message);
        }

        private class MyNode : ConfigurationNode
        {
            public MyNode() : base()
            {
            }
        }
    }
}

#endif