using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200617101958437), Tags("Production")]
    public class MC2313AddLicensedOperatorOfRecordToOperatorLicense : Migration
    {
        public override void Up()
        {
            Alter.Table("OperatorLicenses").AddColumn("LicensedOperatorOfRecord").AsBoolean().NotNullable()
                 .WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("LicensedOperatorOfRecord").FromTable("OperatorLicenses");
        }
    }
}
