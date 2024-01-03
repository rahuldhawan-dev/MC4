using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PublicWaterSupplyCustomerDataMap : ClassMap<PublicWaterSupplyCustomerData>
    {
        public const string TABLE_NAME = "tblPWSID_Customer_Data";

        public PublicWaterSupplyCustomerDataMap()
        {
            Table(TABLE_NAME);
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("CustomerDataID");

            References(x => x.PWSID).Column("PWSID").Nullable();

            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.CreatedBy).Nullable().Length(50);
            Map(x => x.NumberCustomers).Column("Number_Customers").Nullable().Precision(10);
            Map(x => x.PopulationServed).Column("Population_Served").Nullable().Precision(10);
            Map(x => x.Notes).Nullable().Length(255);
            Map(x => x.UpdatedAt).Not.Nullable();
        }
    }
}
