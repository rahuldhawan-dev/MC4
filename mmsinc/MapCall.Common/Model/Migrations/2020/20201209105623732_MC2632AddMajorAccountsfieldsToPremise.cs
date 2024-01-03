using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201209105623732), Tags("Production")]
    public class MC2632AddMajorAccountsFieldsToPremise : Migration
    {
        public override void Up()
        {
            Alter.Table("Premises")
                 .AddColumn("IsMajorAccount").AsBoolean().NotNullable().WithDefaultValue(false)
                 .AddColumn("MajorAccountManager").AsString(100).Nullable()
                 .AddColumn("AccountManagerContactNumber").AsString(20).Nullable()
                 .AddColumn("AccountManagerEmail").AsString(255).Nullable();
        }

        public override void Down()
        {
            Delete.Column("IsMajorAccount").FromTable("Premises");
            Delete.Column("MajorAccountManager").FromTable("Premises");
            Delete.Column("AccountManagerContactNumber").FromTable("Premises");
            Delete.Column("AccountManagerEmail").FromTable("Premises");
        }
    }
}
