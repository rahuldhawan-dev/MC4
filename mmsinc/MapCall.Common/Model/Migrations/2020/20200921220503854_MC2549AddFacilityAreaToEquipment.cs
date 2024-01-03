using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200921220503854), Tags("Production")]
    public class MC2549AddFacilityAreaToEquipment : Migration
    {
        public override void Up()
        {
            Alter.Table("Equipment").AddForeignKeyColumn("FacilityFacilityAreaId", "FacilitiesFacilityAreas")
                 .Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Equipment", "FacilityFacilityAreaId", "FacilitiesFacilityAreas");
        }
    }
}
