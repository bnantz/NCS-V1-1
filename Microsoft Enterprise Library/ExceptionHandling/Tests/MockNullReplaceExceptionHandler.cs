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

#if    UNIT_TESTS
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    /// <summary>
    /// Summary description for MockNullReplaceExceptionHandler.
    /// </summary>
    public class MockNullReplaceExceptionHandler : ExceptionHandler
    {
        public MockNullReplaceExceptionHandler()
        {
        }

        public override Exception HandleException(Exception exception, string policyName, Guid handlingInstanceID)
        {
            return null;
        }

        public override void Initialize(ConfigurationView configurationView)
        {
        }
    }
}

#endif