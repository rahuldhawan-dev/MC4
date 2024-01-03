using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200508165405215), Tags("Production")]
    public class MC2121AddColumnsToPublicWaterSupplyForMergePWSID : Migration
    {
        public override void Up()
        {
            Alter.Table("PublicWaterSupplies")
                 .AddColumn("AnticipatedMergerDate").AsDateTime().Nullable()
                 .AddColumn("ValidTo").AsDateTime().Nullable()
                 .AddColumn("ValidFrom").AsDateTime().Nullable()
                 .AddForeignKeyColumn("AnticipatedMergePublicWaterSupplyId", "PublicWaterSupplies").Nullable();

            Execute.Sql(
                "SET IDENTITY_INSERT [PublicWaterSupplyStatuses] ON; INSERT INTO PublicWaterSupplyStatuses(Id, Description) VALUES(5, 'Pending Merger'); SET IDENTITY_INSERT [PublicWaterSupplyStatuses] OFF");
        }

        public override void Down()
        {
            Delete.Column("AnticipatedMergerDate").FromTable("PublicWaterSupplies");
            Delete.Column("ValidTo").FromTable("PublicWaterSupplies");
            Delete.Column("ValidFrom").FromTable("PublicWaterSupplies");
            Delete.ForeignKeyColumn("PublicWaterSupplies", "AnticipatedMergePublicWaterSupplyId",
                "PublicWaterSupplies");
        }
    }
}
