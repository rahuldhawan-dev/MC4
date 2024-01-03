using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EnvironmentalPermitFeePaymentMethodMap : EntityLookupMap<EnvironmentalPermitFeePaymentMethod>
    {
        public EnvironmentalPermitFeePaymentMethodMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
