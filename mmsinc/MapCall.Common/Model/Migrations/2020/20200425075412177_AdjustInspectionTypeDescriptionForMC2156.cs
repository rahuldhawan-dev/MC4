using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200425075412177), Tags("Production")]
    public class AdjustInspectionTypeDescriptionForMC2156 : Migration
    {
        public override void Up()
        {
            Update.Table("HydrantInspectionTypes")
                  .Set(new {Description = "WQ Complaint"})
                  .Where(new {Description = "WATER QUALITY"});
        }

        public override void Down()
        {
            Update.Table("HydrantInspectionTypes")
                  .Set(new {Description = "WATER QUALITY"})
                  .Where(new {Description = "WQ Complaint"});
        }
    }
}
