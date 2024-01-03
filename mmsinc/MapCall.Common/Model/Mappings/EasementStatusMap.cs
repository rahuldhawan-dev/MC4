using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EasementStatusMap : EntityLookupMap<EasementStatus>
    {
        public const string TABLE_NAME = "EasementStatuses";

        public EasementStatusMap()
        {
            Table(TABLE_NAME);
        }
    }
}
