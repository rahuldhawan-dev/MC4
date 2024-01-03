using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140114104849), Tags("Production")]
    public class FixReportStoredProceduresFor1623 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"ALTER PROCEDURE [dbo].[rpt_HR_Seniority] (@LocalID int)
AS
SELECT 
		LocalBargainingUnits.Name as Local,
		Seniority_Ranking, 
		Position_Start_Date, 
		Last_Name, 
		First_Name, 
		Position,
		ScheduleType,
		Date_Hired,
		Seniority_Date
		
	FROM tblEmployee
	LEFT JOIN tblPosition_History on tblPosition_History.tblemployeeID = tblEmployee.tblemployeeID
	LEFT JOIN scheduletype ON scheduletype.scheduletypeID = tblPosition_History.scheduletypeID
	LEFT JOIN tblPositions_Classifications on tblPositions_Classifications.PositionID = tblPosition_History.Position_ID
	LEFT JOIN LocalBargainingUnits on LocalBargainingUnits.Id = tblPositions_Classifications.LocalID
	where tblPosition_History.Position_End_Date is null and LocalBargainingUnits.Id = @LocalID
	Order by Seniority_Ranking

GO");
        }

        public override void Down()
        {
            Execute.Sql(@"ALTER PROCEDURE [dbo].[rpt_HR_Seniority] (@LocalID int)
AS
SELECT 
		Local,
		Seniority_Ranking, 
		Position_Start_Date, 
		Last_Name, 
		First_Name, 
		Position,
		ScheduleType,
		Date_Hired,
		Seniority_Date
		
	FROM tblEmployee
	LEFT JOIN tblPosition_History on tblPosition_History.tblemployeeID = tblEmployee.tblemployeeID
	LEFT JOIN scheduletype ON scheduletype.scheduletypeID = tblPosition_History.scheduletypeID
	LEFT JOIN tblPositions_Classifications on tblPositions_Classifications.PositionID = tblPosition_History.Position_ID
	LEFT JOIN tblBargaining_Unit_Local on tblBargaining_Unit_Local.LocalID = tblPositions_Classifications.LocalID
	where tblPosition_History.Position_End_Date is null and tblBargaining_Unit_Local.LocalID = @LocalID
	Order by Seniority_Ranking

GO");
        }
    }
}
