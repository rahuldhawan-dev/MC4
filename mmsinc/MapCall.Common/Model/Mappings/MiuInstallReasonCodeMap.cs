using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MiuInstallReasonCodeMap : EntityLookupMap<MiuInstallReasonCode>
    {
        public MiuInstallReasonCodeMap()
        {
            Map(x => x.SAPCode);
        }
    }
}
