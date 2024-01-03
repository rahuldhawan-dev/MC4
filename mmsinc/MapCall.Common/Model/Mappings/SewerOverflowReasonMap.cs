using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SewerOverflowReasonMap : EntityLookupMap<SewerOverflowReason>
    {
        public const string TABLE_NAME = "SewerOverflowReasons";

        public SewerOverflowReasonMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "Id").Not.Nullable();
        }
    }
}
