using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorAgreementMap : ClassMap<ContractorAgreement>
    {
        #region Constructors

        public ContractorAgreementMap()
        {
            Id(x => x.Id, "ContractorAgreementID");

            References(x => x.Contractor).Not.Nullable();
            References(x => x.ContractorCompany).Not.Nullable();
            References(x => x.ContractorWorkCategoryType).Not.Nullable();
            References(x => x.ContractorAgreementStatusType).Not.Nullable();
            References(x => x.ContractorInsurance).Nullable();

            Map(x => x.Title).Length(ContractorAgreement.StringLengths.TITLE).Not.Nullable();
            Map(x => x.Description).Not.Nullable();
            Map(x => x.AgreementOwner).Length(ContractorAgreement.StringLengths.AGREEMENT_OWNER).Not.Nullable();
            Map(x => x.AgreementStartDate).Not.Nullable();
            Map(x => x.AgreementEndDate).Not.Nullable();
            Map(x => x.EstimatedContractValue).Not.Nullable();
            Map(x => x.CreatedBy).Length(ContractorAgreement.StringLengths.CREATED_BY).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.NJAWContractNumber).Length(ContractorAgreement.StringLengths.NJAW_CONTRACT_NUMBER).Nullable();
            Map(x => x.Legacy).Nullable();
            Map(x => x.SupplierName).Length(ContractorAgreement.StringLengths.SUPPLIER_NAME).Nullable();
            Map(x => x.AmendmentNumber).Nullable();
            Map(x => x.StrategicSourcingProjectTrackingNumber).Length(ContractorAgreement.StringLengths.STRATEGIC_SOURCING_PROJECT_TRACKING_NUMBER).Nullable();
            Map(x => x.Accounting).Length(ContractorAgreement.StringLengths.ACCOUNTING).Nullable();
            Map(x => x.EstimatedAnnualSpend).Nullable();
            Map(x => x.EstimatedLifetimePayments).Nullable();
            Map(x => x.EstimatedLifetimeReceipts).Nullable();
            Map(x => x.EstimatedTerminationDate).Nullable();
            Map(x => x.ContractOwnerEmployeeId).Length(ContractorAgreement.StringLengths.CONTRACTOR_OWNER_EMPLOYEE_ID).Nullable();
            Map(x => x.ContractOwnerLastName).Length(ContractorAgreement.StringLengths.CONTRACTOR_OWNER_LAST_NAME).Nullable();

            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();

            HasManyToMany(x => x.OperatingCenters)
               .Table("ContractorAgreementsOperatingCenters")
               .ParentKeyColumn("ContractorAgreementID")
               .ChildKeyColumn("OperatingCenterID")
               .Cascade.All();
            HasManyToMany(x => x.FunctionalAreas)
               .Table("ContractorAgreementsFunctionalAreas")
               .ParentKeyColumn("ContractorAgreementID")
               .ChildKeyColumn("ContractorFunctionalAreaTypeID")
               .Cascade.All();
        }

        #endregion
    }
}
