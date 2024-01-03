using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220620134612628), Tags("Production")]
    public class MC4451_AddPWSIDToPremises : Migration
    {
        public override void Up()
        {
            Alter.Table("Premises").AddForeignKeyColumn("PublicWaterSupplyId", "PublicWaterSupplies");
            Alter.Table("Premises").AddForeignKeyColumn("PremiseTypeId", "PremiseTypes", "PremiseTypeId");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Premises", "PublicWaterSupplyId", "PublicWaterSupplies");
            Delete.ForeignKeyColumn("Premises", "PremiseTypeId", "PremiseTypes", "PremiseTypeId");
        }
    }
}

