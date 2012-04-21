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

#if    UNIT_TESTS
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    public class MockConfigurationData
    {
        private int size;
        private string color;
        private string someText;

        public MockConfigurationData()
        {
            size = 0;
            color = string.Empty;
            someText = string.Empty;
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public string SomeText
        {
            get { return someText; }
            set { someText = value; }
        }

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        public override string ToString()
        {
            return "Color = " + color + "; FontSize = " + size.ToString(CultureInfo.CurrentUICulture) + "; SomeText = " + SomeText;
        }
    }
}

#endif