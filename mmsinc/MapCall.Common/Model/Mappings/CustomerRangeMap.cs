using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class CustomerRangeMap : ClassMap<CustomerRange>
    {
        #region Constructors

        public CustomerRangeMap()
        {
            Id(x => x.Id, "CustomerRangeID");

            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
