using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131219155504), Tags("Production")]
    public class IncreasePremiseNumberFieldSizeOnLotsOfTables : Migration
    {
        public override void Up()
        {
            // [tblConsecutive_Estimates].PREMISE is a float, doesn't need a length change. Needs a reality check though.
            // ILServices.Premise_Num is nvarchar(255).
            // tblWQ_COMPLAINTS.Premise_Number is nvarchar(10).
            // InactiveServices is varchar(50)
            // tblNJAWHydrant is already varchar(10)
            // FireDistrict is varchar(11) cause Alex forgot it was 10.
            // CriticalCustomers is varchar(25)
            // Incidents is varchar(50)
            // TapImages is nvarchar(100)
            // WorkOrders is varchar(10)
            // tblNJAWService.PremNum is already varchar(10)

            Alter.Table("CustomerLocations").AlterColumn("PremiseNumber").AsAnsiString(10).NotNullable();
            Alter.Table("MeterChangeOuts").AlterColumn("PremiseNumber").AsAnsiString(10).Nullable();
            Alter.Table("Premises").AlterColumn("PremiseNumber").AsAnsiString(10).Nullable();
        }

        public override void Down()
        {
            // This can't be rolled back without screwing up data by truncating it.
        }
    }
}
