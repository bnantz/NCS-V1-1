//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Application Block Shared Library
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Common
{
    /// <summary>
    /// <para>Helper class to do all the file work. It assumes that all files are rooted.</para>
    /// </summary>
    public sealed class FileHelper
    {
        private FileHelper()
        {
        }

        /// <summary>
        /// <para>Reset the file attributes of a file so it can be overwritten.</para>
        /// </summary>
        /// <param name="filePath">
        /// <para>The fully qualified path to the file.</para>
        /// </param>
        public static void ChangeFileAttributesToWritable(string filePath)
        {
            ArgumentValidation.CheckForNullReference(filePath, "filePath");

            if (!File.Exists(filePath))
            {
                return;
            }
            FileAttributes attributes = File.GetAttributes(filePath);
            FileAttributes attr = attributes | FileAttributes.ReadOnly;
            if (attr == attributes)
            {
                attributes ^= FileAttributes.ReadOnly;
                File.SetAttributes(filePath, attributes);
            }
        }

        /// <summary>
        /// <para>Determies if the file is read-only.</para>
        /// </summary>
        /// <param name="filePath">
        /// <para>The fully qualified path to the file.</para>
        /// </param>
        /// <returns>
        /// <para><see langword="true"/> if the file is read-only; otherwise <see langword="false"/>.</para>
        /// </returns>
        public static bool IsFileReadOnly(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }
            FileAttributes attributes = File.GetAttributes(filePath);
            FileAttributes attr = attributes | FileAttributes.ReadOnly;
            if (attr == attributes)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// <para>Creat a zero byte length file in the specified path.</para>
        /// </summary>
        /// <param name="filePath">
        /// <para>The absolute path to the file to create.</para>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="filePath"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <para>The caller does not have the required permission.</para>para>
        /// <para>-or-</para>
        /// <para><paramref name="filePath"/> specified a file that is read-only.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="filePath"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.InvalidPathChars"/>.</para>
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// <para>The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</para>
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <para>The specified path is invalid, such as being on an unmapped drive.</para>
        /// </exception>
        /// <exception cref="IOException">
        /// <para>An I/O error occurred while creating the file.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="filePath"/> is in an invalid format.</para>
        /// </exception>
        /// <seealso cref="System.IO.File.Create(string)"/>
        public static void CreateZeroByteFile(string filePath)
        {
            ArgumentValidation.CheckForNullReference(filePath, "filePath");

            using (File.Create(filePath))
            {
            }
        }
    }
}