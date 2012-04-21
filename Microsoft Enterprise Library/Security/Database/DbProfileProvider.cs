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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database
{
    /// <summary>
    /// Represents a database-based profile provider.
    /// </summary>
    /// <remarks>
    /// A profile can contain primitives, serializable objects, or a dictionary of 
    /// primitives and serializable objects.  Primitives are saved and retrieved as strings,
    /// objects are serialized using XmlSerialization.  Each dictionary entry is processed
    /// and saved as a row in the Profile table.
    /// </remarks>
    public class DbProfileProvider : ConfigurationProvider, IProfileProvider
    {
        private const string SPAddProfile = "AddProfile";
        private const string SPGetProfile = "GetProfile";
        private const string SPDeleteProfile = "DeleteProfile";

        private int maxProfileLength = 4096;
        private SecurityConfigurationView securityConfigurationView;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DbProfileProvider"/> class.</para>
        /// </summary>
        public DbProfileProvider()
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
        /// Persist a user's profile to the database.
        /// </summary>
        /// <param name="identity">An authenticated user's identity with a username that 
        /// matches a user in the Identity table.</param>
        /// <param name="profile">Object that represents the user's profile.  
        /// A single primitive, serializable object, or a dictionary of primitives and objects.</param>
        public void SetProfile(IIdentity identity, object profile)
        {
            ArgumentValidation.CheckForNullReference(identity, "identity");
            ArgumentValidation.CheckForNullReference(profile, "profile");

            Data.Database securityDb = GetSecurityDatabase();

            string userName = identity.Name;
            using (IDbConnection connection = securityDb.GetConnection())
            {
                connection.Open();
                IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    DeleteProfile(userName, securityDb, transaction);
                    InsertProfile(userName, SerializeProfile(profile), securityDb, transaction);
                }
                catch (ArgumentException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new InvalidOperationException(SR.SavingProfileException(userName), e);
                }
                transaction.Commit();
            }

            SecurityProfileSaveEvent.Fire(identity.Name);
        }

        /// <summary>
        /// Retreive a user's profile from the database.
        /// </summary>
        /// <param name="identity">An authenticated user's identity with a username that 
        /// matches a user in the Users table.</param>
        /// <returns>An object representing the profile of the user.  Return value can be a string,
        /// deserialized object, or a Hashtable of values.</returns>
        /// <remarks>
        /// <strong>Null</strong> is returned if the user or profile does not exist.</remarks>
        public object GetProfile(IIdentity identity)
        {
            ArgumentValidation.CheckForNullReference(identity, "identity");

            byte[] serializedProfile = LoadProfile(identity.Name);

            SecurityProfileLoadEvent.Fire(identity.Name);

            object profile = null;
            if (serializedProfile != null)
            {
                profile = DeserializeProfile(serializedProfile);
            }

            return profile;
        }

        private byte[] SerializeProfile(object profile)
        {
            byte[] buffer = null;

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, profile);
                buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, (int)ms.Length);
                ms.Flush();
            }

            if (buffer.Length > maxProfileLength)
            {
                throw new ArgumentException(SR.ProfileExceedsMaxLength(buffer.Length, maxProfileLength));
            }

            return buffer;
        }

        private object DeserializeProfile(byte[] serializedProfile)
        {
            object profile = null;

            BinaryFormatter formatter = new BinaryFormatter();

            using (MemoryStream ms = new MemoryStream(serializedProfile))
            {
                profile = formatter.Deserialize(ms);
            }

            return profile;
        }

        private byte[] LoadProfile(string userName)
        {
            Data.Database securityDb = GetSecurityDatabase();
            DBCommandWrapper cmd = securityDb.GetStoredProcCommandWrapper(SPGetProfile);
            cmd.AddInParameter("userName", DbType.String, userName);
            cmd.AddOutParameter("profile", DbType.Binary, maxProfileLength);

            securityDb.ExecuteNonQuery(cmd);

            object profile = cmd.GetParameterValue("profile");
            return profile == DBNull.Value ? null : (byte[])profile;
        }

        private void DeleteProfile(string userName, Data.Database securityDb, IDbTransaction transaction)
        {
            DBCommandWrapper cmd = securityDb.GetStoredProcCommandWrapper(SPDeleteProfile);
            cmd.AddInParameter("userName", DbType.String, userName);
            securityDb.ExecuteNonQuery(cmd, transaction);
        }

        private void InsertProfile(string userName, byte[] serializedProfile, Data.Database securityDb, IDbTransaction transaction)
        {
            DBCommandWrapper cmd = securityDb.GetStoredProcCommandWrapper(SPAddProfile);
            cmd.AddInParameter("userName", DbType.String, userName);
            cmd.AddInParameter("profile", DbType.Binary, serializedProfile);
            securityDb.ExecuteNonQuery(cmd, transaction);
        }

        private Data.Database GetSecurityDatabase()
        {
            ProfileProviderData profileProviderData = securityConfigurationView.GetProfileProviderData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(profileProviderData, typeof(DbProfileProviderData));

            DbProfileProviderData dbProfileProviderData = (DbProfileProviderData)profileProviderData;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(securityConfigurationView.ConfigurationContext);
            return factory.CreateDatabase(dbProfileProviderData.Database);
        }
    }
}