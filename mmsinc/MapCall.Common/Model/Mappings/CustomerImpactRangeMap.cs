using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class CustomerImpactRangeMap : EntityLookupMap<CustomerImpactRange>
    {
        #region Constructors

        public CustomerImpactRangeMap()
        {
            Id(x => x.Id).Column("CustomerImpactRangeID").GeneratedBy.Assigned();
        }

        #endregion
    }
}
