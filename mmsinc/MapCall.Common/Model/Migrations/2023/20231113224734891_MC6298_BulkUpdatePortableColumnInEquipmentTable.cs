using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231113224734891), Tags("Production")]
    public class BulkUpdatePortableColumnInEquipmentTable : Migration
    {
        private const string TABLE = "Equipment";
        public override void Up()
        {
            Execute.Sql("update Equipment set Portable = 0 where Portable is null");
            Alter.Column("Portable").OnTable(TABLE)
                 .AsBoolean().NotNullable();
        }

        public override void Down()
        {
            Alter.Column("Portable").OnTable(TABLE)
                 .AsBoolean().Nullable();
        }
    }
}

