//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
{
    /// <summary>
    ///	This class tracks a file cache dependency.
    /// </summary>
    [Serializable]
    [ComVisible(false)]
    public class FileDependency : ICacheItemExpiration, ISerializable
    {
        private readonly string dependencyFileName;

        private DateTime lastModifiedTime;

        /// <summary>
        ///	Constructor with one argument.
        /// </summary>
        /// <param name="fullFileName">
        ///	Indicates the name of the file
        /// </param>
        public FileDependency(string fullFileName)
        {
            if (Object.Equals(fullFileName, null))
            {
                throw new ArgumentNullException("fullFileName", SR.ExceptionNullFileName);
            }
            if (fullFileName.Length == 0)
            {
                throw new ArgumentOutOfRangeException("fullFileName", SR.ExceptionEmptyFileName);
            }

            dependencyFileName = Path.GetFullPath(fullFileName);
            EnsureTargetFileAccessible();

            if (!File.Exists(dependencyFileName))
            {
                throw new ArgumentException(SR.ExceptionInvalidFileName, "fullFileName");
            }

            this.lastModifiedTime = File.GetLastWriteTime(fullFileName);
        }

        /// <summary>
        ///	This method performs the deserialization of members of the 
        ///	current class.
        /// </summary>
        /// <param name="info">
        ///	A SerializationInfo object which is deserialized by the 
        ///	formatter and then passed to current constructor
        /// </param>
        /// <param name="context">
        ///	A StreamingContext that describes the source of the 
        ///	serialized stream from where the Serialization object 
        ///	is retrieved
        /// </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected FileDependency(SerializationInfo info, StreamingContext context)
        {
            // Getting the value of file name and
            // last accessed time
            this.dependencyFileName = info.GetString("fileName");
            this.lastModifiedTime = Convert.ToDateTime(info.GetValue("lastModifiedTime", typeof(DateTime)), DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Returns time watched file was last modified.
        /// </summary>
        public DateTime LastModifiedTime
        {
            get { return lastModifiedTime; }
        }

        /// <summary>
        ///	Specifies if the item has expired or not.
        /// </summary>
        /// <returns>Returns true if the item has expired, otherwise false.</returns>
        public bool HasExpired()
        {
            EnsureTargetFileAccessible();

            if (File.Exists(this.dependencyFileName) == false)
            {
                return true;
            }

            DateTime currentModifiedTime = File.GetLastWriteTime(dependencyFileName);
            if (DateTime.Compare(lastModifiedTime, currentModifiedTime) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///	Notifies that the item was recently used.
        /// </summary>
        public void Notify()
        {
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="owningCacheItem">Not used</param>
        public void Initialize(CacheItem owningCacheItem)
        {
        }

        /// <summary>
        ///	This method performs the serialization of members of the 
        ///	current class.
        /// </summary>
        /// <param name="info">
        ///	A SerializationInfo object which is deserialized by the 
        ///	formatter and then passed to current constructor
        /// </param>
        /// <param name="context">
        ///	A StreamingContext that describes the source of the 
        ///	serialized stream from where the Serialization object 
        ///	is retrieved
        /// </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Adds the file name and last accessed time 
            // into the SerializationInfo, 
            // where it is associated with the name key
            info.AddValue("fileName", this.dependencyFileName);
            info.AddValue("lastModifiedTime", this.lastModifiedTime);
        }

        private void EnsureTargetFileAccessible()
        {
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Read, dependencyFileName);
            permission.Demand();
        }
    }
}