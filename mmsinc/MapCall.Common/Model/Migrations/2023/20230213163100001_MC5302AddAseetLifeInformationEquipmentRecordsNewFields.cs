using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230213163100001), Tags("Production")]
    public class MC5302AddAseetLifeInformationEquipmentRecordsNewFields : Migration
    {
        public override void Up()
        {
            Alter.Table("EquipmentDetailTypes")
                 .AddColumn("ExtendedLifeMajor").AsDecimal(4, 1).Nullable()
                 .AddColumn("ExtendedLifeMinor").AsDecimal(4, 1).Nullable()
                 .AddColumn("EstimatedLifeSpanTemp").AsDecimal(4, 1).Nullable();

            Execute.Sql(@"UPDATE EquipmentDetailTypes 
                          SET EstimatedLifeSpanTemp = CONVERT(DECIMAL(4,1), EstimatedLifeSpan)");

            Delete.Column("EstimatedLifeSpan").FromTable("EquipmentDetailTypes");

            Rename.Column("EstimatedLifeSpanTemp").OnTable("EquipmentDetailTypes").To("EstimatedLifeSpan");
        }

        public override void Down()
        {
            Delete.Column("ExtendedLifeMajor").FromTable("EquipmentDetailTypes");
            Delete.Column("ExtendedLifeMinor").FromTable("EquipmentDetailTypes");
            // A decimal column cannot be changed to an integer column (loss of data) - EstimatedLifeSpan
        }
    }
}
