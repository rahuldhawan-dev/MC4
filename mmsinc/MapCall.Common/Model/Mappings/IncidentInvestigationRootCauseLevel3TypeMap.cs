using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class IncidentInvestigationRootCauseLevel3TypeMap : EntityLookupMap<IncidentInvestigationRootCauseLevel3Type>
    {
        public IncidentInvestigationRootCauseLevel3TypeMap()
        {
            References(x => x.IncidentInvestigationRootCauseLevel2Type).Not.Nullable();
        }
    }
}
