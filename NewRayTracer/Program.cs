using NewRayTracer.Builders;
using NewRayTracer.Models.Configuration;

using System;
using System.Threading.Tasks;

namespace NewRayTracer
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var gameBuilder = new GameBuilder()
                .CreateDefault();
            gameBuilder.Build();
            Console.ReadKey();
        }
    }
}
