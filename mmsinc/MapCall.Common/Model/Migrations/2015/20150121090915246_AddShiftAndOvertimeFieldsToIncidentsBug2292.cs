using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150121090915246), Tags("Production")]
    public class AddShiftAndOvertimeFieldsToIncidentsBug2292 : Migration
    {
        public override void Up()
        {
            Create.Table("IncidentShifts")
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Description").AsString(50).NotNullable().Unique();

            Insert.IntoTable("IncidentShifts").Row(new {Description = "1"});
            Insert.IntoTable("IncidentShifts").Row(new {Description = "2"});
            Insert.IntoTable("IncidentShifts").Row(new {Description = "3"});

            // This is a required field but we're not setting defaults cause Doug said it'd be unknown for new values.
            Create.Column("IsOvertime").OnTable("Incidents")
                  .AsBoolean().Nullable();

            Create.Column("IncidentShiftId").OnTable("Incidents")
                  .AsInt32().Nullable()
                  .ForeignKey("FK_Incidents_IncidentShifts_IncidentShiftId", "IncidentShifts", "Id");

            Execute.Sql(
                "update [Incidents] set [IncidentShiftId] = (select top 1 Id from IncidentShifts where Description = '1')");

            Alter.Column("IncidentShiftId").OnTable("Incidents")
                 .AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Incidents_IncidentShifts_IncidentShiftId").OnTable("Incidents");
            Delete.Column("IsOvertime").FromTable("Incidents");
            Delete.Column("IncidentShiftId").FromTable("Incidents");
            Delete.Table("IncidentShifts");
        }
    }
}
