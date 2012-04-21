//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestFixture]
    public class ExceptionPolicyFactoryFixture : ConfigurationContextFixtureBase
    {
        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void CreateExceptionPolicyThrowsCorrectly()
        {
            ExceptionPolicyFactory factory = new ExceptionPolicyFactory(Context);
            factory.CreateExceptionPolicy("Non Existent Policy", new Exception());
        }
    }
}

#endif