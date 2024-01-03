using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels.PreJobSafetyBriefs
{
    public abstract class ProductionPreJobSafetyBriefViewModelBase : ViewModel<ProductionPreJobSafetyBrief>
    {
        #region Consts

        public const string
            PLEASE_SPEAK_TO_YOUR_SUPERVISOR =
                "Please speak to your supervisor if hazards and precautions have not been discussed " +
                "before starting work.",
            MUST_HAVE_EMPLOYEE_OR_CONTRACTOR = "At least one employee or contractor must be selected.";

        #endregion

        #region Fields

        private ProductionWorkOrder _productionWorkOrder;

        #endregion

        #region Properties

        [Required, View("Date/Time"), DateTimePicker]
        public DateTime? SafetyBriefDateTime { get; set; }

        [DoesNotAutoMap("Needs to be done manually")]
        [MultiString]
        public string[] Contractors { get; set; }

        [Required]
        public bool? AnyPotentialWeatherHazards { get; set; }
        
        [EntityMap, MultiSelect, EntityMustExist(typeof(ProductionPreJobSafetyBriefWeatherHazardType))]
        [RequiredWhen(nameof(AnyPotentialWeatherHazards), true, FieldOnlyVisibleWhenRequired = true)]
        [View(ProductionPreJobSafetyBrief.Display.IF_YES)]
        public int[] SafetyBriefWeatherHazardTypes { get; set; }
       
        [RequiredWhen(nameof(AnyPotentialWeatherHazards), true, FieldOnlyVisibleWhenRequired = true)]
        public bool? HadConversationAboutWeatherHazard { get; set; }
       
        [Multiline, StringLength(ProductionPreJobSafetyBrief.StringLengths.NOTES), 
         RequiredWhen(nameof(HadConversationAboutWeatherHazard), true, FieldOnlyVisibleWhenRequired = true)]
        public string HadConversationAboutWeatherHazardNotes { get; set; }
       
        [Required]
        public bool? AnyTimeOfDayConstraints { get; set; }
       
        [EntityMap, MultiSelect]
        [EntityMustExist(typeof(ProductionPreJobSafetyBriefTimeOfDayConstraintType))]
        [RequiredWhen(nameof(AnyTimeOfDayConstraints), true, FieldOnlyVisibleWhenRequired = true)]
        [View(ProductionPreJobSafetyBrief.Display.IF_YES)]
        public int[] SafetyBriefTimeOfDayConstraintTypes { get; set; }
       
        [Required]
        public bool? AnyTrafficHazards { get; set; }
       
        [EntityMap, MultiSelect, EntityMustExist(typeof(ProductionPreJobSafetyBriefTrafficHazardType))]
        [RequiredWhen(nameof(AnyTrafficHazards), true, FieldOnlyVisibleWhenRequired = true)]
        [View(ProductionPreJobSafetyBrief.Display.IF_YES)]
        public int[] SafetyBriefTrafficHazardTypes { get; set; }
        
        [Required]
        public bool? InvolveConfinedSpace { get; set; }
       
        [Required]
        public bool? AnyPotentialOverheadHazards { get; set; }
        
        [EntityMap, MultiSelect, EntityMustExist(typeof(ProductionPreJobSafetyBriefOverheadHazardType))]
        [RequiredWhen(nameof(AnyPotentialOverheadHazards), true, FieldOnlyVisibleWhenRequired = true)]
        [View(ProductionPreJobSafetyBrief.Display.IF_YES)]
        public int[] SafetyBriefOverheadHazardTypes { get; set; }
        
        [Required]
        public bool? AnyUndergroundHazards { get; set; }
        
        [EntityMap, MultiSelect, EntityMustExist(typeof(ProductionPreJobSafetyBriefUndergroundHazardType))]
        [RequiredWhen(nameof(AnyUndergroundHazards), true, FieldOnlyVisibleWhenRequired = true)]
        [View(ProductionPreJobSafetyBrief.Display.IF_YES)]
        public int[] SafetyBriefUndergroundHazardTypes { get; set; }
        
        [Required]
        public virtual bool? AreThereElectricalOrOtherEnergyHazards { get; set; }
       
        [EntityMap, MultiSelect, EntityMustExist(typeof(ProductionPreJobSafetyBriefElectricalHazardType))]
        [RequiredWhen(
            nameof(AreThereElectricalOrOtherEnergyHazards),
            true,
            FieldOnlyVisibleWhenRequired = true)]
        [View(ProductionPreJobSafetyBrief.Display.IF_YES)]
        public int[] SafetyBriefElectricalHazardTypes { get; set; }
        
        [Required]
        public bool? AnyWorkPerformedGreaterThanOrEqualToFourFeetAboveGroundLevel { get; set; }
     
        [Multiline, StringLength(ProductionPreJobSafetyBrief.StringLengths.NOTES)]
        [RequiredWhen(
            nameof(AnyWorkPerformedGreaterThanOrEqualToFourFeetAboveGroundLevel),
            true,
            FieldOnlyVisibleWhenRequired = true)]
        public string TypeOfFallPreventionProtectionSystemBeingUsed { get; set; }

        [Required]
        public bool? DoesJobInvolveUseOfChemicals { get; set; }

        [RequiredWhen(nameof(DoesJobInvolveUseOfChemicals), true, FieldOnlyVisibleWhenRequired = true)]
        public bool? IsSafetyDataSheetAvailableForEachChemical { get; set; }

        [Multiline]
        [StringLength(ProductionPreJobSafetyBrief.StringLengths.NOTES)]
        [RequiredWhen(
            nameof(IsSafetyDataSheetAvailableForEachChemical),
            false,
            FieldOnlyVisibleWhenRequired = true)]
        public string IsSafetyDataSheetAvailableForEachChemicalNotes { get; set; }

        [Required]
        public bool? HaveEquipmentToDoJobSafely { get; set; }
       
        [RequiredWhen(nameof(HaveEquipmentToDoJobSafely), false, FieldOnlyVisibleWhenRequired = true)]
        [Multiline, StringLength(ProductionPreJobSafetyBrief.StringLengths.NOTES)]
        public string HaveEquipmentToDoJobSafelyNotes { get; set; }
      
        [Required]
        public bool? HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspection { get; set; }

        [Multiline, StringLength(ProductionPreJobSafetyBrief.StringLengths.NOTES)]
        [RequiredWhen(
            nameof(HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspection),
            false,
            FieldOnlyVisibleWhenRequired = true)]
        public string HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspectionNotes
        {
            get;
            set;
        }

        [Required]
        public bool? ReviewedErgonomicHazards { get; set; }
      
        [RequiredWhen(nameof(ReviewedErgonomicHazards), false, FieldOnlyVisibleWhenRequired = true)]
        [Multiline, StringLength(ProductionPreJobSafetyBrief.StringLengths.NOTES)]
        public string ReviewedErgonomicHazardsNotes { get; set; }
      
        [Required]
        public bool? HasStretchAndFlexBeenPerformed { get; set; }
      
        [RequiredWhen(nameof(HasStretchAndFlexBeenPerformed), false, FieldOnlyVisibleWhenRequired = true)]
        [Multiline, StringLength(ProductionPreJobSafetyBrief.StringLengths.NOTES)]
        public string HasStretchAndFlexBeenPerformedNotes { get; set; }

        [Required]
        public bool? ReviewedLocationOfSafetyEquipment { get; set; }

        [RequiredWhen(
            nameof(ReviewedLocationOfSafetyEquipment),
            false,
            FieldOnlyVisibleWhenRequired = true)]
        [Multiline, StringLength(ProductionPreJobSafetyBrief.StringLengths.NOTES)]
        public string ReviewedLocationOfSafetyEquipmentNotes { get; set; }

        [Required]
        public bool? OtherHazardsIdentified { get; set; }
      
        [RequiredWhen(nameof(OtherHazardsIdentified), true, FieldOnlyVisibleWhenRequired = true)]
        [Multiline, StringLength(ProductionPreJobSafetyBrief.StringLengths.NOTES)]
        public string OtherHazardNotes { get; set; }
      
        [Required]
        public bool? CrewMembersRemindedOfStopWorkAuthority { get; set; }

        [RequiredWhen(
            nameof(CrewMembersRemindedOfStopWorkAuthority),
            false,
            FieldOnlyVisibleWhenRequired = true)]
        [Multiline, StringLength(ProductionPreJobSafetyBrief.StringLengths.NOTES)]
        public string CrewMembersRemindedOfStopWorkAuthorityNotes { get; set; }

        // This is going to annoy users because they're going to have to answer "Yes" every time the page
        // loads. They aren't allowed to save the form until they select yes.
        //
        // If they're editing the form, they still have to confirm that they explained anything they just
        // changed to the job/crew(or to the new person added to the list).
        //
        // Because of this, there is no reason to save this to the database since every saved record would
        // have "True".
        //
        // They did not want a dialog-on-submit for unexplained reasons.
        //
        // I tried telling them that this field is functionally useless and just annoying UX, but it fell on
        // deaf ears.
        [DoesNotAutoMap("This is only for validation to prevent a user from saving when they don't select")]
        [View("Have the hazards and precautions for this job been reviewed with everyone on the job/crew?")]
        // Don't want the default error message for this.
        [Required(ErrorMessage = PLEASE_SPEAK_TO_YOUR_SUPERVISOR)]
        [ClientCallback(
            "SafetyBrief.validateHaveAllHazardsAndPrecautionsBeenReviewedIsTrue",
            ErrorMessage = PLEASE_SPEAK_TO_YOUR_SUPERVISOR)]
        public bool? HaveAllHazardsAndPrecautionsBeenReviewed { get; set; }

        #region PPE

        [CheckBox]
        [ClientCallback(
            "SafetyBrief.validatePpeChecked",
            ErrorMessage = "At least one PPE type must be checked")]
        public bool? HeadProtection { get; set; }
      
        [CheckBox]
        public bool? HandProtection { get; set; }
      
        [CheckBox]
        public bool? ElectricalProtection { get; set; }
       
        [CheckBox]
        public bool? FallProtection { get; set; }

        [CheckBox]
        public bool? FootProtection { get; set; }
       
        [CheckBox]
        public bool? EyeProtection { get; set; }
      
        [CheckBox]
        public bool? FaceShield { get; set; }
       
        [CheckBox]
        public bool? SafetyGarment { get; set; }
       
        [CheckBox]
        public bool? HearingProtection { get; set; }
       
        [CheckBox]
        public bool? RespiratoryProtection { get; set; }
       
        [CheckBox]
        public bool? PPEOther { get; set; }
       
        [Multiline, StringLength(ProductionPreJobSafetyBrief.StringLengths.NOTES),
         RequiredWhen(nameof(PPEOther), true, FieldOnlyVisibleWhenRequired = true)]
        public string PPEOtherNotes { get; set; }

        #endregion

        #endregion
        
        #region Abstract Properties
        
        public abstract int? ProductionWorkOrder { get; set; } 
      
        [DoesNotAutoMap("Needs to be done manually"), EntityMustExist(typeof(Employee))]
        public abstract int[] Employees { get; set; }
        
        #endregion

        #region Constructor

        public ProductionPreJobSafetyBriefViewModelBase(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        // Not a property as we don't want to make these available to the model binder.
        // Should look into customizing the model binder to allow for a [DoesNotBind] attribute
        // type thing. MVC Core has this I think, but not framework.
        public ProductionWorkOrder GetProductionWorkOrderForDisplay()
        {
            return _productionWorkOrder ??
                   (_productionWorkOrder = _container
                                          .GetInstance<IRepository<ProductionWorkOrder>>()
                                          .Find(ProductionWorkOrder.GetValueOrDefault()));
        }

        public override void Map(ProductionPreJobSafetyBrief entity)
        {
            base.Map(entity);
            Employees = entity.Workers.Where(x => x.Employee != null).Select(x => x.Employee.Id).ToArray();
            Contractors = entity.Workers
                                .Where(x => x.Contractor != null)
                                .Select(x => x.Contractor)
                                .ToArray();
        }

        public override ProductionPreJobSafetyBrief MapToEntity(ProductionPreJobSafetyBrief entity)
        {
            entity = base.MapToEntity(entity);

            // both Employees and Contractors have the potential to be null.
            var vmEmployees = Employees ?? Enumerable.Empty<int>();
            
            // Contractors is MultiString. Because of that, if the user enters nothing, we'll get back an
            // empty text value. We don't want those.
            var vmContractors = (Contractors ?? Enumerable.Empty<string>())
                               .Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            var existingEmployees = entity
                                   .Workers
                                   .Where(x => x.Employee != null)
                                   .Select(x => x.Employee.Id).ToList();
            foreach (var employeeToRemove in existingEmployees.Except(vmEmployees))
            {
                entity.Workers.Remove(entity.Workers.Single(x => x.Employee?.Id == employeeToRemove));
            }

            var existingContractors = entity
                                     .Workers
                                     .Where(x => x.Contractor != null)
                                     .Select(x => x.Contractor).ToList();
            foreach (var contractorToRemove in existingContractors.Except(vmContractors))
            {
                entity.Workers.Remove(entity.Workers.Single(x => x.Contractor == contractorToRemove));
            }

            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            var employeeRepo = _container.GetInstance<IRepository<Employee>>();
            foreach (var employeeToAdd in vmEmployees.Except(existingEmployees))
            {
                entity.Workers.Add(new ProductionPreJobSafetyBriefWorker {
                    ProductionPreJobSafetyBrief = entity,
                    Employee = employeeRepo.Find(employeeToAdd),
                    SignedAt = now
                });
            }

            foreach (var contractorToAdd in vmContractors.Except(existingContractors))
            {
                entity.Workers.Add(new ProductionPreJobSafetyBriefWorker {
                    ProductionPreJobSafetyBrief = entity,
                    Contractor = contractorToAdd,
                    SignedAt = now
                });
            }

            return entity;
        }

        // NOTE: This validation is really unnecessary. This could be better served by a confirmation
        // dialog, but I could not convince the business side. -Ross 1/20/2021
        private IEnumerable<ValidationResult> ValidateHaveAllHazardsAndPrecautionsBeenReviewedIsTrue()
        {
            if (HaveAllHazardsAndPrecautionsBeenReviewed != true)
            {
                yield return new ValidationResult(
                    PLEASE_SPEAK_TO_YOUR_SUPERVISOR,
                    new[] { nameof(HaveAllHazardsAndPrecautionsBeenReviewed) });
            }
        }

        private IEnumerable<ValidationResult> ValidateThatThereIsAtleastOneEmployeeOrContractor()
        {
            // This is what RequiredCollection would normally do, but can't use that
            // since this validation involves two collections.
            var hasEmployees = Employees != null && Employees.Any();
            var hasContractors = Contractors != null &&
                                 Contractors.Any(x => !string.IsNullOrWhiteSpace(x));

            if (!hasEmployees && !hasContractors)
            {
                yield return new ValidationResult(MUST_HAVE_EMPLOYEE_OR_CONTRACTOR);
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateHaveAllHazardsAndPrecautionsBeenReviewedIsTrue())
                       .Concat(ValidateThatThereIsAtleastOneEmployeeOrContractor());
        }

        #endregion
    }
}
