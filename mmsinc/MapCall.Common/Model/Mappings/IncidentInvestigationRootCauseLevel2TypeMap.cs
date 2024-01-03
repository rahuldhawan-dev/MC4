using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class IncidentInvestigationRootCauseLevel2TypeMap : EntityLookupMap<IncidentInvestigationRootCauseLevel2Type>
    {
        public IncidentInvestigationRootCauseLevel2TypeMap()
        {
            References(x => x.IncidentInvestigationRootCauseLevel1Type).Not.Nullable();
        }
    }
}
