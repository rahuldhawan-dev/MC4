using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class BacterialWaterSampleMap : ClassMap<BacterialWaterSample>
    {
        #region Constructors

        public BacterialWaterSampleMap()
        {
            Id(x => x.Id);

            #region Table Properties

            References(x => x.SampleSite).Nullable();
            References(x => x.BacterialSampleType).Nullable();
            References(x => x.SampleCoordinate, "CoordinateId").Nullable();
            References(x => x.EstimatingProject).Nullable();
            References(x => x.OriginalBacterialWaterSample).Nullable();
            References(x => x.SampleTown, "TownId").Nullable();
            References(x => x.ColiformConfirmMethod).Nullable();
            References(x => x.EColiConfirmMethod).Nullable();
            References(x => x.HPCConfirmMethod).Nullable();
            References(x => x.RepeatLocationType).Nullable();
            References(x => x.CollectedBy, "CollectedByUserId").Nullable();
            References(x => x.LIMSStatus).Not.Nullable();

            Map(x => x.SampleCollectionDTM).Column("Sample_Date").Nullable();
            Map(x => x.ReceivedByLabDTM).Column("DTM_Received_Lab").Nullable();
            Map(x => x.IsInvalid).Not.Nullable();
            References(x => x.ReasonForInvalidation).Nullable();
            Map(x => x.Collector)
               .Column("Analysis_Performed_By")
               .Nullable()
               .Length(BacterialWaterSample.StringLengths.COLLECTOR);
            Map(x => x.Location)
               .Nullable()
               .Length(BacterialWaterSample.StringLengths.LOCATION);
            Map(x => x.SampleNumber)
               .Nullable()
               .Length(BacterialWaterSample.StringLengths.SAMPLE_NUMBER);
            Map(x => x.Cl2Free).Column("Cl2_Free").Nullable();
            Map(x => x.Cl2Total).Column("Cl2_Total").Nullable();
            Map(x => x.FlushTimeMinutes).Scale(2).Precision(6).Nullable();
            Map(x => x.Nitrite).Nullable();
            Map(x => x.Nitrate).Nullable();
            Map(x => x.FinalHPC).Nullable();
            Map(x => x.Monochloramine).Nullable();
            Map(x => x.FreeAmmonia).Nullable();
            Map(x => x.Ph).Nullable();
            Map(x => x.Temperature).Column("Temp_Celsius").Nullable();
            Map(x => x.Iron).Column("Value_Fe").Nullable();
            Map(x => x.Manganese).Column("Value_Mn").Nullable();
            Map(x => x.Turbidity).Column("Value_Turb").Nullable();
            Map(x => x.OrthophosphateAsP).Nullable();
            Map(x => x.OrthophosphateAsPO4).Nullable();
            Map(x => x.Conductivity).Column("Value_Conductivity").Nullable();
            Map(x => x.Alkalinity).Column("ATP").Nullable();
            Map(x => x.NonSheenColonyCount).Column("Non_Sheen_Colony_Count").Nullable();
            References(x => x.NonSheenColonyCountOperator, "Non_Sheen_Colony_Count_Operator").Nullable();
            Map(x => x.SheenColonyCount).Column("Sheen_Colony_Count").Nullable();
            References(x => x.SheenColonyCountOperator, "Sheen_Colony_Count_Operator").Nullable();
            Map(x => x.ColiformConfirm).Column("Coliform_Confirm").Not.Nullable();
            Map(x => x.EColiConfirm).Column("E_Coli_Confirm").Nullable();
            Map(x => x.DataEntered).Column("DTM_DataEntered").Nullable();
            Map(x => x.SAPWorkOrderId)
               .Nullable()
               .Length(BacterialWaterSample.StringLengths.SAP_WORK_ORDER_ID);
            Map(x => x.Address)
               .Nullable()
               .Length(BacterialWaterSample.StringLengths.ADDRESS);
            Map(x => x.ComplianceSample).Not.Nullable();
            Map(x => x.ColiformSetupDTM).Nullable();
            Map(x => x.ColiformReadDTM).Nullable();
            References(x => x.ColiformSetupAnalyst).Nullable();
            References(x => x.ColiformReadAnalyst).Nullable();
            Map(x => x.HPCSetupDTM).Nullable();
            Map(x => x.HPCReadDTM).Nullable();
            References(x => x.HPCSetupAnalyst).Nullable();
            References(x => x.HPCReadAnalyst).Nullable();
            Map(x => x.IsSpreader).Not.Nullable();
            Map(x => x.LIMSResponse).Length(int.MaxValue).Nullable();
            Map(x => x.SubmittedToLIMSAt).Nullable();

            #endregion

            #region Formulas

            // case bacterial sample type == 1 compliance then sample site's 
            // case bacterial sample type == 2 process control then town's first operating center
            // case bacterial sample type == 3 New Main then Estimating Project's 
            // case bacterial sample type == 4 Recheck then original sample's sample site 
            // case bacterial sample type == 5 System Repair then the town's first operating center
            // TODO: THIS IS FRAGILE AS ALL HELL. CLEAN UP AS MORE OF THESE RELATED MODELS GET COMPLETED
            References(x => x.OperatingCenter).Nullable();

            References(x => x.Town)
               .Formula("(CASE " +
                        "WHEN (BacterialSampleTypeId = 1) THEN (select ss.TownID from SampleSites SS where SS.Id = SampleSiteID) " +
                        "WHEN (BacterialSampleTypeId in (2,5)) THEN TownID " +
                        "WHEN (BacterialSampleTypeId = 3) THEN (Select EP.MunicipalityId from EstimatingProjects EP where EP.Id = EstimatingProjectId) " +
                        "WHEN (BacterialSampleTypeId = 4) THEN (select SS.TownId from BacterialWaterSamples OS join SampleSites SS on SS.Id = os.SampleSiteID WHERE OS.Id = OriginalBacterialWaterSampleId) " +
                        "END)")
               .ReadOnly();

            Map(x => x.Month).Formula("month(Sample_Date)").ReadOnly();
            Map(x => x.Year).Formula("year(Sample_Date)").ReadOnly();

            #endregion

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();

            // POSSIBLE TODO: I feel like this should actually be a formula. This collection isn't meant to be modified directly.
            HasMany(x => x.LinkedBacterialWaterSamples)
               .KeyColumn("OriginalBacterialWaterSampleId").Inverse().Cascade.None();
        }

        #endregion
    }
}
