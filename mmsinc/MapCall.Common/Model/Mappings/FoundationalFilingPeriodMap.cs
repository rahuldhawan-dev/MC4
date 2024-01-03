using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FoundationalFilingPeriodMap : ClassMap<FoundationalFilingPeriod>
    {
        #region Constructors

        public FoundationalFilingPeriodMap()
        {
            Id(x => x.Id, "FoundationalFilingPeriodID");

            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
