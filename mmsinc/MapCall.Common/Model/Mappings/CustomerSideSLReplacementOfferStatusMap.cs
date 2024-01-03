using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations._2016;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class CustomerSideSLReplacementOfferStatusMap : EntityLookupMap<CustomerSideSLReplacementOfferStatus>
    {
        public const string TABLE_NAME = AddColumnsToServicesForBug3096.TableNames.OFFER_STATUS;

        public CustomerSideSLReplacementOfferStatusMap()
        {
            Table(TABLE_NAME);
        }
    }
}
