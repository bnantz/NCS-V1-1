//===============================================================================
// Microsoft patterns & practices Enterprise Library
// XXXXX Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [TestFixture]
    public class ConfigurationContextFixtureBase
    {
        private ConfigurationContext context;

        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            context = ConfigurationManager.GetCurrentContext();
        }

        [TestFixtureTearDown]
        public virtual void FixtureTeardown()
        {
            context = null;
        }

        public virtual ConfigurationContext Context
        {
            get { return context; }
        }
    }
}

#endif