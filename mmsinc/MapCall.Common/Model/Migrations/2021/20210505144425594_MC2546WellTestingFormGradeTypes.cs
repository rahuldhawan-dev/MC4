using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210505144425594), Tags("Production")]
    public class MC2546WellTestingFormGradeTypes : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("WellTestingFormGradeTypes",
                "Above Grade", 
                "Below Grade");

            Alter.Table("WellTestingForms")
                 .AddForeignKeyColumn("GradeTypeId", "WellTestingFormGradeTypes");

            // Default existing records (which are test only) to 'above grade' grade type.
            Execute.Sql("update WellTestingForms set GradeTypeId = 1");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WellTestingForms", "GradeTypeId", "WellTestingFormGradeTypes");
            Delete.Table("WellTestingFormGradeTypes");
        }
    }
}

