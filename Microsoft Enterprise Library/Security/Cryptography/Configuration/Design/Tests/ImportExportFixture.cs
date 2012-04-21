//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Tests 
{
	[TestFixture]
	public class ImportExportFixture 
	{
		private const string Destination = @".\keyExport.txt";
		private ImportExportUtility utility;
		private byte[] key = new byte[32];

		[SetUp]
		public void Setup()
		{
			this.utility = new ImportExportUtility();
			CryptographyUtility.GetRandomBytes(key);
		}

		[TearDown]
		public void TearDown()
		{
			if (File.Exists(Destination)) File.Delete(Destination);
		}

		[Test]
		public void ExportKeyWithNoPassword()
		{
			string passphrase = string.Empty;

			this.utility.Export(this.key, Destination, passphrase);

			string contents = GetFileContents(Destination);

			byte[] contentBytes = Convert.FromBase64String(contents);
			Assert.AreEqual(ImportExportUtility.PlainTextFlag, contentBytes[0], "assert first byte is plain text flag");

			byte[] exportedKey = new byte[contentBytes.Length - 1];
			Buffer.BlockCopy(contentBytes, 1, exportedKey, 0, contentBytes.Length - 1);
			Assert.IsTrue(CryptographyUtility.CompareBytes(this.key, exportedKey), "key contents");
		}

		[Test]
		public void ExportKeyWithPassword()
		{
			string passphrase = "password123";

			this.utility.Export(this.key, Destination, passphrase);

			string contents = GetFileContents(Destination);

			byte[] contentBytes = Convert.FromBase64String(contents);
			Assert.AreEqual(ImportExportUtility.PasswordProtectedFlag, contentBytes[0], "assert first byte is plain text flag");

			byte[] encryptedKey = new byte[64];
			Buffer.BlockCopy(contentBytes, 33, encryptedKey, 0, 64); // 33 = flag(1) + pw hash(32) 

			byte[] cryptoKey = GetPassphraseBytes(passphrase);
			SymmetricAlgorithm algorithm = RijndaelManaged.Create();
			SymmetricCryptographer crypto = new SymmetricCryptographer(algorithm, cryptoKey);
			byte[] decryptedKey = crypto.Decrypt(encryptedKey);

			Assert.IsTrue(CryptographyUtility.CompareBytes(this.key, decryptedKey), "key contents");
		}

		[Test]
		public void ImportKeyWithNoPassword()
		{
			ImportExportAssert(this.key, "");
		}

		[Test]
		public void ImportKeyWithPassword()
		{
			ImportExportAssert(this.key,  "password123");
		}
		
		[Test]
		public void Import1ByteNotPasswordProtected()
		{
			byte[] shortKey = new byte[1];
			CryptographyUtility.GetRandomBytes(shortKey);

			ImportExportAssert(shortKey, string.Empty);
		}

		[Test]
		public void Import2ByteNotPasswordProtected()
		{
			byte[] shortKey = new byte[2];
			CryptographyUtility.GetRandomBytes(shortKey);

			ImportExportAssert(shortKey, string.Empty);
		}

		[Test]
		public void Import1024ByteNotPasswordProtected()
		{
			byte[] shortKey = new byte[1024];
			CryptographyUtility.GetRandomBytes(shortKey);

			ImportExportAssert(shortKey, string.Empty);
		}

		[Test]
		public void Import1BytePasswordProtected()
		{
			byte[] shortKey = new byte[1];
			CryptographyUtility.GetRandomBytes(shortKey);

			ImportExportAssert(shortKey, "12345");
		}

		[Test]
		public void Import2BytePasswordProtected()
		{
			byte[] shortKey = new byte[2];
			CryptographyUtility.GetRandomBytes(shortKey);

			ImportExportAssert(shortKey, "12345");
		}

		[Test]
		public void Import1024BytePasswordProtected()
		{
			byte[] shortKey = new byte[1024];
			CryptographyUtility.GetRandomBytes(shortKey);

			ImportExportAssert(shortKey, "54321");
		}

		private void ImportExportAssert(byte[] plainTextKey, string passphrase)
		{
			this.utility.Export(plainTextKey, Destination, passphrase);
			byte[] importKey = this.utility.Import(Destination, passphrase);

			Assert.IsTrue(CryptographyUtility.CompareBytes(plainTextKey, importKey), "key contents");
		}

		[Test]
		public void IsPasswordProtected()
		{
			string passphrase = "password123";

			this.utility.Export(this.key, Destination, passphrase);

			Assert.IsTrue(this.utility.IsPasswordProtected(Destination));
		}

		[Test]
		[ExpectedException(typeof(CryptographicException))]
		public void AttemptImportWithBadPassword()
		{
			string passphrase1 = "password123";
			string passphrase2 = "password456";

			this.utility.Export(this.key, Destination, passphrase1);
			this.utility.Import(Destination, passphrase2);
		}

		[Test]
		[ExpectedException(typeof(CryptographicException))]
		public void AttemptImportWithBadPassword2()
		{
			string passphrase1 = "qwert";
			string passphrase2 = "asdfg";

			this.utility.Export(this.key, Destination, passphrase1);
			this.utility.Import(Destination, passphrase2);
		}

		[Test]
		[ExpectedException(typeof(CryptographicException))]
		public void AttemptImportWithBadPassword3()
		{
			string key64 = @"AUXRHn9H3Q3Xezw48u+ZK2UdqLEyg0/IXRdod3Znnene";
			byte[] keyBytes = Convert.FromBase64String(key64);
			byte[] keyBytes2 = new byte[keyBytes.Length - 1];
			Buffer.BlockCopy(keyBytes, 1, keyBytes2, 0, keyBytes.Length - 1);

			string passphrase1 = "12345";
			string passphrase2 = "123456";

			this.utility.Export(keyBytes2, Destination, passphrase1);
			this.utility.Import(Destination, passphrase2);
		}

		[Test]
		[ExpectedException(typeof(CryptographicException))]
		public void ImportWithBadKey()
		{
			// encrypted with "12345"
			string enc = @"APhCiBxfgqedbPLkFeO94yqk4j1JhLc27+r6/PtQhdJX/ri33RcZeVUNW3BUxreK9UkR5wFaP9dqyQ4CXKcuVQI=";

			StreamWriter w = new StreamWriter(Destination);
			w.WriteLine(enc);
			w.Flush();
			w.Close();

			string passphrase2 = "123456";
			this.utility.Import(Destination, passphrase2);
		}

		[Test]
		[ExpectedException(typeof(CryptographicException))]
		public void EncryptAndDecryptBadKey()
		{
			string key64 = @"AUXRHn9H3Q3Xezw48u+ZK2UdqLEyg0/IXRdod3Znnene";
			byte[] keyBytes = Convert.FromBase64String(key64);
			byte[] keyBytes2 = new byte[keyBytes.Length - 1];
			Buffer.BlockCopy(keyBytes, 1, keyBytes2, 0, keyBytes.Length - 1);

			string passphrase1 = "12345";
			string passphrase2 = "123456";

			this.utility.Export(keyBytes2, Destination, passphrase1);
			this.utility.Import(Destination, passphrase2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AttemptImportEncryptedExportWithMissingPassphrase()
		{
			string passphrase = "password123";
			this.utility.Export(this.key, Destination, passphrase);

			string emptyPassphrase = string.Empty;
			this.utility.Import(Destination, emptyPassphrase);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullKey()
		{
			this.utility.Export(null, Destination, "");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullDestination()
		{
			this.utility.Export(this.key, null, "");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void MinLengthKey()
		{
			this.utility.Export(new byte[0], null, "");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void MinLengthDestination()
		{
			this.utility.Export(this.key, "", "");
		}


		private byte[] GetPassphraseBytes(string passphrase)
		{
			SHA256 sha = SHA256Managed.Create();

			byte[] passwordBytes = Encoding.Unicode.GetBytes(passphrase);
			return sha.ComputeHash(passwordBytes);
		}

		private string GetFileContents(string file)
		{
			FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(stream);
			string contents = reader.ReadToEnd();
			stream.Close();

			return contents;
		}
	}
}
#endif