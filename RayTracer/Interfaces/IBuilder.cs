namespace RayTracer.Interfaces
{
    public interface IBuilder<out Type>
    {
        Type Build();
    }
}
