using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RayTracer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public interface IGame
    {
        IServiceProvider Services { get; }

        void Run();
    }

    public class Game : IGame
    {
        public Game(IServiceProvider services)
        {
            Services = services;
        }

        public void Run()
        {
            // run the game here
        }

        public IServiceProvider Services { get; }
    }
}
