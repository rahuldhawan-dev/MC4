using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150611132935314), Tags("Production")]
    public class UpdateHydrantRequiresInspectionFunctionForBug2431 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                ALTER FUNCTION [dbo].[HydrantRequiresInspection]
	                (	
		                @hydrantStatusId int, 
		                @hydrantBillingId int,
		                @inspFreqUnitId int,
		                @inspFreq int,
		                @lastInspectionDate datetime
	                )
                Returns BIT
                AS
                BEGIN
	                IF ('ACTIVE' <> (select Upper(hs.Description) from HydrantStatuses hs where hs.Id = @hydrantStatusId))									
		                RETURN 0 
	                IF ('PUBLIC' <> (select Upper(hb.Description) from HydrantBillings hb where hb.Id = @hydrantBillingId) AND (@inspFreq is null or @inspFreqUnitId is null))
		                RETURN 0 

	                -- Has not been inspected yet so yeah it requires inspection
	                IF (@lastInspectionDate is null)
		                RETURN 1 

	                declare @date datetime
	                set @date = getdate()
	                declare @inspFreqUnit varchar(10)
	                set @inspFreqUnit = (select rfu.Description from RecurringFrequencyUnits rfu where rfu.Id = @inspFreqUnitId) 

	                IF (@inspFreqUnit = 'Year')
			                IF (DateDiff(YY, @lastInspectionDate, @date) < @InspFreq)
				                RETURN 0
	                IF (@inspFreqUnit = 'Month')
			                IF (DateDiff(mm, @lastInspectionDate, @date) < @InspFreq)
				                RETURN 0
	                IF (@inspFreqUnit = 'Week')
			                IF (DateDiff(WW, @lastInspectionDate, @date) < @InspFreq)
				                RETURN 0
	                IF (@inspFreqUnit = 'Day')
			                IF (DateDiff(D, @lastInspectionDate, @date) < @InspFreq)
				                 RETURN 0 

	                RETURN 1
                END");
        }

        public override void Down()
        {
            Execute.Sql(@"
                ALTER FUNCTION [dbo].[HydrantRequiresInspection]
	                (	
		                @hydrantStatusId int, 
		                @hydrantBillingId int,
		                @inspFreqUnitId int,
		                @inspFreq int,
		                @lastInspectionDate datetime
	                )
                Returns BIT
                AS
                BEGIN
	                IF ('ACTIVE' <> (select Upper(hs.Description) from HydrantStatuses hs where hs.Id = @hydrantStatusId))									
		                RETURN 0 
	                IF ('PUBLIC' <> (select Upper(hb.Description) from HydrantBillings hb where hb.Id = @hydrantBillingId))
		                RETURN 0 

	                -- Has not been inspected yet so yeah it requires inspection
	                IF (@lastInspectionDate is null)
		                RETURN 1 

	                declare @date datetime
	                set @date = getdate()
	                declare @inspFreqUnit varchar(10)
	                set @inspFreqUnit = (select rfu.Description from RecurringFrequencyUnits rfu where rfu.Id = @inspFreqUnitId) 

	                IF (@inspFreqUnit = 'Year')
			                IF (DateDiff(YY, @lastInspectionDate, @date) < @InspFreq)
				                RETURN 0
	                IF (@inspFreqUnit = 'Month')
			                IF (DateDiff(mm, @lastInspectionDate, @date) < @InspFreq)
				                RETURN 0
	                IF (@inspFreqUnit = 'Week')
			                IF (DateDiff(WW, @lastInspectionDate, @date) < @InspFreq)
				                RETURN 0
	                IF (@inspFreqUnit = 'Day')
			                IF (DateDiff(D, @lastInspectionDate, @date) < @InspFreq)
				                 RETURN 0 

	                RETURN 1
                END");
        }
    }
}
