using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140515140455068), Tags("Production")]
    public class SetDepartmentRequiredOnFacilityForBug1873 : Migration
    {
        public const string TABLE_NAME = "tblFacilities";

        public override void Up()
        {
            Execute.Sql("UPDATE " + TABLE_NAME +
                        " SET [DepartmentID] = (SELECT DepartmentID FROM Departments WHERE [Description] = 'Production') WHERE IsNull(DepartmentID, 0) = 0");
            Alter.Table(TABLE_NAME).AlterColumn("DepartmentID").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Alter.Table(TABLE_NAME).AlterColumn("DepartmentID").AsInt32().Nullable();
        }
    }
}
