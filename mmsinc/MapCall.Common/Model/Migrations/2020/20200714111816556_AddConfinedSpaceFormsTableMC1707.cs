using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200714111816556), Tags("Production")]
    public class AddConfinedSpaceFormsTableMC1707 : Migration
    {
        public override void Up()
        {
            Create.Table("ConfinedSpaceForms")
                  .WithIdentityColumn()

                   // SECTION 1
                  .WithForeignKeyColumn("ProductionWorkOrderId", "ProductionWorkOrders", "Id").NotNullable()
                  .WithColumn("GeneralDateTime").AsDateTime().NotNullable()
                  .WithColumn("LocationAndDescriptionOfConfinedSpace").AsAnsiString(255).NotNullable()
                  .WithColumn("PurposeOfEntry").AsAnsiString(255).NotNullable()

                   // SECTION 2 done in another ticket

                   // SECTION 3
                  .WithColumn("ReclassificationSignedAt").AsDateTime().Nullable()
                  .WithForeignKeyColumn("ReclassificationSignedByEmployeeId", "tblEmployee", "tblEmployeeId").Nullable()

                   // SECTION 4
                  .WithColumn("CanBeControlledByVentilationAlone").AsBoolean().Nullable()
                  .WithColumn("HazardSignedAt").AsDateTime().Nullable()
                  .WithForeignKeyColumn("HazardSignedByEmployeeId", "tblEmployee", "tblEmployeeId").Nullable();

            // SECTION 5 done in another ticket
        }

        public override void Down()
        {
            Delete.Table("ConfinedSpaceForms");
        }
    }
}
