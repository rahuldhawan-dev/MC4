using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200107100057569), Tags("Production")]
    public class MC1836AddEnvironmentalPermitFees : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("EnvironmentalPermitFeeTypes", "Permit", "Membership", "Withdrawal");
            this.CreateLookupTableWithValues("EnvironmentalPermitFeePaymentMethods", "Mail", "Phone", "URL");
            // Annoyingly, some of these fields are being set to nullable, but they will be required fields.
            // This is because we can't automatically import the needed values for existing permits.
            Create.Table("EnvironmentalPermitFees")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("EnvironmentalPermitId", "EnvironmentalPermits", "EnvironmentalPermitID")
                  .NotNullable()
                  .WithColumn("Fee").AsDecimal(18, 2).NotNullable()
                  .WithColumn("PaymentEffectiveDate").AsCustom("date").Nullable()
                  .WithForeignKeyColumn("EnvironmentalPermitFeeTypeId", "EnvironmentalPermitFeeTypes").Nullable()
                  .WithColumn("PaymentDueInterval").AsInt32().Nullable()
                  .WithForeignKeyColumn("PaymentDueRecurringFrequencyUnitId", "RecurringFrequencyUnits").Nullable()
                  .WithColumn("PaymentOrganizationName").AsCustom("varchar(200)").Nullable()
                  .WithColumn("PaymentOrganizationContactInfo").AsCustom("varchar(max)").Nullable()
                  .WithForeignKeyColumn("EnvironmentalPermitFeePaymentMethodId", "EnvironmentalPermitFeePaymentMethods")
                  .Nullable()
                  .WithColumn("PaymentMethodMailAddress").AsCustom("varchar(max)").Nullable()
                  .WithColumn("PaymentMethodUrl").AsCustom("varchar(2000)").Nullable()
                  .WithColumn("PaymentMethodPhone").AsCustom("varchar(20)").Nullable()
                  .WithForeignKeyColumn("PaymentOwnerEmployeeId", "tblEmployee", "tblEmployeeId").Nullable();

            Create.Index().OnTable("EnvironmentalPermitFees").OnColumn("EnvironmentalPermitId");

            Create.Column("RequiresFees").OnTable("EnvironmentalPermits").AsBoolean().NotNullable()
                  .WithDefaultValue(false);
            Execute.Sql("update EnvironmentalPermits set RequiresFees = 'true' where AnnualFee is not null");

            // Move existing AnnualFee values to the new fees table.
            Execute.Sql(@"
insert into EnvironmentalPermitFees (EnvironmentalPermitId, Fee)
select
EnvironmentalPermitId, 
Fee = AnnualFee
from EnvironmentalPermits 
where AnnualFee is not null
");
            Delete.Column("AnnualFee").FromTable("EnvironmentalPermits");
        }

        public override void Down()
        {
            // Recreate AnnualFee column and data
            Create.Column("AnnualFee").OnTable("EnvironmentalPermits").AsDecimal(18, 2).Nullable();
            Execute.Sql(
                @"update permit set AnnualFee = (select top 1 Fee from EnvironmentalPermitFees epf where epf.EnvironmentalPermitId = permit.EnvironmentalPermitId)
                          from EnvironmentalPermits permit");

            Delete.Index().OnTable("EnvironmentalPermitFees").OnColumn("EnvironmentalPermitId");

            Delete.ForeignKeyColumn("EnvironmentalPermitFees", "EnvironmentalPermitId", "EnvironmentalPermits",
                "EnvironmentalPermitID");
            Delete.ForeignKeyColumn("EnvironmentalPermitFees", "EnvironmentalPermitFeeTypeId",
                "EnvironmentalPermitFeeTypes");
            Delete.ForeignKeyColumn("EnvironmentalPermitFees", "EnvironmentalPermitFeePaymentMethodId",
                "EnvironmentalPermitFeePaymentMethods");
            Delete.ForeignKeyColumn("EnvironmentalPermitFees", "PaymentOwnerEmployeeId", "tblEmployee",
                "tblEmployeeId");
            Delete.Table("EnvironmentalPermitFees");
            Delete.Table("EnvironmentalPermitFeePaymentMethods");
            Delete.Table("EnvironmentalPermitFeeTypes");
            Delete.Column("RequiresFees").FromTable("EnvironmentalPermits");
        }
    }
}
