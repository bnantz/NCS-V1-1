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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    /// <summary>
    /// Summary description for MockException.
    /// </summary>
    public class MockException : ArgumentNullException
    {
        public readonly string FieldString = "MockFieldString";

        public MockException() : base("MOCK EXCEPTION")
        {
        }

        public string PropertyString
        {
            get { return "MockPropertyString"; }
        }
    }
}

#endif