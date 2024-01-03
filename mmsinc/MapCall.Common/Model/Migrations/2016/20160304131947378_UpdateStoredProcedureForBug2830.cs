using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160304131947378), Tags("Production")]
    public class UpdateStoredProcedureForBug2830 : Migration
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
		convert(varchar,Date_Hired,101) as Date_Hired,
		convert(varchar,Seniority_Date,101) as Seniority_Date
		
	FROM tblEmployee
	LEFT JOIN tblPosition_History on tblPosition_History.tblemployeeID = tblEmployee.tblemployeeID
	LEFT JOIN scheduletype ON scheduletype.scheduletypeID = tblPosition_History.scheduletypeID
	LEFT JOIN tblPositions_Classifications on tblPositions_Classifications.PositionID = tblPosition_History.Position_ID
	LEFT JOIN LocalBargainingUnits on LocalBargainingUnits.Id = tblPositions_Classifications.LocalID
	where tblPosition_History.Position_End_Date is null and LocalBargainingUnits.Id = @LocalID
	and tblEmployee.StatusId = 1
	Order by Seniority_Ranking");
        }

        public override void Down()
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
	where tblPosition_History.Pos");
        }
    }
}
