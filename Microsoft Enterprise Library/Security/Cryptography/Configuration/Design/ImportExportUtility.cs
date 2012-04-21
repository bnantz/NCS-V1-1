//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// A utility for the importing and exporting of keys.
    /// </summary>
    public class ImportExportUtility
    {
        /// <summary>
        /// Gets the flag which indicates that the exported key is protected by a password.
        /// </summary>
        public const byte PasswordProtectedFlag = 0x0;
        /// <summary>
        /// Gets the flag which indicates that the exported key is not protected by a password.
        /// </summary>
        private const byte PlaintextFlag = 0x1;

        private const int FlagLength = 1;
        private const int PassHashLength = 32;

        private bool passwordProtected = true;

        /// <summary>
        /// Exports a key to a file with a password.
        /// </summary>
        /// <param name="key">The key to export.</param>
        /// <param name="destination">The destination path of the file in which to save the key.</param>
        /// <param name="passphrase">The password which protects the key.</param>
        public void Export(byte[] key, string destination, string passphrase)
        {
            ValidateExportArguments(key, destination);

            if (passphrase.Length == 0)
            {
                passwordProtected = false;
            }

            string exportKey = ExportKey(key, passphrase);
            ExportKeyToFile(exportKey, destination);
        }

        /// <summary>
        /// Imports a key from a file with a password.
        /// </summary>
        /// <param name="destination">The destination path of the file in which to read the key.</param>
        /// <param name="passphrase">The password which protects the key.</param>
        /// <returns>The key.</returns>
        public byte[] Import(string destination, string passphrase)
        {
            ValidateImportArguments(destination);

            string contents = GetFileContents(destination);

            byte[] contentBytes = Convert.FromBase64String(contents);

            if (contentBytes[0] == PasswordProtectedFlag)
            {
                passwordProtected = true;
            }
            else
            {
                passwordProtected = false;
            }

            if (passphrase.Length == 0 && passwordProtected)
            {
                throw new ArgumentException(SR.MissingPassword, "passphrase");
            }

            byte[] importKey = DecryptKey(contentBytes, passphrase);

            return importKey;
        }

        /// <summary>
        /// Determines if a key is protected with a password or not.
        /// </summary>
        /// <param name="exportedKeyFile">The path of the file in which to read the key.</param>
        /// <returns><c>true</c> if the key is protected with a password. Otherwise, <c>false</c>.</returns>
        public bool IsPasswordProtected(string exportedKeyFile)
        {
            string contents = GetFileContents(exportedKeyFile);
            byte[] contentBytes = Convert.FromBase64String(contents);

            return (contentBytes[0] == PasswordProtectedFlag);
        }

        private string GetFileContents(string file)
        {
            FileStream stream = null;
            StreamReader reader = null;
            string contents = String.Empty;

            try
            {
                stream = new FileStream(file, FileMode.Open, FileAccess.Read);
                reader = new StreamReader(stream);
                contents = reader.ReadToEnd();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                else if (stream != null)
                {
                    stream.Close();
                }
            }

            return contents;
        }

        private string ExportKey(byte[] key, string passphrase)
        {
            string exportKey = string.Empty;
            if (passwordProtected)
            {
                exportKey = EncryptKey(key, passphrase);
            }
            else
            {
                byte[] output = new byte[key.Length + FlagLength];
                output[0] = PlaintextFlag;
                Buffer.BlockCopy(key, 0, output, 1, key.Length);
                exportKey = Convert.ToBase64String(output);
            }

            return exportKey;
        }

        private void ExportKeyToFile(string encryptedKey, string destination)
        {
            using (FileStream fs = new FileStream(destination, FileMode.Create,
                                                  FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(encryptedKey);
                }
            }
        }

        private string EncryptKey(byte[] key, string passphrase)
        {
            byte[] output = null;
            byte[] passphraseBytes = GetPassphraseBytes(passphrase);

            using (SymmetricAlgorithm algorithm = RijndaelManaged.Create())
            {
                SymmetricCryptographer crypto = new SymmetricCryptographer(algorithm, passphraseBytes);
                byte[] encryptedKey = crypto.Encrypt(key);
                output = AppendPasswordHash(encryptedKey, passphraseBytes, PasswordProtectedFlag);
            }

            return Convert.ToBase64String(output);
        }

        private byte[] DecryptKey(byte[] contentBytes, string passphrase)
        {
            byte[] importKey = null;

            if (passwordProtected)
            {
                byte[] passHash = CheckPassword(contentBytes, passphrase);

                byte[] encryptedKey = new byte[contentBytes.Length - FlagLength - PassHashLength];
                Buffer.BlockCopy(contentBytes, FlagLength + PassHashLength, encryptedKey, 0, encryptedKey.Length);

                using (SymmetricAlgorithm algorithm = RijndaelManaged.Create())
                {
                    SymmetricCryptographer crypto = new SymmetricCryptographer(algorithm, passHash);
                    importKey = crypto.Decrypt(encryptedKey);
                }
            }
            else
            {
                importKey = new byte[contentBytes.Length - 1];
                Buffer.BlockCopy(contentBytes, 1, importKey, 0, contentBytes.Length - FlagLength);
            }

            return importKey;
        }

        private byte[] CheckPassword(byte[] contentBytes, string passphrase)
        {
            byte[] passHash = GetPassphraseBytes(passphrase);

            byte[] passHashTest = new byte[PassHashLength];
            Buffer.BlockCopy(contentBytes, FlagLength, passHashTest, 0, PassHashLength);

            bool compare = CryptographyUtility.CompareBytes(passHash, passHashTest);
            if (!compare)
            {
                throw new CryptographicException(SR.DecryptPasswordCheckFailure);
            }

            return passHash;
        }

        private byte[] AppendPasswordHash(byte[] exportKey, byte[] passHash, byte flag)
        {
            int flagLength = 1;
            byte[] output = new byte[exportKey.Length + passHash.Length + flagLength];
            output[0] = flag;
            Buffer.BlockCopy(passHash, 0, output, flagLength, passHash.Length);
            Buffer.BlockCopy(exportKey, 0, output, passHash.Length + flagLength, exportKey.Length);

            return output;
        }

        private byte[] GetPassphraseBytes(string passphrase)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(passphrase);
            return SHA256Managed.Create().ComputeHash(passwordBytes);
        }

        private void ValidateExportArguments(byte[] key, string destination)
        {
            if (key == null || key.Length == 0)
            {
                throw new ArgumentNullException("key");
            }

            if (destination == null || destination.Length == 0)
            {
                throw new ArgumentNullException("destination");
            }

            string path = Path.GetDirectoryName(destination);
            if (!Directory.Exists(path))
            {
                throw new ArgumentException(SR.InvalidExportDestination);
            }
        }

        private void ValidateImportArguments(string destination)
        {
            if (destination == null || destination.Length == 0)
            {
                throw new ArgumentNullException("destination");
            }

            if (!File.Exists(destination))
            {
                throw new ArgumentException(SR.InvalidImportSource);
            }
        }
    }
}