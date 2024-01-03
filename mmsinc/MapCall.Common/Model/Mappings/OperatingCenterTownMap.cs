using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class OperatingCenterTownMap : ClassMap<OperatingCenterTown>
    {
        #region Constants

        public const string TABLE_NAME = "OperatingCentersTowns";

        #endregion

        public OperatingCenterTownMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.Town).Not.Nullable();
            References(x => x.MainSAPFunctionalLocation).Nullable();
            References(x => x.SewerMainSAPFunctionalLocation).Nullable();
            References(x => x.DistributionPlanningPlant).Nullable();
            References(x => x.SewerPlanningPlant).Nullable();

            Map(x => x.Abbreviation);
            Map(x => x.MainSAPEquipmentId).Nullable();
            Map(x => x.SewerMainSAPEquipmentId).Nullable();
        }
    }
}
