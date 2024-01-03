using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200225162640262), Tags("Production")]
    public class MC1900AddNewFieldsToPublicWaterSupply : Migration
    {
        public override void Up()
        {
            Alter.Table("PublicWaterSupplies")
                 .AddColumn("AnticipatedActiveDate").AsDateTime().Nullable()
                 .AddColumn("IsConsentOrder").AsBoolean().Nullable()
                 .AddColumn("IsNewAcquisition").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("AnticipatedActiveDate").FromTable("PublicWaterSupplies");
            Delete.Column("IsConsentOrder").FromTable("PublicWaterSupplies");
            Delete.Column("IsNewAcquisition").FromTable("PublicWaterSupplies");
        }
    }
}
