//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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
using System.Xml.Serialization;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Tests
{
    public sealed class Utility
    {
        private Utility()
        {
        }

        public static void SerializationTest(object o)
        {
            SerializationTest(o.GetType(), o);
        }

        public static void SerializationTest(Type t, object o)
        {
            XmlSerializer serializer =
                new XmlSerializer(t);
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, o);
            stream.Position = 0;
            object deserialized = serializer.Deserialize(stream);
            Assert.IsTrue(t.IsAssignableFrom(deserialized.GetType()));
        }
    }
}

#endif