using NewRayTracer.Models.Configuration;

using System;
using System.Threading.Tasks;

namespace NewRayTracer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var env = GameEnvironment.Instance;
            Console.WriteLine(env.Environent);
            Console.ReadKey();
        }
    }
}
