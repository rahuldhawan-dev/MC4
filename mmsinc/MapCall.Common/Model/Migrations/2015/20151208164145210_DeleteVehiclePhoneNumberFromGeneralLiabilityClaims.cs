using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151208164145210), Tags("Production")]
    public class DeleteVehiclePhoneNumberFromGeneralLiabilityClaims : Migration
    {
        public override void Up()
        {
            Delete.Column("VehiclePhoneNumber").FromTable("GeneralLiabilityClaims");
        }

        public override void Down()
        {
            Create.Column("VehiclePhoneNumber").OnTable("GeneralLiabilityClaims").AsString(20).Nullable();
        }
    }
}
