﻿using Newtonsoft.Json;

using RayTracer.Json;

namespace RayTracer.Models.Storage.Textures
{
    [JsonConverter(typeof(TextureConverter))]
    public abstract class TextureModel
    {
        public string Name { get; set; }
        public abstract string TextureType { get; set; }
    }
}
