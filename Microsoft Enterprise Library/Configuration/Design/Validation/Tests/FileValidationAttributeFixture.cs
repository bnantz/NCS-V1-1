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

#if UNIT_TESTS
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests 
{
    [TestFixture]
    public class FileValidationAttributeFixture : ConfigurationDesignHostTestBase
    {
        private readonly string fileName = @"c:\projects\scratch\myapp.config";
//        private readonly string nonAdminFile = @"c:\myapp.config";

        [Test]
        public void EnsureThatCanCreateFileWithValidPath()
        {
            GeneratedApplicationNode.ConfigurationFile = fileName;
            DoValidation(0);
        }

        [Test]
        public void EnsureCanWriteToAValidFile()
        {
            using (File.Create(fileName))
            {
                DoValidation(0);          
            }
        }

//        // if you are not running as a non-admin acount this test will fail
//        [Test]
//        public void ShouldReciveAnErrorForAFileCantCreateAsNonAdmin()
//        {
//            GeneratedApplicationNode.ConfigurationFile = nonAdminFile;
//            DoValidation(1);
//        }
//
//        // if you are not running as a non-admin acount this test will fail
//        [Test]
//        public void ShouldReciveAnErrorForAFileCantWriteAsNonAdmin()
//        {
//            GeneratedApplicationNode.ConfigurationFile = nonAdminFile;
//            DoValidation(1);
//        }

        [Test]
        public void IfFileIsNullValidationPasses()
        {
            GeneratedApplicationNode.ConfigurationFile = null;
            DoValidation(0);
        }

        private void DoValidation(int expectedErrors)
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(base.Host);
            cmd.Execute(GeneratedApplicationNode);
            Assert.AreEqual(expectedErrors, ValidationErrorsCount);
        }
    }
}   
#endif