//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Tests
{
    public sealed class DatabaseSettingsBuilder
    {
        private static readonly string xmlString =
            "<enterpriseLibrary.databaseSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/data\" defaultInstance=\"Service_Dflt\">" +
                "<databaseTypes>" +
                "<databaseType name=\"SqlServer\" type=\"" + typeof(SqlDatabase).AssemblyQualifiedName + "\" />" +
                "<databaseType name=\"Oracle\" type=\"" + typeof(OracleDatabase).AssemblyQualifiedName + "\" />" +
                "</databaseTypes>" +
                "<connectionStrings>" +
                "<connectionString name=\"Northwind\">" +
                "<parameters>" +
                "<parameter name=\"server\" value=\"localhost\" />" +
                "<parameter name=\"user id\" value=\"Northwind\" />" +
                "<parameter name=\"Integrated Security\" value=\"true\" />" +
                "</parameters>" +
                "</connectionString>" +
                "<connectionString xsi:type=\"OracleConnectionStringData\" " +
                "name=\"OracleConnection\">" +
                "<parameters>" +
                "<parameter name=\"server\" value=\"entlib\" />" +
                "<parameter name=\"database\" value=\"testuser\" />" +
                "<parameter name=\"password\" value=\"testuser\" />" +
                "</parameters>" +
                "<packages>" +
                "<package prefix=\"MyStoredProc\" name=\"ENTLIB\" />" +
                "</packages>" +
                "</connectionString>" +
                "</connectionStrings>" +
                "<instances>" +
                "<instance name=\"NewDatabase\" type=\"SqlServer\" connectionString=\"Northwind\"/>" +
                "<instance name=\"Service_Dflt\" type=\"SqlServer\" connectionString=\"Northwind\"/>" +
                "<instance name=\"DbWithSqlServerAuthn\" type=\"SqlServer\" connectionString=\"Northwind\"/>" +
                "</instances>" +
                "</enterpriseLibrary.databaseSettings>";

        private DatabaseSettingsBuilder()
        {
        }

        public static DatabaseSettings Create(IServiceProvider serviceProvider)
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(serviceProvider);

            Type nodeType = typeof(OracleConnectionStringNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(OracleConnectionStringData), SR.OracleConnectionStringNodeFriendlyName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(ConnectionStringNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(ConnectionStringData), SR.ConnectionStringNodeFriendlyName);
            nodeCreationService.AddNodeCreationEntry(entry);
            
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DatabaseSettings));
            return xmlSerializer.Deserialize(xmlReader) as DatabaseSettings;
        }
    }
}

#endif