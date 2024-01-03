using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class NoReadReasonMap : EntityLookupMap<NoReadReason>
    {
        public NoReadReasonMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
