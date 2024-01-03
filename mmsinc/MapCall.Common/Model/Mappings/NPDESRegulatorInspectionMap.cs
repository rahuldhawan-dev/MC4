using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class NpdesRegulatorInspectionMap : ClassMap<NpdesRegulatorInspection>
    {
        #region Constants

        public const string TABLE_NAME = "NpdesRegulatorInspections";

        #endregion

        #region Constructors

        public NpdesRegulatorInspectionMap()
        {
            LazyLoad();

            Id(x => x.Id);
            Map(x => x.ArrivalDateTime).Not.Nullable();
            Map(x => x.DepartureDateTime).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.HasInfiltration).Not.Nullable();
            Map(x => x.IsDischargePresent).Not.Nullable();
            Map(x => x.RainfallEstimate).Nullable();
            Map(x => x.DischargeFlow).Nullable();
            Map(x => x.DischargeDuration).Nullable();
            Map(x => x.IsPlumePresent).Not.Nullable();
            Map(x => x.IsErosionPresent).Not.Nullable();
            Map(x => x.IsSolidFloatPresent).Not.Nullable();
            Map(x => x.IsAdditionalEquipmentNeeded).Not.Nullable();
            Map(x => x.HasSamplesBeenTaken).Not.Nullable();
            Map(x => x.SampleLocation).Nullable();
            Map(x => x.HasFlowMeterMaintenanceBeenPerformed).Not.Nullable();
            Map(x => x.HasDownloadedFlowMeterData).Not.Nullable();
            Map(x => x.HasCalibratedFlowMeter).Not.Nullable();
            Map(x => x.HasInstalledFlowMeter).Not.Nullable();
            Map(x => x.HasRemovedFlowMeter).Not.Nullable();
            Map(x => x.HasFlowMeterBeenMaintainedOther).Not.Nullable();
            Map(x => x.Remarks).Nullable();

            References(x => x.SewerOpening).Not.Nullable();
            References(x => x.WeatherCondition, "WeatherConditionId").Nullable();
            References(x => x.OutfallCondition, "OutfallConditionId").Nullable();
            References(x => x.BlockCondition, "BlockConditionId").Nullable();
            References(x => x.NpdesRegulatorInspectionType, "InspectionTypeId").Nullable();
            References(x => x.GateStatusAnswerType, "GateStatusAnswerTypeId").Nullable();
            References(x => x.InspectedBy).Nullable();
            References(x => x.DischargeCause, "DischargeCauseId").Nullable();
            References(x => x.DischargeWeatherRelatedType, "DischargeWeatherRelatedTypeId").Nullable();

            HasMany(x => x.Notes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
        }

        #endregion
    }
}
