using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class LockoutFormViewModel : ViewModel<LockoutForm>
    {
        #region Constants

        public struct ValidationErrors
        {
            public const string MANAGEMENT = "Response to the management question is required.",
                OUT_OF_SERVICE = "";
        }

        #endregion

        #region Properties

        [Required(ErrorMessage = "The Operating Center field is required."), EntityMap, DropDown]
        [EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Required, EntityMap]
        [EntityMustExist(typeof(Facility))]
        [DropDown("", "Facility", "GetActiveByOperatingCenterId", DependsOn = "OperatingCenter")]
        public int? Facility { get; set; }

        [DropDown("", "EquipmentType", "ByFacilityIdAndSometimesProductionWorkOrder", DependsOn = "Facility,ProductionWorkOrder", DependentsRequired = DependentRequirement.One, PromptText = "Select a facility above.")]
        [EntityMap, EntityMustExist(typeof(EquipmentType))]
        public int? EquipmentType { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(Equipment))]
        [DropDown("", "Equipment", "ByFacilityIdAndSometimesEquipmentTypeIdANDProductionWorkOrder", DependsOn = "Facility, EquipmentType, ProductionWorkOrder", DependentsRequired = DependentRequirement.One)]
        public int? Equipment { get; set; }

        [Required(ErrorMessage = "The Lockout Reason field is required."), Multiline, DropDown, EntityMap, EntityMustExist(typeof(LockoutReason))]
        public virtual int LockoutReason { get; set; }

        [Required(ErrorMessage = "The Lockout Device field is required."), EntityMap, EntityMustExist(typeof(LockoutDevice))]
        [DropDown("HealthAndSafety","LockoutDevice", "ByOperatingCenterForCurrentUserEmployee", DependsOn = "OperatingCenter")]
        public virtual int? LockoutDevice { get; set; }

        [DropDown("Production", "ProductionWorkOrder", "ByFacilityIdForLockoutForms", DependsOn = "Facility"), EntityMap, EntityMustExist(typeof(ProductionWorkOrder))]
        public virtual int? ProductionWorkOrder { get; set; }

        [Required(ErrorMessage = "The IsolationPoint field is required."), DropDown, EntityMap, EntityMustExist(typeof(LockoutDeviceLocation))]
        public virtual int? IsolationPoint { get; set; }

        [Required(ErrorMessage = "The Lockout DateTime field is required.")]
        [DateTimePicker]
        public virtual DateTime? LockoutDateTime { get; set; }

        [Required(ErrorMessage = "The Reason For Lockout field is required."), Multiline, StringLength(int.MaxValue, MinimumLength = 5)]
        public virtual string ReasonForLockout { get; set; }

        [View(Description = LockoutForm.Questions.AUTH_AFFIX_LOCKOUT)]
        public virtual bool? ContractorLockOutTagOut { get; set; }

        #region Contractor Fields 
        
        [DropDown, EntityMap, EntityMustExist(typeof(Contractor))]
        [ClientCallback("LockoutForm.validateContractorEntered", ErrorMessage = "Please select a contractor name.")]
        public int? Contractor { get; set; }
        [StringLength(LockoutForm.StringLengths.CONTRACTOR_NAME), AutoMap(MapDirections.None)]
        [ClientCallback("LockoutForm.validateContractorEntered", ErrorMessage = "Please enter a contractor name.")]
        public string ContractorName { get; set; }
        [StringLength(LockoutForm.StringLengths.CONTRACTOR_FIRST_NAME)]
        [RequiredWhen("ContractorLockOutTagOut", ComparisonType.EqualTo, true)]
        public string ContractorFirstName { get; set; }
        [StringLength(LockoutForm.StringLengths.CONTRACTOR_LAST_NAME)]
        [RequiredWhen("ContractorLockOutTagOut", ComparisonType.EqualTo, true)]
        public string ContractorLastName { get; set; }
        [StringLength(LockoutForm.StringLengths.CONTRACTOR_PHONE)]
        [RequiredWhen("ContractorLockOutTagOut", ComparisonType.EqualTo, true)]
        public string ContractorPhone { get; set; }

        #endregion

        [Required(ErrorMessage = "The Location of Lockout Notes field is required."), Multiline, StringLength(int.MaxValue, MinimumLength = 5)]
        public virtual string LocationOfLockoutNotes { get; set; }

        [ClientCallback("LockoutForm.validateEmployeeAcknowledgedTraining", ErrorMessage = "There are documents attached to this piece of equipment. You must acknowledge that you have read the SOP for the equipment.")]
        public virtual bool EmployeeAcknowledgedTraining { get; set; }

        [StringLength(25), RequiredWhen("IsolationPoint", ComparisonType.EqualTo, LockoutDeviceLocation.Indices.OTHER)]
        public virtual string IsolationPointDescription { get; set; }

        #region OUT OF SERVICE

        [StringLength(int.MaxValue, MinimumLength = 5)]
        //[RequiredWhen("IsolatesEnergySources", ComparisonType.EqualTo, true, ErrorMessage="The Additional Lockout Notes field is required.")]
        public virtual string AdditionalLockoutNotes { get; set; }
        
        [Required(ErrorMessage = "The Out Of Service Authorized Employee field is required.")]
        [EntityMap, EntityMustExist(typeof(Employee))]
        [DropDown("", "Employee", "ActiveLockoutFormEmployeesByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public virtual int? OutOfServiceAuthorizedEmployee { get; set; }

        [Required(ErrorMessage = "The Out Of Service DateTime field is required.")]
        public virtual DateTime? OutOfServiceDateTime { get; set; }

        #endregion

        #region Questions

        public List<LockoutFormAnswerViewModel> LockoutFormAnswers { get; set; }

        #endregion

        #endregion

        #region Constructors

        public LockoutFormViewModel(IContainer container) : base(container)
        {
            LockoutFormAnswers = new List<LockoutFormAnswerViewModel>();
        }

        #endregion

        #region Private Methods

        public override LockoutForm MapToEntity(LockoutForm entity)
        {
            if (!Contractor.HasValue && !string.IsNullOrWhiteSpace(ContractorName))
            {
                var contractorRepo = _container.GetInstance<IRepository<Contractor>>();
                var existingContractor = contractorRepo.Where(x => x.Name == ContractorName).FirstOrDefault();
                var currentUserName = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;
                Contractor = existingContractor?.Id ?? contractorRepo.Save(new Contractor { Name = ContractorName, CreatedBy = currentUserName }).Id;
            }
            base.MapToEntity(entity);
            return entity;
        }

        #endregion
    }
}
