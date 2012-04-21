//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration 
{
    [TestFixture]
    public class DisposableWrapperFixture 
    {
        [Test]
        public void MakeSureThatCallingDisposeCallsConfigurationBuilderDispose()
        {
            MockBuilder mockBuilder = new MockBuilder();
            DisposingWrapper wrapper = new DisposingWrapper(mockBuilder);
            wrapper.Dispose();
            Assert.IsTrue(mockBuilder.disposed);
        }


        [Test]
        public void MakeSureThatCallingDisposeDoesNotCallConfigurationBuilderDispose()
        {
            MockBuilder mockBuilder = new MockBuilder();
            NonDisposingWrapper wrapper = new NonDisposingWrapper(mockBuilder);
            wrapper.Dispose();
            Assert.IsFalse(mockBuilder.disposed);

        }

        

        private class MockBuilder : ConfigurationBuilder
        {
            public bool disposed;

            public MockBuilder() : base()
            {
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose (disposing);
                if (disposing)
                {
                    disposed = true;
                }
            }
        }
    }
}
#endif