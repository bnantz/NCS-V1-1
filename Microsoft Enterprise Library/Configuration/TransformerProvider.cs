//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents an object that transforms configuration data from storage.</para>
    /// </summary>
	public abstract class TransformerProvider : ConfigurationProvider, ITransformer
	{
	    private string currentSectionName;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="TransformerProvider"/> class.</para>
        /// </summary>
	    protected TransformerProvider()
		{
            currentSectionName = string.Empty;
		}

        /// <summary>
        /// <para>Gets the name of the configuration section.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the configuration section.</para>
        /// </value>
	    public string CurrentSectionName
	    {
	        get { return currentSectionName; }
	        set { currentSectionName = value; }
	    }

        /// <summary>
        /// <para>When overriden by a class, deserializes the configuration section data.</para>
        /// </summary>
        /// <param name="section">
        /// <para>The configuration section data.</para>
        /// </param>
        /// <returns>
        /// <para>The deserialized object.</para>
        /// </returns>
	    public abstract object Deserialize(object section);

        /// <summary>
        /// <para>When overriden by a class, serialzies the configuration data so it can be saved to storage.</para>
        /// </summary>
        /// <param name="value">
        /// <para>The configuration data to serialzie.</para>
        /// </param>
        /// <returns>
        /// <para>The serailzied object.</para>
        /// </returns>
	    public abstract object Serialize(object value);
	}
}
