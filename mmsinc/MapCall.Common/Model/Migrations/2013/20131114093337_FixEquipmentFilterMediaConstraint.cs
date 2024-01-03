using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131114093337), Tags("Production")]
    public class FixEquipmentFilterMediaConstraint : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey(CreateTablesForBug1510.ForeignKeyNames.FilterMedia.EQUIPMENT)
                  .OnTable(CreateTablesForBug1510.TableNames.FILTER_MEDIA);
            Create.ForeignKey(CreateTablesForBug1510.ForeignKeyNames.FilterMedia.EQUIPMENT)
                  .FromTable(CreateTablesForBug1510.TableNames.FILTER_MEDIA)
                  .ForeignColumn(CreateTablesForBug1510.ColumnNames.FilterMedia.EQUIPMENT_ID)
                  .ToTable("Equipment").PrimaryColumn("EquipmentID");
        }

        public override void Down()
        {
            Delete.ForeignKey(CreateTablesForBug1510.ForeignKeyNames.FilterMedia.EQUIPMENT)
                  .OnTable(CreateTablesForBug1510.TableNames.FILTER_MEDIA);
            Create.ForeignKey(CreateTablesForBug1510.ForeignKeyNames.FilterMedia.EQUIPMENT)
                  .FromTable(CreateTablesForBug1510.TableNames.FILTER_MEDIA)
                  .ForeignColumn(CreateTablesForBug1510.ColumnNames.FilterMedia.EQUIPMENT_ID)
                  .ToTable("Equipment").PrimaryColumn("EquipmentID");
        }
    }
}
