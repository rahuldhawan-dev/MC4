using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200303085120677), Tags("Production")]
    public class MC189AddPublicWaterSupplyLicensesOperators : Migration
    {
        public override void Up()
        {
            Create.Table("PublicWaterSupplyLicensedOperators")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("PublicWaterSupplyId", "PublicWaterSupplies")
                  .WithForeignKeyColumn("OperatorLicenseId", "OperatorLicenses");

            Execute.Sql(
                "INSERT INTO PublicWaterSupplyLicensedOperators SELECT PWS.Id, OL.Id FROM PublicWaterSupplies PWS INNER JOIN OperatorLicenses OL ON OL.EmployeeId = PWS.T_LOR WHERE PWS.T_LOR IS NOT NULL UNION SELECT PWS.Id, OL.Id FROM PublicWaterSupplies PWS INNER JOIN OperatorLicenses OL ON OL.EmployeeId = PWS.W_LOR WHERE PWS.W_LOR IS NOT NULL");
        }

        public override void Down()
        {
            Delete.Table("PublicWaterSupplyLicensedOperators");
        }
    }
}
