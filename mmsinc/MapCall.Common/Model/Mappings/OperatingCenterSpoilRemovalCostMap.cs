using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    #region Constructors

    public class OperatingCenterSpoilRemovalCostMap : ClassMap<OperatingCenterSpoilRemovalCost>
    {
        public OperatingCenterSpoilRemovalCostMap()
        {
            Id(x => x.Id, "OperatingCenterSpoilRemovalCostID").GeneratedBy.Identity();

            Map(x => x.Cost).Not.Nullable();

            References(x => x.OperatingCenter);
        }
    }

    #endregion
}