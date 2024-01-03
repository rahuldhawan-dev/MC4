using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using System.Linq;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels.RedTagPermits
{
    public class RedTagPermitViewModel : ViewModel<RedTagPermit>
    {
        #region Constants

        public const string AT_LEAST_ONE_PRECAUTION_IS_REQUIRED = "At least one precaution is required.";
        
        #endregion

        #region Fields

        private ProductionWorkOrder _productionWorkOrder;
        private Equipment _equipment;

        public static IEnumerable<int> GetNeedsAdditionalInformationProtectionTypeIds() => RedTagPermitProtectionType.ADDITIONAL_INFORMATION_REQUIRED_TYPES;

        #endregion

        #region Properties

        [DoesNotAutoMap("Display only")]
        public ProductionWorkOrder ProductionWorkOrderDisplay
        {
            get
            {
                if (_productionWorkOrder == null && ProductionWorkOrder.HasValue)
                {
                    _productionWorkOrder = _container.GetInstance<ProductionWorkOrderRepository>()
                                                     .Find(ProductionWorkOrder.GetValueOrDefault());
                }
                return _productionWorkOrder;
            }
            // This setter exists solely for a couple of unit tests because the unit tests
            internal set => _productionWorkOrder = value;
        }

        [DoesNotAutoMap("Display only")]
        public Equipment EquipmentDisplay
        {
            get
            {
                if (_equipment == null && Equipment.HasValue)
                {
                    _equipment = _container.GetInstance<EquipmentRepository>()
                                           .Find(Equipment.GetValueOrDefault());
                }
                return _equipment;
            }
            // This setter exists solely for a couple of unit tests because the unit tests
            internal set => _equipment = value;
        }

        [EntityMap,
         EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [EntityMap,
         EntityMustExist(typeof(Facility))]
        public int? Facility { get; set; }

        [Secured,
         Required,
         EntityMap,
         EntityMustExist(typeof(Equipment))]
        public int? Equipment { get; set; }

        [Secured,
         Required,
         EntityMap, 
         EntityMustExist(typeof(ProductionWorkOrder))]
        public int? ProductionWorkOrder { get; set; }

        [Required,
         EntityMap,
         EntityMustExist(typeof(Employee)),
         DropDown("", "Employee", "ActiveProductionWorkManagementEmployeesByOperatingCenterId", DependsOn = nameof(OperatingCenter), PromptText = "Please select an Operating Center.")]
        public int? PersonResponsible { get; set; }

        [Required,
         DropDown,
         EntityMap,
         EntityMustExist(typeof(RedTagPermitProtectionType))]
        public int? ProtectionType { get; set; }

        [DataType(DataType.MultilineText),
         StringLength(RedTagPermit.StringLengths.ADDITIONAL_INFORMATION_FOR_PROTECTION_TYPE),
         RequiredWhen(nameof(ProtectionType), 
             ComparisonType.EqualToAny, 
             nameof(GetNeedsAdditionalInformationProtectionTypeIds), 
             typeof(RedTagPermitViewModel), 
             ErrorMessage = "AdditionalInformationForProtectionType is required for the specified Protection Type.", 
             FieldOnlyVisibleWhenRequired = true)]
        public string AdditionalInformationForProtectionType { get; set; }

        [Required,
         DataType(DataType.MultilineText),
         StringLength(RedTagPermit.StringLengths.AREA_PROTECTED)]
        public string AreaProtected { get; set; }

        [Required,
         DataType(DataType.MultilineText),
         StringLength(RedTagPermit.StringLengths.REASON_FOR_IMPAIRMENT)]
        public string ReasonForImpairment { get; set; }

        [Required,
         Range(RedTagPermit.Ranges.NUMBER_OF_TURNS_TO_CLOSE_MIN, RedTagPermit.Ranges.NUMBER_OF_TURNS_TO_CLOSE_MAX)]
        public int? NumberOfTurnsToClose { get; set; }

        [Required,
         EntityMap,
         EntityMustExist(typeof(Employee)),
         DropDown("", "Employee", "ActiveProductionWorkManagementEmployeesByOperatingCenterId", DependsOn = nameof(OperatingCenter))]
        public int? AuthorizedBy { get; set; }

        [Required,
         DataType(DataType.MultilineText),
         StringLength(RedTagPermit.StringLengths.FIRE_PROTECTION_EQUIPMENT_OPERATOR)]
        public string FireProtectionEquipmentOperator { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual DateTime? EquipmentImpairedOn { get; set; }

        [CheckBox,
         ClientCallback("RedTagPermit.validatePrecautions", ErrorMessage = "At least one precaution must be checked.")]
        public bool? EmergencyOrganizationNotified { get; set; }

        [CheckBox]
        public bool? PublicFireDepartmentNotified { get; set; }

        [CheckBox]
        public bool? HazardousOperationsStopped { get; set; }

        [CheckBox]
        public bool? HotWorkProhibited { get; set; }

        [CheckBox]
        public bool? SmokingProhibited { get; set; }

        [CheckBox]
        public bool? ContinuousWorkAuthorized { get; set; }

        [CheckBox]
        public bool? OngoingPatrolOfArea { get; set; }

        [CheckBox]
        public bool? HydrantConnectedToSprinkler { get; set; }

        [CheckBox]
        public bool? PipePlugsOnHand { get; set; }

        [CheckBox]
        public bool? FireHoseLaidOut { get; set; }

        [CheckBox]
        public bool? HasOtherPrecaution { get; set; }

        [DataType(DataType.MultilineText),
         StringLength(RedTagPermit.StringLengths.OTHER_PRECAUTION_DESCRIPTION),
         RequiredWhen(nameof(HasOtherPrecaution), 
             ComparisonType.EqualTo, 
             true, 
             FieldOnlyVisibleWhenRequired = true)]
        public string OtherPrecautionDescription { get; set; }

        #endregion

        #region Constructors

        public RedTagPermitViewModel(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateThatThereIsAtLeastOnePrecaution());
        }

        private IEnumerable<ValidationResult> ValidateThatThereIsAtLeastOnePrecaution()
        {
            if (
                !HasOtherPrecaution.GetValueOrDefault() && 
                !FireHoseLaidOut.GetValueOrDefault() && 
                !PipePlugsOnHand.GetValueOrDefault() && 
                !HydrantConnectedToSprinkler.GetValueOrDefault() && 
                !OngoingPatrolOfArea.GetValueOrDefault() && 
                !ContinuousWorkAuthorized.GetValueOrDefault() && 
                !SmokingProhibited.GetValueOrDefault() && 
                !HotWorkProhibited.GetValueOrDefault() && 
                !HazardousOperationsStopped.GetValueOrDefault() && 
                !PublicFireDepartmentNotified.GetValueOrDefault() && 
                !EmergencyOrganizationNotified.GetValueOrDefault())
            {
                yield return new ValidationResult(AT_LEAST_ONE_PRECAUTION_IS_REQUIRED);
            }
        }

        #endregion
    }
}