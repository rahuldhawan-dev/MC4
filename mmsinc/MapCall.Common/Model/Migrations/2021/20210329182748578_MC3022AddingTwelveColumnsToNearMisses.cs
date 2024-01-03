using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210329182748578), Tags("Production")]
    public class MC3022AddingTwelveColumnsToNearMisses : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("WorkOrderTypes", "T&D_Department", "Production", "ShortCycle", "Unknown");
            this.CreateLookupTableWithValues("LifeSavingRuleTypes", "PPE", "Alcohol or Illegal Drugs", "Work Zone Safety", "Cave-in Protection", "Approved Tool/Proper Usage", "Hazardous Energy Control", "Fall Protection", "Confined Space Safeguards", "Contact with Utility Line");
            this.CreateLookupTableWithValues("SeriousInjuryOrFatalityTypes", "SIF", "SIF Potential");
            this.CreateLookupTableWithValues("StopWorkUsageTypes", "Employee", "Contractor", "Public");
            this.CreateLookupTableWithValues("SeverityTypes", "GREEN - Continue and Report", "YELLOW - Use caution and Report", "RED - Stop work and Report");

            Alter.Table("NearMisses")
                 .AddForeignKeyColumn("WorkOrderTypeId", "WorkOrderTypes")
                 .AddForeignKeyColumn("ProductionWorkOrderId", "ProductionWorkOrders")
                 .AddForeignKeyColumn("WorkOrderId", "WorkOrders", "WorkOrderID")
                 .AddForeignKeyColumn("ShortCycleWorkOrderId", "ShortCycleWorkOrders")
                 .AddForeignKeyColumn("LifeSavingRuleTypeId", "LifeSavingRuleTypes")
                 .AddForeignKeyColumn("SeriousInjuryOrFatalityTypeId", "SeriousInjuryOrFatalityTypes")
                 .AddForeignKeyColumn("StopWorkUsageTypeId", "StopWorkUsageTypes")
                 .AlterColumn("IncidentNumber").AsString(15).Nullable()
                 .AddColumn("WorkOrderNumber").AsString(50).Nullable()
                 .AddForeignKeyColumn("TownId", "Towns", "TownID")
                 .AddForeignKeyColumn("FacilityId", "tblFacilities", "RecordId")
                 .AddColumn("LocationDetails").AsString(255).Nullable()
                 .AddForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateID")
                 .AddForeignKeyColumn("EmployeeTypeId", "EmployeeTypes")
                 .AddForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeID")
                 .AddColumn("PhoneCellular").AsString(255).Nullable()
                 .AddColumn("EmailAddress").AsString(255).Nullable()
                 .AddColumn("SeriousInjuryOrFatality").AsBoolean().Nullable()
                 .AddColumn("ContractorName").AsAnsiString(100).Nullable()
                 .AddColumn("ContractorCompany").AsAnsiString(100).Nullable()
                 .AddColumn("DescribeOther").AsAnsiString(100).Nullable()
                 .AddColumn("StopWorkAuthorityPerformed").AsBoolean().Nullable();

            Execute.Sql(@"UPDATE NearMissCategories
            SET description = 'Slip/Trip/Fall' 
            WHERE Description = 'Fall Or Slip';");

            Execute.Sql(@"UPDATE NearMisses
            Set CategoryId = (select id from NearMissCategories where description = 'Other'), SubCategoryId = (select id from NearMissSubCategories where description = 'Other' and CategoryId in (select id from NearMissCategories where description = 'Other'))
            Where CategoryId = (select id from NearMissCategories where Description = 'Misc Cases'); ");

            Execute.Sql(@"Delete from NearMissSubCategories
            where CategoryId in (select id from NearMissCategories where Description = 'Misc Cases');");

            Execute.Sql(@"Delete from NearMissCategories
            where id in (select id from NearMissCategories where Description = 'Misc Cases');");

            // RENAME_COLUMNS
            Rename.Column("Notes").OnTable("NearMisses")
                  .To("Description");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_NearMisses_WorkOrderTypes_WorkOrderTypeId").OnTable("NearMisses");
            Delete.Column("WorkOrderTypeId").FromTable("NearMisses");
            Delete.Column("WorkOrderNumber").FromTable("NearMisses");
            Delete.ForeignKeyColumn("NearMisses", "TownId", "Towns", "TownID");
            Delete.ForeignKeyColumn("NearMisses", "FacilityId", "tblFacilities", "RecordId");
            Delete.ForeignKeyColumn("NearMisses", "LifeSavingRuleTypeId", "LifeSavingRuleTypes");
            Delete.ForeignKeyColumn("NearMisses", "SeriousInjuryOrFatalityTypeId", "SeriousInjuryOrFatalityTypes");
            Delete.ForeignKeyColumn("NearMisses", "StopWorkUsageTypeId", "StopWorkUsageTypes");
            Delete.Column("LocationDetails").FromTable("NearMisses");
            Delete.ForeignKeyColumn("NearMisses", "CoordinateId", "Coordinates", "CoordinateID");
            Delete.Column("PhoneCellular").FromTable("NearMisses");
            Delete.ForeignKeyColumn("NearMisses", "EmployeeId", "tblEmployee", "tblEmployeeID");
            Delete.Column("EmailAddress").FromTable("NearMisses");
            Delete.Column("SeriousInjuryOrFatality").FromTable("NearMisses");
            Delete.Column("ContractorName").FromTable("NearMisses");
            Delete.Column("ContractorCompany").FromTable("NearMisses");
            Delete.Column("DescribeOther").FromTable("NearMisses");
            Delete.Column("StopWorkAuthorityPerformed").FromTable("NearMisses");
            Delete.ForeignKeyColumn("NearMisses", "ProductionWorkOrderId", "ProductionWorkOrders");
            Delete.ForeignKeyColumn("NearMisses", "WorkOrderId", "WorkOrders", "WorkOrderID");
            Delete.ForeignKeyColumn("NearMisses", "ShortCycleWorkOrderId", "ShortCycleWorkOrders");
            Delete.ForeignKeyColumn("NearMisses", "EmployeeTypeId", "EmployeeTypes");

            Execute.Sql(
                @"UPDATE [NearMissCategories] SET description = 'Fall Or Slip' WHERE Description = 'Slip/Trip/Fall';");

            Delete.Table("WorkOrderTypes");
            Delete.Table("LifeSavingRuleTypes");
            Delete.Table("SeriousInjuryOrFatalityTypes");
            Delete.Table("StopWorkUsageTypes");
            Delete.Table("SeverityTypes");

            // RENAME_COLUMNS
            Rename.Column("Description").OnTable("NearMisses")
                  .To("Notes");
        }
    }
}