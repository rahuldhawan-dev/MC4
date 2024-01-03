using System;
using StructureMap;

namespace MMSINC.ClassExtensions
{
    public static class ObjectFactoryExtensions
    {
        /// <summary>
        /// Disposes of the given container and reinitializes ObjectFactory to use a new
        /// container if its current Container is this container. Container.
        /// </summary>
        public static void Reset(this IContainer container)
        {
            // Dispose ejects all the current container's instances.
            container.Dispose();
        }
    }
}
