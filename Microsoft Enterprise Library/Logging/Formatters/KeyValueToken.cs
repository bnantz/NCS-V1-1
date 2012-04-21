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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
    /// <summary>
    /// Formats a keyvalue token and displays the dictionary entries value.
    /// </summary>
    public class KeyValueToken : TokenFunction
    {
        /// <summary>
        /// Initialize a new instance of a <see cref="TimeStampToken"/>.
        /// </summary>
        public KeyValueToken() : base("{keyvalue(")
        {
        }

        /// <summary>
        /// Format the timestamp property with the specified date time format string.
        /// </summary>
        /// <param name="tokenTemplate">Dictionary key name.</param>
        /// <param name="log">Log entry containing with extended properties dictionary values.</param>
        /// <returns>Value of the key from the extended properties dictionary.</returns>
        public override string FormatToken(string tokenTemplate, LogEntry log)
        {
            if (log.ExtendedProperties == null)
            {
                return "";
            }

            string propertyString = "";
            object propertyObject = log.ExtendedProperties[tokenTemplate];
            if (propertyObject != null)
            {
                propertyString = propertyObject.ToString();
            }

            return propertyString;
        }
    }
}