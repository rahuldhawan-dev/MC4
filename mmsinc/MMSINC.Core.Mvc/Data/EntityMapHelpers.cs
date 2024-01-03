using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMSINC.ClassExtensions.ReflectionExtensions;

namespace MMSINC.Data
{
    internal static class EntityMapHelpers
    {
        public static bool IsPropertyASupportedGenericCollectionType(Type propertyType)
        {
            return propertyType.IsSubclassOfRawGeneric(typeof(IList<>)) ||
                   propertyType.IsSubclassOfRawGeneric(typeof(ISet<>));
        }
    }
}
