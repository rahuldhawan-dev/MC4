using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20191017085812440), Tags("Production")]
    public class MC1522SeparateOutArcFlashDataFromFacilities : Migration
    {
        public const string FACILITIES = "tblFacilities",
                            ARC_FLASH_STUDIES = "ArcFlashStudies",
                            CREATE_VIEW = @"CREATE VIEW MostRecentArcFlashStudies
AS
WITH MostFutureArcFlashStudies AS (
	SELECT
		MAX(DateLabelsApplied) AS [DateLabelsApplied]
		,FacilityId
        ,(CASE WHEN (MAX(DateLabelsApplied) is not null AND (datediff(day, GetDate(), dateadd(year, 5, max(DateLabelsApplied)))) BETWEEN 0 AND 365) THEN 1 ELSE 0 END) as ExpiringWithinAYear
	FROM
		ArcFlashStudies
	GROUP BY
		FacilityId
)
select
	mf.DateLabelsApplied
	,mf.FacilityId
	,afs.Id as [ArcFlashStudyId]
    ,mf.ExpiringWithinAYear
from
	MostFutureArcFlashStudies mf
inner join
	ArcFlashStudies afs
on
	afs.FacilityId = mf.FacilityId and afs.DateLabelsApplied = mf.DateLabelsApplied
",
                            CREATE_VIEW_SQLITE = @"CREATE VIEW MostRecentArcFlashStudies
AS
WITH MostFutureArcFlashStudies AS (
	SELECT
		MAX(DateLabelsApplied) AS [DateLabelsApplied]
		,FacilityId
        ,(CASE WHEN (max(DateLabelsApplied) is not null AND (JulianDay(date(max(DateLabelsApplied),'+5 years')) - JulianDay(date('now')) BETWEEN 0 AND 365)) THEN 1 ELSE 0 END) as ExpiringWithinAYear
	FROM
		ArcFlashStudies
	GROUP BY
		FacilityId
)
select
	mf.DateLabelsApplied
	,mf.FacilityId
	,afs.Id as [ArcFlashStudyId]
    ,mf.ExpiringWithinAYear
from
	MostFutureArcFlashStudies mf
inner join
	ArcFlashStudies afs
on
	afs.FacilityId = mf.FacilityId and afs.DateLabelsApplied = mf.DateLabelsApplied
",
                            DROP_VIEW = "DROP VIEW MostRecentArcFlashStudies";

        public override void Up()
        {
            // Create a new table to store the arcflash data
            Create.Table(ARC_FLASH_STUDIES)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("FacilityId", FACILITIES, "RecordID")
                  .WithForeignKeyColumn("ArcFlashStatusId", "ArcFlashStatuses")
                  .WithColumn("Priority").AsAnsiString(25).Nullable()
                  .WithForeignKeyColumn("FacilitySizeId", "FacilitySizes").Nullable()
                  .WithColumn("PowerCompanyDataReceived").AsBoolean().NotNullable().WithDefaultValue(0)
                  .WithColumn("UtilityCompanyDataReceivedDate").AsDateTime().Nullable()
                  .WithColumn("AFHAAnalysisPerformed").AsBoolean().Nullable()
                  .WithForeignKeyColumn("TypeOfArcFlashAnalysisId", "ArcFlashAnalysisTypes")
                  .WithForeignKeyColumn("ArcFlashLabelTypeId", "ArcFlashLabelTypes")
                  .WithColumn("UtilityAccountNumber").AsString(50).Nullable()
                  .WithColumn("UtilityMeterNumber").AsString(25).Nullable()
                  .WithColumn("UtilityPoleNumber").AsString(25).Nullable()
                  .WithColumn("PrimaryVoltageKV").AsDecimal(18, 2).Nullable()
                  .WithForeignKeyColumn("VoltageId", "Voltages").Nullable()
                  .WithForeignKeyColumn("PowerPhaseId", "PowerPhases").Nullable()
                  .WithForeignKeyColumn("UtilityTransformerKVARatingId", "UtilityTransformerKVARatings").Nullable()
                  .WithColumn("TransformerKVAFieldConfirmed").AsBoolean().NotNullable().WithDefaultValue(0)
                  .WithColumn("TransformerResistancePercentage").AsDecimal(18, 2).Nullable()
                  .WithColumn("TransformerReactancePercentage").AsDecimal(18, 2).Nullable()
                  .WithForeignKeyColumn("FacilityTransformerWiringTypeId", "FacilityTransformerWiringTypes").Nullable()
                  .WithColumn("PrimaryFuseSize").AsDecimal(18, 2).Nullable()
                  .WithColumn("PrimaryFuseType").AsString(50).Nullable()
                  .WithColumn("PrimaryFuseManufacturer").AsString(100).Nullable()
                  .WithColumn("LineToLineFaultAmps").AsDecimal(18, 2).Nullable()
                  .WithColumn("LineToLineNeutralFaultAmps").AsDecimal(18, 2).Nullable()
                  .WithColumn("ArcFlashNotes").AsText().Nullable()
                  .WithColumn("DateLabelsApplied").AsDateTime().Nullable()
                  .WithColumn("ArcFlashContractor").AsAnsiString(50).Nullable()
                  .WithColumn("ArcFlashHazardAnalysisStudyParty").AsString(50).Nullable()
                  .WithColumn("CostToComplete").AsDecimal(18, 2).Nullable()
                  .WithForeignKeyColumn("UtilityCompanyId", "UtilityCompanies").Nullable()
                  .WithColumn("UtilityCompanyOther").AsAnsiString(50).Nullable();

            // copy data from facilities to arcflash table
            Execute.Sql($"INSERT INTO {ARC_FLASH_STUDIES} (" +
                        "FacilityId,ArcFlashStatusId,Priority,FacilitySizeId,PowerCompanyDataReceived,UtilityCompanyDataReceivedDate," +
                        "AFHAAnalysisPerformed,TypeOfArcFlashAnalysisId,ArcFlashLabelTypeId,UtilityAccountNumber,UtilityMeterNumber," +
                        "UtilityPoleNumber,PrimaryVoltageKV,VoltageId,PowerPhaseId,UtilityTransformerKVARatingId,TransformerKVAFieldConfirmed," +
                        "TransformerResistancePercentage,TransformerReactancePercentage,FacilityTransformerWiringTypeId,PrimaryFuseSize," +
                        "PrimaryFuseType,PrimaryFuseManufacturer,LineToLineFaultAmps,LineToLineNeutralFaultAmps,ArcFlashNotes,DateLabelsApplied,ArcFlashContractor," +
                        "ArcFlashHazardAnalysisStudyParty,CostToComplete, UtilityCompanyId, UtilityCompanyOther " +
                        ") SELECT " +
                        "RecordID,ArcFlashStatusId,Priority,FacilitySizeId,PowerCompanyDataReceived,UtilityCompanyDataReceivedDate," +
                        "AFHAAnalysisPerformed,TypeOfArcFlashAnalysisId,ArcFlashLabelTypeId,UtilityAccountNumber,UtilityMeterNumber," +
                        "UtilityPoleNumber,PrimaryVoltageKV,VoltageId,PowerPhaseId,UtilityTransformerKVARatingId,TransformerKVAFieldConfirmed," +
                        "TransformerResistancePercentage,TransformerReactancePercentage,FacilityTransformerWiringTypeId,PrimaryFuseSize," +
                        "PrimaryFuseType,PrimaryFuseManufacturer,LineToLineFaultAmps,LineToLineNeutralFaultAmps,ArcFlashNotes,DateLabelsApplied,ArcFlashContractor," +
                        "ArcFlashHazardAnalysisStudyParty,CostToComplete, UtilityCompanyId, UtilityCompanyOther " +
                        $"FROM {FACILITIES} " +
                        "WHERE ArcFlashStatusId IS NOT NULL");

            // remove arcflash fields from facility
            Delete.ForeignKeyColumn(FACILITIES, "ArcFlashStatusId", "ArcFlashStatuses");
            Delete.Column("Priority").FromTable(FACILITIES);
            Delete.Column("PowerCompanyDataReceived").FromTable(FACILITIES);
            Delete.ForeignKeyColumn(FACILITIES, "UtilityCompanyId", "UtilityCompanies");
            Delete.Column("UtilityCompanyOther").FromTable(FACILITIES);
            Delete.Column("UtilityCompanyDataReceivedDate").FromTable(FACILITIES);
            Delete.Column("AFHAAnalysisPerformed").FromTable(FACILITIES);
            Delete.ForeignKeyColumn(FACILITIES, "TypeOfArcFlashAnalysisId", "ArcFlashAnalysisTypes");
            Delete.ForeignKeyColumn(FACILITIES, "ArcFlashLabelTypeId", "ArcFlashLabelTypes");
            Delete.Column("UtilityAccountNumber").FromTable(FACILITIES);
            Delete.Column("UtilityMeterNumber").FromTable(FACILITIES);
            Delete.Column("UtilityPoleNumber").FromTable(FACILITIES);
            Delete.Column("PrimaryVoltageKV").FromTable(FACILITIES);
            Delete.ForeignKeyColumn(FACILITIES, "VoltageId", "Voltages");
            Delete.ForeignKeyColumn(FACILITIES, "PowerPhaseId", "PowerPhases");
            //Delete.ForeignKeyColumn(FACILITIES, "UtilityTransformerKVARatingId", "UtilityTransformerKVARatings");
            Delete.Column("TransformerKVAFieldConfirmed").FromTable(FACILITIES);
            Delete.Column("TransformerResistancePercentage").FromTable(FACILITIES);
            Delete.Column("TransformerReactancePercentage").FromTable(FACILITIES);
            //Delete.ForeignKeyColumn(FACILITIES,"FacilityTransformerWiringTypeId", "FacilityTransformerWiringTypes");
            Delete.Column("PrimaryFuseSize").FromTable(FACILITIES);
            Delete.Column("PrimaryFuseType").FromTable(FACILITIES);
            Delete.Column("PrimaryFuseManufacturer").FromTable(FACILITIES);
            Delete.Column("LineToLineFaultAmps").FromTable(FACILITIES);
            Delete.Column("LineToLineNeutralFaultAmps").FromTable(FACILITIES);
            Delete.Column("ArcFlashNotes").FromTable(FACILITIES);
            Delete.Column("DateLabelsApplied").FromTable(FACILITIES);
            Delete.Column("ArcFlashContractor").FromTable(FACILITIES);
            Delete.Column("ArcFlashHazardAnalysisStudyParty").FromTable(FACILITIES);
            Delete.Column("CostToComplete").FromTable(FACILITIES);

            Delete.ForeignKey("FK_Facilities_FacilitySizes_FacilitySizeId").OnTable(FACILITIES);
            Delete.Column("FacilitySizeId").FromTable(FACILITIES);
            Delete.ForeignKey("FK_tblFacilities_UtilityTransformerKVARatings_UtilityTransformerKVARatingId")
                  .OnTable(FACILITIES);
            Delete.Column("UtilityTransformerKVARatingId").FromTable(FACILITIES);
            Delete.ForeignKey("FK_Facilities_FacilityTransformerWiringTypes_FacilityTransformerWiringTypeId")
                  .OnTable(FACILITIES);
            Delete.Column("FacilityTransformerWiringTypeId").FromTable(FACILITIES);

            Execute.Sql(CREATE_VIEW);
        }

        public override void Down()
        {
            Execute.Sql(DROP_VIEW);
            //add arcflash fields back to facility
            ////
            Alter.Table(FACILITIES)
                 .AddForeignKeyColumn("ArcFlashStatusId", "ArcFlashStatuses").Nullable()
                 .AddColumn("Priority").AsString(25).Nullable()
                 .AddColumn("PowerCompanyDataReceived").AsBoolean().NotNullable().WithDefaultValue(0)
                 .AddForeignKeyColumn("UtilityCompanyId", "UtilityCompanies").Nullable()
                 .AddColumn("UtilityCompanyOther").AsAnsiString(50).Nullable()
                 .AddColumn("UtilityCompanyDataReceivedDate").AsDateTime().Nullable()
                 .AddColumn("AFHAAnalysisPerformed").AsBoolean().Nullable()
                 .AddForeignKeyColumn("TypeOfArcFlashAnalysisId", "ArcFlashAnalysisTypes").Nullable()
                 .AddForeignKeyColumn("ArcFlashLabelTypeId", "ArcFlashLabelTypes").Nullable()
                 .AddColumn("UtilityAccountNumber").AsString(50).Nullable()
                 .AddColumn("UtilityMeterNumber").AsString(25).Nullable()
                 .AddColumn("UtilityPoleNumber").AsString(25).Nullable()
                 .AddColumn("PrimaryVoltageKV").AsDecimal(18, 2).Nullable()
                 .AddForeignKeyColumn("VoltageId", "Voltages").Nullable()
                 .AddForeignKeyColumn("PowerPhaseId", "PowerPhases").Nullable()
                 .AddColumn("TransformerKVAFieldConfirmed").AsBoolean().NotNullable().WithDefaultValue(0)
                 .AddColumn("TransformerResistancePercentage").AsDecimal(18, 2).Nullable()
                 .AddColumn("TransformerReactancePercentage").AsDecimal(18, 2).Nullable()
                 .AddColumn("PrimaryFuseSize").AsDecimal(18, 2).Nullable()
                 .AddColumn("PrimaryFuseType").AsString(50).Nullable()
                 .AddColumn("PrimaryFuseManufacturer").AsString(100).Nullable()
                 .AddColumn("LineToLineFaultAmps").AsDecimal(18, 2).Nullable()
                 .AddColumn("LineToLineNeutralFaultAmps").AsDecimal(18, 2).Nullable()
                 .AddColumn("ArcFlashNotes").AsText().Nullable()
                 .AddColumn("DateLabelsApplied").AsDateTime().Nullable()
                 .AddColumn("ArcFlashContractor").AsAnsiString(50).Nullable()
                 .AddColumn("ArcFlashHazardAnalysisStudyParty").AsString(50).Nullable()
                 .AddColumn("CostToComplete").AsDecimal(18, 2).Nullable()
                 .AddColumn("FacilitySizeId").AsInt32().Nullable()
                 .ForeignKey("FK_Facilities_FacilitySizes_FacilitySizeId", "FacilitySizes", "Id").Nullable()
                 .AddColumn("UtilityTransformerKVARatingId").AsInt32().Nullable().ForeignKey(
                      "FK_tblFacilities_UtilityTransformerKVARatings_UtilityTransformerKVARatingId",
                      "UtilityTransformerKVARatings", "Id").Nullable()
                 .AddColumn("FacilityTransformerWiringTypeId").AsInt32().Nullable().ForeignKey(
                      "FK_Facilities_FacilityTransformerWiringTypes_FacilityTransformerWiringTypeId",
                      "FacilityTransformerWiringTypes", "Id").Nullable();

            //populate them from the arcflash table
            Execute.Sql(@"
                UPDATE
                    tblFacilities
                SET
                    ArcFlashStatusId = afs.ArcFlashStatusId,
                    Priority = afs.Priority,
                    FacilitySizeId = afs.FacilitySizeId,
                    PowerCompanyDataReceived = afs.PowerCompanyDataReceived,
                    UtilityCompanyDataReceivedDate = afs.UtilityCompanyDataReceivedDate,
                    AFHAAnalysisPerformed = afs.AFHAAnalysisPerformed,
                    TypeOfArcFlashAnalysisId = afs.TypeOfArcFlashAnalysisId,
                    ArcFlashLabelTypeId = afs.ArcFlashLabelTypeId,
                    UtilityAccountNumber = afs.UtilityAccountNumber,
                    UtilityMeterNumber = afs.UtilityMeterNumber,
                    UtilityPoleNumber = afs.UtilityPoleNumber,
                    PrimaryVoltageKV = afs.PrimaryVoltageKV,
                    VoltageId = afs.VoltageId,
                    PowerPhaseId = afs.PowerPhaseId,
                    UtilityTransformerKVARatingId = afs.UtilityTransformerKVARatingId,
                    TransformerKVAFieldConfirmed = afs.TransformerKVAFieldConfirmed,
                    TransformerResistancePercentage = afs.TransformerResistancePercentage,
                    TransformerReactancePercentage = afs.TransformerReactancePercentage,
                    FacilityTransformerWiringTypeId = afs.FacilityTransformerWiringTypeId,
                    PrimaryFuseSize = afs.PrimaryFuseSize,
                    PrimaryFuseType = afs.PrimaryFuseType,
                    PrimaryFuseManufacturer = afs.PrimaryFuseManufacturer,
                    LineToLineFaultAmps = afs.LineToLineFaultAmps,
                    LineToLineNeutralFaultAmps = afs.LineToLineNeutralFaultAmps,
                    ArcFlashNotes = afs.ArcFlashNotes,
                    DateLabelsApplied = afs.DateLabelsApplied,
                    ArcFlashContractor = afs.ArcFlashContractor,
                    ArcFlashHazardAnalysisStudyParty = afs.ArcFlashHazardAnalysisStudyParty,
                    CostToComplete = afs.CostToComplete,
                    UtilityCompanyId = afs.UtilityCompanyId, 
                    UtilityCompanyOther = afs.UtilityCompanyOther
                FROM
                    tblFacilities F 
                JOIN 
                    ArcFlashStudies afs on afs.FacilityId = F.RecordID");

            //remove arcflash table
            Delete.Table(ARC_FLASH_STUDIES);
        }
    }
}
