using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rest
{
    public static class Extensions
    {
        public static string ToJson(this object value)
        {
            var settings = new JsonSerializerSettings
                               {
                                   Converters = new List<JsonConverter> {new IsoDateTimeConverter()},
                                   NullValueHandling = NullValueHandling.Ignore
                               };

            var json = JsonConvert.SerializeObject(value, Formatting.None, settings);

            return json;
        }

        public static T FromJson<T>(this string value)
        {
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new IsoDateTimeConverter() },
                NullValueHandling = NullValueHandling.Ignore
            };

            var obj = JsonConvert.DeserializeObject(value, typeof(T), settings);

            var castedObj = (T) obj;

            return castedObj;
        }
    }
}
