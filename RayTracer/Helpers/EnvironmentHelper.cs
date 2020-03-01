using Microsoft.Extensions.Options;
using RayTracer.Models.Options;
using System;
using System.IO;

namespace RayTracer.Helpers
{
    public interface IEnvironmentHelper
    {
        string Environment { get; }
        string ConfigurationPath { get; }
    }

    public class EnvironmentHelper : IEnvironmentHelper
    {
        private readonly EnvironmentOptions environmentOptions;

        public EnvironmentHelper(EnvironmentOptions environmentOptions)
        {
            this.environmentOptions = environmentOptions ?? throw new ArgumentNullException(nameof(environmentOptions));
        }

        public string Environment { get => environmentOptions.Environment; }
        public string ConfigurationPath
        {
            get
            {
                string path = environmentOptions.ConfigurationPath;
                if (string.IsNullOrWhiteSpace(path))
                {
                    path = Directory.GetCurrentDirectory();
                }
                return path;
            }
        }
    }
}
