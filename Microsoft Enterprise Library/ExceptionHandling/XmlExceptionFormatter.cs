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
using System.Xml;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Represents an exception formatter that formats exception
    /// objects as XML.
    /// </summary>	
    public class XmlExceptionFormatter : ExceptionFormatter
    {
        private XmlWriter xmlWriter;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="XmlExceptionFormatter"/> class using
        /// the specified <see cref="XmlWriter"/> and
        /// <see cref="Exception"/> objects.
        /// </summary>
        /// <param name="xmlWriter">The underlying stream in which to write the XML.</param>
        /// <param name="exception">The <see cref="Exception"/> to format.</param>
        public XmlExceptionFormatter(XmlWriter xmlWriter, Exception exception) : base(exception)
        {
            this.xmlWriter = xmlWriter;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="XmlExceptionFormatter"/> class
        /// using the specified <see cref="TextWriter"/>
        /// and <see cref="Exception"/> objects.
        /// </summary>
        /// <param name="writer">The underlying stream in which to write the XML.</param>
        /// <param name="exception">The <see cref="Exception"/> to format.</param>
        /// <remarks>
        /// An <see cref="XmlTextWriter"/> with indented formatting
        /// is created from the 
        /// specified <see cref="TextWriter"/>.
        /// </remarks>
        public XmlExceptionFormatter(TextWriter writer, Exception exception) : base(exception)
        {
            XmlTextWriter textWriter = new XmlTextWriter(writer);
            textWriter.Formatting = Formatting.Indented;
            xmlWriter = textWriter;
        }

        /// <summary>
        /// Gets the underlying <see cref="XmlWriter"/> that
        /// the formatted exception is written to.
        /// </summary>
        public XmlWriter Writer
        {
            get { return xmlWriter; }
        }

        /// <summary>
        /// Controls the formatting of the exception
        /// into the underlying XML stream.
        /// </summary>
        /// <remarks>For more information, see the documentation
        /// for the base implementation - 
        /// <see cref="ExceptionFormatter.Format"/></remarks>
        public override void Format()
        {
            Writer.WriteStartElement("Exception");

            base.Format();

            Writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the current date and time to the
        /// underlying XML stream.
        /// </summary>
        /// <param name="utcNow">See <see cref="ExceptionFormatter.WriteDateTime"/></param>
        protected override void WriteDateTime(DateTime utcNow)
        {
            DateTime localTime = utcNow.ToLocalTime();
            string localTimeString = localTime.ToString("u", DateTimeFormatInfo.InvariantInfo);
            WriteSingleElement("DateTime", localTimeString);
        }

        /// <summary>
        /// Writes the value of the <see cref="Exception.Message"/>
        /// property to the underyling XML stream.
        /// </summary>
        /// <param name="message">See <see cref="ExceptionFormatter.WriteMessage"/></param>
        protected override void WriteMessage(string message)
        {
            WriteSingleElement("Message", message);
        }

        /// <summary>
        /// Writes a generic description to the underlying
        /// XML stream.
        /// </summary>
        protected override void WriteDescription()
        {
            WriteSingleElement("Description", SR.ExceptionWasCaught(base.Exception.GetType().FullName));
        }

        /// <summary>
        /// Writes the value of the specified help link taken
        /// from the value of the <see cref="Exception.HelpLink"/>
        /// property.
        /// </summary>
        /// <param name="helpLink">See <see cref="ExceptionFormatter.WriteHelpLink"/></param>
        protected override void WriteHelpLink(string helpLink)
        {
            WriteSingleElement("HelpLink", helpLink);
        }

        /// <summary>
        /// Writes the value of the specified stack trace taken
        /// from the value of the <see cref="Exception.StackTrace"/>
        /// property.
        /// </summary>
        /// <param name="stackTrace">See <see cref="ExceptionFormatter.WriteStackTrace"/></param>
        protected override void WriteStackTrace(string stackTrace)
        {
            WriteSingleElement("StackTrace", stackTrace);
        }

        /// <summary>
        /// Writes the value of the specified source taken
        /// from the value of the <see cref="Exception.Source"/>
        /// property.
        /// </summary>
        /// <param name="source">See <see cref="ExceptionFormatter.WriteSource"/></param>
        protected override void WriteSource(string source)
        {
            WriteSingleElement("Source", source);
        }

        /// <summary>
        /// Writes the value of the <see cref="Type.AssemblyQualifiedName"/>
        /// property for the specified exception type to the underlying
        /// XML stream.
        /// </summary>
        /// <param name="exceptionType">See <see cref="ExceptionFormatter.WriteExceptionType"/></param>
        protected override void WriteExceptionType(Type exceptionType)
        {
            WriteSingleElement("ExceptionType", exceptionType.AssemblyQualifiedName);
        }

        /// <summary>
        /// Controls the formatting of the specified exception and 
        /// its inner exception.
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
                Writer.WriteStartElement("InnerException");

                base.WriteException(e, outerException);

                Writer.WriteEndElement();
            }
            else
            {
                base.WriteException(e, outerException);
            }
        }

        /// <summary>
        /// Writes the name and value of the specified property
        /// to the underlying XML stream.
        /// </summary>
        /// <param name="propertyInfo">See <see cref="ExceptionFormatter.WritePropertyInfo"/></param>
        /// <param name="propertyValue">See <see cref="ExceptionFormatter.WritePropertyInfo"/></param>
        protected override void WritePropertyInfo(PropertyInfo propertyInfo, object propertyValue)
        {
            string propertyValueString = SR.UndefinedValue;

            if (propertyValue != null)
            {
                propertyValueString = propertyValue.ToString();
            }

            Writer.WriteStartElement("Property");
            Writer.WriteAttributeString("name", propertyInfo.Name);
            Writer.WriteString(propertyValueString);
            Writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the name and value of the specified
        /// field to the underlying XML stream.
        /// </summary>
        /// <param name="field">See <see cref="ExceptionFormatter.WriteFieldInfo"/></param>
        /// <param name="fieldValue">See <see cref="ExceptionFormatter.WriteFieldInfo"/></param>
        protected override void WriteFieldInfo(FieldInfo field, object fieldValue)
        {
            string fieldValueString = SR.UndefinedValue;

            if (fieldValueString != null)
            {
                fieldValueString = fieldValue.ToString();
            }

            Writer.WriteStartElement("Field");
            Writer.WriteAttributeString("name", field.Name);
            Writer.WriteString(fieldValue.ToString());
            Writer.WriteEndElement();
        }

        /// <summary>
        /// Writes additional information to the underlying XML stream.
        /// </summary>
        /// <param name="additionalInfo">See <see cref="ExceptionFormatter.WriteAdditionalInfo"/></param>
        protected override void WriteAdditionalInfo(NameValueCollection additionalInfo)
        {
            Writer.WriteStartElement("additionalInfo");

            foreach (string name in additionalInfo.AllKeys)
            {
                Writer.WriteStartElement("info");
                Writer.WriteAttributeString("name", name);
                Writer.WriteAttributeString("value", additionalInfo[name]);
                Writer.WriteEndElement();
            }

            Writer.WriteEndElement();
        }

        private void WriteSingleElement(string elementName, string elementText)
        {
            Writer.WriteStartElement(elementName);
            Writer.WriteString(elementText);
            Writer.WriteEndElement();
        }
    }
}