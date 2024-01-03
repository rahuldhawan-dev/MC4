using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SAPWorkOrderStepMap : EntityLookupMap<SAPWorkOrderStep>
    {
        public SAPWorkOrderStepMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
