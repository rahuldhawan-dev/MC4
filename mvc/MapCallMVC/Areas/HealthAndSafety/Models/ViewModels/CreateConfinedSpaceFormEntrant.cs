using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    /// <summary>
    /// NOTE: There is no edit model for this as users are not supposed to edit these.
    /// </summary>
    public class CreateConfinedSpaceFormEntrant : ViewModel<ConfinedSpaceFormEntrant>
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(ConfinedSpaceFormEntrantType))]
        public int? EntrantType { get; set; }

        [DoesNotAutoMap("This is set in the view from the parent ConfinedSpaceForm.")]
        public int? State { get; set; }

        [DoesNotAutoMap("Only needed for view and validation. If an edit model is added, though, then this should map.")]
        public bool IsEmployee { get; set; }

        [RequiredWhen(nameof(IsEmployee), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true), EntityMap, EntityMustExist(typeof(Employee))]
        [AutoComplete("", "Employee", "ActiveEmployeesByStateIdAndPartial", DisplayProperty = nameof(EmployeeDisplayItem.Display),
            DependsOn = nameof(State), PlaceHolder = "Start typing the employee name to select them.")]
        public int? Employee { get; set; }

        [StringLength(ConfinedSpaceFormEntrant.StringLengths.CONTRACTING_COMPANY)]
        [RequiredWhen(nameof(IsEmployee), ComparisonType.EqualTo, false, FieldOnlyVisibleWhenRequired = true)]
        public string ContractingCompany { get; set; }

        [StringLength(ConfinedSpaceFormEntrant.StringLengths.CONTRACTOR_NAME)]
        [RequiredWhen(nameof(IsEmployee), ComparisonType.EqualTo, false, FieldOnlyVisibleWhenRequired = true)]
        public string ContractorName { get; set; }

        #endregion

        #region Constructor

        public CreateConfinedSpaceFormEntrant(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override ConfinedSpaceFormEntrant MapToEntity(ConfinedSpaceFormEntrant entity)
        {
            // Users are only allowed to enter either an employee or a contractor.
            // Users may have entered values for both and we want to make sure we 
            // only keep the one that was intended. This is for data integrity purposes.
            if (IsEmployee)
            {
                ContractingCompany = null;
                ContractorName = null;
            }
            else
            {
                Employee = null;
            }
            return base.MapToEntity(entity);
        }

        #endregion
    }
}