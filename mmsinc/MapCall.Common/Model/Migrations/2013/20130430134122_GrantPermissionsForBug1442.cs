using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130430134122), Tags("Production")]
    public class GrantPermissionsForBug1442 : Migration
    {
        public override void Up()
        {
            Execute.Sql("GRANT ALL ON Equipment TO MCUser;");
            Execute.Sql("GRANT ALL ON EquipmentTypes TO MCUser;");
            Execute.Sql("GRANT ALL ON EquipmentCategories TO MCUser;");
            Execute.Sql("GRANT ALL ON EquipmentSubCategories TO MCUser;");
            Execute.Sql("GRANT ALL ON EquipmentStatuses TO MCUser;");
            Execute.Sql("GRANT ALL ON EquipmentManufacturers TO MCUser;");
            Execute.Sql("GRANT ALL ON EquipmentModels TO MCUser;");
        }

        public override void Down() { }
    }
}
