//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestFixture]
    public class SerializationUtilityFixture
    {
        [Test]
        public void CanSerializeString()
        {
            Assert.AreEqual("this is a string", ToObject(ToBytes("this is a string")));
        }

        [Test]
        [ExpectedException(typeof(SerializationException))]
        public void CanSerializeUnserializableObject()
        {
            ToBytes(new NonSerializableClass());
        }

        [Test]
        public void CanSerializeSerializableObject()
        {
            SerializableClass serializedInstance = (SerializableClass)ToObject(ToBytes(new SerializableClass()));
            Assert.AreEqual(1, serializedInstance.Counter);
        }

        [Test]
        public void CanSerializeMBRObject()
        {
            using (FileStream outputStream = new FileStream("test.out", FileMode.Create))
            {
                new BinaryFormatter().Serialize(outputStream, new MarshalByRefClass(13));
            }

            object deserializedObject = null;
            using (FileStream inputStream = new FileStream("test.out", FileMode.Open))
            {
                deserializedObject = new BinaryFormatter().Deserialize(inputStream);
            }

            File.Delete("test.out");

            MarshalByRefClass serializedInstance = (MarshalByRefClass)deserializedObject;
            Assert.AreEqual(13, serializedInstance.Counter);
        }

        [Test]
        public void TryingToSerializeNullObjectReturnsNull()
        {
            Assert.IsNull(ToBytes(null));
        }

        [Test]
        public void TryingToDeserializeNullArrayOfBytesReturnsNull()
        {
            Assert.IsNull(ToObject(null));
        }

        [Test]
        public void CanSerializeAndDeserializeRefreshAction()
        {
            byte[] refreshBytes = ToBytes(new RefreshAction());
            object refreshObject = ToObject(refreshBytes);

            Assert.AreEqual(typeof(RefreshAction), refreshObject.GetType());
            Assert.IsTrue(refreshBytes.Length < 2048);
        }

        private byte[] ToBytes(object objectToSerialize)
        {
            return SerializationUtility.ToBytes(objectToSerialize);
        }

        private object ToObject(byte[] serializedObject)
        {
            return SerializationUtility.ToObject(serializedObject);
        }

        [Serializable]
        private class RefreshAction : ICacheItemRefreshAction
        {
            #region ICacheItemRefreshAction Members
            public void Refresh(string removedKey, object expiredValue, CacheItemRemovedReason removalReason)
            {
                // TODO:  Add RefreshAction.Refresh implementation
            }
            #endregion
        }
    }

    [Serializable]
    internal class SerializableClass
    {
        public int Counter
        {
            get { return counter; }
        }

        private int counter = 1;
    }

    internal class NonSerializableClass
    {
    }

    [Serializable]
    internal class MarshalByRefClass : MarshalByRefObject
    {
        private int counter;

        public MarshalByRefClass(int counter)
        {
            this.counter = counter;
        }

        public int Counter
        {
            get { return counter; }
        }
    }
}

#endif