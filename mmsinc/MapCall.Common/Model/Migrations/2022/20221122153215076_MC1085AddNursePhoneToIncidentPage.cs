using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221122153215076), Tags("Production")]
    public class MC1085AddNursePhoneToIncidentPage : Migration
    {
        public override void Up()
        {
            Alter.Table("Incidents").
                  AddColumn("NursePhone").AsString(14).Nullable();
        }

        public override void Down()
        {
            Delete.Column("NursePhone").FromTable("Incidents");
        }
    }
}

