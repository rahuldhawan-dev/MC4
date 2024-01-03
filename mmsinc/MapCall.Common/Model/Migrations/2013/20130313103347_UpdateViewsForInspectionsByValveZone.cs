using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130313103347), Tags("Production")]
    public class UpdateViewsForInspectionsByValveZone : Migration
    {
        public struct Sql
        {
            public const string
                ORIGINAL_VIEW = @"

ALTER view [dbo].[tblNJAWValInspDataInfo] as
select 
	tblNJAWValves.valnum, 
	tblNJAWValves.opCntr, 
	tblNJAWValInspLastNonInsp.LastNonInspect, 
	tblNJAWValInspLastInsp.LastInspect, 
	case when (tblNJAWValInspLastNonInsp.LastNonInspect is null) then #2.WoReq1 else tblNJAWValInspData.WoReq1 end as WoReq1, 
	dbo.RequiresInspectionValveByZone( ValveSize, ValCtrl, ValveStatus, BPUKPI, BillInfo, (Select max(dateInspect) from tblNJAWValInspData where UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr) , DateInst, getDate(), ValveZone) as [required]
	from tblNJAWValves 
		left join tblNJAWValInspLastNonInsp on tblNJAWValInspLastNonInsp.ValNum = tblNJAWValves.ValNum and tblNJAWValInspLastNonInsp.OpCntr = tblNJAWValves.OpCntr
		left join tblNJAWValInspLastInsp on tblNJAWValInspLastInsp.ValNum = tblNJAWValves.ValNum and tblNJAWValInspLastInsp.OpCntr = tblNJAWValves.OpCntr
		left join tblNJAWValInspData on tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.OpCntr and tblNJAWValInspLastNonInsp.LastNonInspect = tblNJAWValInspData.DateInspect
		left join tblNJAWValInspData #2 on #2.ValNum = tblNJAWValves.ValNum and #2.OpCntr = tblNJAWValves.OpCntr and tblNJAWValInspLastInsp.LastInspect = #2.DateInspect
",
                NEW_VIEW = @"
ALTER view [dbo].[tblNJAWValInspDataInfo] as
select 
	tblNJAWValves.valnum, 
	tblNJAWValves.opCntr, 
	tblNJAWValInspLastNonInsp.LastNonInspect, 
	tblNJAWValInspLastInsp.LastInspect, 
	case when (tblNJAWValInspLastNonInsp.LastNonInspect is null) then #2.WoReq1 else tblNJAWValInspData.WoReq1 end as WoReq1, 
	dbo.RequiresInspectionValveByZone( ValveSize, ValCtrl, ValveStatus, BPUKPI, BillInfo, (Select max(dateInspect) from tblNJAWValInspData where UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr) , DateInst, getDate(), ValveZone, tblNJAWValves.Town) as [required]
	from tblNJAWValves 
		left join tblNJAWValInspLastNonInsp on tblNJAWValInspLastNonInsp.ValNum = tblNJAWValves.ValNum and tblNJAWValInspLastNonInsp.OpCntr = tblNJAWValves.OpCntr
		left join tblNJAWValInspLastInsp on tblNJAWValInspLastInsp.ValNum = tblNJAWValves.ValNum and tblNJAWValInspLastInsp.OpCntr = tblNJAWValves.OpCntr
		left join tblNJAWValInspData on tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.OpCntr and tblNJAWValInspLastNonInsp.LastNonInspect = tblNJAWValInspData.DateInspect
		left join tblNJAWValInspData #2 on #2.ValNum = tblNJAWValves.ValNum and #2.OpCntr = tblNJAWValves.OpCntr and tblNJAWValInspLastInsp.LastInspect = #2.DateInspect
";
        }

        public override void Up()
        {
            Execute.Sql(Sql.NEW_VIEW);
        }

        public override void Down()
        {
            //Execute.Sql(Sql.ORIGINAL_VIEW);
        }
    }
}
