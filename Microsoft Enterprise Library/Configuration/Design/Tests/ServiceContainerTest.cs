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
using System;
using System.ComponentModel.Design;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class ServiceContainerTest
    {
        private bool serviceCallbackExecuted;

        [SetUp]
        public void SetUp()
        {
            serviceCallbackExecuted = false;
        }

        [Test]
        public void ConstructorNoArgsTest()
        {
            using (TestContainer testContainer = new TestContainer())
            {
                Assert.AreEqual(0, testContainer.services.Count);
                Assert.AreEqual(0, testContainer.profferedServices.Count);
            }
        }

        [Test]
        public void ConstructorWithServiceProvider()
        {
            MyServiceProvider serviceProvider = new MyServiceProvider();
            using (TestContainer testContainer = new TestContainer(serviceProvider))
            {
                Assert.AreSame(serviceProvider, testContainer.parentServiceProvider);
                Assert.AreEqual(0, testContainer.services.Count);
                Assert.AreEqual(0, testContainer.profferedServices.Count);
            }
        }

        [Test]
        public void AddServiceWithCallback()
        {
            using (TestContainer testContainer = new TestContainer())
            {
                testContainer.AddService(typeof(SayHelloService), new ServiceCreatorCallback(SayHelloServiceCreatorCallback));
                Assert.AreEqual(1, testContainer.services.Count);
                SayHelloService sayHello = testContainer.GetService(typeof(SayHelloService)) as SayHelloService;
                Assert.IsNotNull(sayHello);
                Assert.IsTrue(serviceCallbackExecuted);
            }
        }

        [Test]
        public void AddServicePromotedWithCallback()
        {
            using (ParentContainer parentContainer = new ParentContainer())
            {
                using (TestContainer testContainer = new TestContainer(parentContainer))
                {
                    testContainer.AddService(typeof(SayHelloService), new ServiceCreatorCallback(SayHelloServiceCreatorCallback), true);
                    SayHelloService sayHello = testContainer.GetService(typeof(SayHelloService)) as SayHelloService;
                    Assert.AreEqual(1, testContainer.services.Count);
                    Assert.AreEqual(1, parentContainer.services.Count);
                    Assert.IsNotNull(sayHello);
                    Assert.IsTrue(serviceCallbackExecuted);
                }
            }
        }

        [Test]
        public void RemoveServiceTest()
        {
            using (TestContainer testContainer = new TestContainer())
            {
                testContainer.AddService(typeof(SayHelloService), new ServiceCreatorCallback(SayHelloServiceCreatorCallback));
                Assert.AreEqual(1, testContainer.services.Count);
                testContainer.RemoveService(typeof(SayHelloService));
                Assert.AreEqual(0, testContainer.services.Count);
            }
        }

        private object SayHelloServiceCreatorCallback(IServiceContainer serviceContainer, Type serviceType)
        {
            serviceCallbackExecuted = true;
            return new SayHelloService();
        }

        private class TestContainer : ServiceContainer
        {
            public TestContainer() : base()
            {
            }

            public TestContainer(IServiceProvider provider) : base(provider)
            {
            }
        }

        private class ParentContainer : ServiceContainer
        {
            public ParentContainer() : base()
            {
            }
        }

        private class MyServiceProvider : IServiceProvider
        {
            public object GetService(Type serviceType)
            {
                if (serviceType.Equals(typeof(SayHelloService)))
                {
                    return new SayHelloService();
                }
                return null;
            }
        }

        private class SayHelloService
        {
            public string SayHello(string name)
            {
                return "Hello" + name;
            }
        }
    }
}

#endif