using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class MainBreakMaterialRepository : WorkOrdersRepository<MainBreakMaterial>
    {
        private struct Indices
        {
            public const short CASTIRON = 1,
                               CEMENT = 2,
                               GALVANIZED = 3,
                               DICL = 4,
                               PVC = 5,
                               HDPE = 6,
                               LOCKJOINTPSCP = 7,
                               ASBESTOSCEMENT = 8;
        }

        public static MainBreakMaterial CastIron
        {
            get { return RetrieveAndAttach(Indices.CASTIRON); }
        }

        public static MainBreakMaterial Cement
        {
            get { return RetrieveAndAttach(Indices.CEMENT); }
        }

        public static MainBreakMaterial Galvanized
        {
            get { return RetrieveAndAttach(Indices.GALVANIZED); }
        }

        public static MainBreakMaterial DICL
        {
            get { return RetrieveAndAttach(Indices.DICL); }
        }

        public static MainBreakMaterial PVC
        {
            get { return RetrieveAndAttach(Indices.PVC); }
        }

        public static MainBreakMaterial HDPE
        {
            get { return RetrieveAndAttach(Indices.HDPE); }
        }

        public static MainBreakMaterial LockjointPSCP
        {
            get { return RetrieveAndAttach(Indices.LOCKJOINTPSCP); }
        }

        public static MainBreakMaterial AsbestosCement
        {
            get { return RetrieveAndAttach(Indices.ASBESTOSCEMENT); }
        }

        public static MainBreakMaterial RetrieveAndAttach(int index)
        {
            var entity = GetEntity(index);
            if (!DataTable.Contains(entity))
                DataTable.Attach(entity);
            return entity;
        }

        public static IEnumerable<MainBreakMaterial> GetMainBreakMaterials()
        {
            return (from m in DataTable
                    orderby m.Description
                    select m);
        }
    }
}
