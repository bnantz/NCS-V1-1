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

#if  UNIT_TESTS
using System;
using System.Data;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Tests
{
    [TestFixture]
    public class UserRoleManagerFixture : ConfigurationContextFixtureBase
    {
        private const string DbServiceKey = "EntLibSecurity";
        private const string userName = "fredFlinstone";
        private const string roleName = "Executives";
        private const string roleName2 = "Senior Executives";

        private UserRoleManager manager;
        private Data.Database userDb;

        public override void FixtureSetup()
        {
            base.FixtureSetup();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(Context);
            userDb = factory.CreateDatabase(DbServiceKey);
        }

        public override void FixtureTeardown()
        {
            FlushData();
            base.FixtureTeardown();
        }

        [SetUp]
        public void Setup()
        {
            manager = new UserRoleManager(DbServiceKey, Context);
            FlushData();
        }

        [Test]
        public void CreateAndDeleteUser()
        {
            byte[] password = ASCIIEncoding.ASCII.GetBytes("zab123");
            Assert.IsTrue(manager.CreateUser(userName, password));

            DataSet ds = SelectIdentity();
            Assert.AreEqual(1, ds.Tables[0].Rows.Count);
            Assert.AreEqual(userName, ds.Tables[0].Rows[0]["UserName"]);

            ds = GetPassword(userName);
            Assert.AreEqual(password.Length, ((byte[])ds.Tables[0].Rows[0]["Password"]).Length);

            manager.DeleteUser(userName);
            ds = SelectIdentity();
            Assert.AreEqual(0, ds.Tables[0].Rows.Count);
        }

        [Test]
        public void CreateExistingUser()
        {
            byte[] password = ASCIIEncoding.ASCII.GetBytes("zab123");
            manager.CreateUser(userName, password);

            Assert.IsFalse(manager.CreateUser(userName, password));
            manager.DeleteUser(userName);
        }

        [Test]
        public void GetPassword()
        {
            byte[] password = ASCIIEncoding.ASCII.GetBytes("zab123");
            manager.CreateUser(userName, password);

            byte[] result = manager.GetPassword(userName);
            Assert.AreEqual(result.Length, password.Length);

            manager.DeleteUser(userName);
        }

        [Test]
        public void GetNullPassword()
        {
            byte[] result = manager.GetPassword(Guid.NewGuid().ToString());
            Assert.IsNull(result);
        }

        [Test]
        public void UserExists()
        {
            byte[] password = ASCIIEncoding.ASCII.GetBytes("zab123");
            manager.CreateUser(userName, password);

            bool exists = manager.UserExists(userName);

            Assert.IsTrue(exists);

            exists = manager.UserExists("badUser");

            Assert.IsFalse(exists);
            manager.DeleteUser(userName);
        }

        [Test]
        public void ChangePassword()
        {
            byte[] password = ASCIIEncoding.ASCII.GetBytes("zab123");
            manager.CreateUser(userName, password);

            byte[] newPassword = ASCIIEncoding.ASCII.GetBytes("5678901234");
            manager.ChangeUserPassword(userName, newPassword);

            DataSet ds = GetPassword(userName);
            Assert.AreEqual(newPassword.Length, ((byte[])ds.Tables[0].Rows[0]["Password"]).Length);

            manager.DeleteUser(userName);
        }

        [Test]
        public void CreateAndDeleteRole()
        {
            manager.CreateRole(roleName);

            DataSet ds = SelectRole(roleName);
            Assert.AreEqual(1, ds.Tables[0].Rows.Count);
            Assert.AreEqual(roleName, ds.Tables[0].Rows[0]["RoleName"]);

            // delete the row
            manager.DeleteRole(roleName);
            ds = SelectIdentity();
            Assert.AreEqual(0, ds.Tables[0].Rows.Count);
        }

        [Test]
        public void RenameRole()
        {
            manager.CreateRole(roleName);

            DataSet ds = SelectRole(roleName);
            Assert.AreEqual(1, ds.Tables[0].Rows.Count);

            manager.RenameRole(roleName, roleName2);
            ds = SelectRole(roleName);
            Assert.AreEqual(0, ds.Tables[0].Rows.Count);

            ds = SelectRole(roleName2);
            Assert.AreEqual(1, ds.Tables[0].Rows.Count);

            manager.DeleteRole(roleName2);
        }

        [Test]
        public void AddAndRemoveUserRoles()
        {
            byte[] password = ASCIIEncoding.ASCII.GetBytes("zab123");

            manager.CreateUser(userName, password);
            manager.CreateRole(roleName);
            manager.CreateRole(roleName2);

            manager.CreateUserRole(userName, roleName);
            manager.CreateUserRole(userName, roleName2);

            DataSet ds = SelectUserRoles(userName);
            Assert.AreEqual(2, ds.Tables[0].Rows.Count);
            Assert.AreEqual(roleName, ds.Tables[0].Rows[0]["RoleName"]);
            Assert.AreEqual(roleName2, ds.Tables[0].Rows[1]["RoleName"]);

            // delete the rows
            manager.DeleteUserRole(userName, roleName);
            manager.DeleteUserRole(userName, roleName2);
            ds = SelectUserRoles(userName);
            Assert.AreEqual(0, ds.Tables[0].Rows.Count);

            manager.DeleteRole(roleName);
            manager.DeleteRole(roleName2);
            manager.DeleteUser(userName);
        }

        [Test]
        public void GetUserRoles()
        {
            byte[] password = ASCIIEncoding.ASCII.GetBytes("password");

            manager.CreateRole(roleName);
            manager.CreateRole(roleName + "2");
            manager.CreateUser("user1", password);
            manager.CreateUserRole("user1", roleName);
            manager.CreateUserRole("user1", roleName + "2");

            try
            {
                DataSet ds = manager.GetUserRoles("user1");
                Assert.AreEqual(2, ds.Tables[0].Rows.Count);
            }
            finally
            {
                manager.DeleteUserRole("user1", roleName);
                manager.DeleteUserRole("user1", roleName + "2");
                manager.DeleteUser("user1");
                manager.DeleteRole(roleName);
                manager.DeleteRole(roleName + "2");
            }
        }

        [Test]
        public void GetRoleUsers()
        {
            byte[] password = ASCIIEncoding.ASCII.GetBytes("password");

            manager.CreateRole(roleName);
            manager.CreateUser("user1", password);
            manager.CreateUser("user2", password);
            manager.CreateUserRole("user1", roleName);
            manager.CreateUserRole("user2", roleName);

            DataSet ds = manager.GetRoleUsers(roleName);
            Assert.AreEqual(2, ds.Tables[0].Rows.Count);

            manager.DeleteUserRole("user1", roleName);
            manager.DeleteUserRole("user2", roleName);
            manager.DeleteUser("user1");
            manager.DeleteUser("user2");
            manager.DeleteRole(roleName);
        }

        [Test]
        public void GetAllUsers()
        {
            byte[] password = ASCIIEncoding.ASCII.GetBytes("password");

            manager.CreateUser("user1", password);
            manager.CreateUser("user2", password);

            DataSet ds = manager.GetAllUsers();
            Assert.AreEqual(2, ds.Tables[0].Rows.Count);

            manager.DeleteUser("user1");
            manager.DeleteUser("user2");
        }

        [Test]
        public void GetAllRoles()
        {
            manager.CreateRole("role1");
            manager.CreateRole("role2");

            DataSet ds = manager.GetAllRoles();
            Assert.AreEqual(2, ds.Tables[0].Rows.Count);

            manager.DeleteRole("role1");
            manager.DeleteRole("role2");
        }

        [Test]
        public void AddAndRemoveMultipleUsersToARole()
        {
            byte[] password = ASCIIEncoding.ASCII.GetBytes("123");

            manager.CreateUser("user1", password);
            manager.CreateUser("user2", password);
            manager.CreateUser("user3", password);
            manager.CreateRole(roleName);
            manager.CreateRole(roleName2);

            // create batch
            manager.CreateUserRoleBatch(GetBatchData());
            DataSet ds = SelectUserRoles("user1");
            Assert.AreEqual(2, ds.Tables[0].Rows.Count);
            Assert.AreEqual(roleName, ds.Tables[0].Rows[0]["RoleName"]);
            Assert.AreEqual(roleName2, ds.Tables[0].Rows[1]["RoleName"]);

            ds = SelectUserRoles("user2");
            Assert.AreEqual(1, ds.Tables[0].Rows.Count);
            Assert.AreEqual(roleName, ds.Tables[0].Rows[0]["RoleName"]);

            ds = SelectUserRoles("user3");
            Assert.AreEqual(1, ds.Tables[0].Rows.Count);
            Assert.AreEqual(roleName, ds.Tables[0].Rows[0]["RoleName"]);

            // delete batch
            manager.DeleteUserRoleBatch(GetBatchData());
            ds = SelectUserRoles("user1");
            Assert.AreEqual(0, ds.Tables[0].Rows.Count);

            ds = SelectUserRoles("user2");
            Assert.AreEqual(0, ds.Tables[0].Rows.Count);

            ds = SelectUserRoles("user3");
            Assert.AreEqual(0, ds.Tables[0].Rows.Count);

            manager.DeleteRole(roleName);
            manager.DeleteRole(roleName2);
            manager.DeleteUser("user1");
            manager.DeleteUser("user2");
            manager.DeleteUser("user3");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeletingNonExistantUserThrowsException()
        {
            manager.DeleteUser("UnknownUser");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeletingNonExistantRoleThrowsException()
        {
            manager.DeleteRole("UnknownRole");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeletingNonExistantUserRoleThrowsExceptionWithNonExistantUser()
        {
            manager.CreateRole("SuperHero");
            manager.DeleteUserRole("Batman", "SuperHero");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeletingNonExistantUserRoleThrowsExceptionWithNonExistantRole()
        {
            byte[] password = ASCIIEncoding.ASCII.GetBytes("zab123");
            manager.CreateUser("Batman", password);
            manager.DeleteUserRole("Batman", "SuperHero");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeletingBatchUserRolesWithMissingUserThrowsException()
        {
            byte[] password = ASCIIEncoding.ASCII.GetBytes("123");

            manager.CreateUser("user1", password);
            manager.CreateUser("user2", password);
            manager.CreateUser("user3", password);
            manager.CreateRole(roleName);
            manager.CreateRole(roleName2);

            DataTable data = GetBatchData();
            try
            {
                manager.CreateUserRoleBatch(data);
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            DataTable stuffToRemove = new DataTable("Table");
            stuffToRemove.Columns.Add("UserName", typeof(string));
            stuffToRemove.Columns.Add("RoleName", typeof(string));

            DataRow goodRow = stuffToRemove.NewRow();
            goodRow["UserName"] = "user1";
            goodRow["RoleName"] = roleName;
            stuffToRemove.Rows.Add(goodRow);

            DataRow badRow = stuffToRemove.NewRow();
            badRow["UserName"] = "Unknown User";
            badRow["RoleName"] = roleName;
            stuffToRemove.Rows.Add(badRow);

            manager.DeleteUserRoleBatch(stuffToRemove);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeletingBatchUserRolesWithMissingRoleThrowsException()
        {
            byte[] password = ASCIIEncoding.ASCII.GetBytes("123");

            manager.CreateUser("user1", password);
            manager.CreateUser("user2", password);
            manager.CreateUser("user3", password);
            manager.CreateRole(roleName);
            manager.CreateRole(roleName2);

            DataTable data = GetBatchData();
            manager.CreateUserRoleBatch(data);

            DataTable stuffToRemove = new DataTable("Table");
            stuffToRemove.Columns.Add("UserName", typeof(string));
            stuffToRemove.Columns.Add("RoleName", typeof(string));

            DataRow goodRow = stuffToRemove.NewRow();
            goodRow["UserName"] = "user1";
            goodRow["RoleName"] = roleName;
            stuffToRemove.Rows.Add(goodRow);

            DataRow badRow = stuffToRemove.NewRow();
            badRow["UserName"] = "user1";
            badRow["RoleName"] = "Unknown Role";
            stuffToRemove.Rows.Add(badRow);

            manager.DeleteUserRoleBatch(stuffToRemove);
        }

        private DataTable GetBatchData()
        {
            DataTable batch = new DataTable("Table");
            batch.Columns.Add("UserName", typeof(string));
            batch.Columns.Add("RoleName", typeof(string));

            DataRow newRow;
            newRow = batch.NewRow();
            newRow["UserName"] = "user1";
            newRow["RoleName"] = roleName;
            batch.Rows.Add(newRow);

            newRow = batch.NewRow();
            newRow["UserName"] = "user1";
            newRow["RoleName"] = roleName2;
            batch.Rows.Add(newRow);

            newRow = batch.NewRow();
            newRow["UserName"] = "user2";
            newRow["RoleName"] = roleName;
            batch.Rows.Add(newRow);

            newRow = batch.NewRow();
            newRow["UserName"] = "user3";
            newRow["RoleName"] = roleName;
            batch.Rows.Add(newRow);

            return batch;
        }

        private DataSet SelectIdentity()
        {
            DBCommandWrapper cmd = userDb.GetSqlStringCommandWrapper("select * from Users where UserName = @name");
            cmd.AddInParameter("name", DbType.String, userName);
            return userDb.ExecuteDataSet(cmd);
        }

        private DataSet SelectRole(string role)
        {
            DBCommandWrapper cmd = userDb.GetSqlStringCommandWrapper("select * from Roles where RoleName = @name");
            cmd.AddInParameter("name", DbType.String, role);
            return userDb.ExecuteDataSet(cmd);
        }

        private DataSet SelectUserRoles(string user)
        {
            DBCommandWrapper cmd = userDb.GetStoredProcCommandWrapper("GetRolesByName");
            cmd.AddInParameter("name", DbType.String, user);
            return userDb.ExecuteDataSet(cmd);
        }

        public DataSet GetPassword(string user)
        {
            DBCommandWrapper cmd = userDb.GetStoredProcCommandWrapper("GetPassword");
            cmd.AddInParameter("Name", DbType.String, user);
            return userDb.ExecuteDataSet(cmd);
        }

        private void FlushData()
        {
            DBCommandWrapper userRoleWrapper = userDb.GetSqlStringCommandWrapper("delete from UserRoles");
            userDb.ExecuteNonQuery(userRoleWrapper);

            DBCommandWrapper roleWrapper = userDb.GetSqlStringCommandWrapper("delete from Roles");
            userDb.ExecuteNonQuery(roleWrapper);

            DBCommandWrapper profileWrapper = userDb.GetSqlStringCommandWrapper("delete from Profiles");
            userDb.ExecuteNonQuery(profileWrapper);

            DBCommandWrapper userWrapper = userDb.GetSqlStringCommandWrapper("delete from Users");
            userDb.ExecuteNonQuery(userWrapper);
        }
    }
}

#endif