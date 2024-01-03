using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public class ConfinedSpaceFormHazardViewModel : ViewModel<ConfinedSpaceFormHazard>
    {
        #region Properties

        [DoesNotAutoMap("Display only. Set from the parent ConfinedSpaceFormViewModel")]
        public string HazardTypeDescription { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(ConfinedSpaceFormHazardType))]
        public int? HazardType { get; set; }

        [DoesNotAutoMap("Display only. Set from the parent ConfinedSpaceFormViewModel")]
        [CheckBox]
        public bool? IsChecked { get; set; } // How will this display the actual selected hazard type?

        [RequiredWhen(nameof(IsChecked), true)]
        [StringLength(ConfinedSpaceFormHazard.StringLengths.NOTES)]
        public string Notes { get; set; }

        #endregion

        #region Constructors

        public ConfinedSpaceFormHazardViewModel(IContainer container) : base(container) { }

        #endregion
    }
}