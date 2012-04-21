//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Library Shared Library
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using System;
using System.Security;
using System.Security.Permissions;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    [Serializable]
    [TestFixture]
    public class CasFixture
    {
        [Test]
        [ExpectedException(typeof(SecurityException))]
        public void TestEnvironmentPermissionSet()
        {
            GetMachineNameWithoutPermissions();
        }

        /// <summary>
        /// Try/finally is completely unnecessary. Permissions are associated with stack frames
        /// and go out of scope once stack frame returns.
        /// </summary>
        private string GetMachineNameWithoutPermissions()
        {
            EnvironmentPermission denyPermission = new EnvironmentPermission(PermissionState.None);

            PermissionSet permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(denyPermission);
            permissions.Deny();

            try
            {
                return Environment.MachineName;
            }
            finally
            {
                CodeAccessPermission.RevertDeny();
            }
        }
    }
}

#endif