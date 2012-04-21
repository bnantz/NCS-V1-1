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
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Security.Principal;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Provides functionality for formatting <see cref="Exception"/> objects.
    /// </summary>	
    public abstract class ExceptionFormatter
    {
        private static readonly ArrayList IgnoredProperties = new ArrayList(
            new String[] {"Source", "Message", "HelpLink", "InnerException", "StackTrace"});

        private Exception exception;
        private NameValueCollection additionalInfo = null;

        /// <summary>
        /// Defines the base arguments for constructors
        /// on derived classes.
        /// </summary>
        /// <param name="exception">The exception for formatting.</param>
        protected ExceptionFormatter(Exception exception)
        {
            this.exception = exception;
        }

        /// <summary>
        /// Gets the Exception object containing the exception that is being formatted.
        /// </summary>
        public Exception Exception
        {
            get { return this.exception; }
        }

        /// <summary>
        /// Gets additional information related to the <see cref="Exception"/> but not
        /// stored in the exception (eg: the time in which the <see cref="Exception"/> was 
        /// thrown).
        /// </summary>
        public NameValueCollection AdditionalInfo
        {
            get
            {
                if (this.additionalInfo == null)
                {
                    this.additionalInfo = new NameValueCollection();
                    this.additionalInfo.Add("MachineName", GetMachineName());
                    this.additionalInfo.Add("TimeStamp", DateTime.UtcNow.ToString(CultureInfo.CurrentCulture));
                    this.additionalInfo.Add("FullName", GetExecutingAssembly());
                    this.additionalInfo.Add("AppDomainName", AppDomain.CurrentDomain.FriendlyName);
                    this.additionalInfo.Add("ThreadIdentity", GetThreadIdentity());
                    this.additionalInfo.Add("WindowsIdentity", GetWindowsIdentity());
                }

                return this.additionalInfo;
            }
        }

        /// <summary>
        /// Controls the formatting of the exception into the underlying stream.
        /// </summary>
        public virtual void Format()
        {
            this.WriteDescription();
            this.WriteDateTime(DateTime.UtcNow);
            this.WriteException(this.exception, null);
        }

        /// <summary>
        /// Formats the exception and all nested inner exceptions.
        /// </summary>
        /// <param name="e">The exception to format.</param>
        /// <param name="outerException">The outer exception. This 
        /// value will be null when writing the outer-most exception.</param>
        /// <remarks>
        /// <para>This method calls itself recursively until it reaches
        /// an exception that does not have an inner exception.</para>
        /// <para>
        /// This is a template method which calls the following
        /// methods in order
        /// <list type="number">
        /// <item>
        /// <description><see cref="WriteExceptionType"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="WriteMessage"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="WriteSource"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="WriteHelpLink"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="WriteReflectionInfo"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="WriteStackTrace"/></description>
        /// </item>
        /// <item>
        /// <description>If the specified exception has an inner exception
        /// then it makes a recursive call. <see cref="WriteException"/></description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        protected virtual void WriteException(Exception e, Exception outerException)
        {
            this.WriteExceptionType(e.GetType());
            this.WriteMessage(e.Message);
            this.WriteSource(e.Source);
            this.WriteHelpLink(e.HelpLink);
            this.WriteReflectionInfo(e);
            this.WriteStackTrace(e.StackTrace);

            // We only want additional information on the top most exception
            if (outerException == null)
            {
                this.WriteAdditionalInfo(this.AdditionalInfo);
            }

            Exception inner = e.InnerException;

            if (inner != null)
            {
                // recursive call
                this.WriteException(inner, e);
            }
        }

        /// <summary>
        /// Formats an exception using reflection.
        /// </summary>
        /// <param name="e">
        /// The exception to be formatted.
        /// </param>
        /// <remarks>
        /// <para>This method reflects over the public, instance properties 
        /// and public, instance fields
        /// of the specified exception and prints them to the formatter.  
        /// Certain property names are ignored
        /// because they are handled explicitly in other places.</para>
        /// </remarks>
        protected void WriteReflectionInfo(Exception e)
        {
            Type type = e.GetType();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            object value;

            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead && IgnoredProperties.IndexOf(property.Name) == -1)
                {
                    value = property.GetValue(e, null);
                    WritePropertyInfo(property, value);
                }
            }

            foreach (FieldInfo field in fields)
            {
                value = field.GetValue(e);
                WriteFieldInfo(field, value);
            }
        }

        /// <summary>
        /// Writes a description of the caught exception.
        /// </summary>
        protected abstract void WriteDescription();

        /// <summary>
        /// Writes the current time.
        /// </summary>
        /// <param name="utcNow">The current time.</param>
        protected abstract void WriteDateTime(DateTime utcNow);

        /// <summary>
        /// Writes the <see cref="Type"/> of the current exception.
        /// </summary>
        /// <param name="exceptionType">The <see cref="Type"/> of the exception.</param>
        protected abstract void WriteExceptionType(Type exceptionType);

        /// <summary>
        /// Writes the value of the <see cref="System.Exception.Message"/> property.
        /// </summary>
        /// <param name="message">The message to write.</param>
        protected abstract void WriteMessage(string message);

        /// <summary>
        /// Writes the value of the <see cref="System.Exception.Source"/> property.
        /// </summary>
        /// <param name="source">The source of the exception.</param>
        protected abstract void WriteSource(string source);

        /// <summary>
        /// Writes the value of the <see cref="System.Exception.HelpLink"/> property.
        /// </summary>
        /// <param name="helpLink">The help link for the exception.</param>
        protected abstract void WriteHelpLink(string helpLink);

        /// <summary>
        /// Writes the value of the <see cref="System.Exception.StackTrace"/> property.
        /// </summary>
        /// <param name="stackTrace">The stack trace of the exception.</param>
        protected abstract void WriteStackTrace(string stackTrace);

        /// <summary>
        /// Writes the value of a <see cref="PropertyInfo"/> object.
        /// </summary>
        /// <param name="propertyInfo">The reflected PropertyInfo object.</param>
        /// <param name="value">The value of the Property.</param>
        protected abstract void WritePropertyInfo(PropertyInfo propertyInfo, object value);

        /// <summary>
        /// Writes the value of a <see cref="FieldInfo"/> object.
        /// </summary>
        /// <param name="field">The reflected FieldInfo object.</param>
        /// <param name="value">The value of the Field.</param>
        protected abstract void WriteFieldInfo(FieldInfo field, object value);

        /// <summary>
        /// Writes additional properties if available.
        /// </summary>
        /// <param name="additionalInfo">additional information to be included with the exception report</param>
        protected abstract void WriteAdditionalInfo(NameValueCollection additionalInfo);

        private string GetMachineName()
        {
            string machineName = String.Empty;
            try
            {
                machineName = Environment.MachineName;
            }
            catch (InvalidOperationException)
            {
                machineName = SR.PermissionDenied;
            }
            catch
            {
                machineName = SR.UnknownReadError;
            }

            return machineName;
        }

        private string GetThreadIdentity()
        {
            string threadIdentity = String.Empty;
            try
            {
                threadIdentity = Thread.CurrentPrincipal.Identity.Name;
            }
            catch (SecurityException)
            {
                threadIdentity = SR.PermissionDenied;
            }
            catch
            {
                threadIdentity = SR.UnknownReadError;
            }

            return threadIdentity;
        }

        private string GetWindowsIdentity()
        {
            string windowsIdentity = String.Empty;
            try
            {
                windowsIdentity = WindowsIdentity.GetCurrent().Name;
            }
            catch (SecurityException)
            {
                windowsIdentity = SR.PermissionDenied;
            }
            catch
            {
                windowsIdentity = SR.UnknownReadError;
            }

            return windowsIdentity;
        }

        private string GetExecutingAssembly()
        {
            string executingAssembly = String.Empty;

            try
            {
                executingAssembly = Assembly.GetExecutingAssembly().FullName;
            }
            catch (SecurityException)
            {
                executingAssembly = SR.PermissionDenied;
            }
            catch
            {
                executingAssembly = SR.UnknownReadError;
            }

            return executingAssembly;
        }
    }
}