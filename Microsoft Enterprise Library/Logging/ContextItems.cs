//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Provides methods to maintain a key/value dictionary that is stored in the <see cref="CallContext"/>.
    /// </summary>
    /// <remarks>
    /// A context item represents a key/value that needs to be logged with each message
    /// on the same CallContext.
    /// </remarks>
    internal class ContextItems
    {
        public const string CallContextSlotName = "EntLibLoggerContextItems";

        /// <summary>
        /// Create a new instance of a <see cref="ContextItems"/> class.
        /// </summary>
        public ContextItems()
        {
        }

        /// <summary>
        /// Add a key/value pair to a dictionary in the <see cref="CallContext"/>.  
        /// Each context item will be recorded with every log entry.
        /// </summary>
        /// <param name="key">Hashtable key.</param>
        /// <param name="value">Value of the context item.  Objects and byte arrays will be base64 encoded.</param>
        /// <example>The following example demonstrates use of the AddContextItem method.
        /// <code>Logger.SetContextItem("SessionID", myComponent.SessionId);</code></example>
        public void SetContextItem(object key, object value)
        {
            Hashtable contextItems = (Hashtable)CallContext.GetData(CallContextSlotName);
            if (contextItems == null)
            {
                contextItems = new Hashtable();
            }

            contextItems[key] = value;

            CallContext.SetData(CallContextSlotName, contextItems);
        }

        /// <summary>
        /// Empty the context items dictionary.
        /// </summary>
        public void FlushContextItems()
        {
            CallContext.FreeNamedDataSlot(CallContextSlotName);
        }

        /// <summary>
        /// Merges each key/value pair from the context items dictionary with the ExtendedProperties
        /// dictionary of the <see cref="LogEntry"/>.
        /// </summary>
        /// <param name="log"><see cref="LogEntry"/> object that is being logged.</param>
        public void ProcessContextItems(LogEntry log)
        {
            Hashtable contextItems = (Hashtable)CallContext.GetData(CallContextSlotName);
            if (contextItems == null || contextItems.Count == 0)
            {
                return;
            }

            if (log.ExtendedProperties == null)
            {
                log.ExtendedProperties = new Hashtable();
            }

            foreach (DictionaryEntry entry in contextItems)
            {
                string itemValue = GetContextItemValue(entry.Value);
                log.ExtendedProperties.Add(entry.Key.ToString(), itemValue);
            }
        }

        private string GetContextItemValue(object contextData)
        {
            string value = string.Empty;
            try
            {
                // convert to base 64 string if data type is byte array
                if (contextData.GetType() == typeof(byte[]))
                {
                    value = Convert.ToBase64String((byte[])contextData);
                }
                else
                {
                    value = contextData.ToString();
                }
            }
            catch
            { /* ignore exceptions */
            }

            return value;
        }
    }
}