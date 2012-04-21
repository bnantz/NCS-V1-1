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

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents the method that handles a  <see cref="ConfigurationNodeCommand.Executing"/> event.</para>
    /// </summary>
    /// <param name="sender">
    /// <para>The source of the event.</para>
    /// </param>
    /// <param name="e">
    /// <para>A <see cref="CommandExecutingEventArgs"/> that contains the event data.</para>
    /// </param>
    public delegate void CommandExecutingEventHandler(object sender, CommandExecutingEventArgs e);

    /// <summary>
    /// <para>Provides data for a <see cref="ConfigurationNodeCommand.Executing"/> event.</para>
    /// </summary>
    [Serializable]
    public class CommandExecutingEventArgs : EventArgs
    {
        private bool cancel;

        /// <summary>
        /// <para>Initializes a new instance of  the <see cref="CommandExecutingEventArgs"/> class.</para>
        /// </summary>
        public CommandExecutingEventArgs()
        {
        }

        /// <summary>
        /// <para>
        /// Initializes a new instance of  the <see cref="CommandExecutingEventArgs"/> class with the <seealso cref="Cancel"/> property set to the given value.
        /// </para>
        /// </summary>
        /// <param name="cancel">
        /// <para><see langword="true"/> to cancel the event; otherwise, <see langword="false"/>.</para>
        /// </param>
        public CommandExecutingEventArgs(bool cancel)
        {
            this.cancel = cancel;
        }

        /// <summary>
        /// <para>Gets or sets a value indicating whether the event should be canceled.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if the event should be canceled; otherwise, <see langword="false"/>.</para>
        /// </value>
        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }
    }
}