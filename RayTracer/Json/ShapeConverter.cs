using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

using RayTracer.Models.Shapes;

using System;

namespace RayTracer.Json
{
    internal class ShapeConverterContractResolver : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(ShapeModel).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null;
            return base.ResolveContractConverter(objectType);
        }
    }

    internal class ShapeConverter : JsonConverter
    {
        private static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings
        {
            ContractResolver = new ShapeConverterContractResolver()
        };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ShapeModel);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            if (jo["ShapeType"].Value<string>() == "Sphere")
                return JsonConvert.DeserializeObject<SphereModel>(jo.ToString(), SpecifiedSubclassConversion);

            if (jo["ShapeType"].Value<string>() == "Plane")
                return JsonConvert.DeserializeObject<PlaneModel>(jo.ToString(), SpecifiedSubclassConversion);

            return null;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
