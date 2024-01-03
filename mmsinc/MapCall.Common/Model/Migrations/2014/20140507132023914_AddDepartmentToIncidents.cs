using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140507132023914), Tags("Production")]
    public class AddDepartmentToIncidents : Migration
    {
        public override void Up()
        {
            Alter.Table("Incidents")
                 .AddColumn("DepartmentId")
                 .AsInt32()
                 .ForeignKey("FK_Incidents_EmployeeDepartments_DepartmentId", "EmployeeDepartments", "Id")
                 .Nullable();

            Execute.Sql(
                "UPDATE Incidents SET DepartmentId = d.Id FROM tblEmployee e INNER JOIN EmployeeDepartments d ON e.DepartmentId = d.Id WHERE e.tblEmployeeID = Incidents.EmployeeId");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Incidents_EmployeeDepartments_DepartmentId").OnTable("Incidents");
            Delete.Column("DepartmentID").FromTable("Incidents");
        }
    }
}
