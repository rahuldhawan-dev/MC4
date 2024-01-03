using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class MainBreakFlushMethodRepository : WorkOrdersRepository<MainBreakFlushMethod>
    {
        private struct Indices
        {
            public const short HYDRANT = 1,
                               BLOWOFF = 2,
                               SERVICE = 3;
        }

        public static MainBreakFlushMethod Hydrant
        {
            get { return RetrieveAndAttach(Indices.HYDRANT); }
        }

        public static MainBreakFlushMethod Blowoff
        {
            get { return RetrieveAndAttach(Indices.BLOWOFF); }
        }

        public static MainBreakFlushMethod Service
        {
            get { return RetrieveAndAttach(Indices.SERVICE); }
        }

        public static MainBreakFlushMethod RetrieveAndAttach(int index)
        {
            var entity = GetEntity(index);
            if (!DataTable.Contains(entity))
                DataTable.Attach(entity);
            return entity;
        }

        public static IEnumerable<MainBreakFlushMethod> GetMainBreakFlushMethods()
        {
            return (from m in DataTable
                    orderby m.Description
                    select m);
        }
    }
}
