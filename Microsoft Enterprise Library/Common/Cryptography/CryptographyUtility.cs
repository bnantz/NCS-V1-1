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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Cryptography
{
    /// <summary>
    /// <para>Common Cryptography methods.</para>
    /// </summary>
    public sealed class CryptographyUtility
    {
        private const string applicationName = "Application";
        private static readonly string eventLogSource = SR.EnterpriseLibraryCryptography;

        private CryptographyUtility()
        {
        }

        /// <summary>
        /// <para>Logs an exception that occurs during a cryptography operation to the event log.</para>
        /// </summary>
        /// <param name="ex">
        /// <para>The <see cref="Exception"/> to log.</para>
        /// </param>
        public static void LogCryptographyException(Exception ex)
        {
            ExceptionFormatter formatter = new ExceptionFormatter(new NameValueCollection(), applicationName);
            EventLog.WriteEntry(eventLogSource, formatter.GetMessage(ex), EventLogEntryType.Error);
        }

        /// <summary>
        /// <para>Determine if two byte arrays are equal.</para>
        /// </summary>
        /// <param name="byte1">
        /// <para>The first byte array to compare.</para>
        /// </param>
        /// <param name="byte2">
        /// <para>The byte array to compare to the first.</para>
        /// </param>
        /// <returns>
        /// <para><see langword="true"/> if the two byte arrays are equal; otherwise <see langword="false"/>.</para>
        /// </returns>
        public static bool CompareBytes(byte[] byte1, byte[] byte2)
        {
            if (byte1 == null || byte2 == null)
            {
                return false;
            }
            if (byte1.Length != byte2.Length)
            {
                return false;
            }

            bool result = true;
            for (int i = 0; i < byte1.Length; i++)
            {
                if (byte1[i] != byte2[i])
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// <para>Returns a byte array from a string representing a hexidecimal number.</para>
        /// </summary>
        /// <param name="hexidecimalNumber">
        /// <para>The string containing a valid hexidecimal number.</para>
        /// </param>
        /// <returns><para>The byte array representing the hexidecimal.</para></returns>
        public static byte[] GetBytesFromHexString(string hexidecimalNumber)
        {
            StringBuilder sb = new StringBuilder(hexidecimalNumber.ToUpper(CultureInfo.CurrentCulture));

            if (sb[0].Equals('0') && sb[1].Equals('X'))
            {
                sb.Remove(0, 2);
            }

            if (sb.Length % 2 != 0)
            {
                throw new ArgumentException(SR.InvalidHexString);
            }

            byte[] hexBytes = new byte[sb.Length / 2];
            try
            {
                for (int i = 0; i < hexBytes.Length; i++)
                {
                    int stringIndex = i * 2;
                    hexBytes[i] = Convert.ToByte(sb.ToString(stringIndex, 2), 16);
                }
            }
            catch (FormatException ex)
            {
                throw new ArgumentException(SR.InvalidHexString, ex);
            }

            return hexBytes;
        }

        /// <summary>
        /// <para>Returns a string from a byte array represented as a hexidecimal number (eg: 0F351A).</para>
        /// </summary>
        /// <param name="bytes">
        /// <para>The byte array to convert to forat as a hexidecimal number.</para>
        /// </param>
        /// <returns>
        /// <para>The formatted representation of the bytes as a hexidcimal number.</para>
        /// </returns>
        public static string GetHexStringFromBytes(byte[] bytes)
        {
            ArgumentValidation.CheckForNullReference(bytes, "bytes");
            ArgumentValidation.CheckForZeroBytes(bytes, "bytes");

            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("X2", CultureInfo.InvariantCulture));
            }
            return sb.ToString();
        }

        /// <summary>
        /// <para>Combines two byte arrays into one.</para>
        /// </summary>
        /// <param name="buffer1"><para>The prefixed bytes.</para></param>
        /// <param name="buffer2"><para>The suffixed bytes.</para></param>
        /// <returns><para>The combined byte arrays.</para></returns>
        public static byte[] CombineBytes(byte[] buffer1, byte[] buffer2)
        {
            byte[] combinedBytes = new byte[buffer1.Length + buffer2.Length];
            Buffer.BlockCopy(buffer1, 0, combinedBytes, 0, buffer1.Length);
            Buffer.BlockCopy(buffer2, 0, combinedBytes, buffer1.Length, buffer2.Length);

            return combinedBytes;
        }

        /// <summary>
        /// Creates a cryptographically strong random set of bytes.
        /// </summary>
        /// <param name="size">The size of the byte array to generate.</param>
        /// <returns>The computed bytes.</returns>
        public static byte[] GetRandomBytes(int size)
        {
            byte[] randomBytes = new byte[size];
            GetRandomBytes(randomBytes);
            return randomBytes;
        }

        /// <summary>
        /// <para>Fills a byte array with a cryptographically strong random set of bytes.</para>
        /// </summary>
        /// <param name="bytes"><para>The byte array to fill.</para></param>
        public static void GetRandomBytes(byte[] bytes)
        {
            RNGCryptoServiceProvider.Create().GetBytes(bytes);
        }

        /// <summary>
        /// <para>Fills <paramref name="bytes"/> zeros.</para>
        /// </summary>
        /// <param name="bytes">
        /// <para>The byte array to fill.</para>
        /// </param>
        public static void ZeroOutBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                return;
            }

            for (int index = 0; index < bytes.Length; ++index)
            {
                bytes[index] = 0;
            }
        }
    }
}