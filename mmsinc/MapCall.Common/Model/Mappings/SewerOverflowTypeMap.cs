using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SewerOverflowTypeMap : EntityLookupMap<SewerOverflowType>
    {
        public const string TABLE_NAME = "SewerOverflowTypes";

        public SewerOverflowTypeMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
