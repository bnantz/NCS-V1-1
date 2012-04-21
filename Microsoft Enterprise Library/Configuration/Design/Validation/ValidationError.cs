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
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// <para>Collects information relevant to a warning or error returned by validation.</para>
    /// </summary>
    [Serializable]
    public class ValidationError : ISerializable
    {
        private object invalidItem;
        private string errorMessage;
        private string propertyName;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ValidationError"/> calss with the invalid object, property name, and error message.</para>
        /// </summary>
        /// <param name="invalidItem">
        /// <para>The object that did not validate.</para>
        /// </param>
        /// <param name="propertyName">
        /// <para>The name of the property that is invalid.</para>
        /// </param>
        /// <param name="errorMessage">
        /// <para>The message that describes the error.</para>
        /// </param>
        public ValidationError(object invalidItem, string propertyName, string errorMessage)
        {
            this.invalidItem = invalidItem;
            this.propertyName = propertyName;
            this.errorMessage = errorMessage;
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ValidationError"/> class with serialized data.</para>
        /// </summary>
        /// <param name="info"><para>The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</para></param>
        /// <param name="context"><para>The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</para></param>
        protected ValidationError(SerializationInfo info, StreamingContext context)
        {
            this.propertyName = info.GetString("propertyName");
            this.errorMessage = info.GetString("message");
            object typeObject = info.GetValue("instanceType", typeof(Type));
            if (typeObject != null)
            {
                Type t = (Type)typeObject;
                this.invalidItem = info.GetValue("instance", t);
            }
        }

        /// <summary>
        /// <para>Gets the property name of the failed validation.</para>
        /// </summary>
        /// <value>
        /// <para>The property name of the failed validation.</para>
        /// </value>
        public string PropertyName
        {
            get { return this.propertyName; }
            set { this.propertyName = value; }
        }

        /// <summary>
        /// <para>Gets or sets the message for the error.</para>
        /// </summary>
        /// <value>
        /// <para>The message for the error.</para>
        /// </value>
        public string Message
        {
            get { return this.errorMessage; }
            set { this.errorMessage = value; }
        }

        /// <summary>
        /// <para>Gets or sets the invalid object that that valid validation.</para>
        /// </summary>
        /// <value>
        /// <para>The invalid object that that valid validation.</para>
        /// </value>
        public object InvalidObject
        {
            get { return this.invalidItem; }
            set { this.invalidItem = value; }
        }

        /// <summary>
        /// <para>Returns the string representatio of the error.</para>
        /// </summary>
        /// <returns>
        /// <para>The string representatio of the error</para>
        /// </returns>
        public override string ToString()
        {
            return SR.ValidationErrorToString(PropertyName, InvalidObject.ToString(), InvalidObject.GetType().ToString(), Message);
        }

        /// <summary>
        /// <para>When overridden in a derived class, sets the SerializationInfo with information about the exception.</para>
        /// </summary>
        /// <param name="info">
        /// <para>The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</para>
        /// </param>
        /// <param name="context">
        /// <para>The <see cref="StreamingContext"/> that contains contextual information about the source or destination. </para>
        /// </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            GetObjectData(info, context);
        }

        /// <summary>
        /// <para>When overridden in a derived class, sets the SerializationInfo with information about the exception.</para>
        /// </summary>
        /// <param name="info">
        /// <para>The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</para>
        /// </param>
        /// <param name="context">
        /// <para>The <see cref="StreamingContext"/> that contains contextual information about the source or destination. </para>
        /// </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("message", this.errorMessage);
            info.AddValue("propertyName", this.propertyName);

            if (this.invalidItem != null)
            {
                Type t = this.invalidItem.GetType();

                if ((t.IsSerializable) || (t.IsSubclassOf(typeof(MarshalByRefObject))))
                {
                    info.AddValue("instanceType", t);
                    info.AddValue("instance", this.invalidItem);
                }
            }
        }
    }
}