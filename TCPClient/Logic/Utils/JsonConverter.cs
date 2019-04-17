using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient.Logic
{
    public class JsonConverter
    {
        public static string Serialize(object obj)
        {
            var jsonFormatter = new DataContractJsonSerializer(obj.GetType());
            using (var memStream = new MemoryStream()) {
                jsonFormatter.WriteObject(memStream, obj);
                memStream.Position = 0;
                return new StreamReader(memStream).ReadToEnd();
            }
        }

        public static byte[] SerializeToBytes(object obj)
        {
            return Encoding.UTF8.GetBytes(Serialize(obj));
        }
        
        public static T Deserialize<T>(string str)
        {
            var jsonFormatter = new DataContractJsonSerializer(typeof(T));
            using (var memStream = new MemoryStream())
            {
                var streamWriter = new StreamWriter(memStream);
                streamWriter.Write(str);
                streamWriter.Flush();
                memStream.Position = 0;
                var obj = jsonFormatter.ReadObject(memStream);
                return (T)obj;
            }
        }

        public static T DeserializeFromBytes<T>(byte[] bytes)
        {
            return Deserialize<T>(Encoding.UTF8.GetString(bytes));
        }
    }
}
