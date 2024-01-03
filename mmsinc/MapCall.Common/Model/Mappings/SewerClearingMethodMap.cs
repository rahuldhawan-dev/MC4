using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SewerClearingMethodMap : EntityLookupMap<SewerClearingMethod>
    {
        public SewerClearingMethodMap()
        {
            Id(x => x.Id, "SewerClearingMethodID").Not.Nullable();
        }
    }
}
