using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200817095654850), Tags("Production")]
    public class MC2311AddFiveWhysToIncidents : Migration
    {
        public override void Up()
        {
            Alter.Table("Incidents")
                 .AddColumn("Why1").AsString(255).Nullable();
            Alter.Table("Incidents")
                 .AddColumn("Why2").AsString(255).Nullable();
            Alter.Table("Incidents")
                 .AddColumn("Why3").AsString(255).Nullable();
            Alter.Table("Incidents")
                 .AddColumn("Why4").AsString(255).Nullable();
            Alter.Table("Incidents")
                 .AddColumn("Why5").AsString(255).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Why1").FromTable("Incidents");
            Delete.Column("Why2").FromTable("Incidents");
            Delete.Column("Why3").FromTable("Incidents");
            Delete.Column("Why4").FromTable("Incidents");
            Delete.Column("Why5").FromTable("Incidents");
        }
    }
}
