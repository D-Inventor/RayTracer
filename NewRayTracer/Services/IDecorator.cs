namespace NewRayTracer.Services
{
    public interface IDecorator<out T>
    {
        T Decoratee { get; }
    }
}
