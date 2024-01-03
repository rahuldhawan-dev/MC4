using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class PublicWaterSupplyTypeMap : EntityLookupMap<PublicWaterSupplyType>
    {
        public const string TABLE_NAME = "PublicWaterSuppliesTypes";

        public PublicWaterSupplyTypeMap()
        {
            Table(TABLE_NAME);
        }
    }
}
