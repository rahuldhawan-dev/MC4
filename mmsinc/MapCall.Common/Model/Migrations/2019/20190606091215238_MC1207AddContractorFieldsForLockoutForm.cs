using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190606091215238), Tags("Production")]
    public class MC1207AddContractorFieldsForLockoutForm : Migration
    {
        public override void Up()
        {
            Alter.Table("LockoutForms")
                 .AddForeignKeyColumn("ContractorId", "Contractors", "ContractorId")
                 .AddColumn("ContractorFirstName").AsAnsiString(255)
                 .Nullable()
                 .AddColumn("ContractorLastName").AsAnsiString(255)
                 .Nullable()
                 .AddColumn("ContractorPhone").AsAnsiString(20).Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("LockoutForms", "ContractorId", "Contractors", "ContractorId");
            Delete.Column("ContractorFirstName").FromTable("LockoutForms");
            Delete.Column("ContractorLastName").FromTable("LockoutForms");
            Delete.Column("ContractorPhone").FromTable("LockoutForms");
        }
    }
}
