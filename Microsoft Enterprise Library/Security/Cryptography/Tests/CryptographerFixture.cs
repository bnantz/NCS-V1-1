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
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
	[TestFixture]
	public class CryptographerFixture
	{
		private ConfigurationContext context = new TestConfigurationContext();
		private const string hashInstance = "hmac1";
		private const string symmInstance = "dpapiSymmetric1";

		private const string plainTextString = "secret";
		private readonly byte[] plainTextBytes = new byte[] {0, 1, 2, 3};


		public void FixtureSetup()
		{
			byte[] key = new byte[] {0, 1, 2, 3, 4};

			CryptographySettings settings = (CryptographySettings) context.GetConfiguration(CryptographySettings.SectionName);
			((KeyedHashAlgorithmProviderData) settings.HashProviders[hashInstance]).Key = key;
		}


		[Test]
		public void CreateHashBytes()
		{
			byte[] hash = Cryptographer.CreateHash(hashInstance, plainTextBytes, context);
			Assert.IsFalse(CryptographyUtility.CompareBytes(plainTextBytes, hash));
		}

		[Test]
		public void CreateHashString()
		{
			string hashString = Cryptographer.CreateHash(hashInstance, plainTextString, context);
			Assert.IsFalse(plainTextString == hashString);
		}

		[Test]
		public void CreateAndCompareHashBytes()
		{
			byte[] hash = Cryptographer.CreateHash(hashInstance, plainTextBytes, context);
			bool result = Cryptographer.CompareHash(hashInstance, plainTextBytes, hash, context);

			Assert.IsTrue(result);
		}

		[Test]
		public void CreateAndCompareInvalidHashBytes()
		{
			byte[] hash = Cryptographer.CreateHash(hashInstance, plainTextBytes, context);

			byte[] badPlainText = new byte[] {2, 1, 0};
			bool result = Cryptographer.CompareHash(hashInstance, badPlainText, hash, context);

			Assert.IsFalse(result);
		}

		[Test]
		public void CreateAndCompareHashString()
		{
			string hashString = Cryptographer.CreateHash(hashInstance, plainTextString, context);

			bool result = Cryptographer.CompareHash(hashInstance, plainTextString, hashString, context);
			Assert.IsTrue(result);
		}

		[Test]
		public void EncryptAndDecryptBytes()
		{
			byte[] encrypted = Cryptographer.EncryptSymmetric(symmInstance, plainTextBytes, context);
			Assert.IsFalse(CryptographyUtility.CompareBytes(plainTextBytes, encrypted), "enc");

			byte[] decrypted = Cryptographer.DecryptSymmetric(symmInstance, encrypted, context);
			Assert.IsTrue(CryptographyUtility.CompareBytes(plainTextBytes, decrypted), "dec");
		}

		[Test]
		public void EncryptAndDecryptString()
		{
			string encrypted = Cryptographer.EncryptSymmetric(symmInstance, plainTextString, context);
			Assert.IsFalse(plainTextString == encrypted, "enc");

			string decrypted = Cryptographer.DecryptSymmetric(symmInstance, encrypted, context);
			Assert.IsTrue(plainTextString == decrypted, "dec");
		}

		[Test]
		public void EncryptAndDecryptOneByte()
		{
			byte[] onebyte = new byte[1];
			CryptographyUtility.GetRandomBytes(onebyte);

			byte[] encrypted = Cryptographer.EncryptSymmetric(symmInstance, onebyte, context);
			Assert.IsFalse(CryptographyUtility.CompareBytes(onebyte, encrypted), "enc");

			byte[] decrypted = Cryptographer.DecryptSymmetric(symmInstance, encrypted, context);

			Assert.IsTrue(CryptographyUtility.CompareBytes(onebyte, decrypted), "dec");
		}

		[Test]
		public void EncryptAndDecryptOneMegabyte()
		{
			byte[] megabyte = new byte[1024*1024];
			CryptographyUtility.GetRandomBytes(megabyte);

			byte[] encrypted = Cryptographer.EncryptSymmetric(symmInstance, megabyte, context);
			Assert.IsFalse(CryptographyUtility.CompareBytes(megabyte, encrypted), "enc");

			byte[] decrypted = Cryptographer.DecryptSymmetric(symmInstance, encrypted, context);

			Assert.IsTrue(CryptographyUtility.CompareBytes(megabyte, decrypted), "dec");
		}
	}
}

#endif