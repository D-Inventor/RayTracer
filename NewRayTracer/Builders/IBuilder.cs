namespace NewRayTracer.Builders
{
    public interface IBuilder<out TOutput>
    {
        TOutput Build();
    }
}
