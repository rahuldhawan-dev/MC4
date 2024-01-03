using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantProblemMap : EntityLookupMap<HydrantProblem>
    {
        public HydrantProblemMap()
        {
            Id(x => x.Id, "HydrantProblemId");
        }
    }
}
