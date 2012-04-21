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

//---------------------------------------------------------------------
// <copyright file="UserRoleManagerBoundsFixture.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Defines the UserRoleManagerBoundsFixture class.
// </summary>
//---------------------------------------------------------------------

#if UNIT_TESTS
using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Tests
{
    [TestFixture]
    public class UserRoleManagerBoundsFixture : ConfigurationContextFixtureBase
    {
        private const string DbServiceKey = "EntLibSecurity";
        private UserRoleManager manager;

        private delegate void ParamsDelegate(params object[] methodParams);

        [SetUp]
        public void Setup()
        {
            manager = new UserRoleManager(DbServiceKey, Context);
        }

        [Test]
        public void CreateUser()
        {
            ParamsDelegate createUser = new ParamsDelegate(CreateUserDelegate);
            Assert.IsTrue(ExpectArgumentNull(createUser, null, new byte[1]), "Null username");
            Assert.IsTrue(ExpectArgumentNull(createUser, "test", null), "Null password");
            Assert.IsTrue(ExpectArgumentNull(createUser, String.Empty, new byte[1]), "Empty username");
        }

        [Test]
        public void ChangeUserPassword()
        {
            ParamsDelegate changeUserPassword = new ParamsDelegate(ChangeUserPasswordDelegate);
            Assert.IsTrue(ExpectArgumentNull(changeUserPassword, null, new byte[1]), "Null username");
            Assert.IsTrue(ExpectArgumentNull(changeUserPassword, "test", null), "Null password");
            Assert.IsTrue(ExpectArgumentNull(changeUserPassword, String.Empty, new byte[1]), "Empty username");
        }

        [Test]
        public void GetUserIdFromUsername()
        {
            ParamsDelegate getUserIdFromUsername = new ParamsDelegate(GetUserIdFromUsernameDelegate);
            Assert.IsTrue(ExpectArgumentNull(getUserIdFromUsername, null), "Null username");
            Assert.IsTrue(ExpectArgumentNull(getUserIdFromUsername, String.Empty), "Empty username");
        }

        [Test]
        public void DeleteUser()
        {
            ParamsDelegate deleteUser = new ParamsDelegate(DeleteUserDelegate);
            Assert.IsTrue(ExpectArgumentNull(deleteUser, null), "Null username");
            Assert.IsTrue(ExpectArgumentNull(deleteUser, String.Empty), "Empty username");
        }

        [Test]
        public void CreateRole()
        {
            ParamsDelegate createRole = new ParamsDelegate(CreateRoleDelegate);
            Assert.IsTrue(ExpectArgumentNull(createRole, null), "Null role");
            Assert.IsTrue(ExpectArgumentNull(createRole, String.Empty), "Empty role");
        }

        [Test]
        public void GetRoleIdFromRoleName()
        {
            ParamsDelegate getRoleIdFromRoleName = new ParamsDelegate(GetRoleIdFromRoleNameDelegate);
            Assert.IsTrue(ExpectArgumentNull(getRoleIdFromRoleName, null), "Null role");
            Assert.IsTrue(ExpectArgumentNull(getRoleIdFromRoleName, String.Empty), "Empty role");
        }

        [Test]
        public void RenameRole()
        {
            ParamsDelegate renameRole = new ParamsDelegate(RenameRoleDelegate);
            Assert.IsTrue(ExpectArgumentNull(renameRole, null, "test"), "Original role null");
            Assert.IsTrue(ExpectArgumentNull(renameRole, String.Empty, "test"), "Original role empty");
            Assert.IsTrue(ExpectArgumentNull(renameRole, "test", null), "New role null");
            Assert.IsTrue(ExpectArgumentNull(renameRole, "test", String.Empty), "New role empty");
        }

        [Test]
        public void DeleteRole()
        {
            ParamsDelegate deleteRole = new ParamsDelegate(DeleteRoleDelegate);
            Assert.IsTrue(ExpectArgumentNull(deleteRole, null), "Null role");
            Assert.IsTrue(ExpectArgumentNull(deleteRole, String.Empty), "Empty role");
        }

        [Test]
        public void CreateUserRole()
        {
            ParamsDelegate createUserRole = new ParamsDelegate(CreateUserRoleDelegate);
            Assert.IsTrue(ExpectArgumentNull(createUserRole, null, "test"), "Null username");
            Assert.IsTrue(ExpectArgumentNull(createUserRole, String.Empty, "test"), "Empty username");
            Assert.IsTrue(ExpectArgumentNull(createUserRole, "test", null), "Null role");
            Assert.IsTrue(ExpectArgumentNull(createUserRole, "test", String.Empty), "Empty role");
        }

        [Test]
        public void DeleteUserRole()
        {
            ParamsDelegate deleteUserRole = new ParamsDelegate(DeleteUserRoleDelegate);
            Assert.IsTrue(ExpectArgumentNull(deleteUserRole, null, "test"), "Null username");
            Assert.IsTrue(ExpectArgumentNull(deleteUserRole, String.Empty, "test"), "Empty username");
            Assert.IsTrue(ExpectArgumentNull(deleteUserRole, "test", null), "Null role");
            Assert.IsTrue(ExpectArgumentNull(deleteUserRole, "test", String.Empty), "Empty role");
        }

        [Test]
        public void CreateUserRoleBatch()
        {
            ParamsDelegate createUserRoleBatch = new ParamsDelegate(CreateUserRoleBatchDelegate);
            Assert.IsTrue(ExpectArgumentNull(createUserRoleBatch, null), "Null dataTable");
        }

        [Test]
        public void DeleteUserRoleBatch()
        {
            ParamsDelegate deleteUserRoleBatch = new ParamsDelegate(DeleteUserRoleBatchDelegate);
            Assert.IsTrue(ExpectArgumentNull(deleteUserRoleBatch, null), "Null dataTable");
        }

        [Test]
        public void GetRoleUsers()
        {
            ParamsDelegate getRoleUsers = new ParamsDelegate(GetRoleUsersDelegate);
            Assert.IsTrue(ExpectArgumentNull(getRoleUsers, null), "Null role");
            Assert.IsTrue(ExpectArgumentNull(getRoleUsers, String.Empty), "Empty role");
        }

        [Test]
        public void GetPassword()
        {
            ParamsDelegate getPassword = new ParamsDelegate(GetPasswordDelegate);
            Assert.IsTrue(ExpectArgumentNull(getPassword, null), "Null password");
            Assert.IsTrue(ExpectArgumentNull(getPassword, String.Empty), "Empty password");
        }

        [Test]
        public void UserExists()
        {
            ParamsDelegate userExists = new ParamsDelegate(UserExistsDelegate);
            Assert.IsTrue(ExpectArgumentNull(userExists, null), "Null username");
            Assert.IsTrue(ExpectArgumentNull(userExists, String.Empty), "Empty username");
        }

        [Test]
        public void GetUserRoles()
        {
            ParamsDelegate getUserRoles = new ParamsDelegate(GetUserRolesDelegate);
            Assert.IsTrue(ExpectArgumentNull(getUserRoles, null), "Null username");
            Assert.IsTrue(ExpectArgumentNull(getUserRoles, String.Empty), "Empty username");
        }

        private void GetUserRolesDelegate(params object[] methodParams)
        {
            manager.GetUserRoles((string)methodParams[0]);
        }

        private void UserExistsDelegate(params object[] methodParams)
        {
            manager.UserExists((string)methodParams[0]);
        }

        private void GetPasswordDelegate(params object[] methodParams)
        {
            manager.GetPassword((string)methodParams[0]);
        }

        private void GetRoleUsersDelegate(params object[] methodParams)
        {
            manager.GetRoleUsers((string)methodParams[0]);
        }

        private void DeleteUserRoleBatchDelegate(params object[] methodParams)
        {
            manager.DeleteUserRoleBatch((DataTable)methodParams[0]);
        }

        private void CreateUserRoleBatchDelegate(params object[] methodParams)
        {
            manager.CreateUserRoleBatch((DataTable)methodParams[0]);
        }

        private void DeleteUserRoleDelegate(params object[] methodParams)
        {
            manager.DeleteUserRole((string)methodParams[0], (string)methodParams[1]);
        }

        private void CreateUserRoleDelegate(params object[] methodParams)
        {
            manager.CreateUserRole((string)methodParams[0], (string)methodParams[1]);
        }

        private void DeleteRoleDelegate(params object[] methodParams)
        {
            manager.DeleteRole((string)methodParams[0]);
        }

        private void RenameRoleDelegate(params object[] methodParams)
        {
            manager.RenameRole((string)methodParams[0], (string)methodParams[1]);
        }

        private void GetRoleIdFromRoleNameDelegate(params object[] methodParams)
        {
            manager.GetRoleIdFromRoleName((string)methodParams[0]);
        }

        private void CreateRoleDelegate(params object[] methodParams)
        {
            manager.CreateRole((string)methodParams[0]);
        }

        private void DeleteUserDelegate(params object[] methodParams)
        {
            manager.DeleteUser((string)methodParams[0]);
        }

        private void GetUserIdFromUsernameDelegate(params object[] methodParams)
        {
            manager.GetUserIdFromUserName((string)methodParams[0]);
        }

        private void ChangeUserPasswordDelegate(params object[] methodParams)
        {
            manager.ChangeUserPassword((string)methodParams[0], (byte[])methodParams[1]);
        }

        private void CreateUserDelegate(params object[] methodParams)
        {
            manager.CreateUser((string)methodParams[0], (byte[])methodParams[1]);
        }

        private bool ExpectArgumentNull(ParamsDelegate myMethod, params object[] methodParams)
        {
            bool ret = false;

            try
            {
                if (methodParams == null)
                {
                    myMethod(new object[] {null});
                }
                else
                {
                    myMethod(methodParams);
                }
            }
            catch (ArgumentNullException)
            {
                ret = true;
            }

            return ret;
        }
    }
}

#endif