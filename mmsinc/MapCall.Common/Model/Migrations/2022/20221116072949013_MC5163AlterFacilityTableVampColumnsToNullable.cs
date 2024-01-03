using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221116072949013), Tags("Production")]
    public class MC5163AlterFacilityTableVampColumnsToNullable : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities").AlterColumn("IsInVamp").AsBoolean().Nullable();
        }

        public override void Down()
        {
            // Cannot revert from NULL to NOT NULL without removing all occurrences of NULL in the database
            // Alter.Table("tblFacilities").AlterColumn("IsInVamp").AsBoolean().NotNullable();
        }
    }
}

