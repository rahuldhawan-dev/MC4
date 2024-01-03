using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SewerOverflowAreaMap : EntityLookupMap<SewerOverflowArea>
    {
        public const string TABLE_NAME = "AreaCleanedUpTo";

        public SewerOverflowAreaMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "AreaCleanedUpToID").Not.Nullable();
        }
    }
}
