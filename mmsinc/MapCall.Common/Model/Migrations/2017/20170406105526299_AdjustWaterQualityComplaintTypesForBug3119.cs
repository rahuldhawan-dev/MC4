using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170406105526299), Tags("Production")]
    public class AdjustWaterQualityComplaintTypesForBug3119 : Migration
    {
        private object[] Rows => new object[] {
            new {Description = "Aesthetic-Stained Laundry"},
            new {Description = "Aesthetic-Discolored Water"}
        };

        public override void Up()
        {
            Insert.IntoTable("WaterQualityComplaintTypes").Rows(Rows);
        }

        public override void Down()
        {
            Delete.FromTable("WaterQualityComplaintTypes").Rows(Rows);
        }
    }
}
