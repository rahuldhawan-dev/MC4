using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160229100350424), Tags("Production")]
    public class AddEnvironmentalPermitTypeForBug2815 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                @"IF NOT EXISTS(SELECT 1 FROM EnvironmentalPermitTypes WHERE Description = 'Remedial Action Permit') 
                BEGIN 
                SET IDENTITY_INSERT EnvironmentalPermitTypes ON
                INSERT INTO EnvironmentalPermitTypes(EnvironmentalPermitTypeID, Description) Values(19, 'Remedial Action Permit')
                SET IDENTITY_INSERT EnvironmentalPermitTypes OFF
                END");
        }

        public override void Down()
        {
            Execute.Sql("DELETE FROM EnvironmentalPermitTypes WHERE EnvironmentalPermitTypeID = 19");
        }
    }
}
