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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Tests
{
    [TestFixture]
    public class ExceptionTypeNodeFixture
    {

        [Test]
        public void PropertiesTest()
        {
            string typeName = "testType";
            PostHandlingAction postHandlingAction = PostHandlingAction.ThrowNewException;

            ExceptionTypeNode node = new ExceptionTypeNode();
            node.TypeName = typeName;
            node.PostHandlingAction = postHandlingAction;

            Assert.AreEqual(typeName, node.TypeName);
            Assert.AreEqual(postHandlingAction, node.PostHandlingAction);
        }
    }
}

#endif