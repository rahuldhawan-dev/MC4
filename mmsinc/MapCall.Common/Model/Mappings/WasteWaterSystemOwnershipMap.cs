using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class WasteWaterSystemOwnershipMap : EntityLookupMap<WasteWaterSystemOwnership>
    {
        public const string TABLE_NAME = "WasteWaterSystemOwnerships";

        public WasteWaterSystemOwnershipMap()
        {
            Table(TABLE_NAME);
        }
    }
}
