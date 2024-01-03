using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;
using System;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20210505161631144), Tags("Production")]
    public class MC1988AddEndOfPipeExceedanceAndLookups : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("EndOfPipeExceedanceTypes",
                "BOD/CBOD",
                "Fecal/E Coli/Total Coliform",
                "Plant Flow",
                "Nitrate/Total N",
                "Ammonia",
                "Phosphorous",
                "pH",
                "TSS/TDS",
                "Dissolved Oxygen",
                "Other");

            this.CreateLookupTableWithValues("EndOfPipeExceedanceRootCauses",
                "Power failure",
                "Mechanical failure",
                "Treatment disruption",
                "Treatment limitation",
                "Plant capacity",
                "Weather event",
                "Unknown",
                "Other");

            Create.Table("EndOfPipeExceedances")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("StateId", "States", "StateId", false)
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId", false)
                  .WithForeignKeyColumn("WasteWaterSystemId", "WasteWaterSystems", nullable: false)
                  .WithForeignKeyColumn("FacilityId", "tblFacilities", "RecordId")
                  .WithColumn("EventDate").AsDateTime().NotNullable()
                  .WithForeignKeyColumn("EndOfPipeExceedanceTypeId", "EndOfPipeExceedanceTypes", nullable: false)
                  .WithColumn("EndOfPipeExceedanceTypeOtherReason").AsAnsiString(255).Nullable()
                  .WithForeignKeyColumn("EndOfPipeExceedanceRootCauseId", "EndOfPipeExceedanceRootCauses", nullable: false)
                  .WithColumn("EndOfPipeExceedanceRootCauseOtherReason").AsAnsiString(255).Nullable()
                  .WithColumn("ConsentOrder").AsBoolean().NotNullable()
                  .WithColumn("NewAcquisition").AsBoolean().NotNullable()
                  .WithColumn("BriefDescription").AsAnsiString(Int32.MaxValue);

            this.AddDataType("EndOfPipeExceedances");
            this.AddDocumentType("End Of Pipe Exceedance Document", "EndOfPipeExceedances");
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("End Of Pipe Exceedance Document", "EndOfPipeExceedances");
            this.RemoveDataType("EndOfPipeExceedances");
            Delete.Table("EndOfPipeExceedances");
            Delete.Table("EndOfPipeExceedanceTypes");
            Delete.Table("EndOfPipeExceedanceRootCauses");
        }
    }
}

