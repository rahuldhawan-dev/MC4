using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PublicWaterSupplyLicensedOperatorMap : ClassMap<PublicWaterSupplyLicensedOperator>
    {
        public PublicWaterSupplyLicensedOperatorMap()
        {
            Id(x => x.Id);

            References(x => x.PublicWaterSupply).Not.Nullable();
            References(x => x.LicensedOperator, "OperatorLicenseId").Not.Nullable();
        }
    }
}
