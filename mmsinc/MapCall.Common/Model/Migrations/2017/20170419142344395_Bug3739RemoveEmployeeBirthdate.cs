using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170419142344395), Tags("Production")]
    public class Bug3739RemoveEmployeeBirthdate : Migration
    {
        public override void Up()
        {
            Delete.Column("Birthdate").FromTable("tblEmployee");
        }

        public override void Down()
        {
            Create.Column("Birthdate").OnTable("tblEmployee").AsDateTime().Nullable();
        }
    }
}
