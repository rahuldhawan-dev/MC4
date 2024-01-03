using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230109122317372), Tags("Production")]
    public class MC5212AddDescriptionOfWorkToPreJobSafetyBrief : Migration
    {
        public override void Up()
        {
            Create.Column("DescriptionOfWork")
                  .OnTable("ProductionPreJobSafetyBriefs")
                  .AsString(100)
                  .Nullable();
        }

        public override void Down()
        {
            Delete.Column("DescriptionOfWork")
                  .FromTable("ProductionPreJobSafetyBriefs");
        }
    }
}

