using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class OperatingCenterPublicWaterSupplyMap : ClassMap<OperatingCenterPublicWaterSupply>
    {
        #region Constants

        public const string TABLE_NAME = "OperatingCentersPublicWaterSupplies";

        #endregion

        public OperatingCenterPublicWaterSupplyMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();

            // TODO: Neither of these should be nullable.
            References(x => x.OperatingCenter).Nullable();
            References(x => x.PublicWaterSupply).Nullable();
        }
    }
}
