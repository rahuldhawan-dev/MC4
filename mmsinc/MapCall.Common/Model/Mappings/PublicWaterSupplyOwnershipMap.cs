using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class PublicWaterSupplyOwnershipMap : EntityLookupMap<PublicWaterSupplyOwnership>
    {
        public const string TABLE_NAME = "PublicWaterSuppliesOwnerships";

        public PublicWaterSupplyOwnershipMap()
        {
            Table(TABLE_NAME);
        }
    }
}
