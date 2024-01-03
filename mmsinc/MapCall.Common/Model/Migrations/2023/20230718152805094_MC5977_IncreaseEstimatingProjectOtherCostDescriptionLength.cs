using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230718152805094), Tags("Production")]
    public class MC5977_IncreaseEstimatingProjectOtherCostDescriptionLength : Migration
    {
        public override void Up()
        {
            Alter.Table("EstimatingProjectOtherCosts")
                 .AlterColumn("Description").AsAnsiString(250).NotNullable();
        }

        //We don't do rollbacks for length changes because the length change would cause an error if anyone enters a value greater than length being rolled-back to.
        public override void Down() { }
    }
}

