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
using System.Security.Permissions;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// <para>Specifies a property or event that is validated based on a regular expression.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple=true, Inherited=true)]
    public sealed class RegexAttribute : ValidationAttribute
    {
        private readonly Type compiledRegexType;
        private readonly string pattern;
        private readonly RegexOptions options;
        private readonly bool optionsSpecified;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="RegexAttribute"/> class with the regular expression pattern and options.</para>
        /// </summary>
        /// <param name="pattern">
        /// <para>The regular expression pattern to match.</para>
        /// </param>     
        /// <param name="options">
        /// The regular expression options.
        /// </param>
        public RegexAttribute(string pattern, RegexOptions options) : this(pattern, options, true, null)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="RegexAttribute"/> class with the regular expression pattern.</para>
        /// </summary>
        /// <param name="pattern">
        /// <para>The regular expression pattern to match.</para>
        /// </param>
        public RegexAttribute(string pattern) : this(pattern, RegexOptions.None, false, null)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="RegexAttribute"/> class with the regular expression type.</para>
        /// </summary>
        /// <param name="compiledRegexType">
        /// <para>The compiled <see cref="Type"/> for the regular expression.</para>
        /// </param>
        public RegexAttribute(Type compiledRegexType) : this(null, RegexOptions.None, false, compiledRegexType)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="RegexAttribute"/> class with the regular expression type and options.</para>
        /// </summary>
        /// <param name="compiledRegexType">
        /// <para>The compiled <see cref="Type"/> for the regular expression.</para>
        /// </param>
        /// <param name="options">
        /// The regular expression options.
        /// </param>
        public RegexAttribute(Type compiledRegexType, RegexOptions options) : this(null, options, true, compiledRegexType)
        {
        }

        private RegexAttribute(string pattern, RegexOptions options, bool optionsSpecified, Type compiledRegexType)
        {
            this.pattern = pattern;
            this.options = options;
            this.optionsSpecified = optionsSpecified;
            this.compiledRegexType = compiledRegexType;
        }

        /// <summary>
        /// <para>Gets the <see cref="RegexOptions"/> for the regular expression.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="RegexOptions"/> for the regular expression.</para>
        /// </value>
        public RegexOptions Options
        {
            get { return options; }
        }

        /// <summary>
        /// <para>Gets the pattern for the regular expression.</para>
        /// </summary>
        /// <value>
        /// <para>The pattern for the regular expression</para>
        /// </value>
        public string Pattern
        {
            get { return pattern; }
        }

        /// <summary>
        /// <para>Gets the compiled <see cref="Type"/> for the regular expression.</para>
        /// </summary>
        /// <value>
        /// <para>The compiled <see cref="Type"/> for the regular expression.</para>
        /// </value>
        public Type CompiledRegexType
        {
            get { return compiledRegexType; }
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
        [ReflectionPermission(SecurityAction.Demand,  Flags=ReflectionPermissionFlag.MemberAccess)]
        public override void Validate(object instance, PropertyInfo propertyInfo, ValidationErrorCollection errors)
        {
            ArgumentValidation.CheckForNullReference(instance, "instance");
            ArgumentValidation.CheckForNullReference(propertyInfo, "propertyInfo");
            ArgumentValidation.CheckForNullReference(errors, "errors");

            object propertyValue = propertyInfo.GetValue(instance, null);
            Regex expression = null;
            if (this.compiledRegexType != null)
            {
                if (this.optionsSpecified)
                {
                    expression = (Regex)Activator.CreateInstance(this.compiledRegexType, new object[] {this.options});
                }
                else
                {
                    expression = (Regex)Activator.CreateInstance(this.compiledRegexType);
                }
            }
            else if (this.optionsSpecified)
            {
                expression = new Regex(this.pattern, this.options);
            }
            else
            {
                expression = new Regex(this.pattern);
            }

            if (!expression.IsMatch((string)propertyValue))
            {
                errors.Add(instance, propertyInfo.Name, SR.RegExErrorMessage(propertyValue.ToString()));
            }
        }
    }
}
