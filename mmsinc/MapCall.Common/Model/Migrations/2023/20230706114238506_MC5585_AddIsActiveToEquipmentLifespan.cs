using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230706114238506), Tags("Production")]
    public class MC5585_AddIsActiveToEquipmentLifespan : Migration
    {
        public override void Up()
        {
            Alter.Table("EquipmentLifespans")
                 .AddColumn("IsActive")
                 .AsBoolean()
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("IsActive")
                  .FromTable("EquipmentLifespans");
        }
    }
}

