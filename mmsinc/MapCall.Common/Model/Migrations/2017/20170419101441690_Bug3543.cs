using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170419101441690), Tags("Production")]
    public class Bug3543 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("FacilityAWSecurityTiers", "1", "2", "3", "4");
            Create.Column("FacilityAWSecurityTierId").OnTable("tblFacilities")
                  .AsInt32().Nullable()
                  .ForeignKey("FK_tblFacilities_FacilityAWSecurityTiers_FacilityAWSecurityTierId",
                       "FacilityAWSecurityTiers", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_tblFacilities_FacilityAWSecurityTiers_FacilityAWSecurityTierId")
                  .OnTable("tblFacilities");
            Delete.Column("FacilityAWSecurityTierId").FromTable("tblFacilities");
            Delete.Table("FacilityAWSecurityTiers");
        }
    }
}
