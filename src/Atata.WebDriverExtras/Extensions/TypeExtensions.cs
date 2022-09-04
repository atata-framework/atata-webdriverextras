using System;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Provides a set of extensions methods for <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the type is subclass of the specified raw generic type (e.g. <c>typeof(List&lt;&gt;)</c>).
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="genericType">Type of the generic.</param>
        /// <returns>
        ///   <c>true</c> if the type is a subclass of the specified raw generic type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSubclassOfRawGeneric(this Type type, Type genericType) =>
            type.GetDepthOfInheritanceOfRawGeneric(genericType) != null;

        /// <summary>
        /// Gets the depth of inheritance of the specified raw generic type (e.g. <c>typeof(List&lt;&gt;)</c>).
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="genericType">Type of the generic.</param>
        /// <returns>The depth of inheritance or <see langword="null"/>.</returns>
        public static int? GetDepthOfInheritanceOfRawGeneric(this Type type, Type genericType)
        {
            if (genericType == null)
                return null;

            Type typeToCheck = type;
            int depth = 0;

            while (typeToCheck != null && typeToCheck != typeof(object))
            {
                if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType)
                    return depth;

                typeToCheck = typeToCheck.BaseType;
                depth++;
            }

            return null;
        }

        /// <summary>
        /// Determines whether the type implements the specified generic interface type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="genericType">Type of the generic interface.</param>
        /// <returns>
        ///   <c>true</c> if it implements the generic interface type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsImplementGenericInterface(this Type type, Type genericType) =>
            type.GetGenericInterfaceType(genericType) != null;

        /// <summary>
        /// Gets an actual type of the specified generic interface that this type implements.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="genericType">Type of the generic interface.</param>
        /// <returns>The actual generic interface type or <see langword="null"/>.</returns>
        public static Type GetGenericInterfaceType(this Type type, Type genericType) =>
            type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericType);
    }
}
