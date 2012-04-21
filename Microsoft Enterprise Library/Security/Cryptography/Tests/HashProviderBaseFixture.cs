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
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
	public abstract class HashProviderBaseFixture
	{
		private ConfigurationContext context = new TestConfigurationContext();
		private readonly byte[] plainText = new byte[] {0, 1, 2, 3, 4, 5, 6};

		public abstract IHashProvider DefaultHashProvider { get; }

		public abstract IHashProvider SaltedHashProvider { get; }

		public abstract IHashProvider GetProvider();

		public abstract HashProviderData GetNewData();

		public ConfigurationContext Context
		{
			get { return context; }
		}

		[Test]
		public void CreateHash()
		{
			byte[] providerHash = DefaultHashProvider.CreateHash(plainText);
			Assert.IsFalse(CryptographyUtility.CompareBytes(plainText, providerHash));
		}

		[Test]
		public void CompareEqualHash()
		{
			byte[] hashedText = DefaultHashProvider.CreateHash(this.plainText);

			Assert.IsTrue(DefaultHashProvider.CompareHash(this.plainText, hashedText));
		}

		[Test]
		public void CompareHashOfDifferentText()
		{
			byte[] plainText1 = new byte[] {0, 1, 0, 0};
			byte[] plainText2 = new byte[] {0, 0, 1, 0};
			byte[] hashedText = DefaultHashProvider.CreateHash(plainText2);

			Assert.IsFalse(DefaultHashProvider.CompareHash(plainText1, hashedText));
		}

		[Test]
		public void HashWithSalt()
		{
			IHashProvider hashProviderWithSalt = SaltedHashProvider;
			IHashProvider hashProvider = DefaultHashProvider;

			byte[] origHash1 = hashProvider.CreateHash(plainText);
			byte[] providerHash1 = hashProviderWithSalt.CreateHash(plainText);

			Assert.IsFalse(CryptographyUtility.CompareBytes(origHash1, providerHash1), "original");
			Assert.IsFalse(CryptographyUtility.CompareBytes(plainText, providerHash1), "plain");
		}

		[Test]
		public void UniqueSaltedHashes()
		{
			IHashProvider hashProviderWithSalt = SaltedHashProvider;
			byte[] providerHash1 = hashProviderWithSalt.CreateHash(plainText);
			byte[] providerHash2 = hashProviderWithSalt.CreateHash(plainText);
			Assert.IsFalse(CryptographyUtility.CompareBytes(providerHash1, providerHash2), "compare");
		}

		[Test]
		public void CompareHashWithSalt()
		{
			IHashProvider hashProvider = SaltedHashProvider;

			byte[] providerHash = hashProvider.CreateHash(plainText);
			Assert.IsTrue(hashProvider.CompareHash(plainText, providerHash), "true");

			byte[] badHash = new byte[50];
			RNGCryptoServiceProvider.Create().GetBytes(badHash);
			Assert.IsFalse(hashProvider.CompareHash(plainText, badHash), "false");
		}

		[Test]
		public void VerifyHashAsUnique()
		{
			byte[] hash1 = SaltedHashProvider.CreateHash(this.plainText);
			byte[] hash2 = SaltedHashProvider.CreateHash(this.plainText);

			Assert.IsFalse(CryptographyUtility.CompareBytes(hash1, hash2));
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void CompareHashZeroLengthHashedText()
		{
			byte[] hashedText = new byte[] {};

			DefaultHashProvider.CompareHash(this.plainText, hashedText);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void CompareHashNullHashedText()
		{
			DefaultHashProvider.CompareHash(this.plainText, null);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void CompareHashNullPlainText()
		{
			byte[] hashedText = DefaultHashProvider.CreateHash(new byte[] {1});

			DefaultHashProvider.CompareHash(null, hashedText);
		}

		[Test]
		public void CompareHashZeroLengthPlainText()
		{
			byte[] plainTextZero = new byte[] {};
			byte[] hashedText = DefaultHashProvider.CreateHash(plainTextZero);

			Assert.IsTrue(DefaultHashProvider.CompareHash(plainTextZero, hashedText));
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void CreateHashNullPlainText()
		{
			DefaultHashProvider.CreateHash(null);
		}

		[Test]
		public void CreateHashZeroLengthPlainText()
		{
			byte[] hashForEmptyPlainText = DefaultHashProvider.CreateHash(new byte[] {});
			Assert.IsTrue(hashForEmptyPlainText.Length > 0);
		}

		[Test]
		public void CompareHashInvalidHashedText()
		{
			byte[] hashedText = new byte[] {0, 1, 2, 3};

			Assert.IsFalse(DefaultHashProvider.CompareHash(this.plainText, hashedText));
		}

		protected IHashProvider CreateProvider(HashProviderData providerData)
		{
			IHashProvider provider = GetProvider();
			provider.Initialize(new TestCryptographyConfigurationView(providerData));
			return provider;
		}

		private class TestCryptographyConfigurationView : CryptographyConfigurationView
		{
			private readonly HashProviderData data;

			public TestCryptographyConfigurationView(HashProviderData data) : base(new ConfigurationContext(new ConfigurationDictionary()))
			{
				this.data = data;
			}

			public override HashProviderData GetHashProviderData(string name)
			{
				return data;
			}
		}
	}
}

#endif