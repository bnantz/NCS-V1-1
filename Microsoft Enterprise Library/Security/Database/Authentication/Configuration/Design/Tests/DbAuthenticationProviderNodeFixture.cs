//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.Configuration.Design.Tests
{
	[TestFixture]
	public class DbAuthenticationProviderNodeFixture : ConfigurationDesignHostTestBase
	{
        [Test]
        public void SaveDefaultAuthenticationProviderAndSaveAndReloadsCorrectly()
        {
            GeneratedHierarchy.Load();

            AddConfigurationSectionCommand cmd = new AddConfigurationSectionCommand(Host, typeof(SecuritySettingsNode), SecuritySettings.SectionName);	    
            cmd.Execute(GeneratedApplicationNode);

            SecuritySettingsNode securitySettingsNode = GeneratedHierarchy.FindNodeByType(typeof(SecuritySettingsNode)) as SecuritySettingsNode;
            Assert.IsNotNull(securitySettingsNode);
            AuthenticationProviderCollectionNode authenticationProviderCollectionNode = GeneratedHierarchy.FindNodeByType(securitySettingsNode, typeof(AuthenticationProviderCollectionNode)) as AuthenticationProviderCollectionNode;
            Assert.IsNotNull(authenticationProviderCollectionNode);

            AddChildNodeCommand addChildNodeCommand = new AddChildNodeCommand(Host, typeof(DbAuthenticationProviderNode));
            addChildNodeCommand.Execute(authenticationProviderCollectionNode);
            Assert.AreEqual(typeof(DbAuthenticationProviderNode), addChildNodeCommand.ChildNode.GetType());
            DbAuthenticationProviderNode dbAuthenticationProviderNode = (DbAuthenticationProviderNode)addChildNodeCommand.ChildNode;
            securitySettingsNode.DefaultAuthenticationInstance = (AuthenticationProviderNode)addChildNodeCommand.ChildNode;

            addChildNodeCommand = new AddChildNodeCommand(Host, typeof(HashAlgorithmProviderNode));
            CryptographySettingsNode cryptographySettingsNode = GeneratedHierarchy.FindNodeByType(typeof(CryptographySettingsNode)) as CryptographySettingsNode;
            Assert.IsNotNull(cryptographySettingsNode);
            HashProviderCollectionNode hashProviderCollectionNode = GeneratedHierarchy.FindNodeByType(typeof(HashProviderCollectionNode)) as HashProviderCollectionNode;   
            Assert.IsNotNull(hashProviderCollectionNode);
            addChildNodeCommand.Execute(hashProviderCollectionNode);
            Assert.AreEqual(typeof(HashAlgorithmProviderNode), addChildNodeCommand.ChildNode.GetType());

            dbAuthenticationProviderNode.HashProvider = (HashProviderNode)addChildNodeCommand.ChildNode;

            DatabaseSettingsNode databaseSettingsNode = GeneratedHierarchy.FindNodeByType(typeof(DatabaseSettingsNode)) as DatabaseSettingsNode;
            Assert.IsNotNull(databaseSettingsNode);
            InstanceCollectionNode instanceCollectionNode = GeneratedHierarchy.FindNodeByType(databaseSettingsNode,  typeof(InstanceCollectionNode)) as InstanceCollectionNode;
            Assert.IsNotNull(instanceCollectionNode);
            Assert.IsTrue(instanceCollectionNode.Nodes.Count > 0);
            InstanceNode instanceNode = (InstanceNode)instanceCollectionNode.Nodes[0];

            dbAuthenticationProviderNode.Database = instanceNode;

            ValidateNodeCommand validateNodeCommand = new ValidateNodeCommand(Host);
            validateNodeCommand.Execute(GeneratedApplicationNode);
            Assert.AreEqual(0, ConfigurationErrorsCount);

            SaveApplicationConfigurationNodeCommand saveCmd = new SaveApplicationConfigurationNodeCommand(Host);
            saveCmd.Execute(GeneratedApplicationNode);

            ApplicationConfigurationNode applicationConfigurationNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(applicationConfigurationNode, CreateDefaultConfiguration());
            HierarchyService.SelectedHierarchy = hierarchy;
            hierarchy.Open();
        }
	}
}
#endif