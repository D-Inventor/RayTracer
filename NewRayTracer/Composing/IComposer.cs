using NewRayTracer.Models.Composition;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewRayTracer.Composing
{
    public interface IComposer
    {
        void Compose(CompositionContext context);
    }
}
