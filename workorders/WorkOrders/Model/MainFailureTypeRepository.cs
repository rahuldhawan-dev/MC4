using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class MainFailureTypeRepository : WorkOrdersRepository<MainFailureType>
    {
        private struct Indices
        {
            public const short SPLIT = 1,
                               CIRCULAR = 2,
                               DETERIORATION = 3,
                               PHYSICALDAMAGE = 4,
                               JOINTLEAK = 5,
                               PINHOLE = 6,
                               STRESS = 7;
        }

        public static MainFailureType Split
        {
            get { return RetrieveAndAttach(Indices.SPLIT); }
        }

        public static MainFailureType Circular
        {
            get { return RetrieveAndAttach(Indices.CIRCULAR); }
        }

        public static MainFailureType Deterioration
        {
            get { return RetrieveAndAttach(Indices.DETERIORATION); }
        }

        public static MainFailureType PhysicalDamage
        {
            get { return RetrieveAndAttach(Indices.PHYSICALDAMAGE); }
        }

        public static MainFailureType JointLeak
        {
            get { return RetrieveAndAttach(Indices.JOINTLEAK); }
        }

        public static MainFailureType Pinhole
        {
            get { return RetrieveAndAttach(Indices.PINHOLE); }
        }

        public static MainFailureType Stress
        {
            get { return RetrieveAndAttach(Indices.STRESS); }
        }

        public static MainFailureType RetrieveAndAttach(int index)
        {
            var entity = GetEntity(index);
            if (!DataTable.Contains(entity))
                DataTable.Attach(entity);
            return entity;
        }

        public static IEnumerable<MainFailureType> GetMainFailureTypes()
        {
            return (from m in DataTable
                    orderby m.Description
                    select m);
        }
    }
}
