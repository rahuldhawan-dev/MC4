using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20220111120251040), Tags("Production")]
    public class MC5113_UpdateTownTableDistrictIdColumnDataType : Migration
    {
        public const int DISTRICT_ID_LENGTH = 12;

        public override void Up()
        {
            // Update DistrictId datatype to varchar(12) in Town table
            Alter.Column("DistrictId").OnTable("Town").AsAnsiString(DISTRICT_ID_LENGTH).Nullable();
        }

        public override void Down()
        {
            // Revert the changes if needed in Down method
            Alter.Column("DistrictId").OnTable("Town").AsDouble().Nullable();
        }
    }
}