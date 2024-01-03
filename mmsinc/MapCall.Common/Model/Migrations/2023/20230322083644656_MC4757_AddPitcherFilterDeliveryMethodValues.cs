using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230322083644656), Tags("Production")]
    public class MC4757_AddPitcherFilterDeliveryMethodValues : Migration
    {
        public override void Up()
        {
            Insert
               .IntoTable("PitcherFilterCustomerDeliveryMethods")
               .Rows(
                    new { Description = "Handed to Customer" },
                    new { Description = "Left on Porch/Doorstep" },
                    new { Description = "Other" });
        }

        public override void Down()
        {
            Delete.FromTable("PitcherFilterCustomerDeliveryMethods").AllRows();
        }
    }
}

