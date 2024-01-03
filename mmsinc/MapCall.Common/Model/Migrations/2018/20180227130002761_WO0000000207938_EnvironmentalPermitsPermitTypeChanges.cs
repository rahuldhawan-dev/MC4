using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180227130002761), Tags("Production")]
    public class WO0000000207938 : Migration
    {
        public override void Up()
        {
            //changes & adds values to dropdown
            Execute.Sql(
                "Update EnvironmentalPermitTypes SET Description = 'Industrial Discharge Permit' WHERE Description = 'Waste Water Permit'");
            Execute.Sql("INSERT INTO EnvironmentalPermitTypes VALUES ('Lab Certification')");
            Execute.Sql("INSERT INTO EnvironmentalPermitTypes VALUES ('Fire Certification')");
            Execute.Sql("INSERT INTO EnvironmentalPermitTypes VALUES ('Treatment Works Approval (TWA)')");
            //adds PDES FK to EnvironmentalPermits
            Alter.Table("EnvironmentalPermits").AddForeignKeyColumn("PDESID", "PDESSystems");
        }

        public override void Down()
        {
            //reverses name change
            Execute.Sql(
                "Update EnvironmentalPermitTypes SET Description = 'Waste Water Permit' WHERE Description = 'Industrial Discharge Permit'");
            //Sets new values added, if any, to null
            Execute.Sql(
                "Update EnvironmentalPermits SET EnvironmentalPermitTypeID = null WHERE EnvironmentalPermitTypeID = (select EnvironmentalPermitTypeId from EnvironmentalPermitTypes where Description = 'Lab Certification')");
            Execute.Sql(
                "Update EnvironmentalPermits SET EnvironmentalPermitTypeID = null WHERE EnvironmentalPermitTypeID = (select EnvironmentalPermitTypeId from EnvironmentalPermitTypes where Description = 'Fire Certification')");
            Execute.Sql(
                "Update EnvironmentalPermits SET EnvironmentalPermitTypeID = null WHERE EnvironmentalPermitTypeID = (select EnvironmentalPermitTypeId from EnvironmentalPermitTypes where Description = 'Treatment Works Approval (TWA)')");
            //deletes new values/columns
            Execute.Sql("DELETE FROM EnvironmentalPermitTypes WHERE Description = 'Lab Certification'");
            Execute.Sql("DELETE FROM EnvironmentalPermitTypes WHERE Description = 'Fire Certification'");
            Execute.Sql("DELETE FROM EnvironmentalPermitTypes WHERE Description = 'Treatment Works Approval (TWA)'");
            Delete.ForeignKeyColumn("EnvironmentalPermits", "PDESID", "PDESSystems");
        }
    }
}
