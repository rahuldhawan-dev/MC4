using FluentMigrator;
using FluentMigrator.Expressions;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210202094401000), Tags("Production")]
    public class MC2825AddGrievanceLookAndMatrix : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("GrievanceCategories", "Benefit", "Compensation", "Discipline", "Management Rights", "Pay Rule", "Position Related");
            Alter.Table("GrievanceCategorizations").AddForeignKeyColumn("GrievanceCategoryId", "GrievanceCategories").Nullable();

            void addGrievanceCategories(int grievanceCategorizationsId, int grievanceCategory)
            {
                Update.Table("GrievanceCategorizations").Set(new { GrievanceCategoryId = grievanceCategory }).Where(new { Id = grievanceCategorizationsId });
            }

            addGrievanceCategories(1, 1);
            addGrievanceCategories(2, 1);
            addGrievanceCategories(3, 1);
            addGrievanceCategories(4, 1);
            addGrievanceCategories(5, 1);
            addGrievanceCategories(6, 6);
            addGrievanceCategories(7, 2);
            addGrievanceCategories(8, 3);
            addGrievanceCategories(9, 3);
            addGrievanceCategories(10, 3);
            addGrievanceCategories(11, 3);
            addGrievanceCategories(12, 3);
            addGrievanceCategories(13, 3);
            addGrievanceCategories(14, 3);
            addGrievanceCategories(15, 1);
            addGrievanceCategories(16, 4);
            addGrievanceCategories(17, 1);
            addGrievanceCategories(18, 5);
            addGrievanceCategories(19, 6);
            addGrievanceCategories(20, 4);
            addGrievanceCategories(21, 1);
            addGrievanceCategories(22, 1);

            Alter.Table("UnionGrievances").AddForeignKeyColumn("GrievanceCategoryId", "GrievanceCategories", "Id").Nullable();

            Execute.Sql($@"
            update UnionGrievances
            set UnionGrievances.GrievanceCategoryId = gc.GrievanceCategoryId from GrievanceCategorizations gc
            where UnionGrievances.CategorizationId = gc.Id
            ");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("GrievanceCategorizations", "GrievanceCategoryId", "GrievanceCategories", "Id");
            Delete.ForeignKeyColumn("UnionGrievances", "GrievanceCategoryId", "GrievanceCategories");
            Delete.Table("GrievanceCategories");
        }
    }
}