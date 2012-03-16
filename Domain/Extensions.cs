using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Domain
{
    public static class Extensions
    {
        public static Dictionary<Type, Dictionary<string, Type>> ContentTypeTypes { get; set; }

        static Extensions()
        {
            var fixtureContentTypeTypes = new Dictionary<string, Type>
                                              {
                                                  {Fixture.ContentTypeTennis, typeof (TennisFixture)},
                                                  {Fixture.ContentTypeFootball, typeof (FootballFixture)}
                                              };

            var contentTypeTypes = new Dictionary<Type, Dictionary<string, Type>>
                                       {
                                           {typeof (Fixture), fixtureContentTypeTypes}
                                       };

            ContentTypeTypes = contentTypeTypes;
        }

        public static T FromJson<T>(this string value, string contentType)
        {
            var baseType = typeof(T);
            var contentTypeType = ContentTypeTypes[baseType][contentType];

            var obj = JsonConvert.DeserializeObject(value, contentTypeType);

            var castedObj = (T)obj;

            return castedObj;
        }
    }
}
