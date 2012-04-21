//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    /// <summary>
    /// <para>
    /// Represents the package information to the connection string for an Oracle database.
    /// </para>
    /// </summary>    
    [Image(typeof(OraclePackageNode))]
    public class OraclePackageNode : ConfigurationNode
    {
        private OraclePackageData oraclePackageData;

        /// <summary>
        /// <para>
        /// Initialize a new instance of the <see cref="OraclePackageNode"/> class.
        /// </para>
        /// </summary>
        public OraclePackageNode() : this(new OraclePackageData(SR.DefaultOraclePackageNodeName, string.Empty))
        {
        }

        /// <summary>
        /// <para>Initalize a new instance of the <see cref="OraclePackageNode"/> class with the runtime version that this node will represent.</para>
        /// </summary>
        /// <param name="oraclePackage">
        /// <para>The <see cref="OraclePackageData"/> that this node will represent.</para>
        /// </param>
        public OraclePackageNode(OraclePackageData oraclePackage) : base()
        {
            this.oraclePackageData = oraclePackage;
        }

        /// <summary>
        /// <para>
        /// Gets or sets the value of the parameter.
        /// </para>
        /// </summary>
        /// <value>
        /// <para>
        /// The name of the parameter.
        /// </para>
        /// </value>
        [Required]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string Prefix
        {
            get { return this.oraclePackageData.Prefix; }
            set { this.oraclePackageData.Prefix = value; }
        }

        /// <summary>
        /// <para>Gets the <see cref="OraclePackageData"/> runtime configuration.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="OraclePackageData"/> runtime configuration.</para>
        /// </value>
        [Browsable(false)]
        public virtual OraclePackageData OraclePackage
        {
            get { return this.oraclePackageData; }
        }

        /// <summary>
        /// <para>Sets the <seeals cref="ConfigurationNode.Name"/> to the <seealso cref="OraclePackageData.Name"/></para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = oraclePackageData.Name;
        }

        /// <summary>
        /// <para>Sets the <seeals cref="OraclePackageData.Name"/> to the <seealso cref="ConfigurationNode.Name"/></para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            oraclePackageData.Name = e.Node.Name;
        }
    }
}