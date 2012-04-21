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
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration.Tests
{
    public class DistributorSchemaBuilder
    {
        private static readonly string xmlString =
            "<enterpriseLibrary.loggingDistributorSettings " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xmlns=\"" + DistributorSettings.ConfigurationNamespace + "\" " +
                "defaultCategory=\"AppError\" " +
                "eventLogName=\"Application\"> " +
                "<distributorService serviceName=\"EntLib Distributor\" " +
                "queueTimerInterval=\"1000\" msmqPath=\".\\Private$\\entlib\"/>" +
                "<sinks>" +
                "<sink " +
                "xsi:type=\"CustomSinkData\" " +
                "name=\"MockSink\" type=\"" + typeof(MockLogSink).AssemblyQualifiedName + "\"/>" +
                "<sink " +
                "xsi:type=\"CustomSinkData\" " +
                "name=\"MockSink2\" type=\"" + typeof(MockLogSink).AssemblyQualifiedName + "\"/>" +
                "</sinks>" +
                "<categories>" +
                "<category name=\"MockCategoryOne\" categoryID=\"99\">" +
                "<destinations>" +
                "<destination name=\"MockSink\" sink=\"MockSink\"/>" +
                "</destinations>" +
                "</category>" +
                "<category name=\"MockCategoryMany\" categoryID=\"98\">" +
                "<destinations>" +
                "<destination name=\"MockSink\" sink=\"MockSink\"/>" +
                "<destination name=\"MockSink2\" sink=\"MockSink2\"/>" +
                "</destinations>" +
                "</category>" +
                "<category name=\"NoDestinations\" categoryID=\"101\">" +
                "</category>" +
                "</categories>" +
                "</enterpriseLibrary.loggingDistributorSettings>";

        private DistributorSchemaBuilder()
        {
        }

        public static DistributorSettings GetDistributorSettings()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DistributorSettings));
            try
            {
                return xmlSerializer.Deserialize(xmlReader) as DistributorSettings;
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            return null;
        }
    }
}

#endif