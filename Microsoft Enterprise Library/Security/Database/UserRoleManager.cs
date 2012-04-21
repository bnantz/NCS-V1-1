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

using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EntLibData = Microsoft.Practices.EnterpriseLibrary.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database
{
    /// <summary>
    /// Provides utility functions for managing the Security database.
    /// </summary>
    public class UserRoleManager
    {
        private Data.Database securityDb;

        private const string SPCreateUser = "InsertUser";
        private const string SPDeleteUser = "DeleteUserByName";
        private const string SPCreateRole = "InsertRole";
        private const string SPDeleteRole = "DeleteRoleByName";
        private const string SPCreateUserRole = "AddUserToRoleByName";
        private const string SPDeleteUserRole = "RemoveUserFromRoleByName";
        private const string SPGetIdentityId = "GetUserIdByName";
        private const string SPGetRoleId = "GetRoleIdByName";
        private const string SPRenameRole = "UpdateRoleById";
        private const string SPChangePassword = "ChangePasswordByName";
        private const string SPGetPassword = "GetPassword";
        private const string SPGetRolesByName = "GetRolesByName";

        private const string SPGetAllRoles = "GetAllRoles";
        private const string SPGetAllUsers = "GetAllUsers";
        private const string SPGetRoleUsers = "GetUserInRoleByName";

        /// <summary>
        /// Creates an instance of the utility class with a specified configuration context.
        /// </summary>
        /// <param name="databaseService">Database service instance name.</param>
        /// <param name="context">The configuration context to use.</param>
        public UserRoleManager(string databaseService, ConfigurationContext context)
        {
            DatabaseProviderFactory dbProviderFactory = new DatabaseProviderFactory(context);
            this.securityDb = dbProviderFactory.CreateDatabase(databaseService);
        }

        /// <summary>
        /// Create a new user record.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password bytes.  Be sure to hash or encrypt.</param>
        /// <returns><c>true</c> if the user was created. <c>false</c> if the user already exists.</returns>
        public bool CreateUser(string userName, byte[] password)
        {
            ValidateParam("userName", userName);
            ValidateParam("password", password);

            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPCreateUser);
            cmd.AddInParameter("name", DbType.String, userName);
            cmd.AddInParameter("password", DbType.Binary, password);
            cmd.AddOutParameter("userExists", DbType.Byte, 1);

            this.securityDb.ExecuteNonQuery(cmd);

            int ret = (byte)cmd.GetParameterValue("userExists");

            return (ret == 0);
        }

        /// <summary>
        /// Change the password for a user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="newEncryptedPassword">New password. Should be encrypted before being passed to this method.</param>
        public void ChangeUserPassword(string userName, byte[] newEncryptedPassword)
        {
            ValidateParam("userName", userName);
            ValidateParam("newPassword", newEncryptedPassword);

            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPChangePassword);
            cmd.AddInParameter("name", DbType.String, userName);
            cmd.AddInParameter("password", DbType.Binary, newEncryptedPassword);

            this.securityDb.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Get a user's ID from the user name.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns>User ID</returns>
        public int GetUserIdFromUserName(string userName)
        {
            ValidateParam("userName", userName);

            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPGetIdentityId);
            cmd.AddInParameter("name", DbType.String, userName);
            cmd.AddOutParameter("userId", DbType.Int32, 4);
            this.securityDb.ExecuteNonQuery(cmd);

            object userId = cmd.GetParameterValue("userId");
            return userId == DBNull.Value ? -1 : (int)userId;
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="userName">User name.</param>
        public void DeleteUser(string userName)
        {
            ValidateParam("userName", userName);

            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPDeleteUser);
            cmd.AddInParameter("name", DbType.String, userName);
            this.securityDb.ExecuteNonQuery(cmd);

            if (cmd.RowsAffected == 0)
            {
                throw new InvalidOperationException(SR.NoUserExists(userName));
            }
        }

        /// <summary>
        /// Create a role.
        /// </summary>
        /// <param name="role">Role name.</param>
        public void CreateRole(string role)
        {
            ValidateParam("role", role);

            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPCreateRole);
            cmd.AddInParameter("name", DbType.String, role);
            this.securityDb.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Get the role ID from a role's name.
        /// </summary>
        /// <param name="role">Role name.</param>
        /// <returns>The RoleID if exists. <value>-1</value> if it doesn't exist.</returns>
        public int GetRoleIdFromRoleName(string role)
        {
            ValidateParam("role", role);

            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPGetRoleId);
            cmd.AddInParameter("name", DbType.String, role);
            cmd.AddOutParameter("roleID", DbType.Int32, 4);
            this.securityDb.ExecuteNonQuery(cmd);

            object ret = cmd.GetParameterValue("RoleID");
            return ret == DBNull.Value ? -1 : (int)cmd.GetParameterValue("RoleID");
        }

        /// <summary>
        /// Rename a role.
        /// </summary>
        /// <param name="originalName">Original role name.</param>
        /// <param name="newName">New role name.</param>
        public void RenameRole(string originalName, string newName)
        {
            ValidateParam("originalName", originalName);
            ValidateParam("newName", newName);

            int id = GetRoleIdFromRoleName(originalName);
            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPRenameRole);
            cmd.AddInParameter("roleId", DbType.Int32, id);
            cmd.AddInParameter("roleName", DbType.String, newName);
            this.securityDb.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Delete a role.
        /// </summary>
        /// <param name="role">Role name.</param>
        public void DeleteRole(string role)
        {
            ValidateParam("role", role);

            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPDeleteRole);
            cmd.AddInParameter("name", DbType.String, role);
            try
            {
                this.securityDb.ExecuteNonQuery(cmd);
            }
            catch (SqlException e)
            {
                if (e.State == 99)
                {
                    throw new InvalidOperationException(SR.NoRoleExists(role));
                }
                throw;
            }
        }

        /// <summary>
        /// Associate a user to a role.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="role">Role name.</param>
        public void CreateUserRole(string userName, string role)
        {
            ValidateParam("userName", userName);
            ValidateParam("role", role);

            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPCreateUserRole);
            cmd.AddInParameter("name", DbType.String, userName);
            cmd.AddInParameter("roleName", DbType.String, role);
            this.securityDb.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Delete an association of a user to a role.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="role">Role name.</param>
        public void DeleteUserRole(string userName, string role)
        {
            ValidateParam("userName", userName);
            ValidateParam("role", role);

            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPDeleteUserRole);
            cmd.AddInParameter("userName", DbType.String, userName);
            cmd.AddInParameter("roleName", DbType.String, role);
            try
            {
                this.securityDb.ExecuteNonQuery(cmd);
            }
            catch (SqlException e)
            {
                if (e.State == 99)
                {
                    throw new InvalidOperationException(SR.NoUserExists(userName));
                }
                if (e.State == 98)
                {
                    throw new InvalidOperationException(SR.NoRoleExists(role));
                }
                throw;
            }
        }

        /// <summary>
        /// Create a batch of user roles.
        /// </summary>
        /// <param name="batch">Table containing user role assignments.</param>
        public void CreateUserRoleBatch(DataTable batch)
        {
            ValidateParam("batch", batch);

            DataSet ds = new DataSet();
            ds.Tables.Add(batch);
            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPCreateUserRole);
            cmd.AddInParameter("name", DbType.String, "UserName", DataRowVersion.Current);
            cmd.AddInParameter("roleName", DbType.String, "RoleName", DataRowVersion.Current);
            this.securityDb.UpdateDataSet(ds, batch.TableName, cmd, null, null, UpdateBehavior.Transactional);
        }

        /// <summary>
        /// Delete a batch of user roles.
        /// </summary>
        /// <param name="batch">Table containing user role assignments to delete.</param>
        public void DeleteUserRoleBatch(DataTable batch)
        {
            ValidateParam("batch", batch);

            DataSet ds = new DataSet();
            ds.Tables.Add(batch);
            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPDeleteUserRole);
            cmd.AddInParameter("userName", DbType.String, "UserName", DataRowVersion.Default);
            cmd.AddInParameter("roleName", DbType.String, "RoleName", DataRowVersion.Default);
            try
            {
                this.securityDb.UpdateDataSet(ds, batch.TableName, cmd, null, null, UpdateBehavior.Transactional);
            }
            catch (SqlException e)
            {
                if (e.State == 99)
                {
                    throw new InvalidOperationException(SR.BatchNoUserExists);
                }
                if (e.State == 98)
                {
                    throw new InvalidOperationException(SR.BatchNoRoleExists);
                }
                throw;
            }
        }

        /// <summary>
        /// Get the users associated with a role.
        /// </summary>
        /// <param name="role">Role name.</param>
        /// <returns>DataSet of users associated to a role.</returns>
        public DataSet GetRoleUsers(string role)
        {
            ValidateParam("role", role);

            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPGetRoleUsers);
            cmd.AddInParameter("roleName", DbType.String, role);
            return this.securityDb.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// Get the password for a user.
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>Password.</returns>
        public byte[] GetPassword(string userName)
        {
            ValidateParam("userName", userName);

            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(
                SPGetPassword);
            cmd.AddInParameter("Name", DbType.String, userName);

            byte[] password = (byte[])securityDb.ExecuteScalar(cmd);

            return password;
        }

        /// <summary>
        /// Check to see if a user exists.
        /// </summary>
        /// <param name="userName">User name to test for.</param>
        /// <returns><b>True</b> if the user exists.</returns>
        public bool UserExists(string userName)
        {
            ValidateParam("userName", userName);

            bool result = false;
            int identId = GetUserIdFromUserName(userName);
            if (identId > 0)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get all roles.
        /// </summary>
        /// <returns>A DataSet containing a row for each role.</returns>
        public DataSet GetAllRoles()
        {
            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPGetAllRoles);
            return this.securityDb.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// Get a list of users.
        /// </summary>
        /// <returns>DataSet containing a row for each user.</returns>
        public DataSet GetAllUsers()
        {
            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPGetAllUsers);
            return this.securityDb.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// Get all roles.
        /// </summary>
        /// <param name="userName">User name for which to retrieve all roles</param>
        /// <returns>A DataSet containing a row for each role.</returns>
        public DataSet GetUserRoles(string userName)
        {
            ValidateParam("userName", userName);

            DBCommandWrapper cmd = this.securityDb.GetStoredProcCommandWrapper(SPGetRolesByName);
            cmd.AddInParameter("name", DbType.String, userName);
            return this.securityDb.ExecuteDataSet(cmd);
        }

        private void ValidateParam(string paramName, object param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }
            else if (param is string)
            {
                string stringParam = (string)param;
                if (stringParam.Length == 0)
                {
                    throw new ArgumentNullException(paramName);
                }
            }
        }
    }
}