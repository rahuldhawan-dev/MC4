using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180531145200892), Tags("Production")]
    public class AddTimeZonesForMC450 : Migration
    {
        #region Private Methods

        private void AddTimeZone(string zone, string description, int i)
        {
            Execute.Sql($"INSERT INTO TimeZones Values('{zone}', '{description}', {i});");
        }

        #endregion

        #region Exposed Methods

        public override void Up()
        {
            Create.Table("TimeZones")
                  .WithIdentityColumn()
                  .WithColumn("Zone").AsAnsiString(5)
                  .WithColumn("Description").AsAnsiString(255)
                  .WithColumn("UTCOffset").AsDecimal(4, 2);
            Alter.Table("OperatingCenters")
                 .AddForeignKeyColumn("TimeZoneId", "TimeZones");

            AddTimeZone("EST", "Eastern Standard Time", -5);
            AddTimeZone("CST", "Central Standard Time", -6);
            AddTimeZone("MST", "Mountain Standard Time", -7);
            AddTimeZone("PST", "Pacific Standard Time", -8);
            AddTimeZone("HST", "Hawaii–Aleutian Time", -10);
            Execute.Sql(
                "UPDATE OperatingCenters SET TimeZoneId = (SELECT Id FROM TimeZones WHERE [Zone] = 'EST') WHERE [StateID] in (SELECT StateID from States where Abbreviation in ('NJ', 'NY','PA','WV','VA','IN','MD','FL'));");
            Execute.Sql(
                "UPDATE OperatingCenters SET TimeZoneId = (SELECT Id FROM TimeZones WHERE [Zone] = 'CST') WHERE [StateID] in (SELECT StateID from States where Abbreviation in ('IL','KY','TN','MO','IA'));");
            Execute.Sql(
                "UPDATE OperatingCenters SET TimeZoneId = (SELECT Id FROM TimeZones WHERE [Zone] = 'PST') WHERE [StateID] in (SELECT StateID from States where Abbreviation in ('CA'));");
            Execute.Sql(
                "UPDATE OperatingCenters SET TimeZoneId = (SELECT Id FROM TimeZones WHERE [Zone] = 'HST') WHERE [StateID] in (SELECT StateID from States where Abbreviation in ('HI'));");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("OperatingCenters", "TimeZoneId", "TimeZones");
            Delete.Table("TimeZones");
        }

        #endregion
    }
}
