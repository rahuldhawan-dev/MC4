using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200821144747867), Tags("Production")]
    public class AddEntrantsForConfinedSpacePermitsForMC2500 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("ConfinedSpaceFormEntrantTypes", "Entrant", "Attendent",
                "Entry Supervisor");
            Create.Table("ConfinedSpaceFormEntrants")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ConfinedSpaceFormId", "ConfinedSpaceForms", nullable: false)
                  .WithForeignKeyColumn("ConfinedSpaceFormEntrantTypeId", "ConfinedSpaceFormEntrantTypes",
                       nullable: false)
                  .WithForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeId")
                  .WithColumn("ContractingCompany").AsAnsiString(255).Nullable()
                  .WithColumn("ContractorName").AsAnsiString(255).Nullable();
            Delete.ForeignKeyColumn("ConfinedSpaceForms", "AttendantEmployeeId", "tblEmployee");
            Delete.ForeignKeyColumn("ConfinedSpaceForms", "EntrantEmployeeId", "tblEmployee");
            Delete.ForeignKeyColumn("ConfinedSpaceForms", "EntrySupervisorEmployeeId", "tblEmployee");
        }

        public override void Down()
        {
            Alter.Table("ConfinedSpaceForms")
                 .AddForeignKeyColumn("EntrantEmployeeId", "tblEmployee", "tblEmployeeId").Nullable()
                 .AddForeignKeyColumn("AttendantEmployeeId", "tblEmployee", "tblEmployeeId").Nullable()
                 .AddForeignKeyColumn("EntrySupervisorEmployeeId", "tblEmployee", "tblEmployeeId").Nullable();
            Delete.Table("ConfinedSpaceFormEntrants");
            Delete.Table("ConfinedSpaceFormEntrantTypes");
        }
    }
}
