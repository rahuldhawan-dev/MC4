using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200401114602152), Tags("Production")]
    public class MC2135DeleteNullEntriesFromPublicWaterSupplyLicensedOperators : Migration
    {
        public override void Up()
        {
            Execute.Sql("DELETE FROM PublicWaterSupplyLicensedOperators WHERE OperatorLicenseId is null;");
            Alter.Table("PublicWaterSupplyLicensedOperators").AlterColumn("OperatorLicenseId").AsInt32().NotNullable();
            Alter.Table("PublicWaterSupplyLicensedOperators").AlterColumn("PublicWaterSupplyId").AsInt32()
                 .NotNullable();
        }

        public override void Down()
        {
            Alter.Table("PublicWaterSupplyLicensedOperators").AlterColumn("OperatorLicenseId").AsInt32().Nullable();
            Alter.Table("PublicWaterSupplyLicensedOperators").AlterColumn("PublicWaterSupplyId").AsInt32().Nullable();
        }
    }
}
