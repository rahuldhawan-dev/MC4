using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RestorationTypeCostMap : ClassMap<RestorationTypeCost>
    {
        #region Constructors

        public RestorationTypeCostMap()
        {
            Id(x => x.Id, "RestorationTypeCostID");

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.RestorationType).Not.Nullable();

            Map(x => x.Cost).Not.Nullable();
            Map(x => x.FinalCost).Not.Nullable();
        }

        #endregion
    }
}
