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
    /// Specifies a property or event will be validated on a specific range.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple=true, Inherited=true)]
    public class AssertRangeAttribute : ValidationAttribute
    {
        private readonly IComparable lowerBound;
        private readonly IComparable upperBound;
        private readonly RangeBoundaryType lowerBoundType;
        private readonly RangeBoundaryType upperBoundType;

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
        public AssertRangeAttribute(int lowerBound, RangeBoundaryType lowerBoundType, int upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
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
        public AssertRangeAttribute(float lowerBound, RangeBoundaryType lowerBoundType, float upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
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
        public AssertRangeAttribute(string lowerBound, RangeBoundaryType lowerBoundType, string upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
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
        public AssertRangeAttribute(double lowerBound, RangeBoundaryType lowerBoundType, double upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
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
        public AssertRangeAttribute(short lowerBound, RangeBoundaryType lowerBoundType, short upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
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
        public AssertRangeAttribute(byte lowerBound, RangeBoundaryType lowerBoundType, byte upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
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
        public AssertRangeAttribute(char lowerBound, RangeBoundaryType lowerBoundType, char upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
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
        public AssertRangeAttribute(decimal lowerBound, RangeBoundaryType lowerBoundType, decimal upperBound, RangeBoundaryType upperBoundType) : this((IComparable)lowerBound, lowerBoundType, (IComparable)upperBound, upperBoundType)
        {
        }

        /// <summary>
        /// <para>Initialzie a new instance of the <see cref="AssertRangeAttribute"/> class with an <see cref="IComparable"/> lower and upper bounds.</para>
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
        protected AssertRangeAttribute(IComparable lowerBound, RangeBoundaryType lowerBoundType, IComparable upperBound, RangeBoundaryType upperBoundType)
        {
            if (lowerBound.CompareTo(upperBound) > 0)
            {
                throw new ArgumentOutOfRangeException("lowerBound", SR.ExceptionLowerBoundOutOfRange(lowerBound.ToString(), upperBound.ToString()));
            }
            this.lowerBound = lowerBound;
            this.lowerBoundType = lowerBoundType;
            this.upperBound = upperBound;
            this.upperBoundType = upperBoundType;
        }

        /// <summary>
        /// <para>Gets the lower bound of the range.</para>
        /// </summary>
        /// <value>
        /// <para>The lower bound of the range.</para>
        /// </value>
        public IComparable LowerBound
        {
            get { return lowerBound; }
        }

        /// <summary>
        /// <para>Gets the upper bound of the range.</para>
        /// </summary>
        /// <value>
        /// <para>The upper bound of the range.</para>
        /// </value>
        public IComparable UpperBound
        {
            get { return upperBound; }
        }

        /// <summary>
        /// <para>Gets the lower bound type condition.</para>
        /// </summary>
        /// <value>
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </value>
        public RangeBoundaryType LowerBoundType
        {
            get { return lowerBoundType; }
        }

        /// <summary>
        /// <para>Gets the upper bound type condition.</para>
        /// </summary>
        /// <value>
        /// <para>One of the <see cref="RangeBoundaryType"/> values.</para>
        /// </value>
        public RangeBoundaryType UpperBoundType
        {
            get { return upperBoundType; }
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
            if (propertyValue == null)
            {
                return;
            }
            IComparable compareToObject = (IComparable)propertyValue;
            int lowerBoundGreaterThanValue = this.lowerBound.CompareTo(compareToObject);
            int upperBoundLessThanValue = this.upperBound.CompareTo(compareToObject);
            if ((lowerBoundGreaterThanValue == 0) && (this.lowerBoundType == RangeBoundaryType.Exclusive))
            {
                errors.Add(instance, propertyInfo.Name, SR.ValueNotInRangeErrorMessage(propertyInfo.Name));
            }
            if (lowerBoundGreaterThanValue > 0)
            {
                errors.Add(instance, propertyInfo.Name, SR.ValueNotInRangeErrorMessage(propertyInfo.Name));
            }

            if ((upperBoundLessThanValue == 0) && (this.upperBoundType == RangeBoundaryType.Exclusive))
            {
                errors.Add(instance, propertyInfo.Name, SR.ValueNotInRangeErrorMessage(propertyInfo.Name));
            }
            if (upperBoundLessThanValue < 0)
            {
                errors.Add(instance, propertyInfo.Name, SR.ValueNotInRangeErrorMessage(propertyInfo.Name));
            }
        }
    }
}