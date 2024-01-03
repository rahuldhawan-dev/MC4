using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210818114322575), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC2486OperatingLicenseLevelLength : Migration
    {
        public override void Up()
        {
            Alter.Column("LicenseLevel").OnTable("OperatorLicenses").AsAnsiString(30).NotNullable();
        }

        public override void Down()
        {
            Alter.Column("LicenseLevel").OnTable("OperatorLicenses").AsAnsiString(4).NotNullable();
        }
    }
}

