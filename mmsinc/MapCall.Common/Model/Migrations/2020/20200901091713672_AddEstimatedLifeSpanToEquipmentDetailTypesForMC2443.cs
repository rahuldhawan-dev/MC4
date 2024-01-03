using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200901091713672), Tags("Production")]
    public class AddEstimatedLifeSpanToEquipmentDetailTypesForMC2443 : Migration
    {
        public override void Up()
        {
            Alter.Table("EquipmentDetailTypes").AddColumn("EstimatedLifespan").AsInt32().Nullable();
            Execute.Sql(@"
UPDATE EquipmentDetailTypes SET EstimatedLifespan = 10 WHERE Description = 'RTU';
UPDATE EquipmentDetailTypes SET EstimatedLifespan = 25 WHERE Description = 'Chemical Tank';
UPDATE EquipmentDetailTypes SET EstimatedLifespan = 25 WHERE Description = 'Motor';");
        }

        public override void Down()
        {
            Delete.Column("EstimatedLifespan").FromTable("EquipmentDetailTypes");
        }
    }
}
