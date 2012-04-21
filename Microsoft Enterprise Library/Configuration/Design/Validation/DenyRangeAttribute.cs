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
    /// Specifies a property or event will be validated on a specific range and make sure that the value is outside that range.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=true)]
    public sealed class DenyRangeAttribute : AssertRangeAttribute
    {
        /// <summary>
        /// <para>Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Int32"/> lower and upper bounds.</para>
        /// </summary>
        /// <param name="lowerBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="lowerBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>
        /// <param name="upperBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="upperBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>
        public DenyRangeAttribute(int lowerBound, RangeBoundaryType lowerBoundType, int upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// <para>Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Single"/> lower and upper bounds.</para>
        /// </summary>
        /// <param name="lowerBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="lowerBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>
        /// <param name="upperBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="upperBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>
        public DenyRangeAttribute(float lowerBound, RangeBoundaryType lowerBoundType, float upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// <para>Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="String"/> lower and upper bounds.</para>
        /// </summary>
        /// <param name="lowerBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="lowerBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>
        /// <param name="upperBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="upperBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>       
        public DenyRangeAttribute(string lowerBound, RangeBoundaryType lowerBoundType, string upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// <para>Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Double"/> lower and upper bounds.</para>
        /// </summary>
        /// <param name="lowerBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="lowerBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>
        /// <param name="upperBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="upperBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>
        public DenyRangeAttribute(double lowerBound, RangeBoundaryType lowerBoundType, double upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// <para>Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Int16"/> lower and upper bounds.</para>
        /// </summary>
        /// <param name="lowerBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="lowerBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>
        /// <param name="upperBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="upperBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>
        public DenyRangeAttribute(short lowerBound, RangeBoundaryType lowerBoundType, short upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// <para>Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Byte"/> lower and upper bounds.</para>
        /// </summary>
        /// <param name="lowerBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="lowerBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>
        /// <param name="upperBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="upperBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>
        public DenyRangeAttribute(byte lowerBound, RangeBoundaryType lowerBoundType, byte upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// <para>Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Char"/> lower and upper bounds.</para>
        /// </summary>
        /// <param name="lowerBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="lowerBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>
        /// <param name="upperBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="upperBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>
        public DenyRangeAttribute(char lowerBound, RangeBoundaryType lowerBoundType, char upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// <para>Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="Decimal"/> lower and upper bounds.</para>
        /// </summary>
        /// <param name="lowerBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="lowerBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>
        /// <param name="upperBound">
        /// <para>The lower bound of the range.</para>
        /// </param>
        /// <param name="upperBoundType">
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </param>
        public DenyRangeAttribute(decimal lowerBound, RangeBoundaryType lowerBoundType, decimal upperBound, RangeBoundaryType upperBoundType) : base((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
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
            base.Validate(instance, propertyInfo, errors);
            if (errors.Count == 0)
            {
                string name = propertyInfo.Name;
                errors.Add(instance, name, SR.ValueOutsideRangeErrorMessage(name));
            }
        }
    }
}