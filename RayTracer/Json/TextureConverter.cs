using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

using RayTracer.Models.Textures;

using System;

namespace RayTracer.Json
{
    internal class TextureConverterContractResolver : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(TextureModel).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null;
            return base.ResolveContractConverter(objectType);
        }
    }

    internal class TextureConverter : JsonConverter
    {
        private static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings
        {
            ContractResolver = new TextureConverterContractResolver()
        };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TextureModel);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            if (jo["TextureType"].Value<string>() == "Image")
                return JsonConvert.DeserializeObject<ImageTextureModel>(jo.ToString(), SpecifiedSubclassConversion);

            if (jo["TextureType"].Value<string>() == "Colour")
                return JsonConvert.DeserializeObject<ColourTextureModel>(jo.ToString(), SpecifiedSubclassConversion);

            return null;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
