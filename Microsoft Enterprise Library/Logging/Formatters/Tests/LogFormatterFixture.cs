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
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests
{
    [TestFixture]
    public class LogFormatterFixture
    {
        [Test]
        public void ApplyTextFormat()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();

            string actual = FormatEntry("{timestamp}: {title} - {message}", entry);
            string expected = entry.TimeStampString + ": " + entry.Title + " - " + entry.Message;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ApplyTextXmlFormat()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            string template = "<EntLibLog><message>{message}</message><timestamp>{timestamp}</timestamp><title>{title}</title></EntLibLog>";
            string actual = FormatEntry(template, entry);

            string expected = "<EntLibLog><message>My message body</message><timestamp>12/31/9999 11:59:59 PM</timestamp><title>=== Header ===</title></EntLibLog>";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteFormattedEntry()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Category = "FormattedCategory";

            Logger.Write(entry);

            string actual = CommonUtil.GetLastEventLogEntryCustom();
            string expected = "12/31/9999 11:59:59 PM: === Header ===\r\n\r\nMy message body";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TextFormatExtendedProperties()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Category = "DictionaryCategory";
            entry.ExtendedProperties = CommonUtil.GetPropertiesHashtable();

            string template = "{timestamp}: {title} - {message}{newline}{dictionary({key} = {value}{newline})}";
            string actual = FormatEntry(template, entry);

            string expected = "12/31/9999 11:59:59 PM: === Header === - My message body\r\nkey1 = value1\r\nkey3 = value3\r\nkey2 = value2\r\n";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TextFormatMutipleExtendedProperties()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Category = "DictionaryCategory";
            entry.ExtendedProperties = CommonUtil.GetPropertiesHashtable();

            string template = "{timestamp}: {title} - {message}{newline}" +
                "Dictionary 1:{newline}{dictionary(((({key} =-= {value}{newline}))))}{newline}{newline}" +
                "Dictionary 1 reformatted:{newline}{dictionary([[{key} === {value}{newline}]])}";
            string actual = FormatEntry(template, entry);

            string expected = "12/31/9999 11:59:59 PM: === Header === - My message body\r\n" +
                "Dictionary 1:\r\n(((key1 =-= value1\r\n)))(((key3 =-= value3\r\n)))(((key2 =-= value2\r\n)))\r\n\r\n" +
                "Dictionary 1 reformatted:\r\n[[key1 === value1\r\n]][[key3 === value3\r\n]][[key2 === value2\r\n]]";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void KeyValuePairExtendedPropertiesFormat()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Category = "DictionaryCategory";
            entry.ExtendedProperties = CommonUtil.GetPropertiesHashtable();

            string template = "{timestamp}: {title} - {message}{newline}" +
                "{dictionary({key} = {value}{newline})}{newline}" +
                "== KeyValue Pair Format Function =={newline}" +
                "Key1 = {keyvalue(key1)}{newline}" +
                "Key2 = {keyvalue(key2)}{newline}" +
                "Key3 = {keyvalue(key3)}";
            string actual = FormatEntry(template, entry);

            string expected = "12/31/9999 11:59:59 PM: === Header === - My message body\r\nkey1 = value1\r\nkey3 = value3\r\nkey2 = value2\r\n\r\n" +
                "== KeyValue Pair Format Function ==\r\nKey1 = value1\r\nKey2 = value2\r\nKey3 = value3";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TimestampFormat()
        {
            string template = "Time is: {timestamp(D)}";
            string actual = FormatEntry(template, CommonUtil.GetDefaultLogEntry());

            string expected = "Time is: Friday, December 31, 9999";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TimestampTokenWithEmptyTemplate()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();

            string template = "Time is: {timestamp()}";
            string actual = FormatEntry(template, entry);

            string expected = "Time is: 12/31/9999 11:59:59 PM";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DictionaryTokenWithEmptyTemplate()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.ExtendedProperties = CommonUtil.GetPropertiesHashtable();

            string template = "Dictionary: {dictionary()} value";
            string actual = FormatEntry(template, entry);

            string expected = "Dictionary:  value";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void KeyValueTokenWithEmptyTemplate()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.ExtendedProperties = CommonUtil.GetPropertiesHashtable();

            string template = "Key is: {keyvalue()} value";
            string actual = FormatEntry(template, entry);

            string expected = "Key is:  value";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void KeyValueTokenWithInvalidTemplate()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.ExtendedProperties = CommonUtil.GetPropertiesHashtable();

            string template = "Key is: {keyvalue(INVALIDKEY)} value";
            string actual = FormatEntry(template, entry);

            string expected = "Key is:  value";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void KeyValueTokenWithMissingDictionary()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            // do not set extended properties field

            string template = "Key is: {keyvalue(key1)} value";
            string actual = FormatEntry(template, entry);

            string expected = "Key is:  value";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DictionaryTokenWithMissingDictionary()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            // do not set extended properties field

            string template = "Key is: {dictionary({key} - {value}\n)} value";
            string actual = FormatEntry(template, entry);

            string expected = "Key is:  value";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TimestampCustomFormat()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();

            string template = "Time is: {timestamp(MM - dd - yyyy @ hh:mm:ss)}";
            string actual = FormatEntry(template, entry);

            string expected = "Time is: 12 - 31 - 9999 @ 11:59:59";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MultipleTimestampCustomFormats()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();

            string template = "Month: {timestamp(MM )}, Day: {timestamp(dd )}, Year: {timestamp(yyyy )}";
            string actual = FormatEntry(template, entry);

            string expected = "Month: 12 , Day: 31 , Year: 9999 ";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MultipleCustomFormats()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.ExtendedProperties = CommonUtil.GetPropertiesHashtable();

            string template = "Key1 = \"{keyvalue(key1)}\", Month: {timestamp(MM )}, Day: {timestamp(dd )}, Year: {timestamp(yyyy )}";
            string actual = FormatEntry(template, entry);
            string expected = "Key1 = \"value1\", Month: 12 , Day: 31 , Year: 9999 ";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TimestampFormatWithInvalidFormatString()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();

            string actual = FormatEntry("Time is: {timestamp(INVALIDFORMAT)}", entry);

            string expected = "Time is: INVALIDFOR12AT";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]

        public void TokenFunctionConstructorArgs1()
        {
            new ExceptionTokenFunction();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]

        public void TokenFunctionConstructorArgs2()
        {
            new ExceptionTokenFunction(1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]

        public void TokenFunctionConstructorArgs3()
        {
            new ExceptionTokenFunction("1");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]

        public void TokenFunctionConstructorArgs4()
        {
            new ExceptionTokenFunction((long)1);
        }

        public class ExceptionTokenFunction : TokenFunction
        {
            public ExceptionTokenFunction() : base(null)
            {
            }

            public ExceptionTokenFunction(int i) : base(null, null)
            {
            }

            public ExceptionTokenFunction(string s) : base("")
            {
            }

            public ExceptionTokenFunction(long l) : base("", "")
            {
            }

            public override string FormatToken(string tokenTemplate, LogEntry log)
            {
                return null;
            }
        }

        private string FormatEntry(string template, LogEntry entry)
        {
            TextFormatterData data = new TextFormatterData();
            data.Template.Value = template;

            TextFormatter formatter = new TextFormatter(data);
            return formatter.Format(entry);
        }

        [Test]
        public void FormatCustomTokenFunction()
        {
            CustomLogEntry entry = new CustomLogEntry();

            CustomTextFormatterData data = new CustomTextFormatterData();
            data.Template.Value = "Acme custom token template: [[AcmeDBLookup{value1}]]";

            ILogFormatter formatter = new CustomTextFormatter(data);
            string actual = formatter.Format(entry);

            string expected = "Acme custom token template: 1234";
            Assert.AreEqual(expected, actual);
        }
    }
}

#endif