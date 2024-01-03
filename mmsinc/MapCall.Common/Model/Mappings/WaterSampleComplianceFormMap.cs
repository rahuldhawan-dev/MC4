using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using System;

namespace MapCall.Common.Model.Mappings
{
    public class WaterSampleComplianceFormMap : ClassMap<WaterSampleComplianceForm>
    {
        #region Constructors

        public WaterSampleComplianceFormMap()
        {
            Id(x => x.Id);

            Map(x => x.DateCertified).Not.Nullable();
            Map(x => x.CertifiedMonth).Not.Nullable();
            Map(x => x.CertifiedYear).Not.Nullable();
            Map(x => x.NoteText).Length(int.MaxValue).Nullable(); // It's an ntext field
            Map(x => x.BactiSamplesReason).Length(Int32.MaxValue).Nullable();
            Map(x => x.CentralLabSamplesReason).Length(Int32.MaxValue).Nullable();
            Map(x => x.ContractedLabsSamplesReason).Length(Int32.MaxValue).Nullable();
            Map(x => x.ChlorineResidualsReason).Length(Int32.MaxValue).Nullable();
            Map(x => x.InternalLabSamplesReason).Length(Int32.MaxValue).Nullable();
            Map(x => x.LeadAndCopperSamplesReason).Length(Int32.MaxValue).Nullable();
            Map(x => x.SurfaceWaterPlantSamplesReason).Length(Int32.MaxValue).Nullable();
            Map(x => x.WQPSamplesReason).Length(Int32.MaxValue).Nullable();

            References(x => x.PublicWaterSupply).Not.Nullable();
            References(x => x.CertifiedBy, "CertifiedByUserId").Not.Nullable();
            References(x => x.CentralLabSamplesHaveBeenCollected, "CentralLabSamplesHaveBeenCollectedAnswerId")
               .Nullable();
            References(x => x.ContractedLabsSamplesHaveBeenCollected, "ContractedLabsSamplesHaveBeenCollectedAnswerId")
               .Nullable();
            References(x => x.InternalLabsSamplesHaveBeenCollected, "InternalLabsSamplesHaveBeenCollectedAnswerId")
               .Nullable();
            References(x => x.BactiSamplesHaveBeenCollected, "BactiSamplesHaveBeenCollectedAnswerId").Nullable();
            References(x => x.LeadAndCopperSamplesHaveBeenCollected, "LeadAndCopperSamplesHaveBeenCollectedAnswerId")
               .Nullable();
            References(x => x.WQPSamplesHaveBeenCollected, "WQPSamplesHaveBeenCollectedAnswerId").Nullable();
            References(x => x.SurfaceWaterPlantSamplesHaveBeenCollected,
                "SurfaceWaterPlantSamplesHaveBeenCollectedAnswerId").Nullable();
            References(x => x.ChlorineResidualsHaveBeenCollected, "ChlorineResidualsHaveBeenCollectedAnswerId")
               .Nullable();
            References(x => x.CentralLabSamplesHaveBeenReported, "CentralLabSamplesHaveBeenReportedAnswerId")
               .Nullable();
            References(x => x.ContractedLabsSamplesHaveBeenReported, "ContractedLabsSamplesHaveBeenReportedAnswerId")
               .Nullable();
            References(x => x.InternalLabsSamplesHaveBeenReported, "InternalLabsSamplesHaveBeenReportedAnswerId")
               .Nullable();
            References(x => x.BactiSamplesHaveBeenReported, "BactiSamplesHaveBeenReportedAnswerId").Nullable();
            References(x => x.LeadAndCopperSamplesHaveBeenReported, "LeadAndCopperSamplesHaveBeenReportedAnswerId")
               .Nullable();
            References(x => x.WQPSamplesHaveBeenReported, "WQPSamplesHaveBeenReportedAnswerId").Nullable();
            References(x => x.SurfaceWaterPlantSamplesHaveBeenReported,
                "SurfaceWaterPlantSamplesHaveBeenReportedAnswerId").Nullable();
            References(x => x.ChlorineResidualsHaveBeenReported, "ChlorineResidualsHaveBeenReportedAnswerId")
               .Nullable();

            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
        }

        #endregion
    }
}
