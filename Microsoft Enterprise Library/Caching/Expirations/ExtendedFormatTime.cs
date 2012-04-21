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
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
{
    /// <summary>
    ///	This provider tests if a item was expired using a extended format.
    /// </summary>
    [Serializable]
    [ComVisible(false)]
    public class ExtendedFormatTime : ICacheItemExpiration, ISerializable
    {
        private string extendedFormat;
        private DateTime lastUsedTime;

        /// <summary>
        ///	Creates an instance of the class.
        /// </summary>
        public ExtendedFormatTime()
        {
        }

        /// <summary>
        ///	Convert the input format to the extented time format.
        /// </summary>
        /// <param name="timeFormat">
        ///	This contains the expiration information
        /// </param>
        public ExtendedFormatTime(string timeFormat)
        {
            // check arguments
            if (Object.Equals(timeFormat, null))
            {
                throw new ArgumentNullException("timeFormat",
                                                SR.ExceptionNullTimeFormat);
            }
            if (timeFormat.Length == 0)
            {
                throw new ArgumentOutOfRangeException("timeFormat",
                                                      SR.ExceptionRangeTimeFormat);
            }

            ExtendedFormat.Validate(timeFormat);

            // Get the modified extended format
            this.extendedFormat = timeFormat;

            // Convert to UTC in order to compensate for time zones		
            this.lastUsedTime = DateTime.Now.ToUniversalTime();
        }

        /// <summary>
        ///	This method performs the deserialziaton of members of the 
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
        protected ExtendedFormatTime(SerializationInfo info, StreamingContext context)
        {
            this.extendedFormat = info.GetString("extendedFormat");
            this.lastUsedTime = Convert.ToDateTime(info.GetValue("lastUsedTime", typeof(DateTime)),
                                                   DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///	Specifies if item has expired or not.
        /// </summary>
        /// <returns>
        ///	Returns true if the data is expired otherwise false
        /// </returns>
        public bool HasExpired()
        {
            // Convert to UTC in order to compensate for time zones		
            DateTime nowDateTime = DateTime.Now.ToUniversalTime();

            ExtendedFormat format = new ExtendedFormat(extendedFormat);
            // Check expiration
            if (format.IsExpired(lastUsedTime, nowDateTime))
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
        ///	formatter and then passed to current constructor.
        /// </param>
        /// <param name="context">
        ///	A StreamingContext that describes the source of the 
        ///	serialized stream from where the Serialization object 
        ///	is retrieved.
        /// </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("extendedFormat", this.extendedFormat);
            info.AddValue("lastUsedTime", this.lastUsedTime);
        }
    }
}