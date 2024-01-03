using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210311140736480), Tags("Production")]
    public class MC2546WellTestingForm : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("WellTestingForms")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ProductionWorkOrderId", "ProductionWorkOrders", "Id", false)
                  .WithForeignKeyColumn("EquipmentId", "Equipment", "EquipmentID", false)
                  .WithForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeID", false)
                  .WithColumn("PumpingRate").AsInt32().NotNullable()
                  .WithColumn("DateOfTest").AsDateTime().NotNullable()
                  .WithColumn("MeasurementPoint").AsDecimal(7, 2).NotNullable()
                  .WithColumn("StaticWaterLevel").AsDecimal(7, 2).NotNullable()
                  .WithColumn("PumpingWaterLevel").AsDecimal(7, 2).NotNullable();
        }
    }
}
