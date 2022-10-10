using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using Xunit;

namespace NetCore.Assumptions.Runtime.Serialization
{
    public class AboutSerializationBinder
    {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
        [Fact]
        public void Can_deserialize_A_as_B()
        {
            MemoryStream ms = new MemoryStream();
            var a = new A();
            a.name = "test";

            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, a);

            ms.Seek(0, SeekOrigin.Begin);
            formatter.Binder = new AToBDeserializationBinder();
            var b = (B)formatter.Deserialize(ms);

            Assert.Equal(a.name, b.name);
        }
#pragma warning restore SYSLIB0011 // Type or member is obsolete

        [Serializable]
        public class A
        {
            public string name;
        }

#pragma warning disable SYSLIB0003 // Type or member is obsolete
        [Serializable]
        public class B : ISerializable
        {
            public string name;

            // The security attribute demands that code that calls
            // this method have permission to perform serialization.
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("name", name);
            }

            // The security attribute demands that code that calls  
            // this method have permission to perform serialization.
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            private B(SerializationInfo info, StreamingContext context)
            {
                name = info.GetString("name");
            }
        }
#pragma warning restore SYSLIB0003 // Type or member is obsolete

        sealed class AToBDeserializationBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                return typeof(B);
            }
        }
    }
}
