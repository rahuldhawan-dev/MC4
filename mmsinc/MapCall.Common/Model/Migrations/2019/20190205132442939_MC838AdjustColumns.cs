using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190205132442939), Tags("Production")]
    public class MC838AdjustColumns : Migration
    {
        public override void Up()
        {
            Delete.Column("Address").FromTable("LockoutForms");
            Delete.ForeignKeyColumn("LockoutForms", "CoordinateId", "Coordinates", "CoordinateId");
        }

        public override void Down()
        {
            Alter.Table("LockoutForms").AddColumn("Address").AsCustom("text").Nullable();
            Alter.Table("LockoutForms").AddForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateId");
        }
    }
}
