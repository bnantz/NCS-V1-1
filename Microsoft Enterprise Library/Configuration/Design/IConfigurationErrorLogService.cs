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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Provides a service to collect errors while processing commands for nodes.</para>
    /// </summary>
    public interface IConfigurationErrorLogService
    {
        /// <summary>
        /// <para>When implemented by a class, gets the <see cref="ReadOnlyConfigurationErrorCollection"/> for containing all <see cref="ConfigurationError"/> objects logged.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="ReadOnlyConfigurationErrorCollection"/> for containing all <see cref="ConfigurationError"/> objects logged.</para>
        /// </value>
        ReadOnlyConfigurationErrorCollection ConfigurationErrors { get; }

        /// <summary>
        /// <para>When implemented by a class, gets the <see cref="ReadOnlyValidationErrorCollection"/> for containing all <see cref="ValidationError"/> objects logged.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="ReadOnlyValidationErrorCollection"/> for containing all <see cref="ValidationError"/> objects logged.</para>
        /// </value>
        ReadOnlyValidationErrorCollection ValidationErrors { get; }

        /// <summary>
        /// <para>When implemented by a class, clears all <see cref="ValidationErrors"/> and <see cref="ConfigurationErrors"/> from the service.</para>
        /// </summary>
        void ClearErrorLog();

        /// <summary>
        /// <para>When implemented by a class, logs a <see cref="ValidationError"/>.</para>
        /// </summary>
        /// <param name="validationError">
        /// <para>A <see cref="ValidationError"/> object.</para>
        /// </param>
        void LogError(ValidationError validationError);

        /// <summary>
        /// <para>When implemented by a class, logs a <see cref="ConfigurationError"/>.</para>
        /// </summary>
        /// <param name="configurationError">
        /// <para>A <see cref="ConfigurationError"/> object.</para>
        /// </param>
        void LogError(ConfigurationError configurationError);
    }
}