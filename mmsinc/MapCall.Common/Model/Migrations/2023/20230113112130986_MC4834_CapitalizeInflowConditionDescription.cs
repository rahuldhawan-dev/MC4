using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230113112130986), Tags("Production")]
    public class MC4834_CapitalizeInflowConditionDescription : Migration
    {
        public override void Up()
        {
            Update.Table("OpeningConditions")
                  .Set(new {Description = "INFLOW"})
                  .Where(new {Description = "Inflow"});
        }

        public override void Down() { } // no need to reverse
    }
}

