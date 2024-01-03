using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200527125119095), Tags("Production")]
    public class RenameSecurityThreatColumnForMC2318 : Migration
    {
        public override void Up()
        {
            Rename.Column("ThreatActive").OnTable("ShortCycleWorkOrderSecurityThreats").To("PoliceEscortActive");
        }

        public override void Down()
        {
            Rename.Column("PoliceEscortActive").OnTable("ShortCycleWorkOrderSecurityThreats").To("ThreatActive");
        }
    }
}
