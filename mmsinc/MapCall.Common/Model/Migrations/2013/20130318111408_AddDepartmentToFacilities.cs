using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130318111408), Tags("Production")]
    public class AddDepartmentToFacilities : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities").AddColumn("DepartmentID").AsInt32().Nullable();
            Create.ForeignKey("FK_tblFacilities_Departments_DepartmentID")
                  .FromTable("tblFacilities").ForeignColumn("DepartmentID")
                  .ToTable("Departments").PrimaryColumn("DepartmentID");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_tblFacilities_Departments_DepartmentID").OnTable("tblFacilities");
            Delete.Column("DepartmentID").FromTable("tblFacilities");
        }
    }
}
