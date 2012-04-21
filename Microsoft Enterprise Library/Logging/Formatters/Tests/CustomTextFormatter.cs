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
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests
{
    public class CustomTextFormatter : TextFormatter
    {
        public CustomTextFormatter() : base()
        {
        }

        public CustomTextFormatter(CustomTextFormatterData data) : base(data)
        {
        }

        public override string Format(LogEntry log)
        {
            CustomLogEntry customEntry = (CustomLogEntry)log;
            base.TemplateBuilder.Replace("{field1}", customEntry.AcmeCoField1);
            base.TemplateBuilder.Replace("{field2}", customEntry.AcmeCoField2);
            base.TemplateBuilder.Replace("{field3}", customEntry.AcmeCoField3);

            CustomToken custom = new CustomToken();
            custom.Format(base.TemplateBuilder, log);

            return base.Format(log);
        }
    }
}

#endif