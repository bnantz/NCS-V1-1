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
using System;
using System.Collections;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Tests
{
    [TestFixture]
    public class DbProfileProviderBoundsFixture : ConfigurationContextFixtureBase
    {
        private GenericIdentity ident;
        private IProfileProvider profile;
        private string veryLongString;
        
        public override void FixtureSetup()
        {
            base.FixtureSetup();
            TestDataSetup.CreateTestData(Context);
        }

        public override void FixtureTeardown()
        {
            TestDataSetup.DeleteTestData(Context);
            base.FixtureTeardown();
        }

        [SetUp]
        public void Setup()
        {
            this.ident = new GenericIdentity("testuser");
            this.profile = ProfileFactory.GetProfileProvider("MyDbProfile1") as DbProfileProvider;
            this.veryLongString = "testing123".PadLeft(10000, 'X');
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetNullValue()
        {
            this.profile.SetProfile(this.ident, null);
            object result = this.profile.GetProfile(this.ident);

            Assert.AreEqual(null, result);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceedMaximumLength()
        {
            Assert.AreEqual(10000, veryLongString.Length);
            this.profile.SetProfile(this.ident, veryLongString);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceedMaximumLengthInHash()
        {
            Hashtable hash = new Hashtable();
            hash["key1"] = veryLongString;
            this.profile.SetProfile(this.ident, hash);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceedMaximumLengthInObject()
        {
            DbProfileProviderFixture.TestClass profileObject1 = new DbProfileProviderFixture.TestClass();
            profileObject1.Field1 = veryLongString;

            this.profile.SetProfile(this.ident, profileObject1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceedMaximumLengthInObjectInHash()
        {
            DbProfileProviderFixture.TestClass profileObject1 = new DbProfileProviderFixture.TestClass();
            profileObject1.Field1 = veryLongString;

            Hashtable hash = new Hashtable();
            hash["key1"] = profileObject1;

            this.profile.SetProfile(this.ident, hash);
        }
    }
}

#endif