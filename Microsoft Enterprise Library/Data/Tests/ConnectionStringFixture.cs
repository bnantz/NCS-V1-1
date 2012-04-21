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

#if  UNIT_TESTS
using System;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    [TestFixture]
    public class ConnectionStringFixture
    {
        private static readonly string userName = "User";
        private static readonly string password = "Password";
        private static readonly string userIdTokens = "user id=,uid=";
        private static readonly string passwordTokens = "password=,pwd=";
        private ConnectionString connectionString;

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidOperationExceptionThrownWhenConnectionStringIsNull()
        {
            this.connectionString = new ConnectionString(null, userIdTokens, passwordTokens);
            string password = this.connectionString.Password;
            Assert.IsTrue(password != null);
        }

        [Test]
        public void EmptyCredentialsReturnedForEmptyConnectionString()
        {
            this.connectionString = new ConnectionString(string.Empty, userIdTokens, passwordTokens);
            Assert.AreEqual(0, this.connectionString.UserName.Length);
            Assert.AreEqual(0, this.connectionString.Password.Length);
        }

        [Test]
        public void CanGetCredentialsFromRealSqlDataClass()
        {
            string initialConnectionString = String.Format("server=localhost; database=JoeRandom; uid={0}; pwd={1}; ;", userName, password);
            this.connectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            Assert.AreEqual(userName, this.connectionString.UserName);
            Assert.AreEqual(password, this.connectionString.Password);
        }

        [Test]
        public void CreateNewConnectionStringTest()
        {
            string initialConnectionString = String.Format("server=localhost; database=JoeRandom; uid={0}; pwd={1}; ;", userName, password);
            this.connectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens).CreateNewConnectionString(initialConnectionString);
            Assert.AreEqual(userName, this.connectionString.UserName);
            Assert.AreEqual(password, this.connectionString.Password);
        }

        [Test]
        public void CanGetCredentialsUsingAlternatePatternsForUidAndPwd()
        {
            string initialConnectionString = String.Format("server=localhost; database=JoeRandom; user id={0}; password={1}; ;", userName, password);
            this.connectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            Assert.AreEqual(userName, this.connectionString.UserName);
            Assert.AreEqual(password, this.connectionString.Password);
        }

        [Test]
        public void CanAddCredentialsToConnectionStringThatDoesNotHaveThem()
        {
            string initialConnectionString = "server=localhost; database=RandomData; ; ;";
            this.connectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            this.connectionString.UserName = userName;
            this.connectionString.Password = password;
            string actualConnectionString = String.Format("server=localhost; database=RandomData; ; ;user id={0};password={1};",
                                                          userName, password);
            Assert.AreEqual(actualConnectionString, this.connectionString.ToString());
        }

        [Test]
        public void CanSetUserIdAndPasswordInConnectionStringThatAlreadyHasOne()
        {
            string initialConnectionString = String.Format("server=localhost; database=JoeRandom; user id={0}; password={1}; ;", "Kill", "Bill");
            ConnectionString newConnectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            newConnectionString.UserName = userName;
            newConnectionString.Password = password;
            string actualConnectionString = String.Format("server=localhost; database=JoeRandom; user id={0}; password={1}; ;", userName, password);
            Assert.AreEqual(actualConnectionString, newConnectionString.ToString());
        }

        /// <summary>
        /// Test to see if ToStringNoCredentials works properly for a connection string 
        /// without a username and password
        /// </summary>
        [Test]
        public void RemovingCredentialsFromConnectionStringWithoutThemIsOk()
        {
            string initialConnectionString = "server=localhost;database=RandomData;";
            ConnectionString newConnectionString = new ConnectionString(initialConnectionString, userIdTokens, passwordTokens);
            string expectedConnectionString = "server=localhost;database=randomdata;";
            string strippedConnectionString = newConnectionString.ToStringNoCredentials();
            Assert.AreEqual(expectedConnectionString, strippedConnectionString);
        }

        /// <summary>
        /// Test to see that ToStringNoCredentials removes the username and password
        /// </summary>
        [Test]
        public void WillRemoveCredentialsFromConnectionString()
        {
            string initialConnectionString = "server=localhost;database=RandomData;user id=Bill;pwd=goodPassword";
            ConnectionString newConnectionString = new ConnectionString(initialConnectionString,
                                                                        userIdTokens, passwordTokens);
            string expectedConnectionString = "server=localhost;database=randomdata;";
            string strippedConnectionString = newConnectionString.ToStringNoCredentials();
            Assert.AreEqual(expectedConnectionString, strippedConnectionString);
        }

        [Test]
        public void NullTests()
        {
            ConnectionString connectionString = new ConnectionString(null, null, null);
            Assert.AreEqual(string.Empty, connectionString.ToStringNoCredentials());
        }
    }
}

#endif