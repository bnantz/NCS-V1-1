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
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// <para>
    /// Specifies a property or event will be validated on a specific minimum length.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited=true)]
    public sealed class MinimumLengthAttribute : ValidationAttribute
    {
        private int minimumLength;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="MinimumLengthAttribute"/> class with a minimum length.</para>
        /// </summary>
        /// <param name="minimumLength">
        /// <para>The minimum length.</para>
        /// </param>
        public MinimumLengthAttribute(int minimumLength)
        {
            this.minimumLength = Math.Abs(minimumLength);
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
        public override void Validate(object instance, PropertyInfo propertyInfo, ValidationErrorCollection errors)
        {
            ArgumentValidation.CheckForNullReference(instance, "instance");
            ArgumentValidation.CheckForNullReference(propertyInfo, "propertyInfo");
            ArgumentValidation.CheckForNullReference(errors, "errors");

            object propertyValue = propertyInfo.GetValue(instance, null);
            string name = null;
            if (propertyValue == null)
            {
                name = propertyInfo.Name;
                errors.Add(instance, propertyInfo.Name, SR.MinLengthExceededErrorMessage(name, minimumLength));
            }
            if (propertyValue is String)
            {
                string valueString = (string)propertyValue;
                if ((valueString == null) || (valueString.Length == 0) || (valueString.Length < minimumLength))
                {
                    name = propertyInfo.Name;
                    errors.Add(instance, name, SR.MinLengthExceededErrorMessage(name, minimumLength));
                }
            }
        }
    }
}