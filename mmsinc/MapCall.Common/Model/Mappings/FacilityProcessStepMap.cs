using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityProcessStepMap : ClassMap<FacilityProcessStep>
    {
        #region Constructors

        public FacilityProcessStepMap()
        {
            Id(x => x.Id);

            Map(x => x.ElevationInFeet).Not.Nullable();
            Map(x => x.NormalRangeMin).Not.Nullable().Precision(18).Scale(6);
            Map(x => x.NormalRangeMax).Not.Nullable().Precision(18).Scale(6);
            Map(x => x.Description).Length(FacilityProcessStep.MAX_DESCRIPTION_LENGTH).Not.Nullable();
            Map(x => x.StepNumber).Not.Nullable();
            Map(x => x.ProcessTarget).Not.Nullable();
            Map(x => x.ContingencyOperation).Nullable();
            Map(x => x.LossOfCommunicationPowerImpact).Nullable();

            References(x => x.Equipment)
               .Nullable();
            References(x => x.FacilityProcess)
               .Not.Nullable();
            References(x => x.FacilityProcessStepSubProcess)
               .Not.Nullable();
            References(x => x.UnitOfMeasure)
               .Not.Nullable();

            HasMany(x => x.Triggers).KeyColumn("FacilityProcessStepId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.FacilityProcessStepDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.FacilityProcessStepNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }

        #endregion
    }
}
