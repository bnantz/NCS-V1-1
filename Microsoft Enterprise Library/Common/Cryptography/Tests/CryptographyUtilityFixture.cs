//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Library Shared Library
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Cryptography.Tests
{
    [TestFixture]
    public class CryptographyUtilityFixture
    {
        private const string EventLogName = "Application";
        private const string EventLogSource = "Enterprise Library Cryptography";

        [Test]
        public void LogHandlingError()
        {
            Exception ex = new Exception("Unable to init crypto");
            CryptographyUtility.LogCryptographyException(ex);

            using (EventLog log = new EventLog(EventLogName))
            {
                EventLogEntry entry = log.Entries[log.Entries.Count - 1];

                Assert.AreEqual(EventLogEntryType.Error, entry.EntryType);
                Assert.AreEqual(EventLogSource, entry.Source);
            }
        }

        [Test]
        public void GetBytesFromValidHexString()
        {
            string hex = "00FF";

            byte[] hexByte = CryptographyUtility.GetBytesFromHexString(hex);

            Assert.AreEqual(0, hexByte[0]);
            Assert.AreEqual(255, hexByte[1]);
        }

        [Test]
        public void GetBytesFromValidPrefixedHexString()
        {
            string hex = "0x00FF";

            byte[] hexByte = CryptographyUtility.GetBytesFromHexString(hex);

            Assert.AreEqual(0, hexByte[0]);
            Assert.AreEqual(255, hexByte[1]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetBytesFromInvalidLengthHexString()
        {
            string hex = "0FF";

            CryptographyUtility.GetBytesFromHexString(hex);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetBytesFromInvalidCharactersHexString()
        {
            string hex = "Invalid!";

            CryptographyUtility.GetBytesFromHexString(hex);
        }

        [Test]
        public void GetHexStringFromBytes()
        {
            byte[] bytes = new byte[] {0, 255};
            string hex = CryptographyUtility.GetHexStringFromBytes(bytes);
            Assert.AreEqual("00FF", hex);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetHexStringFromZeroBytes()
        {
            CryptographyUtility.GetHexStringFromBytes(new byte[0]);
        }

        [Test]
        public void CombineBytes()
        {
            byte[] test1 = new byte[] {0, 1, 2, 3};
            byte[] test2 = new byte[] {0, 1, 2, 3};

            byte[] combinedBytes = CryptographyUtility.CombineBytes(test1, test2);
            Assert.AreEqual(test1.Length + test2.Length, combinedBytes.Length);
        }

        [Test]
        public void GenerateRandomBytes()
        {
            int rndSize = 16;
            byte[] rnd1 = CryptographyUtility.GetRandomBytes(rndSize);
            byte[] rnd2 = CryptographyUtility.GetRandomBytes(rndSize);

            Assert.IsFalse(CryptographyUtility.CompareBytes(rnd1, rnd2));
        }
    }
}

#endif