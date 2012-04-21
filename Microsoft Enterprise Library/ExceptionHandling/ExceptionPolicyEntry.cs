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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Represents an entry in an <see cref="ExceptionPolicy"/> containing
    /// an exception type as the key and a list of 
    /// <see cref="IExceptionHandler"/> objects as the value.
    /// </summary>
    internal sealed class ExceptionPolicyEntry
    {
        private ExceptionHandlerFactory factory;
        private PostHandlingAction postHandlingAction;
        private string policyName;
        private string typeDataName;

        /// <summary>
        /// Instantiates a new instance of the 
        /// <see cref="ExceptionPolicyEntry"/> class.
        /// </summary>
        internal ExceptionPolicyEntry(string policyName, ExceptionTypeData typeData, ConfigurationContext context)
        {
            this.policyName = policyName;
            this.postHandlingAction = typeData.PostHandlingAction;
            this.typeDataName = typeData.Name;
            this.factory = new ExceptionHandlerFactory(context);
        }

        /// <summary>
        /// Handles all exceptions in the chain.
        /// </summary>
        /// <param name="ex">The <c>Exception</c> to handle.</param>
        /// <returns>Whether or not a rethrow is recommended.</returns>
        internal bool Handle(Exception ex)
        {
            Guid handlingInstanceID = Guid.NewGuid();

            Exception chainException = ExecuteHandlerChain(ex, handlingInstanceID);

            return RethrowRecommended(chainException, ex);
        }

        /// <devdoc>
        /// Rethrows the given exception.  Placed in a seperate method for
        /// easier viewing in the stack trace.
        /// </devdoc>
        private void IntentionalRethrow(Exception chainException, Exception originalException)
        {
            if (chainException != null)
            {
                ExceptionHandledEvent.Fire();
                throw chainException;
            }
            else
            {
                Exception wrappedException = new ExceptionHandlingException(SR.ExceptionNullException);
                ExceptionUtility.LogHandlingException(policyName, wrappedException, chainException, originalException);
                throw wrappedException;
            }
        }

        private bool RethrowRecommended(Exception chainException, Exception originalException)
        {
            switch (postHandlingAction)
            {
                case PostHandlingAction.None:
                    return false;
                case PostHandlingAction.ThrowNewException:
                    IntentionalRethrow(chainException, originalException);
                    return true;
                case PostHandlingAction.NotifyRethrow:
                default:
                    return true;
            }
        }

        private Exception ExecuteHandlerChain(Exception ex, Guid handlingInstanceID)
        {
            string lastHandlerName = String.Empty;
            Exception originalException = ex;
            
            try
            {
                IExceptionHandler[] handlers = this.factory.CreateExceptionHandlers(policyName, typeDataName);
                for (int i = 0; i < handlers.Length; i++)
                {
                    lastHandlerName = handlers[i].GetType().Name;
                    ex = handlers[i].HandleException(ex, policyName, handlingInstanceID);
                }
            }
            catch (Exception handlingException)
            {
                ExceptionUtility.LogHandlingException(
                    policyName,
                    new ExceptionHandlingException(SR.UnableToHandleException(lastHandlerName), handlingException), 
                    ex, 
                    originalException
                    );
                throw new ExceptionHandlingException(SR.UnableToHandleException(lastHandlerName));
            }

            return ex;
        }
    }
}
