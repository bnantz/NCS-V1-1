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

using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents a single category filter configuration settings.
    /// </summary>
    [XmlRoot("categoryFilter", Namespace=LoggingSettings.ConfigurationNamespace)]
    public class CategoryFilterData
    {
		private string name;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CategoryFilterData"/> class.</para>
        /// </summary>
		public CategoryFilterData()
    	{
    	}

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CategoryFilterData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="CategoryFilterData"/>.</para>
        /// </param>
    	public CategoryFilterData(string name)
    	{
    		this.name = name;
    	}

        /// <summary>
        /// Name of the category.
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
    }
}