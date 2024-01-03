using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Contractors.Models.ViewModels
{
    public class ContractorAgreementViewModel : ViewModel<ContractorAgreement>
    {
        #region Constructor

        public ContractorAgreementViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required, StringLength(ContractorAgreement.StringLengths.TITLE)]
        public virtual string Title { get; set; }

        [Multiline, Required]
        public virtual string Description { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Contractor)), Required]
        public int? Contractor { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ContractorCompany)), Required]
        public int? ContractorCompany { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ContractorWorkCategoryType)), Required]
        public int? ContractorWorkCategoryType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ContractorAgreementStatusType)), Required]
        public int? ContractorAgreementStatusType { get; set; }

        [DropDown("Contractors", "ContractorInsurance", "ByContractorId", DependsOn = nameof(Contractor), PromptText = "Please select a contractor above."),
         EntityMap, EntityMustExist(typeof(ContractorInsurance))]
        public int? ContractorInsurance { get; set; }

        [Required, StringLength(ContractorAgreement.StringLengths.AGREEMENT_OWNER)]
        public string AgreementOwner { get; set; }

        [Required]
        public DateTime? AgreementStartDate { get; set; }

        [Required]
        public DateTime? AgreementEndDate { get; set; }

        [Required]
        public decimal? EstimatedContractValue { get; set; }

        [StringLength(ContractorAgreement.StringLengths.NJAW_CONTRACT_NUMBER)]
        public string NJAWContractNumber { get; set; }
        
        public bool? Legacy { get; set; }

        public float? AmendmentNumber { get; set; }
        
        [StringLength(ContractorAgreement.StringLengths.STRATEGIC_SOURCING_PROJECT_TRACKING_NUMBER)]
        public string StrategicSourcingProjectTrackingNumber { get; set; }

        [StringLength(ContractorAgreement.StringLengths.ACCOUNTING)]
        public string Accounting { get; set; }

        public float? EstimatedAnnualSpend { get; set; }

        public float? EstimatedLifetimePayments { get; set; }

        public float? EstimatedLifetimeReceipts { get; set; }

        public DateTime? EstimatedTerminationDate { get; set; }

        [StringLength(ContractorAgreement.StringLengths.CONTRACTOR_OWNER_EMPLOYEE_ID)]
        public string ContractOwnerEmployeeId { get; set; }

        [StringLength(ContractorAgreement.StringLengths.CONTRACTOR_OWNER_LAST_NAME)]
        public string ContractOwnerLastName { get; set; }

        [EntityMap, CheckBoxList, EntityMustExist(typeof(OperatingCenter)), View(ContractorAgreement.Display.OPERATING_CENTERS)]
        public int[] OperatingCenters { get; set; }

        [EntityMap, CheckBoxList, EntityMustExist(typeof(ContractorFunctionalAreaType)), View(ContractorAgreement.Display.FUNCTIONAL_AREAS)]
        public int[] FunctionalAreas { get; set; }

        #endregion
    }

    public class CreateContractorAgreement : ContractorAgreementViewModel
    {
        #region Constructors

        public CreateContractorAgreement(IContainer container) : base(container) { }

        #endregion
        
        #region Mapping

        public override ContractorAgreement MapToEntity(ContractorAgreement entity)
        {
            base.MapToEntity(entity);

            entity.CreatedBy =
                _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;

            return entity;
        }

        #endregion
    }

    public class EditContractorAgreement : ContractorAgreementViewModel
    {
        #region Constructors

        public EditContractorAgreement(IContainer container) : base(container) { }

        #endregion
    }
}