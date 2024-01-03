using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class CovidQuarantineStatusMap : EntityLookupMap<CovidQuarantineStatus>
    {
        public const string TABLE_NAME = "CovidQuarantineStatuses";

        public CovidQuarantineStatusMap()
        {
            Table(TABLE_NAME);
        }
    }
}
