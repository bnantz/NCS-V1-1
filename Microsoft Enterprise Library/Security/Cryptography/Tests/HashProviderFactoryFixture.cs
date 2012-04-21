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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
	[TestFixture]
	public class HashProviderFactoryFixture
	{
		private ConfigurationContext context = new TestConfigurationContext();

		[Test]
		public void GetProvider1()
		{
			HashProviderFactory factory = new HashProviderFactory(context);
			IHashProvider provider = factory.CreateHashProvider("provider1");
			Assert.IsNotNull(provider);

			MockHashProvider mockProvider = provider as MockHashProvider;
			Assert.IsNotNull(mockProvider);
			Assert.IsTrue(mockProvider.Initialized);
		}

		[Test]
		public void GetByNameTest()
		{
			HashProviderFactory factory = new HashProviderFactory(context);
			IHashProvider provider = factory.CreateHashProvider("provider1");
			Assert.IsNotNull(provider);
			Assert.IsTrue(provider is MockHashProvider);
		}

		[Test]
		[ExpectedException(typeof (ConfigurationException))]
		public void ProviderNotFoundTest()
		{
			HashProviderFactory factory = new HashProviderFactory(context);
			factory.CreateHashProvider("provider3");
		}

		[Test]
		public void GetMockHashProvider()
		{
			HashProviderFactory factory = new HashProviderFactory(new TestConfigurationContext());
			MockHashProvider store = (MockHashProvider) factory.CreateHashProvider("mockHashProvider1");
			Assert.IsNotNull(store);
		}
	}
}

#endif