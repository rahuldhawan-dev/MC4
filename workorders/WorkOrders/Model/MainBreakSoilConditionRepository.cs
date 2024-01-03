using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class MainBreakSoilConditionRepository : WorkOrdersRepository<MainBreakSoilCondition>
    {
        private struct Indices
        {
            public const short GRAVEL = 1,
                               SANDY = 2,
                               ROCKY = 3,
                               CLAY = 4,
                               LOAM = 5,
                               SILTY = 6;
        }

        public static MainBreakSoilCondition Gravel
        {
            get { return RetrieveAndAttach(Indices.GRAVEL); }
        }

        public static MainBreakSoilCondition Sandy
        {
            get { return RetrieveAndAttach(Indices.SANDY); }
        }
        public static MainBreakSoilCondition Rocky
        {
            get { return RetrieveAndAttach(Indices.ROCKY); }
        }
        public static MainBreakSoilCondition Clay
        {
            get { return RetrieveAndAttach(Indices.CLAY); }
        }
        public static MainBreakSoilCondition Loam
        {
            get { return RetrieveAndAttach(Indices.LOAM); }
        }
        public static MainBreakSoilCondition Silty
        {
            get { return RetrieveAndAttach(Indices.SILTY); }
        }

        public static MainBreakSoilCondition RetrieveAndAttach(int index)
        {
            var entity = GetEntity(index);
            if (!DataTable.Contains(entity))
                DataTable.Attach(entity);
            return entity;
        }

        public static IEnumerable<MainBreakSoilCondition> GetMainBreakSoilConditions()
        {
            return (from m in DataTable
                    orderby m.Description
                    select m);
        }
    }
}
