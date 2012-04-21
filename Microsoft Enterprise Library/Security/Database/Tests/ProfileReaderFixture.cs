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
using Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Tests
{
    [TestFixture]
    public class ProfileReaderFixture : ConfigurationContextFixtureBase
    {
        private GenericIdentity ident;
        private DbProfileProvider profile;
        private ProfileReader reader;

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
        public void GetEachPrimitiveFromHashtable()
        {
            bool expectedBool = true;
            string expectedString = "test";
            DateTime expectedDateTime = DateTime.Now;
            byte expectedByte = 0xA;
            char expectedChar = 'a';

            DbProfileProviderFixture.TestClass expectedObject = new DbProfileProviderFixture.TestClass();
            expectedObject.Field1 = "A";
            expectedObject.Field2 = -100;

            short expectedShort = 1;
            int expectedInt = 100;
            long expectedLong = 10101010101010;
            float expectedFloat = 10101.101f;
            double expectedDouble = 10101.10101;
            decimal expectedDecimal = 10101.101010101010101m;

            Hashtable hash = new Hashtable();
            hash["string1"] = expectedString;
            hash["bool1"] = expectedBool;
            hash["dateTime1"] = expectedDateTime;
            hash["byte1"] = expectedByte;
            hash["char1"] = expectedChar;
            hash["object1"] = expectedObject;

            hash["short1"] = expectedShort; // 16 bits
            hash["int1"] = expectedInt; // 32 bits
            hash["long1"] = expectedLong; // 64 bits
            hash["float1"] = expectedFloat; // 32bits
            hash["double1"] = expectedDouble; // 64 bits
            hash["decimal1"] = expectedDecimal; // 128 bits

            this.profile.SetProfile(this.ident, hash);

            object result = this.profile.GetProfile(this.ident);
            ProfileReader reader = new ProfileReader(result);

            bool resultBool = reader.GetBool("bool1");
            string resultString = reader.GetString("string1");
            DateTime resultDateTime = reader.GetDateTime("dateTime1");
            byte resultByte = reader.GetByte("byte1");
            char resultChar = reader.GetChar("char1");

            short resultShort = reader.GetShortInt("short1");
            int resultInt = reader.GetInt("int1");
            long resultLong = reader.GetLongInt("long1");
            float resultFloat = reader.GetFloat("float1");
            double resultDouble = reader.GetDouble("double1");
            decimal resultDecimal = reader.GetDecimal("decimal1");
            DbProfileProviderFixture.TestClass resultObject =
                (DbProfileProviderFixture.TestClass)reader.GetObject("object1");

            Assert.AreEqual(expectedBool, resultBool);
            Assert.AreEqual(expectedString, resultString);
            Assert.AreEqual(expectedDateTime, resultDateTime);
            Assert.AreEqual(expectedByte, resultByte);
            Assert.AreEqual(expectedChar, resultChar);

            Assert.AreEqual(expectedObject.Field1, resultObject.Field1);
            Assert.AreEqual(expectedObject.Field2, resultObject.Field2);

            Assert.AreEqual(expectedShort, resultShort);
            Assert.AreEqual(expectedInt, resultInt);
            Assert.AreEqual(expectedLong, resultLong);
            Assert.AreEqual(expectedFloat, resultFloat);
            Assert.AreEqual(expectedDouble, resultDouble);
            Assert.AreEqual(expectedDecimal, resultDecimal);
        }

        [Test]
        public void ReadObject()
        {
            DbProfileProviderFixture.TestClass expectedObject = new DbProfileProviderFixture.TestClass();
            expectedObject.Field1 = "A";
            expectedObject.Field2 = -100;

            this.profile.SetProfile(this.ident, expectedObject);
            reader = new ProfileReader(this.profile.GetProfile(this.ident));
            DbProfileProviderFixture.TestClass resultObject =
                (DbProfileProviderFixture.TestClass)reader.GetObject(null);

            Assert.AreEqual(expectedObject.Field1, resultObject.Field1);
            Assert.AreEqual(expectedObject.Field2, resultObject.Field2);
        }

        [Test]
        public void ReadBool()
        {
            bool expectedBool = true;
            this.profile.SetProfile(this.ident, expectedBool);
            reader = new ProfileReader(this.profile.GetProfile(this.ident));
            bool resultBool = reader.GetBool(null);
            Assert.AreEqual(expectedBool, resultBool);
        }

        [Test]
        public void ReadString()
        {
            string expectedString = "test";
            this.profile.SetProfile(this.ident, expectedString);
            reader = new ProfileReader(this.profile.GetProfile(this.ident));
            string resultString = reader.GetString(null);
            Assert.AreEqual(expectedString, resultString);
        }

        [Test]
        public void ReadDateTime()
        {
            DateTime expectedDateTime = DateTime.Now;
            this.profile.SetProfile(this.ident, expectedDateTime);
            reader = new ProfileReader(this.profile.GetProfile(this.ident));
            DateTime resultDateTime = reader.GetDateTime(null);
            Assert.AreEqual(expectedDateTime, resultDateTime);
        }

        [Test]
        public void ReadByte()
        {
            byte expectedByte = 0xA;
            this.profile.SetProfile(this.ident, expectedByte);
            reader = new ProfileReader(this.profile.GetProfile(this.ident));
            byte resultByte = reader.GetByte(null);
            Assert.AreEqual(expectedByte, resultByte);
        }

        [Test]
        public void ReadChar()
        {
            char expectedChar = 'a';
            this.profile.SetProfile(this.ident, expectedChar);
            reader = new ProfileReader(this.profile.GetProfile(this.ident));
            char resultChar = reader.GetChar(null);
            Assert.AreEqual(expectedChar, resultChar);
        }

        [Test]
        public void ReadShort()
        {
            short expectedShort = 1;
            this.profile.SetProfile(this.ident, expectedShort);
            reader = new ProfileReader(this.profile.GetProfile(this.ident));
            short resultShort = reader.GetShortInt(null);
            Assert.AreEqual(expectedShort, resultShort);
        }

        [Test]
        public void ReadInt()
        {
            int expectedInt = 100;
            this.profile.SetProfile(this.ident, expectedInt);
            reader = new ProfileReader(this.profile.GetProfile(this.ident));
            int resultInt = reader.GetInt(null);
            Assert.AreEqual(expectedInt, resultInt);
        }

        [Test]
        public void ReadLong()
        {
            long expectedLong = 10101010101010;
            this.profile.SetProfile(this.ident, expectedLong);
            reader = new ProfileReader(this.profile.GetProfile(this.ident));
            long resultLong = reader.GetLongInt(null);
            Assert.AreEqual(expectedLong, resultLong);
        }

        [Test]
        public void ReadFloat()
        {
            float expectedFloat = 10101.101f;
            this.profile.SetProfile(this.ident, expectedFloat);
            reader = new ProfileReader(this.profile.GetProfile(this.ident));
            float resultFloat = reader.GetFloat(null);
            Assert.AreEqual(expectedFloat, resultFloat);
        }

        [Test]
        public void ReadDouble()
        {
            double expectedDouble = 10101.10101;
            this.profile.SetProfile(this.ident, expectedDouble);
            reader = new ProfileReader(this.profile.GetProfile(this.ident));
            double resultDouble = reader.GetDouble(null);
            Assert.AreEqual(expectedDouble, resultDouble);
        }

        [Test]
        public void ReadDecimal()
        {
            decimal expectedDecimal = 10101.101010101010101m;
            this.profile.SetProfile(this.ident, expectedDecimal);
            reader = new ProfileReader(this.profile.GetProfile(this.ident));
            decimal resultDecimal = reader.GetDecimal(null);
            Assert.AreEqual(expectedDecimal, resultDecimal);
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void InvalidDateTimeCast()
        {
            this.profile.SetProfile(this.ident, "INVALIDDATE");
            reader = new ProfileReader(this.profile.GetProfile(this.ident));
            reader.GetDateTime(null);
        }
    }
}

#endif