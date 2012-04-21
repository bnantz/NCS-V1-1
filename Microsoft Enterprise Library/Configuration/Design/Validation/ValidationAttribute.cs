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
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// <para>
    /// Specifies a property or event will be validated.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public abstract class ValidationAttribute : Attribute
    {
        private bool active;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ValidationAttribute"/> class.</para>
        /// </summary>
        protected ValidationAttribute()
        {
            active = true;
        }

        /// <summary>
        /// <para>Gets or sets if the validation is active.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if the validation is active; otherwise <see langword="false"/>.</para>
        /// </value>
        public bool IsActive
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// <para>Validate the ranige data for the given <paramref name="instance"/> and the <paramref name="propertyInfo"/>.</para>
        /// </summary>
        /// <param name="instance">
        /// <para>The instance to validate.</para>
        /// </param>
        /// <param name="propertyInfo">
        /// <para>The property contaning the value to validate.</para>
        /// </param>
        /// <param name="errors">
        /// <para>The collection to add any errors that occur durring the validation.</para>
        /// </param>
        public abstract void Validate(object instance, PropertyInfo propertyInfo, ValidationErrorCollection errors);

    }
}