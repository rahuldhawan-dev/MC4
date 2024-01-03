using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SampleIdMatrixMap : ClassMap<SampleIdMatrix>
    {
        public const string TABLE_NAME = "SampleIDMatrices";

        public SampleIdMatrixMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id);

            References(x => x.SampleSite).Not.Nullable();
            References(x => x.WaterConstituent);

            Map(x => x.RoutineSample).Not.Nullable();
            Map(x => x.ProcessStage).Nullable();
            Map(x => x.ProcessStageSequence).Nullable();
            Map(x => x.ParameterSequence).Nullable();
            Map(x => x.Parameter).Nullable();
            Map(x => x.SamplePurpose).Nullable();
            Map(x => x.ProcessReasonForSample).Nullable();
            Map(x => x.PerformedBy).Nullable();
            Map(x => x.Frequency, "WeeklyFrequency").Nullable();
            Map(x => x.DataStorageLocation).Nullable();
            Map(x => x.MethodInstrumentLaboratory).Nullable();
            Map(x => x.TatBellvilleLabHrs).Nullable();
            Map(x => x.BellevilleSampleId).Nullable();
            Map(x => x.InterferenceBy).Nullable();
            Map(x => x.DataStorageLocationOnLineInstrument).Nullable();
            Map(x => x.IHistorianSignalIdOnLineInstrument).Nullable();
            Map(x => x.ComplianceReq).Nullable();
            Map(x => x.ProcessTarget).Nullable();
            Map(x => x.TriggerPhase1).Nullable();
            Map(x => x.ActionPhase1).Nullable();
            Map(x => x.TriggerPhase2).Nullable();
            Map(x => x.ActionPhase2).Nullable();
            Map(x => x.Comment).Nullable();
            Map(x => x.ScadaNotes).Nullable();

            HasMany(x => x.WaterSamples).KeyColumn("SampleMatrixId").LazyLoad().ReadOnly();

            HasMany(x => x.SampleIdMatrixDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.SampleIdMatrixNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
