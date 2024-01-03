using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170914111248252), Tags("Production")]
    public class Bug4061FixContractors : Migration
    {
        public override void Up()
        {
            Execute.Sql("update Contractors set ContractorsAccess = 0 where ContractorsAccess is null");
            Alter.Column("ContractorsAccess").OnTable("Contractors").AsBoolean().NotNullable();
            Create.Column("HasAccess").OnTable("ContractorUsers").AsBoolean().NotNullable().WithDefaultValue(true);

            Execute.Sql("update ContractorUsers set HasAccess = 0 where Password = '00000'");
        }

        public override void Down()
        {
            Alter.Column("ContractorsAccess").OnTable("Contractors").AsBoolean().Nullable();
            Delete.Column("HasAccess").FromTable("ContractorUsers");
        }
    }
}
