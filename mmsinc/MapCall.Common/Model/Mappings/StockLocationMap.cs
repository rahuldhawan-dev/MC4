using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class StockLocationMap : EntityLookupMap<StockLocation>
    {
        protected override string IdName => "StockLocationID";

        protected override bool IsDescriptionUnique => false;

        public StockLocationMap()
        {
            Map(x => x.SAPStockLocation).Nullable().Length(StockLocation.StringLengths.SAP_STOCK_LOCATION);
            Map(x => x.IsActive).Not.Nullable();

            References(x => x.OperatingCenter).Not.Nullable();
        }
    }
}
