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
    public class ContractorInsuranceViewModel : ViewModel<ContractorInsurance>
    {
        #region Constructor

        public ContractorInsuranceViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(Contractor)), Required]
        public int? Contractor { get; set; }

        [StringLength(ContractorInsurance.StringLengths.INSURANCE_PROVIDER)]
        public string InsuranceProvider { get; set; }

        [StringLength(ContractorInsurance.StringLengths.POLICY_NUMBER)]
        public string PolicyNumber { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ContractorInsuranceMinimumRequirement)), Required]
        public int? ContractorInsuranceMinimumRequirement { get; set; }

        [CheckBox]
        public bool? MeetsCurrentContractualLimits { get; set; }

        [Required]
        public DateTime? EffectiveDate { get; set; }

        [Required]
        public DateTime? TerminationDate { get; set; }

        #endregion
    }

    public class CreateContractorInsurance : ContractorInsuranceViewModel
    {
        #region Constructors

        public CreateContractorInsurance(IContainer container) : base(container) { }

        #endregion
        
        #region Mapping

        public override ContractorInsurance MapToEntity(ContractorInsurance entity)
        {
            base.MapToEntity(entity);

            entity.CreatedBy =
                _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;

            return entity;
        }

        #endregion
    }

    public class EditContractorInsurance : ContractorInsuranceViewModel
    {
        #region Constructors

        public EditContractorInsurance(IContainer container) : base(container) { }

        #endregion
    }
}