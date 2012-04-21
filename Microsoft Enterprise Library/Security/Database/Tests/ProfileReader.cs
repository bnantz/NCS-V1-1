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

#if UNIT_TESTS
using System;
using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Tests
{
    public class ProfileReader
    {
        private object profile;
        private IDictionary profileDictionary;

        public ProfileReader(object profile)
        {
            this.profile = profile;
            this.profileDictionary = profile as IDictionary;
        }

        public object GetObject(string keyName)
        {
            return GetKeyValue(keyName);
        }

        public bool GetBool(string keyName)
        {
            object keyValue = GetKeyValue(keyName);
            try
            {
                return Convert.ToBoolean(keyValue);
            }
            catch (Exception e)
            {
                throw new InvalidCastException(SR.ConversionException(keyValue.ToString(), "bool"), e);
            }
        }

        public string GetString(string keyName)
        {
            object keyValue = GetKeyValue(keyName);
            try
            {
                return Convert.ToString(keyValue);
            }
            catch (Exception e)
            {
                throw new InvalidCastException(SR.ConversionException(keyValue.ToString(), "string"), e);
            }
        }

        public DateTime GetDateTime(string keyName)
        {
            object keyValue = GetKeyValue(keyName);
            try
            {
                return Convert.ToDateTime(keyValue);
            }
            catch (Exception e)
            {
                throw new InvalidCastException(SR.ConversionException(keyValue.ToString(), "DateTime"), e);
            }
        }

        public char GetChar(string keyName)
        {
            object keyValue = GetKeyValue(keyName);
            try
            {
                return Convert.ToChar(keyValue);
            }
            catch (Exception e)
            {
                throw new InvalidCastException(SR.ConversionException(keyValue.ToString(), "char"), e);
            }
        }

        public byte GetByte(string keyName)
        {
            object keyValue = GetKeyValue(keyName);
            try
            {
                return Convert.ToByte(keyValue);
            }
            catch (Exception e)
            {
                throw new InvalidCastException(SR.ConversionException(keyValue.ToString(), "byte"), e);
            }
        }

        public short GetShortInt(string keyName)
        {
            object keyValue = GetKeyValue(keyName);
            try
            {
                return Convert.ToInt16(keyValue);
            }
            catch (Exception e)
            {
                throw new InvalidCastException(SR.ConversionException(keyValue.ToString(), "short"), e);
            }
        }

        public int GetInt(string keyName)
        {
            object keyValue = GetKeyValue(keyName);
            try
            {
                return Convert.ToInt32(keyValue);
            }
            catch (Exception e)
            {
                throw new InvalidCastException(SR.ConversionException(keyValue.ToString(), "int"), e);
            }
        }

        public long GetLongInt(string keyName)
        {
            object keyValue = GetKeyValue(keyName);
            try
            {
                return Convert.ToInt64(keyValue);
            }
            catch (Exception e)
            {
                throw new InvalidCastException(SR.ConversionException(keyValue.ToString(), "long"), e);
            }
        }

        public float GetFloat(string keyName)
        {
            object keyValue = GetKeyValue(keyName);
            try
            {
                return Convert.ToSingle(keyValue);
            }
            catch (Exception e)
            {
                throw new InvalidCastException(SR.ConversionException(keyValue.ToString(), "float"), e);
            }
        }

        public decimal GetDecimal(string keyName)
        {
            object keyValue = GetKeyValue(keyName);
            try
            {
                return Convert.ToDecimal(keyValue);
            }
            catch (Exception e)
            {
                throw new InvalidCastException(SR.ConversionException(keyValue.ToString(), "decimal"), e);
            }
        }

        public double GetDouble(string keyName)
        {
            object keyValue = GetKeyValue(keyName);
            try
            {
                return Convert.ToDouble(keyValue);
            }
            catch (Exception e)
            {
                throw new InvalidCastException(SR.ConversionException(keyValue.ToString(), "double"), e);
            }
        }

        private object GetKeyValue(string keyName)
        {
            object keyValue = this.profile;
            if (this.profileDictionary != null)
            {
                keyValue = this.profileDictionary[keyName];
            }

            return keyValue;
        }
    }
}
#endif