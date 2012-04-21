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
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using NUnit.Framework;
using TestingInternal;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests 
{
    [TestFixture]
    public class RuntimeConfigurationViewSecurityDemandsFixture 
    {
        [Test, ExpectedException(typeof(SecurityException))]
        public void CannotInvokeNonPublicMethodWhenCallsMadeDirectly()
        {
            ReflectionPermission permission = new ReflectionPermission(ReflectionPermissionFlag.TypeInformation);
            permission.Deny();

            XmlIncludeTypeDataCollection xmlIncludeTypes = new XmlIncludeTypeDataCollection();
            xmlIncludeTypes.Add(new XmlIncludeTypeData("shouldNotBeAccessible", typeof(TestingInternal.ShouldNotBeAccessible).AssemblyQualifiedName));

            RuntimeConfigurationView view = new RuntimeConfigurationView(new ConfigurationContext(new ConfigurationDictionary()));
            Type [] collectedTypes = view.CollectTypes(xmlIncludeTypes);

            Assert.AreEqual(1, collectedTypes.Length);
        }

        [Test, ExpectedException(typeof(TargetInvocationException))]
        public void CannotInvokeNonPublicMethodWhenCallsMadeThroughReflection()
        {
            ReflectionPermission permission = new ReflectionPermission(ReflectionPermissionFlag.TypeInformation);
            permission.Deny();

            XmlIncludeTypeDataCollection xmlIncludeTypes = new XmlIncludeTypeDataCollection();
            xmlIncludeTypes.Add(new XmlIncludeTypeData("shouldNotBeAccessible", typeof(TestingInternal.ShouldNotBeAccessible).AssemblyQualifiedName));

            RuntimeConfigurationView view = new RuntimeConfigurationView(new ConfigurationContext(new ConfigurationDictionary()));
            MethodInfo collectTypes = view.GetType().GetMethod("CollectTypes", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(XmlIncludeTypeDataCollection)}, null);
            object returnedTypes = collectTypes.Invoke(view, new object[] { xmlIncludeTypes });
            Type [] collectedTypes = returnedTypes as Type[];

            Assert.AreEqual(1, collectedTypes.Length);
        }
    }
}

namespace TestingInternal
{
    internal class ShouldNotBeAccessible
    {
        internal ShouldNotBeAccessible()
        {
        }
    }
}

#endif