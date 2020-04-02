using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Framework.JsonConverters
{
    public static class JsonConverterHelpers
    {
        public static T DeserializeJsonFromStream<T>(Stream stream)
        {
            var serializer = new JsonSerializer();

            if (stream is null || stream.CanRead is false)
            {
                return default;
            }

            using (StreamReader streamReader = new StreamReader(stream))
            {
                return (T)serializer.Deserialize(streamReader, typeof(T));
            }
        }

        public static T DeserializJsonFromString<T>(this String json)
        {
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(json)))
            {
                return DeserializeJsonFromStream<T>(stream);
            }
        }
    }
}
