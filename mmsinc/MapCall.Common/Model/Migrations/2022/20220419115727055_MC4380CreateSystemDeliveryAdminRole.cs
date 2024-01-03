using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220419115727055), Tags("Production")]
    public class MC4380CreateSystemDeliveryAdminRole : Migration
    {
        public override void Up()
        {
            this.CreateModule("System Delivery Admin", "Production", 99);
            Create.Column("IsHyperionFileCreated")
                  .OnTable("SystemDeliveryEntries")
                  .AsBoolean()
                  .WithDefaultValue(false);
        }

        public override void Down()
        {
            this.DeleteModuleAndAssociatedRoles("Production", "System Delivery Admin");
            Delete.Column("IsHyperionFileCreated").FromTable("SystemDeliveryEntries");
        } 
    }
}
