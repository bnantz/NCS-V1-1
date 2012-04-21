//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    public class MockExceptionHandler : ExceptionHandler
    {
        public static string testProperty;
        public static int handleExceptionCount = 0;
        public static string lastMessage;
        public static Guid handlingInstanceID;

        public MockExceptionHandler()
        {
        }

        public override void Initialize(ConfigurationView configurationView)
        {
        	ExceptionHandlingConfigurationView exceptionHandlingConfigurationView = (ExceptionHandlingConfigurationView)configurationView;
            CustomHandlerData data = (CustomHandlerData)exceptionHandlingConfigurationView.GetExceptionHandlerData(CurrentPolicyName, CurrentExceptionTypeName, ConfigurationName);
            if (data.Attributes["TestProperty"] != null)
            {
                testProperty = data.Attributes["TestProperty"];
            }
        }

        public static void Clear()
        {
            handleExceptionCount = 0;
            lastMessage = String.Empty;
            handlingInstanceID = Guid.Empty;
        }

        public static string FormatExceptionMessage(string message, Guid handlingInstanceID)
        {
            return ExceptionUtility.FormatExceptionMessage(message, handlingInstanceID);
        }

        public override Exception HandleException(Exception ex, string policyName, Guid handlingInstanceID)
        {
            handleExceptionCount++;
            lastMessage = ex.Message;
            MockExceptionHandler.handlingInstanceID = handlingInstanceID;
            return ex;
        }
    }
}

#endif