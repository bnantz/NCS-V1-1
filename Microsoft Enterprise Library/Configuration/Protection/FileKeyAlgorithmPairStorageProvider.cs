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

using System;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Protection
{
    /// <summary>
    /// <para>Provides file storage for a <see cref="KeyAlgorithmPair"/> with optional protection.</para>
    /// </summary>
    public class FileKeyAlgorithmPairStorageProvider : ConfigurationProvider, IKeyAlgorithmPairStorageProvider
    {
        private RuntimeConfigurationView runtimeConfigurationView;

        /// <summary>
        /// <para>Initializes the provider with a <see cref="ConfigurationView"/>.</para>
        /// </summary>
        /// <param name="configurationView">
        /// <para>A <see cref="ConfigurationView"/> object.</para>
        /// </param>
        /// <remarks>
        /// <para>The method expects a <see cref="RuntimeConfigurationView"/> object for the <paramref name="configurationView"/></para>
        /// </remarks>
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");

        	runtimeConfigurationView = GetStorageCursor(configurationView);
            GetFileKeyAlgorithmPairStorageProviderData();
        }

        /// <summary>
        /// <para>Loads a <see cref="KeyAlgorithmPair"/> from the configured file.</para>
        /// </summary>
        /// <returns>
        /// <para>The deserialized <see cref="KeyAlgorithmPair"/>.</para>
        /// </returns>
        /// <exception cref="InvalidCastException">Thrown when a valid object is loaded, but it is not a <see cref="KeyAlgorithmPair"></see></exception> 
        /// <exception cref="ConfigurationException">Thrown when system is unable to deserialize the stored <see cref="KeyAlgorithmPair"></see></exception>
        public KeyAlgorithmPair Load()
        {
            KeyAlgorithmPair keyAlgorithmPair = null;

            FileKeyAlgorithmPairStorageProviderData fileKeyAlgorithmPairStorageProviderData = GetFileKeyAlgorithmPairStorageProviderData();

            using (FileStream fs = new FileStream(fileKeyAlgorithmPairStorageProviderData.Path, FileMode.Open, FileAccess.Read))
            {
                if (null != fileKeyAlgorithmPairStorageProviderData.DpapiSettings)
                {
                    keyAlgorithmPair = DeserializeProtectedFileStream(fs);
                }
                else
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        keyAlgorithmPair = formatter.Deserialize(fs) as KeyAlgorithmPair;
                    }
                    catch (SerializationException e)
                    {
                        throw new ConfigurationException(e.Message, e);
                    }
                }
            }
            return keyAlgorithmPair;
        }

        /// <summary>
        /// <para>Saves a <see cref="KeyAlgorithmPair"/> to the configured file.</para>
        /// </summary>
        /// <param name="keyAlgorithmPair">
        /// <para>The <see cref="KeyAlgorithmPair"/> to store.</para>
        /// </param>
        public void Save(KeyAlgorithmPair keyAlgorithmPair)
        {
            FileKeyAlgorithmPairStorageProviderData fileKeyAlgorithmPairStorageProviderData = GetFileKeyAlgorithmPairStorageProviderData();

            using (FileStream fs = new FileStream(fileKeyAlgorithmPairStorageProviderData.Path, FileMode.Create))
            {
                if (null != fileKeyAlgorithmPairStorageProviderData.DpapiSettings)
                {
                    SerializeAndProtectFileStream(fs, keyAlgorithmPair);
                }
                else
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        formatter.Serialize(fs, keyAlgorithmPair);
                    }
                    catch (SerializationException e)
                    {
                        throw new ConfigurationException(e.Message, e);
                    }
                }

                fs.Flush();
            }
        }

        private static RuntimeConfigurationView GetStorageCursor(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckExpectedType(configurationView, typeof (RuntimeConfigurationView));

            RuntimeConfigurationView runtimeConfigurationView = (RuntimeConfigurationView) configurationView;
            return runtimeConfigurationView;
        }

        private void SerializeAndProtectFileStream(FileStream fs, KeyAlgorithmPair keyAlgorithmPair)
        {
            byte[] buffer = null;

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, keyAlgorithmPair);
                buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);
                ms.Flush();
            }

            byte[] cipherBuffer = ProtectBytes(buffer);
            fs.Write(cipherBuffer, 0, cipherBuffer.Length);
        }

        private KeyAlgorithmPair DeserializeProtectedFileStream(FileStream fs)
        {
            KeyAlgorithmPair keyAlgorithmPair = null;
            BinaryFormatter formatter = new BinaryFormatter();

            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);

            using (MemoryStream ms = new MemoryStream(UnprotectBytes(buffer)))
            {
                keyAlgorithmPair = formatter.Deserialize(ms) as KeyAlgorithmPair;
            }

            return keyAlgorithmPair;
        }

        private byte[] ProtectBytes(byte[] plaintext)
        {
            FileKeyAlgorithmPairStorageProviderData fileKeyAlgorithmPairStorageProviderData = GetFileKeyAlgorithmPairStorageProviderData();
            DpapiCryptographer dpapi = new DpapiCryptographer(fileKeyAlgorithmPairStorageProviderData.DpapiSettings.Mode);
            return dpapi.Encrypt(plaintext, fileKeyAlgorithmPairStorageProviderData.DpapiSettings.Entropy);
        }

        private byte[] UnprotectBytes(byte[] ciphertext)
        {
            FileKeyAlgorithmPairStorageProviderData fileKeyAlgorithmPairStorageProviderData = GetFileKeyAlgorithmPairStorageProviderData();
            DpapiCryptographer dpapi = new DpapiCryptographer(fileKeyAlgorithmPairStorageProviderData.DpapiSettings.Mode);
            return dpapi.Decrypt(ciphertext, fileKeyAlgorithmPairStorageProviderData.DpapiSettings.Entropy);
        }

        private FileKeyAlgorithmPairStorageProviderData GetFileKeyAlgorithmPairStorageProviderData()
        {
            KeyAlgorithmPairStorageProviderData keyAlgorithmPairStorageProviderData = runtimeConfigurationView.GetKeyAlgorithmPairStorageProviderData();
            ArgumentValidation.CheckExpectedType(keyAlgorithmPairStorageProviderData, typeof (FileKeyAlgorithmPairStorageProviderData));

            return (FileKeyAlgorithmPairStorageProviderData) keyAlgorithmPairStorageProviderData;
        }
    }
}