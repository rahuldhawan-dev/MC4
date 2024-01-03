using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200417095831988), Tags("Production")]
    public class MC2204DecreaseContractorUserEmailLength : Migration
    {
        public override void Up()
        {
            // unique constraint needs to be deleted in order to alter the column. Throws sql error otherwise.
            Delete.UniqueConstraint("UQ__Contract__A9D105340AF43244").FromTable("ContractorUsers").Column("Email");
            Alter.Column("Email").OnTable("ContractorUsers").AsString(254).NotNullable();

            // recreate unique constraint
            Create.UniqueConstraint("UQ__Contract__A9D105340AF43244").OnTable("ContractorUsers").Column("Email");
        }

        public override void Down()
        {
            // No rolling back the column length. Truncation errors will occur.
        }
    }
}
