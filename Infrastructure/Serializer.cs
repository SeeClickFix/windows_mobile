using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SeeClickFix.WP8.Infrastructure.Serialization
{
    internal static class Serializer
    {
        internal static object Write(object obj, Type t)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            //byte[] bytes = new byte[str.Length * sizeof(char)];
            //System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            //return bytes;

            //byte[] buff = null;
            //DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
            //using (var ms = new MemoryStream())
            //{
            //    serializer.WriteObject(ms, obj);
            //    ms.Position = 0;
            //    ms.Read(buff, 0, (int)ms.Length);
            //}
            //return buff;
        }

        internal static object Read(object data, Type t)
        {
            //char[] chars = new char[buff.Length / sizeof(char)];
            //System.Buffer.BlockCopy(buff, 0, chars, 0, buff.Length);
            //string str = new string(chars);
            return Newtonsoft.Json.JsonConvert.DeserializeObject((string)data, t);

            //T t;
            //DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            //using (var ms = new MemoryStream())
            //{
            //    t = (T)serializer.ReadObject(ms);
            //    ms.Position = 0;
            //    ms.Read(buff, 0, (int)ms.Length);
            //}
            //return t;
        }
    }
}
