using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PlanningPlantPublicWaterSupplyMap : ClassMap<PlanningPlantPublicWaterSupply>
    {
        #region Constants

        public const string TABLE_NAME = "PlanningPlantsPublicWaterSupplies";

        #endregion

        public PlanningPlantPublicWaterSupplyMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.PlanningPlant);
            References(x => x.PublicWaterSupply);
        }
    }
}
