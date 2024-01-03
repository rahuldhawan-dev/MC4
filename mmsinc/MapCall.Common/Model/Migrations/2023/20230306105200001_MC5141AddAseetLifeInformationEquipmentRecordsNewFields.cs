using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230306105200001), Tags("Production")]
    public class MC5141AddAseetLifeInformationEquipmentRecordsNewFields : Migration
    {
        public override void Up()
        {
            Alter.Table("Equipment")
                 .AddColumn("PlannedReplacementYear").AsInt32().Nullable()
                 .AddColumn("EstimatedReplaceCost").AsDecimal(8, 2).Nullable()
                 .AddColumn("ExtendedUsefulLifeWorkOrderId").AsInt32().Nullable()
                 .AddColumn("LifeExtendedOnDate").AsDateTime().Nullable()
                 .AddColumn("ExtendedUsefulLifeComment").AsAnsiString().Nullable();
        }

        public override void Down()
        {
            Delete.Column("PlannedReplacementYear").FromTable("Equipment");
            Delete.Column("EstimatedReplaceCost").FromTable("Equipment");
            Delete.Column("ExtendedUsefulLifeWorkOrderId").FromTable("Equipment");
            Delete.Column("LifeExtendedOnDate").FromTable("Equipment");
            Delete.Column("ExtendedUsefulLifeComment").FromTable("Equipment");
        }
    }
}