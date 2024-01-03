using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160222154949584), Tags("Production")]
    public class CreateScadaTagNamesAndScadaReadingsForBug2799 : Migration
    {
        public struct TableNames
        {
            public const string TAG_NAMES = "ScadaTagNames", READINGS = "ScadaReadings";
        }

        public const string INDEX_NAME = "IX_" + TableNames.READINGS + "_TagNameId_TimeStamp";

        public override void Up()
        {
            Create.Table(TableNames.TAG_NAMES)
                  .WithIdentityColumn()
                  .WithColumn("TagName").AsString(30).NotNullable().Unique()
                  .WithForeignKeyColumn("EquipmentId", "Equipment", "EquipmentId");

            Create.Table(TableNames.READINGS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("TagNameId", TableNames.TAG_NAMES, nullable: false)
                  .WithColumn("Timestamp").AsDateTime().NotNullable()
                  .WithColumn("Value").AsDecimal(8, 5).NotNullable();

            Create.Index(INDEX_NAME)
                  .OnTable(TableNames.READINGS)
                  .OnColumn("TagNameId")
                  .Ascending()
                  .OnColumn("Timestamp")
                  .Ascending()
                  .WithOptions()
                  .Unique();
        }

        public override void Down()
        {
            Delete.Index(INDEX_NAME).OnTable(TableNames.READINGS);

            Delete.Table(TableNames.READINGS);
            Delete.Table(TableNames.TAG_NAMES);
        }
    }
}
