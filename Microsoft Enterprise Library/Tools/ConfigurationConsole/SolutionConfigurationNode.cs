//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Tools.ConfigurationConsole
{
    /// <devdoc>
    /// Represents the top level node that all other nodes are attached.
    /// </devdoc>
    [Image(typeof(SolutionConfigurationNode))]
    internal class SolutionConfigurationNode : ConfigurationNode
    {
        public SolutionConfigurationNode() : base()
        {
        }

        [ReadOnly(true)]
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }
        
        protected override void OnAddMenuItems()
        {
        }

        
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.DefaultSolutionNodeName;
        }
    }
}
