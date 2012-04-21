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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Tests
{
    [TestFixture]
    public class DbProfileProviderFixture : ConfigurationContextFixtureBase
    {
        private GenericIdentity ident;
        private DbProfileProvider profile;
        
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
            DbProfileProviderData data = new DbProfileProviderData("name", "EntLibSecurity");
            this.profile = new DbProfileProvider();
            profile.Initialize(new TestProfileConfigurationView(data, Context));
        }

        [Test]
        public void CreateFromFactory()
        {
            IProfileProvider dbProfile = ProfileFactory.GetProfileProvider("MyDbProfile1");
            dbProfile.SetProfile(this.ident, "test123");
            string result = dbProfile.GetProfile(this.ident).ToString();
            Assert.AreEqual("test123", result);
        }

        [Test]
        public void SimpleStringProfile()
        {
            string profileString = "test";
            this.profile.SetProfile(this.ident, profileString);
            string result = (string)this.profile.GetProfile(this.ident);

            Assert.AreEqual(profileString, result);
        }

        [Test]
        public void DictionaryOfStrings()
        {
            Hashtable hash = new Hashtable();
            hash["key1"] = "value1";
            hash["key2"] = "value2";
            hash["key3"] = "value3";

            this.profile.SetProfile(this.ident, hash);

            IDictionary result = (IDictionary)this.profile.GetProfile(this.ident);

            Assert.AreEqual(hash.Count, result.Count, "item count");
            Assert.AreEqual(hash["key1"], result["key1"], "value1");
            Assert.AreEqual(hash["key2"], result["key2"], "value2");
            Assert.AreEqual(hash["key3"], result["key3"], "value3");
        }

        [Test]
        public void DictionaryOfMixedPrimitives()
        {
            Hashtable hash = GetPrimitiveHash();
            this.profile.SetProfile(this.ident, hash);

            IDictionary result = (IDictionary)this.profile.GetProfile(this.ident);

            Assert.AreEqual(hash.Count, result.Count, "item count");
            Assert.AreEqual(hash["string1"], result["string1"], "value1");
            Assert.AreEqual(hash["int1"], Convert.ToInt32(result["int1"]), "value2");
            Assert.AreEqual(hash["bool1"], Convert.ToBoolean(result["bool1"]), "value3");
        }

        [Serializable]
        public class TestClass
        {
            public string Field1;
            public int Field2;
            public bool Field3;
        }

        [Test]
        public void SerializableObject()
        {
            TestClass profileObject = new TestClass();
            profileObject.Field1 = "apples";
            profileObject.Field2 = 100;
            profileObject.Field3 = false;

            this.profile.SetProfile(this.ident, profileObject);

            TestClass result = (TestClass)this.profile.GetProfile(this.ident);

            Assert.AreEqual(profileObject.Field1, result.Field1);
            Assert.AreEqual(profileObject.Field2, result.Field2);
            Assert.AreEqual(profileObject.Field3, result.Field3);
        }

        [Test]
        public void DictionaryOfObjects()
        {
            TestClass profileObject1 = new TestClass();
            profileObject1.Field1 = "apples";
            profileObject1.Field2 = 100;
            profileObject1.Field3 = false;

            TestClass profileObject2 = new TestClass();
            profileObject2.Field1 = "oranges";
            profileObject2.Field2 = 200;
            profileObject2.Field3 = true;

            Hashtable hash = new Hashtable();
            hash["object1"] = profileObject1;
            hash["object2"] = profileObject2;

            this.profile.SetProfile(this.ident, hash);

            IDictionary result = (IDictionary)this.profile.GetProfile(this.ident);

            Assert.AreEqual(hash.Count, result.Count, "item count");
            TestClass resultObject1 = (TestClass)result["object1"];
            Assert.AreEqual(profileObject1.Field1, resultObject1.Field1);
            Assert.AreEqual(profileObject1.Field2, resultObject1.Field2);
            Assert.AreEqual(profileObject1.Field3, resultObject1.Field3);

            TestClass resultObject2 = (TestClass)result["object2"];
            Assert.AreEqual(profileObject2.Field1, resultObject2.Field1);
            Assert.AreEqual(profileObject2.Field2, resultObject2.Field2);
            Assert.AreEqual(profileObject2.Field3, resultObject2.Field3);
        }

        [Test]
        public void DictionaryOfObjectsAndPrimitives()
        {
            TestClass profileObject1 = new TestClass();
            profileObject1.Field1 = "apples";
            profileObject1.Field2 = 100;
            profileObject1.Field3 = false;

            Hashtable hash = GetPrimitiveHash();
            hash["object1"] = profileObject1;

            this.profile.SetProfile(this.ident, hash);

            IDictionary result = (IDictionary)this.profile.GetProfile(this.ident);

            Assert.AreEqual(hash.Count, result.Count, "item count");
            TestClass resultObject1 = (TestClass)result["object1"];
            Assert.AreEqual(profileObject1.Field1, resultObject1.Field1);
            Assert.AreEqual(profileObject1.Field2, resultObject1.Field2);
            Assert.AreEqual(profileObject1.Field3, resultObject1.Field3);
            Assert.AreEqual(hash["string1"], result["string1"], "value1");
            Assert.AreEqual(hash["int1"], Convert.ToInt32(result["int1"]), "value2");
            Assert.AreEqual(hash["bool1"], Convert.ToBoolean(result["bool1"]), "value3");
        }

        [Test]
        public void DictionaryWithOnePrimitive()
        {
            Hashtable hash = new Hashtable();
            hash["key1"] = "test123";

            this.profile.SetProfile(this.ident, hash);
            IDictionary result = (IDictionary)this.profile.GetProfile(this.ident);

            Assert.IsNotNull(result);
            Assert.AreEqual(result["key1"], hash["key1"]);
        }

        [Test]
        public void DictionaryWithOneSerializedObject()
        {
            Hashtable hash = new Hashtable();
            TestClass profileObject1 = new TestClass();
            profileObject1.Field1 = "apples";
            profileObject1.Field2 = 100;
            profileObject1.Field3 = false;

            hash["key1"] = profileObject1;

            this.profile.SetProfile(this.ident, hash);
            IDictionary result = (IDictionary)this.profile.GetProfile(this.ident);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            TestClass resultObject1 = (TestClass)result["key1"];
            Assert.AreEqual(profileObject1.Field1, resultObject1.Field1);
            Assert.AreEqual(profileObject1.Field2, resultObject1.Field2);
            Assert.AreEqual(profileObject1.Field3, resultObject1.Field3);
        }

        private Hashtable GetPrimitiveHash()
        {
            Hashtable hash = new Hashtable();
            hash["string1"] = "test";
            hash["int1"] = 100;
            hash["bool1"] = true;

            return hash;
        }

        [Test]
        public void DictionaryWithNullValues()
        {
            Hashtable hash = new Hashtable();
            hash["key1"] = null;
            this.profile.SetProfile(this.ident, hash);
            IDictionary result = (IDictionary)this.profile.GetProfile(this.ident);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsNull(result["key1"]);
        }

        [Test]
        public void DictionaryWithMultipleValues()
        {
            Hashtable hash = new Hashtable();
            hash["key1"] = null;
            hash["key2"] = "value2";
            hash["key3"] = "value3";

            // Set the profile
            this.profile.SetProfile(this.ident, hash);

            // Retreive a user's profile from the database.
            Hashtable hashtable = this.profile.GetProfile(this.ident) as Hashtable;
            Assert.IsNull(hashtable["key1"]);
        }

        [Test]
        public void SetProfileForMultipleUsers()
        {
            GenericIdentity ident1 = new GenericIdentity("testuser");
            GenericIdentity ident2 = new GenericIdentity("bogususer");

            string profile1 = "test1";
            string profile2 = "test2";

            this.profile.SetProfile(ident1, profile1);
            this.profile.SetProfile(ident2, profile2);

            string result1 = (string)this.profile.GetProfile(ident1);
            string result2 = (string)this.profile.GetProfile(ident2);

            Assert.AreEqual(profile1, result1);
            Assert.AreEqual(profile2, result2);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetProfileForInvalidUser()
        {
            GenericIdentity ident1 = new GenericIdentity("INVALID");
            this.profile.SetProfile(ident1, "test");
        }

        [Test]
        public void GetProfileForInvalidUser()
        {
            GenericIdentity ident1 = new GenericIdentity("INVALID");
            object result = this.profile.GetProfile(ident1);
            Assert.IsNull(result);
        }

        [Test]
        public void DeleteUserWithAProfile()
        {
            UserRoleManager urm = new UserRoleManager("EntLibSecurity", Context);
            urm.CreateUser("testuser3", ASCIIEncoding.ASCII.GetBytes("Password"));

            GenericIdentity ident1 = new GenericIdentity("testuser3");

            this.profile.SetProfile(ident1, "test");

            try
            {
                urm.DeleteUser("testuser3");
            }
            catch (Exception ex)
            {
                Assert.Fail("no exception should occur when deleting a user: " + ex.Message);
            }
        }

    }
}

#endif