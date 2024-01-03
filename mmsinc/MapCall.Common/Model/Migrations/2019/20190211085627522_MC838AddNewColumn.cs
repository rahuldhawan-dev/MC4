using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190211085627522), Tags("Production")]
    public class MC838AddNewColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("LockoutForms").AddForeignKeyColumn("SAPEquipmentTypeId", "SapEquipmentTypes");
            Execute.Sql(
                "UPDATE LockoutForms SET SapEquipmentTypeId = E.SAPEquipmentTypeId FROM LockoutForms LF JOIN Equipment E on E.EquipmentID = LF.EquipmentId");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("LockoutForms", "SAPEquipmentTypeId", "SAPEquipmentTypes");
        }
    }
}
