using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190625085538325), Tags("Production")]
    public class MakeArcFlashAdjustmentsForMC1042 : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("ArcFlashStatuses")
                  .Rows(new {Description = "Valid"},
                       new {Description = "Not Valid"});

            Execute.Sql($@"
declare @validId int, @notValidId int, @completedId int, @afhaRequiredId int, @currentYear datetime;

select @validId = Id from {"ArcFlashStatuses"} where Description = 'Valid';
select @notValidId = Id from {"ArcFlashStatuses"} where Description = 'Not Valid';
select @completedId = Id from {"ArcFlashStatuses"} where Description = 'Completed';
select @afhaRequiredId = Id from {"ArcFlashStatuses"} where Description = 'AFHA Required';

update {"tblFacilities"} set ArcFlashStatusId = @validId where ArcFlashStatusId = @completedId and DateLabelsApplied IS NOT NULL and datediff(YEAR, DateLabelsApplied, getdate()) <= 5;
update {"tblFacilities"} set ArcFlashStatusId = @notValidId where ArcFlashStatusId in (@completedId, @afhaRequiredId);");

            Delete.FromTable("ArcFlashStatuses")
                  .Rows(new {Description = "Completed"},
                       new {Description = "AFHA Required"});

            Create.LookupTable("ArcFlashAnalysisTypes");

            Insert.IntoTable("ArcFlashAnalysisTypes")
                  .Rows(new {Description = "Utility Data"},
                       new {Description = "Infinite Bus"});

            Create.LookupTable("ArcFlashLabelTypes");

            Insert.IntoTable("ArcFlashLabelTypes")
                  .Rows(new {Description = "Standard Label"},
                       new {Description = "Custom Label"});

            Alter.Table("tblFacilities")
                 .AddColumn("UtilityCompanyDataReceivedDate").AsDateTime().Nullable()
                 .AddColumn("AFHAAnalysisPerformed").AsBoolean().Nullable()
                 .AddForeignKeyColumn("TypeOfArcFlashAnalysisId", "ArcFlashAnalysisTypes")
                 .AddForeignKeyColumn("ArcFlashLabelTypeId", "ArcFlashLabelTypes")
                 .AlterColumn("Priority").AsString(25).Nullable()
                 .AddColumn("UtilityMeterNumber").AsString(25).Nullable()
                 .AddColumn("UtilityPoleNumber").AsString(25).Nullable();
        }

        public override void Down()
        {
            Alter.Table("tblFacilities")
                 .AlterColumn("Priority").AsDecimal().Nullable();

            Delete.ForeignKeyColumn("tblFacilities", "TypeOfArcFlashAnalysisId", "ArcFlashAnalysisTypes");

            Delete.Table("ArcFlashAnalysisTypes");

            Delete.ForeignKeyColumn("tblFacilities", "ArcFlashLabelTypeId", "ArcFlashLabelTypes");

            Delete.Table("ArcFlashLabelTypes");

            Delete.Column("UtilityCompanyDataReceivedDate").FromTable("tblFacilities");
            Delete.Column("AFHAAnalysisPerformed").FromTable("tblFacilities");
            Delete.Column("UtilityMeterNumber").FromTable("tblFacilities");
            Delete.Column("UtilityPoleNumber").FromTable("tblFacilities");

            Delete.FromTable("ArcFlashStatuses")
                  .Rows(new {Description = "Valid"},
                       new {Description = "Not Valid"});

            Insert.IntoTable("ArcFlashStatuses")
                  .Rows(new {Description = "Completed"},
                       new {Description = "AFHA Required"});
        }
    }
}
