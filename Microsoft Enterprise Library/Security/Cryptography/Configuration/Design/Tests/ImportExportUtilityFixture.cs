//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © 2004 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Tests 
{
    [TestFixture]
    public class ImportExportUtilityFixture 
    {
        private string keyFileName = "./Key.out";

        [Test]
        public void CanExportAndReimportTextWithNoPassphrase()
        {
            byte[] key = GenerateKey();

            ImportExportUtility utility = new ImportExportUtility();
            utility.Export(key, keyFileName, "");

            byte[] returnedKey = utility.Import(keyFileName, "");

            ArraysEqual(key, returnedKey);
        }

        [Test, ExpectedException(typeof(CryptographicException))]
        public void CannotReimportWithIncorrectPassphrase()
        {
            byte[] key = GenerateKey();

            ImportExportUtility utility = new ImportExportUtility();
            utility.Export(key, keyFileName, "foo");

            utility.Import(keyFileName, "bar");
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void CannotReimportWithEmptyPassphrase()
        {
            byte[] key = GenerateKey();

            ImportExportUtility utility = new ImportExportUtility();
            utility.Export(key, keyFileName, "foo");

            utility.Import(keyFileName, "");
        }

        [Test]
        public void CanReimportWithCorrectPassphrase()
        {
            byte[] key = GenerateKey();

            ImportExportUtility utility = new ImportExportUtility();
            utility.Export(key, keyFileName, "foo");

            byte[] returnedKey = utility.Import(keyFileName, "foo");

            ArraysEqual(key, returnedKey);
        }

        [Test, ExpectedException(typeof(FormatException))]
        public void ReimportingInvalidKeyFileWithNoPassphraseThrowsException()
        {
            CreateInvalidKeyFile();

            ImportExportUtility utility = new ImportExportUtility();
            utility.Import(keyFileName, "");
        }

        [Test, ExpectedException(typeof(FormatException))]
        public void ReimportingInvalidKeyFileWithPassphraseThrowsException()
        {
            CreateInvalidKeyFile();

            ImportExportUtility utility = new ImportExportUtility();
            utility.Import(keyFileName, "adsf");
        }

        [Test]
        public void CanDetermineThatKeyFileIsNotPasswordProtected()
        {
            byte[] key = GenerateKey();

            ImportExportUtility utility = new ImportExportUtility();
            utility.Export(key, keyFileName, "");

            Assert.IsFalse(utility.IsPasswordProtected(keyFileName));
        }

        [Test]
        public void CanDetermineThatKeyFileIsPasswordProtected()
        {
            byte[] key = GenerateKey();

            ImportExportUtility utility = new ImportExportUtility();
            utility.Export(key, keyFileName, "foo");

            Assert.IsTrue(utility.IsPasswordProtected(keyFileName));
        }

        [Test, ExpectedException(typeof(FormatException))]
        public void DeterminingIfInvalidKeyFileIsPasswordProtectedThrowsException()
        {
            CreateInvalidKeyFile();

            ImportExportUtility utility = new ImportExportUtility();
            Assert.IsTrue(utility.IsPasswordProtected(keyFileName));
        }

        private void CreateInvalidKeyFile()
        {
            using(FileStream fileStream = new FileStream(keyFileName, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(fileStream, Encoding.Unicode))
                {
                    writer.Write("Say, your last name is an adverb!");
                }
            }
        }

        private void ArraysEqual(byte[] expectedArray, byte[] actualArray)
        {
            Assert.AreEqual(expectedArray.Length, actualArray.Length, "Array lengths not equal");

            ArrayList mismatchedIndices = new ArrayList();
            for(int i = 0; i < expectedArray.Length; i++)
            {
                if(expectedArray[i] != actualArray[i])
                {
                    mismatchedIndices.Add(i);
                }
            }

            if(mismatchedIndices.Count != 0)
            {
                string errorMessage = "Arrays did not compare equal. Differences at:" + System.Environment.NewLine;
                
                for(int i = 0; i < mismatchedIndices.Count; i++)
                {
                    errorMessage += mismatchedIndices[i].ToString() + ": " + 
                        "[" + expectedArray[(int)mismatchedIndices[i]].ToString() + 
                        "], [" + 
                        actualArray[(int)mismatchedIndices[i]].ToString() + "]" + 
                        System.Environment.NewLine;
                }

                throw new AssertionException(errorMessage);
            }
        }

        private byte[] GenerateKey()
        {
            return new byte[] {0x00, 0x01, 0x03, 0x05};
        }
    }
}
#endif