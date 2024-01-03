using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140520094355570), Tags("Production")]
    public class AlterViewForTerribleValveInspectionProcess : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"ALTER view [dbo].[tblNJAWBOInspDataInfo] as 
                            select 
	                            tblNJAWValves.valnum, 
	                            tblNJAWValves.opCntr, 
	                            tblNJAWHydInspLastNonInsp.LastNonInspect, 
	                            tblNJAWHydInspLastInsp.LastInspect, 
	                            tblNJAWHydInspData.WoReq1, 
	                            dbo.RequiresInspectionHydrant(tblNJAWValves.Town, ValveStatus, BillInfo, getDate(), dateInst, (Select max(dateInspect) from tblNJAWHydInspData where tblNJAWHydInspData.HydNum = tblNJAWValves.ValNum and tblNJAWHydInspData.OpCntr = tblNJAWValves.opCntr and isNull(Inspect,'') in ('Inspect','INSPECT/FLUSH')),1, 'Y') as [required]
	                            from tblNJAWValves 
		                            left join tblNJAWHydInspLastNonInsp on tblNJAWHydInspLastNonInsp.HydNum = tblNJAWValves.ValNum and tblNJAWHydInspLastNonInsp.OpCntr = tblNJAWValves.OpCntr
		                            left join tblNJAWHydInspLastInsp on tblNJAWHydInspLastInsp.HydNum = tblNJAWValves.ValNum and tblNJAWHydInspLastInsp.OpCntr = tblNJAWValves.OpCntr
		                            left join tblNJAWHydInspData on tblNJAWHydInspData.HydNum = tblNJAWValves.ValNum and tblNJAWHydInspData.OpCntr = tblNJAWValves.OpCntr and tblNJAWHydInspLastNonInsp.LastNonInspect = tblNJAWHydInspData.DateInspect
                        ");
        }

        public override void Down()
        {
            Execute.Sql(@"ALTER view [dbo].[tblNJAWBOInspDataInfo] as 
                            select 
	                            tblNJAWValves.valnum, 
	                            tblNJAWValves.opCntr, 
	                            tblNJAWHydInspLastNonInsp.LastNonInspect, 
	                            tblNJAWHydInspLastInsp.LastInspect, 
	                            tblNJAWHydInspData.WoReq1, 
	                            dbo.RequiresInspectionHydrant(tblNJAWValves.Town, ValveStatus, BillInfo, getDate(), dateInst, (Select max(dateInspect) from tblNJAWHydInspData where tblNJAWHydInspData.HydNum = tblNJAWValves.ValNum and tblNJAWHydInspData.OpCntr = tblNJAWValves.opCntr and isNull(Inspect,'') in ('Inspect','INSPECT/FLUSH')),InspFreq, InspFreqUnit) as [required]
	                            from tblNJAWValves 
		                            left join tblNJAWHydInspLastNonInsp on tblNJAWHydInspLastNonInsp.HydNum = tblNJAWValves.ValNum and tblNJAWHydInspLastNonInsp.OpCntr = tblNJAWValves.OpCntr
		                            left join tblNJAWHydInspLastInsp on tblNJAWHydInspLastInsp.HydNum = tblNJAWValves.ValNum and tblNJAWHydInspLastInsp.OpCntr = tblNJAWValves.OpCntr
		                            left join tblNJAWHydInspData on tblNJAWHydInspData.HydNum = tblNJAWValves.ValNum and tblNJAWHydInspData.OpCntr = tblNJAWValves.OpCntr and tblNJAWHydInspLastNonInsp.LastNonInspect = tblNJAWHydInspData.DateInspect
                            ");
        }
    }
}
