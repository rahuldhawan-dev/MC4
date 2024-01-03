using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class PlanningPlantMap : EntityLookupMap<PlanningPlant>
    {
        public PlanningPlantMap()
        {
            Id(x => x.Id);

            Map(x => x.Code).Length(PlanningPlant.CODE_LENGTH).Not.Nullable();

            References(x => x.OperatingCenter).Nullable();
        }
    }
}
