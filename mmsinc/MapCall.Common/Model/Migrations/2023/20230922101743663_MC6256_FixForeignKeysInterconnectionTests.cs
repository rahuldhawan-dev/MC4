using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230922101743663), Tags("Production")]
    public class MC6256_FixForeignKeysInterconnectionTests : Migration
    {
        public override void Up()
        {
            Execute.Sql("if exists (select 1 from sysobjects where name = 'FK_InterconnectionTests_tblContractors_ContractorID') alter table InterconnectionTests drop constraint FK_InterconnectionTests_tblContractors_ContractorID");
            Create.ForeignKey(Utilities.CreateForeignKeyName("InterconnectionTests", "Contractors", "ContractorId"))
                  .FromTable("InterconnectionTests")
                  .ForeignColumn("ContractorId")
                  .ToTable("Contractors")
                  .PrimaryColumn("ContractorID");
            Execute.Sql("if exists (select 1 from sysobjects where name = 'tblContractors') drop table tblContractors");
        }

        public override void Down()
        {
            Delete.ForeignKey(Utilities.CreateForeignKeyName("InterconnectionTests", "Contractors", "ContractorId"))
                  .OnTable("InterconnectionTests");
        }
    }
}

