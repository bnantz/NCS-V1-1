//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Represents an exception formatter that formats exception
    /// objects as text.
    /// </summary>	
    public class TextExceptionFormatter : ExceptionFormatter
    {
        private TextWriter writer;
        private int innerDepth;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="TextExceptionFormatter"/> using the specified
        /// <see cref="TextWriter"/> and <see cref="Exception"/>
        /// objects.
        /// </summary>
        /// <param name="writer">The stream to write formatting information to.</param>
        /// <param name="exception">The exception to format.</param>
        public TextExceptionFormatter(TextWriter writer, Exception exception) : base(exception)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Gets the underlying <see cref="TextWriter"/>
        /// that the current formatter is writing to.
        /// </summary>
        public TextWriter Writer
        {
            get { return this.writer; }
        }

        /// <summary>
        /// Gets a value that indicates the depth 
        /// of the exception currently being written.
        /// </summary>
        /// <remarks>The initial value is 0 and is 
        /// incremented with each inner exception.</remarks>
        protected virtual int InnerDepth
        {
            get { return this.innerDepth; }
            set { this.innerDepth = value; }
        }

        /// <summary>
        /// Writes a generic description to the underlying
        /// text stream.
        /// </summary>
        protected override void WriteDescription()
        {
            // An exception of type {0} occurred and was caught.
            // -------------------------------------------------
            string line = SR.ExceptionWasCaught(base.Exception.GetType().FullName);
            this.Writer.WriteLine(line);

            string separator = new string('-', line.Length);

            this.Writer.WriteLine(separator);
        }

        /// <summary>
        /// Controls the formatting of the specified exception and 
        /// its inner exception if there is one.
        /// </summary>
        /// <param name="e">See <see cref="ExceptionFormatter.WriteException"/></param>
        /// <param name="outerException">See <see cref="ExceptionFormatter.WriteException"/></param>
        /// <remarks>For more information, see the documentation
        /// for the base implementation - 
        /// <see cref="ExceptionFormatter.WriteException"/></remarks>
        protected override void WriteException(Exception e, Exception outerException)
        {
            if (outerException != null)
            {
                this.innerDepth++;
                this.Indent();
                string temp = SR.InnerException;
                string separator = new string('-', temp.Length);
                this.Writer.WriteLine(temp);
                this.Indent();
                this.Writer.WriteLine(separator);

                base.WriteException(e, outerException);
                this.innerDepth--;
            }
            else
            {
                base.WriteException(e, outerException);
            }
        }

        /// <summary>
        /// Writes the current date and time to the
        /// underlying text stream.
        /// </summary>
        /// <param name="utcNow">See <see cref="ExceptionFormatter.WriteDateTime"/></param>
        protected override void WriteDateTime(DateTime utcNow)
        {
            DateTime localTime = utcNow.ToLocalTime();
            string localTimeString = localTime.ToString("G", DateTimeFormatInfo.InvariantInfo);

            this.Writer.WriteLine(localTimeString);
        }

        /// <summary>
        /// Writes the value of the <see cref="Type.AssemblyQualifiedName"/>
        /// property for the specified exception type to the underlying
        /// text stream.
        /// </summary>
        /// <param name="exceptionType">See <see cref="ExceptionFormatter.WriteExceptionType"/></param>
        protected override void WriteExceptionType(Type exceptionType)
        {
            IndentAndWriteLine(SR.TypeString, exceptionType.AssemblyQualifiedName);
        }

        /// <summary>
        /// Writes the value of the <see cref="Exception.Message"/>
        /// property to the underyling text stream.
        /// </summary>
        /// <param name="message">See <see cref="ExceptionFormatter.WriteMessage"/></param>
        protected override void WriteMessage(string message)
        {
            IndentAndWriteLine(SR.Message, message);
        }

        /// <summary>
        /// Writes the value of the specified source taken
        /// from the value of the <see cref="Exception.Source"/>
        /// property.
        /// </summary>
        /// <param name="source">See <see cref="ExceptionFormatter.WriteSource"/></param>
        protected override void WriteSource(string source)
        {
            IndentAndWriteLine(SR.Source, source);
        }

        /// <summary>
        /// Writes the value of the specified help link taken
        /// from the value of the <see cref="Exception.HelpLink"/>
        /// property.
        /// </summary>
        /// <param name="helpLink">The exception's help link.</param>
        protected override void WriteHelpLink(string helpLink)
        {
            IndentAndWriteLine(SR.HelpLink, helpLink);
        }

        /// <summary>
        /// Writes the name and value of the specified property
        /// to the underlying text stream.
        /// </summary>
        /// <param name="propertyInfo">See <see cref="ExceptionFormatter.WritePropertyInfo"/></param>
        /// <param name="propertyValue">See <see cref="ExceptionFormatter.WritePropertyInfo"/></param>
        protected override void WritePropertyInfo(PropertyInfo propertyInfo, object propertyValue)
        {
            this.Indent();
            this.Writer.Write(propertyInfo.Name);
            this.Writer.Write(" : ");
            this.Writer.WriteLine(propertyValue);
        }

        /// <summary>
        /// Writes the name and value of the specified
        /// field to the underlying text stream.
        /// </summary>
        /// <param name="field">See <see cref="ExceptionFormatter.WriteFieldInfo"/></param>
        /// <param name="fieldValue">See <see cref="ExceptionFormatter.WriteFieldInfo"/></param>
        protected override void WriteFieldInfo(FieldInfo field, object fieldValue)
        {
            this.Indent();
            this.Writer.Write(field.Name);
            this.Writer.Write(" : ");
            this.Writer.WriteLine(fieldValue);
        }

        /// <summary>
        /// Formats the stack trace.
        /// </summary>
        /// <param name="stackTrace">See <see cref="ExceptionFormatter.WriteStackTrace"/></param>
        /// <remarks>
        /// If there is no stack trace available, an appropriate message
        /// will be displayed.
        /// </remarks>
        protected override void WriteStackTrace(string stackTrace)
        {
            this.Indent();
            this.Writer.Write(SR.StackTrace);
            this.Writer.Write(" : ");
            if (stackTrace == null || stackTrace.Length == 0)
            {
                this.Writer.WriteLine(SR.StackTraceUnavailable);
            }
            else
            {
                // The stack trace has all '\n's prepended with a number
                // of tabs equal to the InnerDepth property in order
                // to make the formatting pretty.
                string indentation = new String('\t', this.innerDepth);
                string indentedStackTrace = stackTrace.Replace("\n", "\n" + indentation);

                this.Writer.WriteLine(indentedStackTrace);
                this.Writer.WriteLine();
            }
        }

        /// <summary>
        /// Writes the additional properties to the underlying stream.
        /// </summary>
        /// <param name="additionalInfo">See <see cref="ExceptionFormatter.WriteAdditionalInfo"/></param>
        protected override void WriteAdditionalInfo(NameValueCollection additionalInfo)
        {
            this.Writer.WriteLine(SR.AdditionalInfo);
            this.Writer.WriteLine();

            foreach (string name in additionalInfo.AllKeys)
            {
                this.Writer.Write(name);
                this.Writer.Write(" : ");
                this.Writer.Write(additionalInfo[name]);
                this.Writer.Write("\n");
            }
        }

        /// <summary>
        /// Indents the underlying text writer 
        /// based on the value of the 
        /// <see cref="InnerDepth"/> property.
        /// </summary>
        protected virtual void Indent()
        {
            for (int i = 0; i < this.InnerDepth; i++)
            {
                this.Writer.Write("\t");
            }
        }

        private void IndentAndWriteLine(string format, params object[] arg)
        {
            this.Indent();
            this.Writer.WriteLine(format, arg);
        }
    }
}