using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200520144135983), Tags("Production")]
    public class AddSecurityThreatFieldsForShortCycleForMC2320 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderSecurityThreats")
                 .AddColumn("ThreatStart").AsDateTime().Nullable()
                 .AddColumn("ThreatEnd").AsDateTime().Nullable()
                 .AddColumn("ThreatActive").AsBoolean().Nullable()
                 .AddColumn("Address").AsAnsiString(150).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Address").FromTable("ShortCycleWorkOrderSecurityThreats");
            Delete.Column("ThreatActive").FromTable("ShortCycleWorkOrderSecurityThreats");
            Delete.Column("ThreatEnd").FromTable("ShortCycleWorkOrderSecurityThreats");
            Delete.Column("ThreatStart").FromTable("ShortCycleWorkOrderSecurityThreats");
        }
    }
}
