using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220810072847093), Tags("Production")]
    public class MC1255_AddCreatedByToFacility : Migration
    {
        public override void Up() =>
            Alter.Table("tblFacilities").AddForeignKeyColumn("CreatedById", "tblPermissions", "RecId");

        public override void Down() =>
            Delete.ForeignKeyColumn("tblFacilities", "CreatedById", "tblPermissions", "RecId");
    }
}
