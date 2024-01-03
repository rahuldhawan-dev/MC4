using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210222122720163), Tags("Production")]
    public class MC2476AddShadowPropertiesToSystemDeliveryEntry : Migration
    {
        public override void Up()
        {
            Alter.Table("SystemDeliveryEntries")
                 .AddForeignKeyColumn("LastUpdatedById", "tblPermissions", "RecId")
                 .AddColumn("LastUpdated").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("SystemDeliveryEntries", "LastUpdatedById", "tblPermissions", "RecId");
            Delete.Column("LastUpdated").FromTable("SystemDeliveryEntries");
        }
    }
}

