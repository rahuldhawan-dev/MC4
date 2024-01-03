using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191125143015882), Tags("Production")]
    public class MC1698AdditionalChemicalStorageFields : Migration
    {
        public override void Up()
        {
            Alter.Table("ChemicalStorage")
                 .AddColumn("Location").AsAnsiString(60).Nullable()
                 .AddColumn("ContainerType").AsAnsiString(30).Nullable()
                 .AddColumn("MaximumDailyInventory").AsAnsiString(30).Nullable()
                 .AddColumn("AverageDailyInventory").AsAnsiString(30).Nullable()
                 .AddColumn("DaysOnSite").AsInt32().Nullable()
                 .AddColumn("StoragePressure").AsAnsiString(30).Nullable()
                 .AddColumn("StorageTemperature").AsAnsiString(30).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Location").FromTable("ChemicalStorage");
            Delete.Column("ContainerType").FromTable("ChemicalStorage");
            Delete.Column("MaximumDailyInventory").FromTable("ChemicalStorage");
            Delete.Column("AverageDailyInventory").FromTable("ChemicalStorage");
            Delete.Column("DaysOnSite").FromTable("ChemicalStorage");
            Delete.Column("StoragePressure").FromTable("ChemicalStorage");
            Delete.Column("StorageTemperature").FromTable("ChemicalStorage");
        }
    }
}
