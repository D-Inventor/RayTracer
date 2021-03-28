using NewRayTracer.Builders;

using System;
using System.Threading.Tasks;

namespace NewRayTracer
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            GameBuilder gameBuilder = new GameBuilder()
                .CreateDefault();
            gameBuilder.Build();
            Console.ReadKey();
        }
    }
}
