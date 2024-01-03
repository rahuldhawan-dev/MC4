using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MainCrossingInspectionMap : ClassMap<MainCrossingInspection>
    {
        public MainCrossingInspectionMap()
        {
            Id(x => x.Id);

            Map(x => x.InspectedOn)
               .Not.Nullable();
            Map(x => x.Comments)
               .CustomSqlType("text")
               .Nullable();
            Map(x => x.AdjacentFacilityHasBankErosion).Not.Nullable();
            Map(x => x.AdjacentFacilityHasBridgeDamage).Not.Nullable();
            Map(x => x.AdjacentFacilityHasPavementFailure).Not.Nullable();
            Map(x => x.AdjacentFacilityOverheadPowerLinesAreDown).Not.Nullable();
            Map(x => x.AdjacentFacilityHasPropertyDamage).Not.Nullable();
            Map(x => x.EnvironmentIsInHazardousLocation).Not.Nullable();
            Map(x => x.EnvironmentHasDebrisBuildUp).Not.Nullable();
            Map(x => x.EnvironmentIsSubmergedInWater).Not.Nullable();
            Map(x => x.EnvironmentIsExposedToVehicleImpact).Not.Nullable();
            Map(x => x.EnvironmentIsNotSecuredFromPublic).Not.Nullable();
            Map(x => x.EnvironmentIsSusceptibleToStormDamage).Not.Nullable();
            Map(x => x.JointsAreLeaking).Not.Nullable();
            Map(x => x.JointsFailedSeparated).Not.Nullable();
            Map(x => x.JointsRestraintDamaged).Not.Nullable();
            Map(x => x.JointsBondStrapsDamaged).Not.Nullable();
            Map(x => x.PipeIsInService).Not.Nullable();
            Map(x => x.PipeHasExcessiveCorrosion).Not.Nullable();
            Map(x => x.PipeHasDelaminatedSteel).Not.Nullable();
            Map(x => x.PipeIsDamaged).Not.Nullable();
            Map(x => x.PipeHasCracks).Not.Nullable();
            Map(x => x.PipeHasConcreteSpools).Not.Nullable();
            Map(x => x.PipeLacksInsulation).Not.Nullable();
            Map(x => x.SupportsHaveDeficientSupport).Not.Nullable();
            Map(x => x.SupportsAreDamaged).Not.Nullable();
            Map(x => x.SupportsHaveCorrosion).Not.Nullable();

            References(x => x.AssessmentRating)
               .Not.Nullable();
            References(x => x.CreatedBy)
               .Not.Nullable();
            References(x => x.InspectedBy)
               .Not.Nullable();
            References(x => x.MainCrossing)
               .Not.Nullable();

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
