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

#if       UNIT_TESTS
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Tools.ConfigurationConsole.Tests
{
    [TestFixture]
    public class ConfigurationNodeImageContainerFixture
    {
        [SelectedImage(typeof(MyNode), "BALLOON.BMP"), Image(typeof(MyNode), "BALLOON.BMP")]
        private class MyNode : ConfigurationNode
        {
            public MyNode() : base("mynode")
            {
            }
        }

        private class MyDerivedNode : MyNode
        {
            public MyDerivedNode()
            {
            }
        }

        [Test]
        public void GetImageIndexTest()
        {
            ImageList list = new ImageList();
            ConfigurationNodeImageContainer imageContainer = new ConfigurationNodeImageContainer(list);
            int imageIndex = imageContainer.GetImageIndex(typeof(MyNode));
            int selectedImageIndex = imageContainer.GetSelectedImageIndex(typeof(MyNode));
            Assert.IsTrue(imageIndex >= 0);
            Assert.IsTrue(selectedImageIndex >= 0);

            Assert.AreEqual(2, list.Images.Count);
        }

        [Test]
        public void ImageInheritanceTest()
        {
            ImageList list = new ImageList();
            ConfigurationNodeImageContainer imageContainer = new ConfigurationNodeImageContainer(list);
            int imageIndex = imageContainer.GetImageIndex(typeof(MyNode));
            int selectedImageIndex = imageContainer.GetSelectedImageIndex(typeof(MyNode));
            Assert.IsTrue(imageIndex >= 0);
            Assert.IsTrue(selectedImageIndex >= 0);

            Assert.AreEqual(2, list.Images.Count);

            imageIndex = imageContainer.GetImageIndex(typeof(MyDerivedNode));
            selectedImageIndex = imageContainer.GetSelectedImageIndex(typeof(MyDerivedNode));

            Assert.IsTrue(imageIndex >= 0);
            Assert.IsTrue(selectedImageIndex >= 0);

            Assert.AreEqual(2, list.Images.Count);
        }
    }
}

#endif