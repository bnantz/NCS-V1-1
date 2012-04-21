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

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Cryptography
{
    /// <summary>
    /// <para>Represents a managed wrapper over the DPAPI win32 apis.</para>
    /// </summary>
    /// <remarks>
    /// This is a DPAPI wrapper.  It is heavily based on the reference application on the Microsoft whitepaper at:
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnnetsec/html/SecNetHT07.asp?frame=true
    /// There have been some improvements, notably in the stricter cleanup of unmanaged global handles in Finally blocks.
    /// </remarks>
    public sealed class DpapiCryptographer
    {
        private const int CryptProtectUIForbidden = 0x1;
        private const int CryptProtectLocalMachine = 0x4;

        private DpapiStorageMode storeMode;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DpapiCryptographer"/> class with the <see cref="DpapiStorageMode"/> set to <see cref="DpapiStorageMode.Machine"/>.</para>
        /// </summary>
        public DpapiCryptographer() : this(DpapiStorageMode.Machine)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DpapiCryptographer"/> class with a <see cref="DpapiStorageMode"/>.</para>
        /// </summary>
        /// <param name="storageMode"><para>One of the <see cref="DpapiStorageMode"/> values.</para></param>
        public DpapiCryptographer(DpapiStorageMode storageMode)
        {
            this.storeMode = storageMode;
        }

        /// <summary>
        /// <para>Gets the storage mode for this instance.</para>
        /// </summary>
        /// <value>One of the <see cref="DpapiStorageMode"/> values.</value>
        public DpapiStorageMode StorageMode
        {
            get { return storeMode; }
        }

        /// <summary>
        /// <para>
        /// Encrypt given data; this overload can be used ONLY when storage mode is "User", since when storage mode 
        /// is "Machine" we MUST have optional entropy to "salt" the phrase.
        /// This will throw an Invalid Operation Exception if used in Machine mode.
        /// </para>
        /// </summary>
        /// <param name="plaintext"><para>The plain text that will be encrypted.</para></param>
        /// <returns><para>The resulting cipher text.</para></returns>
        public byte[] Encrypt(byte[] plaintext)
        {
            if (DpapiStorageMode.Machine == storeMode)
            {
                throw new InvalidOperationException(SR.DpapiMustHaveEntropyForMachineMode);
            }

            return Encrypt(plaintext, null);
        }

        /// <summary>
        /// <para>
        /// Decrypts the given ciphertext.  Can be used only when in "User" mode, otherwise this will throw 
        /// an InvalidOperationException because entropy is required when using Machine mode.
        /// </para>
        /// </summary>
        /// <param name="cipherText">
        /// <para>The cipher text that will be decrypted.</para>
        /// </param>
        /// <returns>
        /// <para>The resulting plain text.</para>
        /// </returns>
        public byte[] Decrypt(byte[] cipherText)
        {
            if (DpapiStorageMode.Machine == storeMode)
            {
                throw new InvalidOperationException(SR.DpapiMustHaveEntropyForMachineMode);
            }

            return Decrypt(cipherText, null);
        }

        /// <summary>
        /// <para>
        /// Encrypt given data; this overload should be used when storage mode is "Machine", since when storage mode 
        /// is "Machine" you must define entropy to "salt" the phrase.
        /// </para>
        /// </summary>
        /// <param name="plaintext"><para>The plain text that will be encrypted.</para></param>
        /// <param name="entropy"><para>The entropy to salt the phrase.</para></param>
        /// <returns><para>The resulting cipher text.</para></returns>
        public byte[] Encrypt(byte[] plaintext, byte[] entropy)
        {
            ArgumentValidation.CheckForNullReference(plaintext, "plainText");
            ArgumentValidation.CheckForZeroBytes(plaintext, "plainText");

            ValidateStoreEntropyArguments(entropy);
            return EncryptInternal(plaintext, entropy);
        }

        /// <summary>
        /// <para>Decrypt the given data.</para>
        /// </summary>
        /// <param name="cipherText"><para>The cipher text that will be decrypted.</para></param>
        /// <param name="entropy"><para>The entropy that was used to salt the phrase.</para></param>
        /// <returns><para>The resulting plain text.</para></returns>
        public byte[] Decrypt(byte[] cipherText, byte[] entropy)
        {
            ArgumentValidation.CheckForNullReference(cipherText, "cipherText");
            ArgumentValidation.CheckForZeroBytes(cipherText, "cipherText");

            ValidateStoreEntropyArguments(entropy);
            return DecryptInternal(cipherText, entropy);
        }

        private void ValidateStoreEntropyArguments(byte[] entropy)
        {
            if (DpapiStorageMode.Machine == storeMode && null == entropy)
            {
                throw new ArgumentNullException("entropy");
            }
            if (DpapiStorageMode.Machine == storeMode && entropy.Length < 1)
            {
                throw new ArgumentOutOfRangeException("entropy", 0, SR.DpapiZeroLengthEntropy);
            }
        }

        private byte[] EncryptInternal(byte[] plaintext, byte[] entropy)
        {
            NativeMethods.DataBlob plainTextBlob = new NativeMethods.DataBlob();
            NativeMethods.DataBlob cipherBlob = new NativeMethods.DataBlob();
            NativeMethods.DataBlob entropyBlob = new NativeMethods.DataBlob();

            //BUG:  possible bug here, do we need to clean up the prompt struct?
            //  don't think so, but check...
            NativeMethods.CryptProtectPromptStruct prompt = new NativeMethods.CryptProtectPromptStruct();
            InitPromptstruct(ref prompt);

            int flags;
            byte[] cipherText = null;

            try
            {
                plainTextBlob.DataPointer = Marshal.AllocHGlobal(plaintext.Length);

                plainTextBlob.Size = plaintext.Length;
                Marshal.Copy(plaintext, 0, plainTextBlob.DataPointer, plaintext.Length);

                if (DpapiStorageMode.Machine == storeMode)
                {
                    // Using the machine store, should be providing entropy.
                    flags = CryptProtectLocalMachine | CryptProtectUIForbidden;

                    entropyBlob.DataPointer = Marshal.AllocHGlobal(entropy.Length);

                    Marshal.Copy(entropy, 0, entropyBlob.DataPointer, entropy.Length);
                    entropyBlob.Size = entropy.Length;

                }
                else
                {
                    // Using the user store
                    flags = CryptProtectUIForbidden;
                }

                if (!NativeMethods.CryptProtectData(ref plainTextBlob, String.Empty, ref entropyBlob, IntPtr.Zero, ref prompt, flags, ref cipherBlob))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                cipherText = new byte[cipherBlob.Size];
                Marshal.Copy(cipherBlob.DataPointer, cipherText, 0, cipherBlob.Size);
            }
            finally
            {
                // Free the blob and entropy.  
                if (IntPtr.Zero != cipherBlob.DataPointer)
                {
                    Marshal.FreeHGlobal(cipherBlob.DataPointer);
                }
                if (IntPtr.Zero != entropyBlob.DataPointer)
                {
                    Marshal.FreeHGlobal(entropyBlob.DataPointer);
                }
                if (IntPtr.Zero != plainTextBlob.DataPointer)
                {
                    Marshal.FreeHGlobal(plainTextBlob.DataPointer);
                }
            }

            return cipherText;
        }

        private byte[] DecryptInternal(byte[] cipherText, byte[] entropy)
        {
            NativeMethods.DataBlob plainTextBlob = new NativeMethods.DataBlob();
            NativeMethods.DataBlob cipherBlob = new NativeMethods.DataBlob();
            NativeMethods.DataBlob entropyBlob = new NativeMethods.DataBlob();

            NativeMethods.CryptProtectPromptStruct prompt = new NativeMethods.CryptProtectPromptStruct();
            InitPromptstruct(ref prompt);

            int flags = 0;
            byte[] plainText = null;

            try
            {
                cipherBlob.DataPointer = Marshal.AllocHGlobal(cipherText.Length);

                cipherBlob.Size = cipherText.Length;
                Marshal.Copy(cipherText, 0, cipherBlob.DataPointer, cipherText.Length);

                if (DpapiStorageMode.Machine == storeMode)
                {
                    // Using the machine store, should be providing entropy.
                    flags = CryptProtectLocalMachine | CryptProtectUIForbidden;

                    entropyBlob.DataPointer = Marshal.AllocHGlobal(entropy.Length);
                    entropyBlob.Size = entropy.Length;

                    Marshal.Copy(entropy, 0, entropyBlob.DataPointer, entropy.Length);

                }
                else
                {
                    // Using the user store; therefore don't build entropy
                    flags = CryptProtectUIForbidden;
                }

                if (!NativeMethods.CryptUnprotectData(ref cipherBlob, null, ref entropyBlob, IntPtr.Zero, ref prompt, flags, ref plainTextBlob))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                plainText = new byte[plainTextBlob.Size];
                Marshal.Copy(plainTextBlob.DataPointer, plainText, 0, plainTextBlob.Size);
            }
            finally
            {
                // Free the blob and entropy.  
                if (IntPtr.Zero != cipherBlob.DataPointer)
                {
                    Marshal.FreeHGlobal(cipherBlob.DataPointer);
                }
                if (IntPtr.Zero != entropyBlob.DataPointer)
                {
                    Marshal.FreeHGlobal(entropyBlob.DataPointer);
                }
                if (IntPtr.Zero != plainTextBlob.DataPointer)
                {
                    Marshal.FreeHGlobal(plainTextBlob.DataPointer);
                }
            }

            return plainText;
        }

        private static void InitPromptstruct(ref NativeMethods.CryptProtectPromptStruct ps)
        {
            ps.Size = Marshal.SizeOf(typeof(NativeMethods.CryptProtectPromptStruct));
            ps.PromptFlags = 0;
            ps.AppWindowHandle = IntPtr.Zero;
            ps.Prompt = null;
        }
    }
}