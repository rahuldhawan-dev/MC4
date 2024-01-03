using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150519082225007), Tags("Production")]
    public class CreateInspectionFunctionsBug2223 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
-- This function is used by NHibernate for the Hydrant.RequiresInspection mapping. This is strictly for performance!

CREATE FUNCTION [dbo].[HydrantRequiresInspection]
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
            Execute.Sql("grant exec on HydrantRequiresInspection to mcuser");
        }

        public override void Down()
        {
            Execute.Sql(@"DROP FUNCTION [dbo].[HydrantRequiresInspection]");
        }
    }
}
