using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public abstract class BaseMarkoutDamageViewModel : ViewModel<MarkoutDamage>
    {
        #region Private Members
        
        

        #endregion

        #region Properties

        [StringLength(MarkoutDamage.StringLengths.REQUEST_NUM)]
        [RequiredWhen("MarkoutDamageToType", "GetMarkoutDamageToTypeIdForOthers", typeof(BaseMarkoutDamageViewModel), ErrorMessage="Required when markout damage is to others.")]
        public string RequestNumber { get; set; }

        [Required, EntityMap, DropDown]
        [EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [AutoMap("Town.County.State.Id", MapDirections.ToPrimary)]
        [Required, EntityMustExist(typeof(State))]
        [DropDown("", "State", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? State { get; set; }

        [AutoMap("Town.County.Id", MapDirections.ToPrimary)]
        [Required, EntityMustExist(typeof(County))]
        [DropDown("", "County", "ByStateId", DependsOn = "State", PromptText = "Select a state above")]
        public int? County { get; set; }

        [Required, EntityMap]
        [EntityMustExist(typeof(Town))]
        [DropDown("", "Town", "ByCountyId", DependsOn = "County", PromptText = "Select a county above")]
        public int? Town { get; set; }

        [Coordinate(AddressCallback="MarkoutDamage.getAddress"), Required, EntityMap]
        [EntityMustExist(typeof(Coordinate))]
        public int? Coordinate { get; set; }

        [Required, EntityMap, DropDown]
        [EntityMustExist(typeof(MarkoutDamageToType))]
        public int? MarkoutDamageToType { get; set; }

        [EntityMap, EntityMustExist(typeof(Employee))]
        [DropDown("", "Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above")]
        public int? SupervisorSignOffEmployee { get; set; }

        [Required]
        [StringLength(MarkoutDamage.StringLengths.STREET)]
        public string Street { get; set; }

        [EntityMap, EntityMustExist(typeof(WorkOrder)), DisplayName("WorkOrderID")]
        public virtual int? WorkOrder { get; set; }

        [Required]
        [StringLength(MarkoutDamage.StringLengths.CROSS_STREET)]
        public string NearestCrossStreet { get; set; }

        [StringLength(MarkoutDamage.StringLengths.EXCAVATOR)]
        [RequiredWhen("MarkoutDamageToType", "GetMarkoutDamageToTypeIdForOurs", typeof(BaseMarkoutDamageViewModel), ErrorMessage = "Required when markout damage is to our own.")]
        public string Excavator { get; set; }

        [StringLength(MarkoutDamage.StringLengths.EXCAVATOR_ADDRESS)]
        public string ExcavatorAddress { get; set; }

        [StringLength(MarkoutDamage.StringLengths.EXCAVATOR_PHONE)]
        public string ExcavatorPhone { get; set; }

        [Required, DateTimePicker]
        public DateTime? DamageOn { get; set; }

        [Required, Multiline]
        public string DamageComments { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(MarkoutDamageUtilityDamageType))]
        [RequiredWhen("MarkoutDamageToType", "GetMarkoutDamageToTypeIdForOthers", typeof(BaseMarkoutDamageViewModel), ErrorMessage = "Required when markout damage is to others.")]
        public virtual int[] UtilityDamages { get; set; }

        public string EmployeesOnJob { get; set; }

        [Required]
        public bool? IsMarkedOut { get; set; }

        [Required]
        public bool? IsMismarked { get; set; }

        [RequiredWhen("IsMismarked", true)]
        public int? MismarkedByInches { get; set; }

        [Required]
        public bool? ExcavatorDiscoveredDamage { get; set; }

        [Required]
        public bool? ExcavatorCausedDamage { get; set; }

        [Required]
        [DisplayName("Was 911 Called?")]
        public bool? Was911Called { get; set; }

        [Required]
        public bool? WerePicturesTaken { get; set; }
        public DateTime? ApprovedOn { get; set; }

        [StringLength(MarkoutDamage.StringLengths.SAP_WORK_ORDER_ID)]
        public string SAPWorkOrderId { get; set; }

        #endregion

        #region Constructors

        protected BaseMarkoutDamageViewModel(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private static int GetMarkoutDamageTypeForDescription(IRepository<MarkoutDamageToType> repo, string description)
        {
            return repo.Where(x => x.Description == description).Select(x => x.Id).Single();
        }

        #endregion

        #region Exposed Methods

        public static int GetMarkoutDamageToTypeIdForOthers()
        {
            return MapCall.Common.Model.Entities.MarkoutDamageToType.Indices.OTHERS;
        }

        public static int GetMarkoutDamageToTypeIdForOurs()
        {
            return MapCall.Common.Model.Entities.MarkoutDamageToType.Indices.OURS;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Union(ValidateWorkOrder());
        }

        private IEnumerable<ValidationResult> ValidateWorkOrder()
        {
            if (WorkOrder.HasValue)
            {
                var workOrder = _container.GetInstance<IWorkOrderRepository>().Find(WorkOrder.Value);

                if (workOrder == null || (workOrder != null && workOrder.CancelledAt.HasValue))
                {
                    var error = "WorkOrderID's value does not match an existing object.";
                    yield return new ValidationResult(error, new[] { nameof(WorkOrder) });
                }
            }
        }

        #endregion
    }

    public class CreateMarkoutDamage : BaseMarkoutDamageViewModel
    {
        #region Constructors

        public CreateMarkoutDamage(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override MarkoutDamage MapToEntity(MarkoutDamage entity)
        {
            base.MapToEntity(entity);

            entity.CreatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;

            return entity;
        }

        #endregion
    }

    public class EditMarkoutDamage : BaseMarkoutDamageViewModel
    {
        #region Constructors

        public EditMarkoutDamage(IContainer container) : base(container) {}

        #endregion

        public override MarkoutDamage MapToEntity(MarkoutDamage entity)
        {
            if (entity.SupervisorSignOffEmployee == null && SupervisorSignOffEmployee.HasValue && entity.ApprovedOn == null && ApprovedOn == null)
            {
                entity.ApprovedOn = ApprovedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            }

            return base.MapToEntity(entity);
        }
    }

    public class SearchMarkoutDamage : SearchSet<MarkoutDamage>
    {
        #region Properties

        [View("Markout Damage ID")]
        public int? EntityId { get; set; }

        [DropDown("", "OperatingCenter", "ByStateIdForFieldServicesAssets", DependsOn = "State", PromptText = "Select a state below")]
        [EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Search(CanMap = false)]
        public bool? IsSignedOffBySupervisor { get; set; }

        public string CreatedBy { get; set; }

        [View("Damaged On")]
        public DateRange DamageOn { get; set; }
        
        [SearchAlias("Town", "T", "State.Id")]
        [DropDown, EntityMustExist(typeof(State))]
        public int? State { get; set; }
        
        [SearchAlias("Town", "T", "County.Id")]
        [EntityMustExist(typeof(County))]
        [DropDown("", "County", "ByStateId", DependsOn = "State", PromptText = "Select a state above")]
        public int? County { get; set; }

        [EntityMustExist(typeof(Town))]
        [DropDown("", "Town", "ByCountyId", DependsOn = "County", PromptText = "Select a county above")]
        public int? Town { get; set; }

        public string Street { get; set; }
        public string Excavator { get; set; }
        public bool? IsMarkedOut { get; set; }
        public bool? IsMismarked { get; set; }
        public bool? ExcavatorCausedDamage { get; set; }
        public bool? ExcavatorDiscoveredDamage { get; set; }

        [View("Was 911 Called?")]
        public bool? Was911Called { get; set; }

        [View("Were Pictures Taken?")]
        public bool? WerePicturesTaken { get; set; }

        [Search(CanMap = false)]
        public bool? MissingAttachedPictures { get; set; }

        public bool? HasAttachedPictures { get; set; }

        public string SAPWorkOrderId { get; set; }

        // Needs to exist for the search mapper. Not part of the view.
        public int? SupervisorSignOffEmployee { get; set; }
        public int? WorkOrder { get; set; }

        #endregion

        #region Exposed Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            if (IsSignedOffBySupervisor.HasValue)
            {
                mapper.MappedProperties[nameof(SupervisorSignOffEmployee)].Value = IsSignedOffBySupervisor.Value ? SearchMapperSpecialValues.IsNotNull : SearchMapperSpecialValues.IsNull;
            }

            // TODO: What is this MissingAttachedPictures property for?
            // Couldn't the same thing be accomplished with an optional dropdown
            // for HasAttachedPictures?
            if (WerePicturesTaken == true && MissingAttachedPictures.HasValue)
            {
                HasAttachedPictures = false;
                MissingAttachedPictures = null;
            }
        }

        #endregion
    }

    public class SearchMarkoutDamageReport : SearchSet<MarkoutDamage>
    {
        #region Properties

        [SearchAlias("Town", "T", "State.Id")]
        [DropDown, EntityMustExist(typeof(State)), Required]
        public int? State { get; set; }
        [Display(Name = "Damaged On"), Required]
        public DateRange DamageOn { get; set; }

        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State", PromptText = "Select a state above"), EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        [DropDown, EntityMustExist(typeof(MarkoutDamageToType))]
        public int? MarkoutDamageToType { get; set; }

        #endregion
    }
}