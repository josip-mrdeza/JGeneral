

using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace JGeneral.IO.Database
{
    public static class JObjectHelper
    {
        private static JsonSerializer serializer = JsonSerializer.CreateDefault();
        /// <summary>
        /// Encapsulates the instance in a JObject if it is not already of type JObject.
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static JObject<T> Create<T>(this T instance, string id)
        {
            return new (instance, id);
        }

        public static string ToJson<T>(this JObject<T> obj)
        {
            StringBuilder b = new StringBuilder();
            serializer.Serialize(new JsonTextWriter(new StringWriter(b)), obj.ObjectData);
            return b.ToString();
        }
        public static string ToJson<T>(this T obj)
        {
            StringBuilder b = new StringBuilder();
            serializer.Serialize(new JsonTextWriter(new StringWriter(b)), obj);
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(b.ToString()), Formatting.Indented);
        }

        public static T FromJsonFile<T>(this string path)
        {
            return serializer.Deserialize<T>(new JsonTextReader(new StringReader(File.ReadAllText(path))));
        }
        public static T FromJsonText<T>(this string text)
        {
            return serializer.Deserialize<T>(new JsonTextReader(new StringReader(text)));
        }

        public static byte[] ToJsonBytes<T>(this T obj)
        {
            return Encoding.ASCII.GetBytes(obj.ToJson());
        }

        public static void SaveAsConfig<T>(this T obj, string path) where T : notnull
        {
            File.WriteAllText(path, obj.ToJson());
        }

        public static T LoadAsConfig<T>(this string path)
        {
            var encoded = File.ReadAllText(path);
            return serializer.Deserialize<T>(new JsonTextReader(new StringReader(encoded)));
        }
    }
}