using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230118114849875), Tags("Production")]
    public class MC4834_AddNumbersToMainInspectionGradeDescriptions : Migration
    {
        public override void Up()
        {
            Update.Table("SewerMainInspectionGrades")
                  .Set(new { Description = "1 - EXCELLENT" })
                  .Where(new { Description = "EXCELLENT" });
            Update.Table("SewerMainInspectionGrades")
                  .Set(new { Description = "2 - GOOD" })
                  .Where(new { Description = "GOOD" });
            Update.Table("SewerMainInspectionGrades")
                  .Set(new { Description = "3 - FAIR" })
                  .Where(new { Description = "FAIR" });
            Update.Table("SewerMainInspectionGrades")
                  .Set(new { Description = "4 - POOR" })
                  .Where(new { Description = "POOR" });
            Update.Table("SewerMainInspectionGrades")
                  .Set(new { Description = "5 - IMMEDIATE ATTENTION" })
                  .Where(new { Description = "IMMEDIATE ATTENTION" });
        }

        public override void Down()
        {
            Update.Table("SewerMainInspectionGrades")
                  .Set(new { Description = "EXCELLENT" })
                  .Where(new { Description = "1 - EXCELLENT" });
            Update.Table("SewerMainInspectionGrades")
                  .Set(new { Description = "GOOD" })
                  .Where(new { Description = "2 - GOOD" });
            Update.Table("SewerMainInspectionGrades")
                  .Set(new { Description = "FAIR" })
                  .Where(new { Description = "3 - FAIR" });
            Update.Table("SewerMainInspectionGrades")
                  .Set(new { Description = "POOR" })
                  .Where(new { Description = "4 - POOR" });
            Update.Table("SewerMainInspectionGrades")
                  .Set(new { Description = "IMMEDIATE ATTENTION" })
                  .Where(new { Description = "5 - IMMEDIATE ATTENTION" });
        }
    }
}

