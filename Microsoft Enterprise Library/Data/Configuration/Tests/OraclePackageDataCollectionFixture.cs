//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if   UNIT_TESTS
using System;
using System.Collections;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Tests
{
    [TestFixture]
    public class OraclePackageDataCollectionFixture
    {
        private OraclePackageDataCollection oraclePackages;

        [SetUp]
        public void SetUp()
        {
            oraclePackages = new OraclePackageDataCollection();
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void AddInvalidTypeObject()
        {
            oraclePackages.Add("foo");
        }

        [Test]
        public void EnumeratorCurrentTest()
        {
            OraclePackageData oraclePackage = new OraclePackageData();
            oraclePackage.Name = "MyName";
            oraclePackage.Prefix = "MyPrefix";
            oraclePackages.Add(oraclePackage);
            OraclePackageData oraclePackage2 = new OraclePackageData();
            oraclePackage2.Name = "MyName2";
            oraclePackage2.Prefix = "MyPrefix2";
            oraclePackages.Add(oraclePackage2);
            int count = 0;
            foreach (OraclePackageData dbType in oraclePackages)
            {
                Assert.IsNotNull(dbType);
                count++;
                foreach (OraclePackageData dbType2 in oraclePackages)
                {
                    Assert.IsNotNull(dbType2);
                    count++;
                }
            }
            Assert.AreEqual(6, count);
        }

        [Test]
        public void AddRemoveTest()
        {
            OraclePackageData oraclePackage = new OraclePackageData();
            oraclePackage.Name = "MyName";
            oraclePackages.Add(oraclePackage);
            Assert.AreEqual(1, oraclePackages.Count);
            oraclePackages.Remove(oraclePackage.Name);
        }

        [Test]
        public void AddAsObjectTest()
        {
            OraclePackageData oraclePackage = new OraclePackageData();
            oraclePackage.Name = "MyName";
            oraclePackages.Add((Object)oraclePackage);
        }

        [Test]
        public void AddItemTest()
        {
            OraclePackageData oraclePackage = new OraclePackageData();
            oraclePackage.Name = "MyName";
            oraclePackages[oraclePackage.Name] = oraclePackage;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveNullObjectTest()
        {
            oraclePackages.Remove(null);
        }

        [Test]
        public void ItemTest()
        {
            OraclePackageData oraclePackage = new OraclePackageData();
            oraclePackage.Name = "MyName";
            oraclePackages.Add(oraclePackage);
            Assert.AreEqual(1, oraclePackages.Count);
        }

        [Test]
        public void ClearTest()
        {
            OraclePackageData oraclePackage = new OraclePackageData();
            oraclePackage.Name = "MyName";
            oraclePackages.Add(oraclePackage);
            Assert.AreEqual(1, oraclePackages.Count);
            oraclePackages.Clear();
            Assert.AreEqual(0, oraclePackages.Count);
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void AddObjectTest()
        {
            oraclePackages.Add(new object());
        }

        [Test]
        public void EnumeratorTest()
        {
            OraclePackageData oraclePackage = new OraclePackageData();
            oraclePackage.Name = "MyName";
            oraclePackages.Add(oraclePackage);
            IEnumerator enumerator = oraclePackages.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                Assert.IsNotNull(enumerator.Current);
            }
        }
    }
}

#endif