//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS

using System;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Tests
{
    /// <summary>
    /// Summary description for AssertUtilities.
    /// </summary>
    internal class AssertUtilities
    {
        private AssertUtilities()
        {
        }

        internal static void AssertStringDoesNotContain(string o, string s, string message)
        {
            Assert.IsNotNull(o);
            Assert.IsNotNull(s);
            Assert.IsNotNull(message);
            Assert.IsFalse(o.StartsWith(s), String.Format("\nIn {2}, the string:\n\t{0}\ncontains\n\t{1}\nwhen it should not.\n", o, s, message));
        }
    }
}

#endif