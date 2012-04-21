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

#if   UNIT_TESTS
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Tests
{
    public sealed class DatabaseSettingsBuilder
    {
        private static readonly string xmlString =
            "<enterpriseLibrary.databaseSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/data\" defaultInstance=\"Service_Dflt\">" +
                "<databaseTypes>" +
                "<databaseType name=\"SqlServer\" type=\"Microsoft.Practices.EnterpriseLibrary.Data.SqlDatabase, Microsoft.Practices.EnterpriseLibrary.Data\" />" +
                "<databaseType name=\"Oracle\" type=\"Microsoft.Practices.EnterpriseLibrary.Data.OracleDatabase, Microsoft.Practices.EnterpriseLibrary.Data\" />" +
                "<databaseType name=\"DB2\" type=\"Microsoft.Practices.EnterpriseLibrary.Data.Db2Database, Microsoft.Practices.EnterpriseLibrary.Data\" />" +
                "</databaseTypes>" +
                "<connectionStrings>" +
                "<connectionString name=\"localhost\">" +
                "<ParameterCollection>" +
                "<Parameter name=\"server\" value=\"localhost\" />" +
                "<Parameter name=\"database\" value=\"Northwind\" />" +
                "<Parameter name=\"Integrated Security\" value=\"true\" />" +
                "</ParameterCollection>" +
                "</connectionString>" +
                "</connectionStrings>" +
                "<instances>" +
                "<instance name=\"Northwind\" type=\"SqlServer\" connectionString=\"localhost\" />" +
                "<instance name=\"Shadowfax\" type=\"SqlServer\" connectionString=\"localhost\" />" +
                "</instances>" +
                "</enterpriseLibrary.databaseSettings>";

        private DatabaseSettingsBuilder()
        {
        }

        public static DatabaseSettings Create()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DatabaseSettings));
            try
            {
                return xmlSerializer.Deserialize(xmlReader) as DatabaseSettings;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}

#endif