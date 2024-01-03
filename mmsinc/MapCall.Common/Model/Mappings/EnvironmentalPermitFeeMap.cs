using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EnvironmentalPermitFeeMap : ClassMap<EnvironmentalPermitFee>
    {
        public EnvironmentalPermitFeeMap()
        {
            Id(x => x.Id);

            // A bunch of these are marked as nullable because we couldn't import
            // existing values from EnvironmentalPermit with default values.
            // Consider switching these back to not nullable if they ever clean up
            // their data. These fields are PermitDueInterval, FrequencyUnit, PermitFeeType,
            // and EnvironmentalPermitFeePaymentMethodId.

            Map(x => x.Fee).Scale(2).Precision(18).Not.Nullable();
            Map(x => x.PaymentDueInterval).Nullable();
            Map(x => x.PaymentEffectiveDate).Nullable(); // NOTE: This is a "date", not "datetime", in sql server.
            Map(x => x.PaymentMethodMailAddress).AsTextField().Nullable();
            Map(x => x.PaymentMethodPhone).Length(EnvironmentalPermitFee.StringLengths.PAYMENT_METHOD_PHONE).Nullable();
            Map(x => x.PaymentMethodUrl).Length(EnvironmentalPermitFee.StringLengths.PAYMENT_METHOD_URL).Nullable();
            Map(x => x.PaymentOrganizationContactInfo).AsTextField().Nullable();
            Map(x => x.PaymentOrganizationName).Length(EnvironmentalPermitFee.StringLengths.PAYMENT_ORGANIZATION_NAME)
                                               .Nullable();

            References(x => x.EnvironmentalPermit).Not.Nullable();
            References(x => x.EnvironmentalPermitFeeType).Nullable();
            References(x => x.PaymentDueFrequencyUnit, "PaymentDueRecurringFrequencyUnitId").Nullable();
            References(x => x.PaymentMethod, "EnvironmentalPermitFeePaymentMethodId").Nullable();
            References(x => x.PaymentOwner, "PaymentOwnerEmployeeId").Nullable();
        }
    }
}
