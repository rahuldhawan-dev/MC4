using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class CauseOfBlockageMap : EntityLookupMap<CauseOfBlockage>
    {
        public const string TABLE_NAME = "CausesOfBlockages";

        public CauseOfBlockageMap()
        {
            Table(TABLE_NAME);
        }
    }
}
