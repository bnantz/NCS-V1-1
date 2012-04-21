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
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Enables interaction with the user interface of the development environment object that is hosting the designer.</para>
    /// </summary>
    public interface IUIService
    {
        /// <summary>
        /// <para>When implemented by a class, notifies the user interface that an update is occuring.</para>
        /// </summary>
        void BeginUpdate();

        /// <summary>
        /// <para>When implemented by a class, notifies the user interface that an update has ended.</para>
        /// </summary>
        void EndUpdate();

        /// <summary>
        /// <para>When implemented by a class, displays a <see cref="SaveFileDialog"/> in the user interface.</para>
        /// </summary>
        /// <param name="dialog">
        /// <para>The <see cref="SaveFileDialog"/> to display.</para>
        /// </param>
        /// <returns>
        /// <para>One of the <see cref="DialogResult"/> values.</para>
        /// </returns>
        DialogResult ShowSaveDialog(SaveFileDialog dialog);

        /// <summary>
        /// <para>When implemented by a class, displays an <see cref="OpenFileDialog"/> in the user interface.</para>
        /// </summary>
        /// <param name="dialog">
        /// <para>The <see cref="OpenFileDialog"/> to display.</para>
        /// </param>
        /// <returns>
        /// <para>One of the <see cref="DialogResult"/> values.</para>
        /// </returns>
        DialogResult ShowOpenDialog(OpenFileDialog dialog);

        /// <summary>
        /// <para>When implemented by a class, gets the owner window.</para>
        /// </summary>
        /// <value>
        /// <para>The owner window.</para>
        /// </value>
        IWin32Window OwnerWindow { get; }

        /// <summary>
        /// <para>When implemented by a class, avtivates a node.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The <see cref="ConfigurationNode"/> to activate.</para>
        /// </param>
        void ActivateNode(ConfigurationNode node);

        /// <summary>
        /// <para>When implemented by a class, displays the validation errors.</para>
        /// </summary>
        /// <param name="errorLogService">
        /// <para>The errorLogToDisplay.</para>
        /// </param>
        void DisplayErrorLog(IConfigurationErrorLogService errorLogService);

        /// <summary>
        /// <para>When implemented by a class, sets the hierarchy dirty indicating the UI should indicate a model change.</para>
        /// </summary>
        /// <param name="hierarchy">
        /// <para>The <see cref="IUIHierarchy"/> to set dirty in the <see cref="IUIService"/>.</para>
        /// </param>
        void SetUIDirty(IUIHierarchy hierarchy);

        /// <summary>
        /// <para>When implemented by a class, determines if the given <paramref name="hierarchy"/> has been modified.</para>
        /// </summary>
        /// <param name="hierarchy"><para>A <see cref="IUIHierarchy"/> object.</para></param>
        /// <returns><para><see langword="true"/> if the given <paramref name="hierarchy"/> has been modified; otherwise, <see langword="false"/>.</para></returns>
        bool IsDirty(IUIHierarchy hierarchy);

        /// <summary>
        /// <para>When implemented by a class, updates the status text in the user interface.</para>
        /// </summary>
        /// <param name="status">
        /// <para>The status to display.</para>
        /// </param>
        void SetStatus(string status);

        /// <summary>
        /// <para>Clear the errors in the user interface.</para>
        /// </summary>
        void ClearErrorDisplay();

        /// <summary>
        /// <para>When implemented by a class, displays the specified exception and information about the exception.</para>
        /// </summary>
        /// <param name="e">
        /// <para>The <see cref="Exception"/> to display.</para>
        /// </param>
        void ShowError(Exception e);

        /// <summary>
        /// <para>When implemented by a class, displays the specified exception and information about the exception.</para>
        /// </summary>
        /// <param name="e">
        /// <para>The <see cref="Exception"/> to display.</para>
        /// </param>
        /// <param name="message">
        /// <para>A message to display that provides information about the exception</para>
        /// </param>
        void ShowError(Exception e, string message);

        /// <summary>
        /// <para>When implemented by a class, displays the specified exception and information about the exception.</para>
        /// </summary>
        /// <param name="e">
        /// <para>The <see cref="Exception"/> to display.</para>
        /// </param>
        /// <param name="message">
        /// <para>A message to display that provides information about the exception</para>
        /// </param>
        /// <param name="caption">
        /// <para>The caption for the message.</para>
        /// </param>
        void ShowError(Exception e, string message, string caption);

        /// <summary>
        /// <para>When implemented by a class, displays the specified error message.</para>
        /// </summary>
        /// <param name="message">
        /// <para>The error message to display.</para>
        /// </param>
        void ShowError(string message);

        /// <summary>
        /// <para>When implemented by a class, displays the specified error message.</para>
        /// </summary>
        /// <param name="message">
        /// <para>The error message to display.</para>
        /// </param>
        /// <param name="caption">
        /// <para>The caption for the message.</para>
        /// </param>
        void ShowError(string message, string caption);

        /// <summary>
        /// <para>When implemented by a class, displays the specified message.</para>
        /// </summary>
        /// <param name="message">
        /// <para>The message to display.</para>
        /// </param>
        void ShowMessage(string message);

        /// <summary>
        /// <para>When implemented by a class, displays the specified message with the specified caption.</para>
        /// </summary>
        /// <param name="message">
        /// <para>The message to display.</para>
        /// </param>
        /// <param name="caption">
        /// <para>The caption for the message.</para>
        /// </param>
        void ShowMessage(string message, string caption);

        /// <summary>
        /// <para>When implemented by a class, displays the specified message in a message box with the specified caption and buttons to place on the dialog box.</para>
        /// </summary>
        /// <param name="message">
        /// <para>The message to display.</para>
        /// </param>
        /// <param name="caption">
        /// <para>The caption for the message.</para>
        /// </param>
        /// <param name="buttons">
        /// <para>One of the <see cref="MessageBoxButtons"/> values.</para>
        /// </param>
        /// <returns>
        /// <para>One of the <see cref="DialogResult"/> values.</para>
        /// </returns>
        DialogResult ShowMessage(string message, string caption, MessageBoxButtons buttons);
    }
}