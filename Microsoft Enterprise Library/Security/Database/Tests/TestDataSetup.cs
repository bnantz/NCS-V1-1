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
using System.Security.Cryptography;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EntLibData = Microsoft.Practices.EnterpriseLibrary.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Tests
{
    public class TestDataSetup
    {
        public static void CreateTestData(ConfigurationContext context)
        {
            UserRoleManager urm = new UserRoleManager("EntLibSecurity", context);

            DeleteTestData(context);

            byte[] pwd = SHA1Managed.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes("Password"));
            byte [] emptyPasswd = SHA1Managed.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(""));

            urm.CreateUser("testuser", pwd);
            urm.CreateUser("bogususer", pwd);
            urm.CreateUser("emptyUser", emptyPasswd);

            urm.CreateRole("Admins");
            urm.CreateRole("Managers");
            urm.CreateRole("Users");

            urm.CreateUserRole("testuser", "Users");
            urm.CreateUserRole("testuser", "Managers");
        }

        public static void DeleteTestData(ConfigurationContext context)
        {
            Data.Database db = DatabaseFactory.CreateDatabase("EntLibSecurity");
            DBCommandWrapper cmd = db.GetSqlStringCommandWrapper("delete Profiles");
            db.ExecuteNonQuery(cmd);

            UserRoleManager urm = new UserRoleManager("EntLibSecurity", context);

            try
            {
                urm.DeleteUserRole("testuser", "Users");
            }
            catch
            {
            }
            try
            {
                urm.DeleteUserRole("testuser", "Managers");
            }
            catch
            {
            }

            try
            {
                urm.DeleteRole("Admins");
            }
            catch
            {
            }
            try
            {
                urm.DeleteRole("Managers");
            }
            catch
            {
            }
            try
            {
                urm.DeleteRole("Users");
            }
            catch
            {
            }

            try
            {
                urm.DeleteUser("testuser");
            try {urm.DeleteUser("emptyUser");} catch{}
        }
            catch
            {
    }
            try
            {
                urm.DeleteUser("bogususer");
}
            catch
            {
            }
            try
            {
                urm.DeleteUser("emptyUser");
            }
            catch
            {
            }
        }
    }
}

#endif
