using System;
using System.Linq;
using System.Reflection;

namespace Authorization.Mvc
{
    internal static class Extensions
    {
        public static T[] GetCustomAttributes<T>(this ICustomAttributeProvider attributeProvider) where T: Attribute
        {
            if (attributeProvider == null)
                throw new ArgumentNullException(nameof(attributeProvider));

            return attributeProvider.GetCustomAttributes(typeof(T), true).OfType<T>().ToArray();
        }
    }
}
