using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PlanningPlantWasteWaterSystemMap : ClassMap<PlanningPlantWasteWaterSystem>
    {
        #region Constants

        public const string TABLE_NAME = "PlanningPlantsWasteWaterSystems";

        #endregion

        public PlanningPlantWasteWaterSystemMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.PlanningPlant);
            References(x => x.WasteWaterSystem);
        }
    }
}
