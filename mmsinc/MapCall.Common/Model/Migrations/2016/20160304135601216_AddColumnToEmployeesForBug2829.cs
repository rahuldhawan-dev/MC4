using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160304135601216), Tags("Production")]
    public class AddColumnToEmployeesForBug2829 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblEmployees")
                 .AddForeignKeyColumn("ScheduleType", "ScheduleType", "ScheduleTypeID");
            // They do not want this
            //Execute.Sql("update tblEmployee set ScheduleType = (select top 1 ph.ScheduleTypeID from tblPosition_History ph where ph.tblEmployeeID = tblEmployee.tblEmployeeID order by position_start_date desc);");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblEmployees", "ScheduleType", "ScheduleType",
                "ScheduleTypeID");
        }
    }
}
