using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140313083545), Tags("Production")]
    public class OptimizeReferencesForBug1801 : Migration
    {
        public struct NewColumnNames
        {
            public struct Employees
            {
                public const string OPERATING_CENTER_ID = "OperatingCenterId",
                                    REPORTING_FACILITY_ID = "ReportingFacilityId";
            }
        }

        public override void Up()
        {
            Execute.Sql(
                "UPDATE tblEmployee SET OpCode = oc.OperatingCenterID FROM OperatingCenters oc WHERE oc.OperatingCenterCode = tblEmployee.OpCode");
            Execute.Sql("UPDATE tblEmployee SET OpCode = NULL WHERE OpCode = 'ALL';");
            Alter.Column("OpCode").OnTable("tblEmployee")
                 .AsInt32().ForeignKey("FK_tblEmployee_OperatingCenters_OperatingCenterId", "OperatingCenters",
                      "OperatingCenterId").Nullable();
            Rename.Column("OpCode").OnTable("tblEmployee").To(NewColumnNames.Employees.OPERATING_CENTER_ID);

            Execute.Sql(
                "UPDATE tblEmployee SET Reporting_FacilityId = f.RecordId FROM tblFacilities f WHERE f.FacilityId = tblEmployee.Reporting_FacilityId");
            Execute.Sql(
                "UPDATE tblEmployee SET Reporting_FacilityID = NULL WHERE Reporting_FacilityId NOT IN (SELECT FacilityId FROM tblFacilities);");
            Alter.Column("Reporting_FacilityId").OnTable("tblEmployee")
                 .AsInt32().ForeignKey("FK_tblEmployee_tblFacilities_ReportingFacilityId", "tblFacilities", "RecordId")
                 .Nullable();
            Rename.Column("Reporting_FacilityId").OnTable("tblEmployee")
                  .To(NewColumnNames.Employees.REPORTING_FACILITY_ID);

            Execute.Sql(@"ALTER VIEW [dbo].[Employees] AS
SELECT DISTINCT 
	tblEmployeeID, 
	replace(isNull(Last_Name,'') + ', ' + isNull(First_name, '') + ' ' + isNull(Middle_Name,'') + ' - '  + isNull(employeeID,''),'  ', ' ') + ' - ' + isNull(OperatingCenterCode, '')  as [FullName], 
	Last_Name, 
	OperatingCenterCode as OpCode, 
	Reports_To,
	Phone_Work,
	Phone_Cellular,
	EmailAddress
FROM 
	tblEmployee
LEFT JOIN
	OperatingCenters on OperatingCenters.OperatingCenterID = tblEmployee.OperatingCenterId");

            Execute.Sql(@"ALTER view [dbo].[ActiveEmployees] AS
SELECT distinct tblEmployeeID, replace(isNull(Last_Name,'') + ', ' + isNull(First_name, '') + ' ' + isNull(Middle_Name,'') + ' - '  + isNull(employeeID,''),'  ', ' ') + ' - ' + isNull(OperatingCenterCode, '')  as [FullName], Last_Name, OperatingCenterCode as OpCode from tblEmployee LEFT JOIN OperatingCenters on tblEmployee.OperatingCenterId = OperatingCenters.OperatingCenterId where inactive_date is null;");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_tblEmployee_OperatingCenters_OperatingCenterId").OnTable("tblEmployee");
            Rename.Column(NewColumnNames.Employees.OPERATING_CENTER_ID).OnTable("tblEmployee").To("OpCode");
            Alter.Column("OpCode").OnTable("tblEmployee").AsFixedLengthAnsiString(4).Nullable();
            Execute.Sql(
                "UPDATE tblEmployee SET OpCode = left(oc.OperatingCenterCode,4) FROM OperatingCenters oc WHERE oc.OperatingCenterId = tblEmployee.OpCode");

            Execute.Sql(@"ALTER VIEW [dbo].[Employees] AS
SELECT DISTINCT 
	tblEmployeeID, 
	replace(isNull(Last_Name,'') + ', ' + isNull(First_name, '') + ' ' + isNull(Middle_Name,'') + ' - '  + isNull(employeeID,''),'  ', ' ') + ' - ' + isNull(OpCode, '')  as [FullName], 
	Last_Name, 
	OpCode, 
	Reports_To,
	Phone_Work,
	Phone_Cellular,
	EmailAddress
FROM 
	tblEmployee");
            Execute.Sql(@"ALTER view [dbo].[ActiveEmployees] AS
SELECT distinct tblEmployeeID, replace(isNull(Last_Name,'') + ', ' + isNull(First_name, '') + ' ' + isNull(Middle_Name,'') + ' - '  + isNull(employeeID,''),'  ', ' ') + ' - ' + isNull(OpCode, '')  as [FullName], Last_Name, OpCode from tblEmployee where inactive_date is null");

            Delete.ForeignKey("FK_tblEmployee_tblFacilities_ReportingFacilityId").OnTable("tblEmployee");
            Rename.Column(NewColumnNames.Employees.REPORTING_FACILITY_ID).OnTable("tblEmployee")
                  .To("Reporting_FacilityId");
            Alter.Column("Reporting_FacilityId").OnTable("tblEmployee").AsAnsiString(8).Nullable();
            Execute.Sql(
                "UPDATE tblEmployee SET Reporting_FacilityId = f.FacilityId FROM tblFacilities f WHERE f.RecordId = tblEmployee.Reporting_FacilityId");
        }
    }
}
