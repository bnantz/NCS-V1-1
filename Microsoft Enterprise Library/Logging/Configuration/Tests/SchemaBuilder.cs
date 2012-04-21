//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
    public class SchemaBuilder
    {
        private static readonly string xmlString =
            "<enterpriseLibrary.loggingSettings " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xmlns=\"" + LoggingSettings.ConfigurationNamespace + "\" " +
                "loggingEnabled=\"true\" " +
                "tracingEnabled=\"true\" " +
                "msmqPath=\".\\Private$\\entlib\" " +
                "distributionStrategy=\"" + typeof(InProcLogDistributionStrategy).AssemblyQualifiedName + "\" " +
                "minimumPriority=\"5\" " +
                "categoryFilterMode=\"DenyAllExceptAllowed\">" +
                "<distributionStrategies>" +
                "<distributionStrategy " +
                "xsi:type=\"CustomDistributionStrategyData\" " +
                "name=\"InProc\" " +
                "type=\"" + typeof(InProcLogDistributionStrategy).AssemblyQualifiedName + "\"/>" +
                "<distributionStrategy " +
                "xsi:type=\"MsmqDistributionStrategyData\" " +
                "name=\"Msmq\" " +
                "type=\"" + typeof(MsmqLogDistributionStrategy).AssemblyQualifiedName + "\" " +
                "queuePath=\".\\Private$\\entlib\"/>" +
                "</distributionStrategies>" +
                "<categoryFilters>" +
                "<categoryFilter name=\"CategoryOne\"/>" +
                "<categoryFilter name=\"CategoryTwo\"/>" +
                "<categoryFilter name=\"CategoryThree\"/>" +
                "</categoryFilters>" +
                "</enterpriseLibrary.loggingSettings>";

        private SchemaBuilder()
        {
        }

        public static LoggingSettings GetLoggingSettings()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LoggingSettings));
            return xmlSerializer.Deserialize(xmlReader) as LoggingSettings;
        }
    }
}

#endif