using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MerchantTotalFeeMap : ClassMap<MerchantTotalFee>
    {
        public MerchantTotalFeeMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Fee);
            Map(x => x.IsCurrent);
        }
    }
}
