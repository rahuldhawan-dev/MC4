using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MeterSupplementalLocationMap : EntityLookupMap<MeterSupplementalLocation>
    {
        public MeterSupplementalLocationMap()
        {
            Map(x => x.SAPCode);
        }
    }
}
