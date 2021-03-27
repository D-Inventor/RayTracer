using System;
using System.IO;
using System.Text.Json;

namespace NewRayTracer.Models.Configuration
{
    public class GameEnvironment : IGameEnvironment
    {
        public string Environent { get; }
        public string ConfigDirectory { get; }

        #region Singleton implementation
        private static readonly Lazy<GameEnvironment> _lazyGameEnvironment = new Lazy<GameEnvironment>(() => Create());

        private GameEnvironment(EnvironmentJsonModel model)
        {
            Environent = model.Environment;
            ConfigDirectory = !string.IsNullOrWhiteSpace(model.ConfigDirectory) ? model.ConfigDirectory : Directory.GetCurrentDirectory();
        }

        private static GameEnvironment Create()
        {
            using (StreamReader sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Environment.json")))
                return new GameEnvironment(JsonSerializer.Deserialize<EnvironmentJsonModel>(sr.ReadToEnd()));
        }

        public static IGameEnvironment Instance => _lazyGameEnvironment.Value;


        private class EnvironmentJsonModel
        {
            public string Environment { get; set; }
            public string ConfigDirectory { get; set; }
        }
        #endregion
    }
}
