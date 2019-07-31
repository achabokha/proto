using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Embily.Gateways
{
    public class UrlencodeJsonWriter : JsonWriter
    {
        StringWriter _sw;

        public UrlencodeJsonWriter(StringWriter sw)
        {
            _sw = sw;
        }

        public override void WritePropertyName(string name)
        {
            base.WritePropertyName(name);
            _sw.Write($"&{name}=");
        }

        public override void WriteValue(string value)
        {
            base.WriteValue(value);
            _sw.Write(EncodeString(value).Replace("%20", "+"));
        }

        public static string EncodeString(string str)
        {
            //maxLengthAllowed .NET < 4.5 = 32765;
            //maxLengthAllowed .NET >= 4.5 = 65519;
            int maxLengthAllowed = 65519;

            StringBuilder sb = new StringBuilder();
            
            int loops = str.Length / maxLengthAllowed;

            for (int i = 0; i <= loops; i++)
            {
                sb.Append(Uri.EscapeDataString(i < loops ?
                    str.Substring(maxLengthAllowed * i, maxLengthAllowed) :
                    str.Substring(maxLengthAllowed * i)));
            }

            return sb.ToString();
        }

        public override void WriteValue(long value)
        {
            base.WriteValue(value);
            _sw.Write(value);
        }

        public override void WriteValue(int value)
        {
            base.WriteValue(value);
            _sw.Write(value);
        }

        public override void WriteValue(double value)
        {
            base.WriteValue(value);
            _sw.Write(value);
        }

        public override void Flush()
        {
            _sw.Flush();
        }
    }
}