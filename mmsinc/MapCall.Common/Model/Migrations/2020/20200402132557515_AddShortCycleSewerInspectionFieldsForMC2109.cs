using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200402132557515), Tags("Production")]
    public class AddShortCycleSewerInspectionFieldsForMC2109 : Migration
    {
        public override void Up()
        {
            Create.Table("ShortCycleWorkOrderCompletionViolationCodes")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ShortCycleWorkOrderCompletionId", "ShortCycleWorkOrderCompletions")
                  .WithColumn("ViolationCode").AsInt32().NotNullable();
            Alter.Table("ShortCycleWorkOrderCompletions")
                 .AddColumn("InspectionDate").AsDateTime().Nullable()
                 .AddColumn("InspectionPassed").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("InspectionPassed").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Column("InspectionDate").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Table("ShortCycleWorkOrderCompletionSewerInspectionViolationCodes");
        }
    }
}
