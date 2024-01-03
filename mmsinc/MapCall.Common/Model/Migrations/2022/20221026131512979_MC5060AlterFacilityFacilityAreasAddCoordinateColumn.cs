using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221026131512979), Tags("Production")]
    public class MC5060AlterFacilityFacilityAreasAddCoordinateColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("FacilitiesFacilityAreas")
                 .AddForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateId").Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("FacilitiesFacilityAreas", "CoordinateId", "Coordinates");
        }
    }
}

