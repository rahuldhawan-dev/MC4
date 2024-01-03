using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150806084027696), Tags("Production")]
    public class AddOneDayDoctorRestrictionEndDateToEmployeesBug2533 : Migration
    {
        public override void Up()
        {
            Create.Column("OneDayDoctorsNoteRestrictionEndDate")
                  .OnTable("tblEmployee")
                  .AsDateTime()
                  .Nullable();
        }

        public override void Down()
        {
            Delete.Column("OneDayDoctorsNoteRestrictionEndDate")
                  .FromTable("tblEmployee");
        }
    }
}
