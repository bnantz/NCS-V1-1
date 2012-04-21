//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Security.Authorization;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// Represents a validation rule that ensures
    /// that an expression does not have any syntax errors.
    /// 
    /// This class cannot be inherited from
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited=true)]
    public sealed class ValidExpressionAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="ValidExpressionAttribute"/> class.
        /// </summary>
        public ValidExpressionAttribute()
        {
        }

        /// <summary>
        /// Validates that the value of the specified
        /// property does not contain any syntax errors.
        /// </summary>
        /// <param name="instance">
        /// <para>The instance to validate.</para>
        /// </param>
        /// <param name="info">
        /// <para>The property contaning the value to validate.</para>
        /// </param>
        /// <param name="errors">
        /// <para>The collection to add any errors that occur durring the validation.</para>
        /// </param>
        public override void Validate(
            object instance,
            PropertyInfo info,
            ValidationErrorCollection errors)
        {
            string expression = (string)info.GetValue(instance, null);
            if (expression != null
                && expression.Length != 0)
            {
                try
                {
                    Parser parser = new Parser();
                    parser.Parse(expression);
                }
                catch (SyntaxException e)
                {
                    errors.Add(instance, info.Name, e.Message);
                }
            }
        }
    }
}