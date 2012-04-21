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

using System.Data;
using System.Security.Principal;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration;
using EntLibData = Microsoft.Practices.EnterpriseLibrary.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database
{
    /// <summary>
    /// Defines the functionality for a Database-based Roles Provider.
    /// </summary>
    public class DbRolesProvider : SecurityRolesProvider
    {
        private SecurityConfigurationView securityConfigurationView;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DbRolesProvider"/> class.</para>
        /// </summary>
        public DbRolesProvider()
        {
        }

        /// <summary>
        /// <para>Initializes the provider with a <see cref="SecurityConfigurationView"/>.</para>
        /// </summary>
        /// <param name="configurationView">
        /// <para>A <see cref="SecurityConfigurationView"/> object.</para>
        /// </param>        
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof(SecurityConfigurationView));

            this.securityConfigurationView = (SecurityConfigurationView)configurationView;
        }

        /// <summary>
        /// Overridden from SecurityRolesProvider. Retrieves the list of user roles
        /// from the underlying database.
        /// </summary>
        /// <param name="userIdentity">Identity of user used for retrieval</param>
        /// <returns>String array of roles for the given user identity</returns>
        protected override string[] CollectAllUserRoles(IIdentity userIdentity)
        {
            //Copy each role name into a string array
            DbRolesProviderData dbRolesProviderData = (DbRolesProviderData)securityConfigurationView.GetRolesProviderData(ConfigurationName);
            UserRoleManager manager = new UserRoleManager(dbRolesProviderData.Database, securityConfigurationView.ConfigurationContext);
            DataSet dsRoles = manager.GetUserRoles(userIdentity.Name);

            StringBuilder tmpRoles = new StringBuilder();
            foreach (DataRow row in dsRoles.Tables[0].Rows)
            {
                tmpRoles.Append(row["RoleName"]);
                tmpRoles.Append(",");
            }

            return tmpRoles.ToString().TrimEnd(',').Split(',');
        }
    }
}