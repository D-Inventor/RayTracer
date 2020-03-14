namespace RayTracer.Pipeline
{
    public interface IPipelineContext
    {
        bool IsBroken { get; }

        void Break();

        IPipelineContext Copy();
    }

    public class PipelineContext : IPipelineContext
    {
        public PipelineContext(PipelineContext original)
        {
            IsBroken = original.IsBroken;
        }

        public PipelineContext()
        {
            IsBroken = false;
        }

        public bool IsBroken { get; private set; }

        public void Break()
        {
            IsBroken = true;
        }

        public IPipelineContext Copy()
        {
            return new PipelineContext(this);
        }
    }
}
