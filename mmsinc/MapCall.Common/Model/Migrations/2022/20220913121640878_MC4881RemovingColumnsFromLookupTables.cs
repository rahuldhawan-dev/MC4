using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220913121640878), Tags("Production")]
    public class MC4881RemovingColumnsFromLookupTables : Migration
    {
        public override void Up()
        {
            Delete.Column("CreatedBy").FromTable("ContractorWorkCategoryTypes");
            Delete.Column("CreatedOn").FromTable("ContractorWorkCategoryTypes");
            Delete.Column("CreatedBy").FromTable("ContractorInsuranceMinimumRequirements");
            Delete.Column("CreatedOn").FromTable("ContractorInsuranceMinimumRequirements");
            Create.Table("ContractorContactTypes")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ContactTypeId", "ContactTypes", "ContactTypeID");
            Execute.Sql("INSERT INTO ContractorContactTypes Values (1),(2)");
        }

        public override void Down()
        {
            Delete.Table("ContractorContactTypes");
        }
    }
}

