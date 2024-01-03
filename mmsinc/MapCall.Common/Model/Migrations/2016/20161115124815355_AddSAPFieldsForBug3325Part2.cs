using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161115124815355), Tags("Production")]
    public class AddSAPFieldsForBug3325Part2 : Migration
    {
        public override void Up()
        {
            Alter.Table("ValveTypes").AddColumn("SAPCode").AsAnsiString(30).Nullable();
            Execute.Sql("Update ValveTypes set SAPCode = 'BALL'  WHERE Description = 'BALL'");
            Execute.Sql("Update ValveTypes set SAPCode = 'BUTTERFLY'   WHERE Description = 'BUTTERFLY'");
            Execute.Sql("Update ValveTypes set SAPCode = 'CHECK'  WHERE Description = 'CHECK VALVE'");
            Execute.Sql("Update ValveTypes set SAPCode = 'GATE'  WHERE Description = 'GATE'");
            Execute.Sql("Update ValveTypes set SAPCode = 'OTHER'  WHERE Description = 'INSERTION'");
            Execute.Sql("Update ValveTypes set SAPCode = 'PRES REGULATOR'  WHERE Description = 'PRESSURE REDUCING'");
            Execute.Sql("Update ValveTypes set SAPCode = 'OTHER' WHERE Description = 'STUB'");
            Execute.Sql("Update ValveTypes set SAPCode = 'TAPPING' WHERE Description = 'TAPPING'");
            Alter.Column("SAPCode").OnTable("ValveTypes").AsAnsiString(30).Nullable();
        }

        public override void Down()
        {
            Delete.Column("SAPCode").FromTable("ValveTypes");
        }
    }
}
