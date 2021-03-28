using System;
using System.Linq;

namespace NewRayTracer.Extensions
{
    public static class TypeExtensions
    {
        public static string GetFormattedName(this Type type)
        {
            if (type.IsGenericType)
            {
                string genericArguments = string.Join(", ", from arg in type.GetGenericArguments()
                                                            select arg.GetFormattedName());
                string name = type.Name;
                name = name.Substring(0, name.IndexOf("`"));
                return $"{name}<{genericArguments}>";
            }
            return type.Name;
        }
    }
}
