using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MMSINC.Data.NHibernate
{
    public interface IDynamicMapTypeProvider
    {
        IEnumerable<Type> GetMapTypesForAssembly(Assembly assembly);
    }
}
