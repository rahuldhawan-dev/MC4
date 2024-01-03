using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230201100614990), Tags("Production")]
    public class MC2780_CleanUpFireDistrictsColumnNames : AutoReversingMigration
    {
        public override void Up()
        {
            Rename.Table("FireDistrict").To("FireDistricts");

            Rename.Column("FireDistrictId")
                  .OnTable("FireDistricts")
                  .To("Id");

            Rename.Column("Address_City")
                  .OnTable("FireDistricts")
                  .To("AddressCity");

            Rename.Column("Address_Zip")
                  .OnTable("FireDistricts")
                  .To("AddressZip");

            Rename.Column("District_Name")
                  .OnTable("FireDistricts")
                  .To("DistrictName");
        }
    }
}

