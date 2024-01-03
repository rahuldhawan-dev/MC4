using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class HighCostFactorMap : ClassMap<HighCostFactor>
    {
        #region Constructors

        public HighCostFactorMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
