using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190911104643552), Tags("Production")]
    public class RenameNoticeOfViolationDateAndMakeFacilityNullableForMC1617 : Migration
    {
        public override void Up()
        {
            Rename.Column("NoticeOfViolationDate").OnTable("NoticesOfViolation").To("EventDate");
            Execute.Sql(
                $"ALTER TABLE NoticesOfViolation DROP CONSTRAINT [FK_NoticesOfViolation_tblFacilities_FacilityId]");
            Alter.Column("FacilityId").OnTable("NoticesOfViolation")
                 .AsForeignKey("FacilityId", "tblFacilities", "RecordId");
        }

        public override void Down()
        {
            Execute.Sql(
                $"ALTER TABLE NoticesOfViolation DROP CONSTRAINT [FK_NoticesOfViolation_tblFacilities_FacilityId]");
            Alter.Column("FacilityId").OnTable("NoticesOfViolation")
                 .AsForeignKey("FacilityId", "tblFacilities", "RecordId", false);
            Rename.Column("EventDate").OnTable("NoticesOfViolation").To("NoticeOfViolationDate");
        }
    }
}
