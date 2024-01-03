using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorInsuranceMap : ClassMap<ContractorInsurance>
    {
        #region Constructors

        public ContractorInsuranceMap()
        {
            Table("ContractorInsurance");
            Id(x => x.Id, "ContractorInsuranceID");

            References(x => x.Contractor).Not.Nullable();
            References(x => x.ContractorInsuranceMinimumRequirement).Not.Nullable();

            Map(x => x.InsuranceProvider).Length(ContractorInsurance.StringLengths.INSURANCE_PROVIDER).Nullable();
            Map(x => x.MeetsCurrentContractualLimits).Nullable();
            Map(x => x.EffectiveDate).Not.Nullable();
            Map(x => x.TerminationDate).Not.Nullable();
            Map(x => x.CreatedBy).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.PolicyNumber).Length(ContractorInsurance.StringLengths.POLICY_NUMBER).Nullable();

            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
        }

        #endregion
    }
}
