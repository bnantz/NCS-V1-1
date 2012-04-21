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
using System.Collections;
using System.DirectoryServices;
using System.Security.Principal;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory
{
    /// <summary>
    /// Active Directory Roles Provider.
    /// </summary>
    public class AdRolesProvider : SecurityRolesProvider
    {
        private SecurityConfigurationView securityConfigurationView;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AdRolesProvider()
        {
        }

        /// <summary>
        /// <para>
        /// Initializes the provider with its name and any attributes associated with the provider. 
        /// </para>
        /// </summary>  
        /// <param name="configurationView">A <see cref="SecurityConfigurationView"></see> object</param>      
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof (SecurityConfigurationView));
            securityConfigurationView = (SecurityConfigurationView) configurationView;
        }

        /// <summary>
        /// Overridden from SecurityRolesProvider. Retrieves the list of user roles
        /// from the underlying active directory services.
        /// </summary>
        /// <param name="userIdentity">Identity of user used for retrieval</param>
        /// <returns>String array of roles for the given user identity</returns>
        protected override string[] CollectAllUserRoles(IIdentity userIdentity)
        {
            AdRolesProviderData adRolesProviderData = GetAdRolesProviderData();

            string adamBindString = CreateAdamBindString(adRolesProviderData);
            string accountName = adRolesProviderData.AccountName;

            DirectoryEntry adamUser = GetUserFromLdap(adamBindString, accountName, userIdentity);
            object groups = adamUser.Invoke("Groups");

            StringBuilder tmpRoles = CollectRoles(groups);
            return tmpRoles.ToString().TrimEnd(',').Split(',');
        }

        private AdRolesProviderData GetAdRolesProviderData()
        {
            RolesProviderData rolesProviderData = securityConfigurationView.GetRolesProviderData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(rolesProviderData, typeof (AdRolesProviderData));
    
            AdRolesProviderData adRolesProviderData = (AdRolesProviderData) rolesProviderData;
            ValidateConfigData(adRolesProviderData);
            return adRolesProviderData;
        }

        private StringBuilder CollectRoles(object groups)
        {
            DirectoryEntry groupEntry;
            StringBuilder tmpRoles = new StringBuilder();
            foreach (object group in (IEnumerable) groups)
            {
                groupEntry = new DirectoryEntry(group);
                tmpRoles.Append(groupEntry.Name.Substring(3));
                tmpRoles.Append(",");
            }
            return tmpRoles;
        }

        private DirectoryEntry GetUserFromLdap(string adamBindString, string accountName, IIdentity userIdentity)
        {
            DirectoryEntry adamEntLib = new DirectoryEntry(adamBindString);
            string ldapFilter = "(" + accountName + "=" + GetUsernameFromIdentityName(userIdentity) + ")";
    
            DirectorySearcher src = new DirectorySearcher(adamEntLib, ldapFilter, null, SearchScope.OneLevel);
    
            SearchResult res = src.FindOne();
            if(res == null) throw new UserNotFoundException(GetUsernameFromIdentityName(userIdentity));

            return res.GetDirectoryEntry();
        }

        private string CreateAdamBindString(AdRolesProviderData adRolesProviderData)
        {
            string adamBindString = adRolesProviderData.ProviderType + "://";
            adamBindString += adRolesProviderData.Server + (adRolesProviderData.Server.Length > 0 ? "/" : "");
            adamBindString += adRolesProviderData.UserPartition;
            return adamBindString;
        }

        private static void ValidateConfigData(AdRolesProviderData adRolesConfigData)
        {
            if ((adRolesConfigData.ProviderType == null || adRolesConfigData.ProviderType.Length == 0) ||
                (adRolesConfigData.Server == null || adRolesConfigData.Server.Length == 0) ||
                (adRolesConfigData.UserPartition == null || adRolesConfigData.UserPartition.Length == 0))
            {
                throw new ArgumentNullException(SR.ADAuthorizationProviderConfigDataInvalid);
            }
        }

        private string GetUsernameFromIdentityName(IIdentity identity)
        {
            string name = identity.Name;
            int whackIdx = identity.Name.IndexOf('\\');

            if (whackIdx >= 0)
            {
                ++whackIdx;
                name = name.Substring(whackIdx, name.Length - whackIdx);
            }

            return name;
        }
    }
}