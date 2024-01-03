using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230803132356870), Tags("Production")]
    public class MC6056RemovePasswordFromTBLPermissions : Migration
    {
        public override void Up()
        {
            Delete.Column("Password").FromTable("tblPermissions");
        }

        public override void Down()
        {
            Alter.Table("tblPermissions").AddColumn("Password").AsString(20).NotNullable().WithDefaultValue("999999");
        }
    }
}

