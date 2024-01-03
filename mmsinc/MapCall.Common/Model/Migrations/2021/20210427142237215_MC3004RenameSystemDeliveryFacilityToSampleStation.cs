using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210427142237215), Tags("Production")]
    public class MC3004RenameSystemDeliveryFacilityToSampleStation : Migration
    {
        public override void Up()
        {
            Rename.Column("System_Delivery_Facility").OnTable("tblFacilities").To("SampleStation");
        }

        public override void Down()
        {
            Rename.Column("SampleStation").OnTable("tblFacilities").To("System_Delivery_Facility");
        }
    }
}

