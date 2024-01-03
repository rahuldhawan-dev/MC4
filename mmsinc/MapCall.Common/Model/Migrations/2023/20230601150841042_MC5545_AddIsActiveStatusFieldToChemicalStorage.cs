using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230601150841042), Tags("Production")]
    public class MC5545_AddIsActiveStatusFieldToChemicalStorage : Migration
    {
        public override void Up()
        {
            Alter.Table("ChemicalStorage").AddColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
        }

        public override void Down()
        {
            Delete.Column("IsActive").FromTable("ChemicalStorage");
        }
    }
}

