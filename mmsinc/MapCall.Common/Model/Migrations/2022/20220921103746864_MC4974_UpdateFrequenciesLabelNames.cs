using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220921103746864), Tags("Production")]
    public class MC4974_UpdateFrequenciesLabelNames : Migration
    {
        public override void Up()
        {
            Update.Table("ProductionWorkOrderFrequencies").Set(new { Name = "Twice Per Month" }).Where(new { Name = "Bi-Monthly" });
            Update.Table("ProductionWorkOrderFrequencies").Set(new { Name = "Every Four Months" }).Where(new { Name = "Every 4 Months" });
            Update.Table("ProductionWorkOrderFrequencies").Set(new { Name = "Every Six Months" }).Where(new { Name = "Bi-Annual" });
        }

        public override void Down()
        {
            Update.Table("ProductionWorkOrderFrequencies").Set(new { Name = "Bi-Monthly" }).Where(new { Name = "Twice Per Month" });
            Update.Table("ProductionWorkOrderFrequencies").Set(new { Name = "Every 4 Months" }).Where(new { Name = "Every Four Months" });
            Update.Table("ProductionWorkOrderFrequencies").Set(new { Name = "Bi-Annual" }).Where(new { Name = "Every Six Months" });
        }
    }
}

