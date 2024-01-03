using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160411150314573), Tags("Production")]
    public class AddFieldsToLockoutFormsForBug2873 : Migration
    {
        public override void Up()
        {
            Create.Table("WaysToRemoveLocks")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(25).NotNullable();

            Execute.Sql("INSERT INTO WaysToRemoveLocks (Description) VALUES ('Master Key');");
            Execute.Sql("INSERT INTO WaysToRemoveLocks (Description) VALUES ('Bolt Cutter');");

            Alter.Table("LockoutForms")
                 .AddColumn("SameAsInstaller").AsBoolean().Nullable()
                 .AddColumn("ConfirmedByManagement").AsBoolean().Nullable()
                 .AddColumn("ReasonableEffortMade").AsBoolean().Nullable()
                 .AddForeignKeyColumn("SupervisorInvolvedId", "tblEmployee", "tblEmployeeId").Nullable()
                 .AddColumn("DateOfContact").AsDateTime().Nullable()
                 .AddColumn("MethodOfContact").AsString(StringLengths.METHOD_OF_CONTACT).Nullable()
                 .AddColumn("OutcomeOfContact").AsCustom("text").Nullable()
                 .AddColumn("AuthorizedManagementApproved").AsBoolean().Nullable()
                 .AddForeignKeyColumn("AuthorizedManagementPersonId", "tblEmployee", "tblEmployeeId")
                 .Nullable()
                 .AddColumn("SupervisorEnsuresKnowledge").AsBoolean().Nullable()
                 .AddForeignKeyColumn("LockRemovedById", "WaysToRemoveLocks").Nullable();
        }

        public override void Down()
        {
            Delete.Column("SameAsInstaller").FromTable("LockoutForms");
            Delete.Column("ConfirmedByManagement").FromTable("LockoutForms");
            Delete.Column("ReasonableEffortMade").FromTable("LockoutForms");
            Delete.Column("DateOfContact").FromTable("LockoutForms");
            Delete.Column("MethodOfContact").FromTable("LockoutForms");
            Delete.Column("OutcomeOfContact").FromTable("LockoutForms");
            Delete.Column("SupervisorEnsuresKnowledge").FromTable("LockoutForms");
            Delete.Column("AuthorizedManagementApproved").FromTable("LockoutForms");

            Delete.ForeignKeyColumn("LockoutForms", "SupervisorInvolvedId", "tblEmployee",
                "tblEmployeeId");
            Delete.ForeignKeyColumn("LockoutForms", "AuthorizedManagementPersonId", "tblEmployee",
                "tblEmployeeId");
            Delete.ForeignKeyColumn("LockoutForms", "LockRemovedById", "WaysToRemoveLocks");

            Delete.Table("WaysToRemoveLocks");
        }

        private struct StringLengths
        {
            public const int METHOD_OF_CONTACT = 25;
        }
    }
}
