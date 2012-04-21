//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Formatters
{
    /// <summary>
    /// Contains data for a Template.
    /// </summary>
    public struct Template
    {
        private string text;

        /// <summary>
        /// Creates a template with initial text for the template.
        /// </summary>
        /// <param name="text">The text of the template.</param>
        public Template(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// The text of the template.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Returns a placeholder for use in a single line environment.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return SR.TemplatePlaceHolder;
        }

    }
}