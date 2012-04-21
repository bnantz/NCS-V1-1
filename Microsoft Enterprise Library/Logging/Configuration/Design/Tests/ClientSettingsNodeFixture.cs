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

#if UNIT_TESTS
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.DistributionStrategies;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{

	[TestFixture]
	public class ClientSettingsNodeFixture  : ConfigurationDesignHostTestBase
	{
        private IUIHierarchy loggingHierarchy;

        public override void SetUp()
        {
            base.SetUp ();
            ApplicationConfigurationNode appNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            loggingHierarchy = CreateHierarchyAndAddToHierarchyService(appNode, CreateDefaultConfiguration());
            HierarchyService.SelectedHierarchy = loggingHierarchy;
            loggingHierarchy.Open();
            HierarchyService.SelectedHierarchy = GeneratedHierarchy;
        }

        public override void TearDown()
        {
            HierarchyService.SelectedHierarchy = loggingHierarchy;
            loggingHierarchy.Save();
            base.TearDown ();
        }



        [Test]
        public void RemovingDistributionStrategySavesAndReopensCorrectly()
        {
            GeneratedHierarchy.Open();
            LoggingSettingsNode node = new LoggingSettingsNode();
            GeneratedApplicationNode.Nodes.AddWithDefaultChildren(node);
            ClientSettingsNode clientSettingsNode = GeneratedHierarchy.FindNodeByType(node, typeof(ClientSettingsNode)) as ClientSettingsNode;
            DistributionStrategyCollectionNode distributions = GeneratedHierarchy.FindNodeByType(clientSettingsNode, typeof(DistributionStrategyCollectionNode)) as DistributionStrategyCollectionNode;
            distributions.Nodes.AddWithDefaultChildren(new MsmqDistributionStrategyNode());
            GeneratedHierarchy.Save();

            ApplicationConfigurationNode appNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(appNode, CreateDefaultConfiguration());
            HierarchyService.SelectedHierarchy = hierarchy;
            hierarchy.Open();
            LoggingSettingsNode settingsNode = hierarchy.FindNodeByType(typeof(LoggingSettingsNode)) as LoggingSettingsNode;
            Assert.IsNotNull(settingsNode);
            distributions = hierarchy.FindNodeByType(appNode, typeof(DistributionStrategyCollectionNode)) as DistributionStrategyCollectionNode;
            Assert.IsNotNull(distributions);
            MsmqDistributionStrategyNode msmqDistributionStrategyNode = hierarchy.FindNodeByType(distributions, typeof(MsmqDistributionStrategyNode)) as MsmqDistributionStrategyNode;
            Assert.IsNotNull(msmqDistributionStrategyNode);
            // expect 3, inproc, MSMQ, and custom
            Assert.AreEqual(3, distributions.Nodes.Count);
            InProcDistributionStrategyNode inProcNode = hierarchy.FindNodeByType(distributions, typeof(InProcDistributionStrategyNode)) as InProcDistributionStrategyNode;
            Assert.IsNotNull(inProcNode);
            inProcNode.Remove();
            hierarchy.Save();


            appNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            hierarchy = CreateHierarchyAndAddToHierarchyService(appNode, CreateDefaultConfiguration());
            HierarchyService.SelectedHierarchy = hierarchy;
            hierarchy.Open();
            settingsNode = hierarchy.FindNodeByType(typeof(LoggingSettingsNode)) as LoggingSettingsNode;
            Assert.IsNotNull(settingsNode);
            distributions = hierarchy.FindNodeByType(appNode, typeof(DistributionStrategyCollectionNode)) as DistributionStrategyCollectionNode;
            Assert.IsNotNull(distributions);
            // expect 2, inproc & custom
            Assert.AreEqual(2, distributions.Nodes.Count);

        }

        [Test]
        public void RenamingOneDistributionStrategyLeavesOnlyOneStrategy()
        {
            string name = "Test Custom Strategy 1";
            GeneratedHierarchy.Open();
            LoggingSettingsNode node = new LoggingSettingsNode();
            GeneratedApplicationNode.Nodes.AddWithDefaultChildren(node);
            DistributionStrategyCollectionNode distributionStrategyCollectionNode = GeneratedHierarchy.FindNodeByType(typeof(DistributionStrategyCollectionNode)) as DistributionStrategyCollectionNode;
            Assert.IsNotNull(distributionStrategyCollectionNode);
            distributionStrategyCollectionNode.Nodes.Clear();
            CustomDistributionStrategyNode customNode = new CustomDistributionStrategyNode();
            distributionStrategyCollectionNode.Nodes.AddWithDefaultChildren(customNode);
            customNode.TypeName = typeof(InProcDistributionStrategyData).AssemblyQualifiedName;
            customNode.Name = name;
            GeneratedHierarchy.Save();

            ApplicationConfigurationNode appNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(appNode, CreateDefaultConfiguration());
            HierarchyService.SelectedHierarchy = hierarchy;
            hierarchy.Open();
            LoggingSettingsNode settingsNode = hierarchy.FindNodeByType(typeof(LoggingSettingsNode)) as LoggingSettingsNode;
            Assert.IsNotNull(settingsNode);
            distributionStrategyCollectionNode = hierarchy.FindNodeByType(appNode, typeof(DistributionStrategyCollectionNode)) as DistributionStrategyCollectionNode;
            Assert.IsNotNull(distributionStrategyCollectionNode);
            ConfigurationNode [] nodes = hierarchy.FindNodesByType(distributionStrategyCollectionNode, typeof(CustomDistributionStrategyNode));
            Assert.AreEqual(1, nodes.Length);
            Assert.AreEqual(name, nodes[0].Name);
        }

		[Test]
		public void GetResource()
		{
			Image img = GetImageFromResource(typeof(DistributionStrategyNode), null, false);
			Assert.IsNotNull(img);

			img = GetImageFromResource(typeof(ClientSettingsNode), null, false);
			Assert.IsNotNull(img);
		}

		// The following code was taken from Everett ToolboxBitmapAttribute

		private static readonly Point largeDim = new Point(32, 32);
	
		private static Image GetImageFromResource( Type t, string imageName, bool large )
		{
			Image img = null;
			try 
			{

				string name = imageName;

				// if we didn't get a name, use the class name
				//
				if (name == null) 
				{
					name = t.FullName;
					int indexDot = name.LastIndexOf('.');
					if (indexDot != -1) 
					{
						name = name.Substring(indexDot+1);
					}
					name += ".bmp";
				}

				// load the image from the manifest resources. 
				//
				Stream stream = t.Module.Assembly.GetManifestResourceStream(t, name);
				if (stream != null) 
				{
					img = new Bitmap(stream);                    
					MakeBackgroundAlphaZero((Bitmap)img);
					if (large) 
					{
						img = new Bitmap((Bitmap)img, largeDim.X, largeDim.Y);
					}
				}
			}
			catch (Exception e) 
			{
				Debug.Fail("Failed to load toolbox image for " + t.FullName + " " + e.ToString());
			}
			return img;
		}

		private static void MakeBackgroundAlphaZero(Bitmap img) 
		{
			Color bottomLeft = img.GetPixel(0, img.Height - 1);
			img.MakeTransparent();

			Color newBottomLeft = Color.FromArgb(0, bottomLeft);
			img.SetPixel(0, img.Height - 1, newBottomLeft);
		}
	}
}
#endif