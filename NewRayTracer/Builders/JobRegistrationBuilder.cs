using Autofac;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewRayTracer.Builders
{
    public class JobRegistrationBuilder
    {
        private readonly ContainerBuilder _container;
        private readonly string _alias;

        public JobRegistrationBuilder(ContainerBuilder container, string alias)
        {
            _container = container;
            _alias = alias;
        }


    }
}
