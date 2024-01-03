using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160505163100164), Tags("Production")]
    public class AddColumnToRecurringProjectsForBug2872 : Migration
    {
        public override void Up()
        {
            Alter.Table("RecurringProjects").AddColumn("CPSReferenceId").AsAnsiString(5).Nullable();
            Alter.Table("SewerManholes").AddColumn("RimElevation").AsDecimal(18, 2).Nullable();
            Alter.Table("SewerManholes").AddColumn("DepthToInvert").AsDecimal(18, 2).Nullable();
        }

        public override void Down()
        {
            Delete.Column("DepthToInvert").FromTable("SewerManholes");
            Delete.Column("RimElevation").FromTable("SewerManholes");
            Delete.Column("CPSReferenceId").FromTable("RecurringProejcts");
        }
    }
}
