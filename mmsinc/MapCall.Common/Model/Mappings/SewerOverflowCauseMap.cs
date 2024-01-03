using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SewerOverflowCauseMap : EntityLookupMap<SewerOverflowCause>
    {
        public const string TABLE_NAME = "SewerOverflowCauses";

        public SewerOverflowCauseMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
