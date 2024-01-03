using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class LIMSStatusMap : EntityLookupMap<LIMSStatus>
    {
        public LIMSStatusMap()
        {
            Table("LIMSStatuses");
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
