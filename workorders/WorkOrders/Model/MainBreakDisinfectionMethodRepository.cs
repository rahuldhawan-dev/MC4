using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class MainBreakDisinfectionMethodRepository : WorkOrdersRepository<MainBreakDisinfectionMethod>
    {
        private struct Indices
        {
            public const short DISINFECTIONOFFITTINGSPIPE = 1,
                               CHLORINATION = 2;
        }

        public static MainBreakDisinfectionMethod DisinfectionOfFittingsPipe
        {
            get { return RetrieveAndAttach(Indices.DISINFECTIONOFFITTINGSPIPE); }
        }

        public static MainBreakDisinfectionMethod Chlorination
        {
            get { return RetrieveAndAttach(Indices.CHLORINATION); }
        }

        
        public static MainBreakDisinfectionMethod RetrieveAndAttach(int index)
        {
            var entity = GetEntity(index);
            if (!DataTable.Contains(entity))
                DataTable.Attach(entity);
            return entity;
        }

        public static IEnumerable<MainBreakDisinfectionMethod> GetMainBreakDisinfectionMethods()
        {
            return (from m in DataTable
                    orderby m.Description
                    select m);
        }
    }
}
