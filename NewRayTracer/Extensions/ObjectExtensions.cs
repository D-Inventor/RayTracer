using NewRayTracer.Services;

namespace NewRayTracer.Extensions
{
    public static class ObjectExtensions
    {
        public static T UnwrapDecorators<T>(this T instance)
            => (instance is IDecorator<T> decorator)
                ? decorator.Decoratee.UnwrapDecorators()
                : instance;
    }
}
