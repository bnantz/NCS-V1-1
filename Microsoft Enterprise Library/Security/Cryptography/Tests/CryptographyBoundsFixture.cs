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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
	[TestFixture]
	public class CryptographyBoundsFixture
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
		[ExpectedException(typeof (ArgumentNullException))]
		public void CreateHashWithNullInstance()
		{
			Cryptographer.CreateHash(null, plainTextBytes);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void CreateHashWithZeroLengthInstance()
		{
			Cryptographer.CreateHash(string.Empty, plainTextBytes);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void CreateHashWithNullInstanceString()
		{
			Cryptographer.CreateHash(null, plainTextString);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void CreateHashWithZeroLengthInstanceString()
		{
			Cryptographer.CreateHash(string.Empty, plainTextString);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void CompareHashWithNullInstance()
		{
			byte[] hash = Cryptographer.CreateHash(hashInstance, plainTextBytes, context);
			Cryptographer.CompareHash(null, plainTextBytes, hash);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void CompareHashWithZeroLengthInstance()
		{
			byte[] hash = Cryptographer.CreateHash(hashInstance, plainTextBytes, context);
			Cryptographer.CompareHash(string.Empty, plainTextBytes, hash);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void CompareHashWithNullInstanceString()
		{
			string hash = Cryptographer.CreateHash(hashInstance, plainTextString, context);
			Cryptographer.CompareHash(null, plainTextString, hash);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void CompareHashWithZeroLengthInstanceString()
		{
			string hash = Cryptographer.CreateHash(hashInstance, plainTextString, context);
			Cryptographer.CompareHash(string.Empty, plainTextString, hash);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void EncryptWithNullInstance()
		{
			Cryptographer.EncryptSymmetric(null, plainTextBytes, context);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void EncryptWithZeroLengthInstance()
		{
			Cryptographer.EncryptSymmetric(string.Empty, plainTextBytes, context);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void EncryptWithNullInstanceString()
		{
			Cryptographer.EncryptSymmetric(null, plainTextString, context);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void EncryptWithZeroLengthInstanceString()
		{
			Cryptographer.EncryptSymmetric(string.Empty, plainTextString, context);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void DecryptWithNullInstance()
		{
			Cryptographer.DecryptSymmetric(null, plainTextBytes, context);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void DecryptWithZeroLengthInstance()
		{
			Cryptographer.DecryptSymmetric(string.Empty, plainTextBytes, context);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void DecryptWithNullInstanceString()
		{
			Cryptographer.DecryptSymmetric(null, plainTextString, context);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void DecryptWithZeroLengthInstanceString()
		{
			Cryptographer.DecryptSymmetric(string.Empty, plainTextString, context);
		}

		[Test]
		[ExpectedException(typeof (FormatException))]
		public void DecryptWithInvalidString()
		{
			Cryptographer.DecryptSymmetric(symmInstance, "INVALID", context);
		}

		[Test]
		[ExpectedException(typeof (FormatException))]
		public void CompareHashWithInvalidString()
		{
			Cryptographer.CompareHash(hashInstance, plainTextString, "INVALID", context);
		}
	}
}

#endif