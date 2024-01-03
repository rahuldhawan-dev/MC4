using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190424125509227), Tags("Production")]
    public class MC1130AddAgreementColumnForLockoutForms : Migration
    {
        public override void Up()
        {
            Alter.Table("LockoutForms")
                 .AddColumn("EmployeeAcknowledgedTraining").AsBoolean().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("EmployeeAcknowledgedTraining").FromTable("LockoutForms");
        }
    }
}
