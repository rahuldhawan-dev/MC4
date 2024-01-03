using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150323161444051), Tags("Production")]
    public class UpdatePremiseNumbersForBug2365 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                update
	                TapImages
                set
	                PremiseNumber = '9' + PremiseNumber
                where 
	                PremiseNumber like '52%'
                and
	                len(PremiseNumber) = 9;

                update
	                WorkOrders 
                set
	                PremiseNumber = '9' + PremiseNumber
                where
	                PremiseNumber like '52%'
                and
	                len(premiseNumber) = 9;

                update
	                tblNJAWService
                set
	                PremNum = '9' + PremNum
                where
	                PremNum like '52%'
                and
	                len(premNum) = 9
            ");
        }

        public override void Down() { }
    }
}
