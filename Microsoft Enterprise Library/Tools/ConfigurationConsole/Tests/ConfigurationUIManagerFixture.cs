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

#if   UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Tools.ConfigurationConsole.Tests
{
    [TestFixture]
    public class ConfigurationUIManagerFixture
    {
        private CommandInterceptorContainer interceptorContainer;
        private ConfigurationUIManager manager;

        [SetUp]
        public void SetUp()
        {
            interceptorContainer = new CommandInterceptorContainer();
            manager = new ConfigurationUIManager();
            manager.RegisterCommands(interceptorContainer);
        }

        [Test]
        public void OpenApplicationCommandInterceptorTest()
        {
            CommandInterceptor[] interceptors = interceptorContainer.GetCommandInterceptors(@"root/Open/Application");
            Assert.AreEqual(1, interceptors.Length);
            Assert.AreEqual(typeof(OpenApplicationCommandInterceptor), interceptors[0].GetType());
        }

        [Test]
        public void RegisterCommandTests()
        {
            CommandInterceptor[] interceptors = interceptorContainer.GetCommandInterceptors(@"root/Save");
            Assert.AreEqual(1, interceptors.Length);
            Assert.AreEqual(typeof(SaveApplicationCommandInterceptor), interceptors[0].GetType());
        }
    }
}

#endif