//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests
{
    [TestFixture]
    public class FileDependencyFixture
    {
        [SetUp]
        public void InitializeTestFile()
        {
            if (File.Exists("TestFile"))
            {
                File.Delete("TestFile");
            }
            FileStream newFile = File.Create("TestFile");
            newFile.Close();
        }

        [TearDown]
        public void RemoveTestFile()
        {
            if (File.Exists("TestFile"))
            {
                File.Delete("TestFile");
            }
        }

        [Test]
        public void ExpiresReturnsTrueIfFileDisappears()
        {
            FileDependency dependency = new FileDependency("TestFile");

            File.Delete("TestFile");

            Assert.IsTrue(dependency.HasExpired(), "Deleted files should always be considered expired");
        }

        [Test]
        public void ExpiresIfTouched()
        {
            FileDependency dependency = new FileDependency("TestFile");
            Thread.Sleep(1500);
            using (FileStream outputStream = File.OpenWrite("TestFile"))
            {
                outputStream.WriteByte(0x00);
            }

            Assert.IsTrue(dependency.HasExpired(), "File was touched, so it should be considered expired");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void SecurityPermissionsDoNotCauseExceptionIfFileNotPresent()
        {
            new FileDependency("shouldNeverExist");
        }

        [Test]
        [ExpectedException(typeof(SecurityException))]
        public void NoPermissionToReadFileWhenCreated()
        {
            FileIOPermission denyPermission =
                new FileIOPermission(FileIOPermissionAccess.Read, Path.GetFullPath("TestFile"));

            PermissionSet permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(denyPermission);
            permissions.Deny();

            try
            {
                new FileDependency("TestFile");
            }
            finally
            {
                CodeAccessPermission.RevertDeny();
            }
        }

        [Test]
        [ExpectedException(typeof(SecurityException))]
        public void NoPermissionToReadWhenCheckingExpiration()
        {
            FileDependency dependency = new FileDependency("TestFile");

            FileIOPermission denyPermission =
                new FileIOPermission(FileIOPermissionAccess.Read, Path.GetFullPath("TestFile"));

            PermissionSet permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(denyPermission);
            permissions.Deny();

            try
            {
                dependency.HasExpired();
            }
            finally
            {
                CodeAccessPermission.RevertDeny();
            }
        }
    }
}

#endif