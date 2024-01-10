using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class CrewViewModel : ViewModel<Crew>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [Required]
        public int OperatingCenter { get; set; }

        [StringLength(Crew.StringLengths.CREW_NAME)]
        [Required, DisplayName(Crew.CREW_NAME)]
        public string Description { get; set; }

        [Required, View(Crew.AVAILABILITY, MMSINC.Utilities.FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public decimal? Availability { get; set; }

        [CheckBox]
        public bool? Active { get; set; }

        #region Constructors

        public CrewViewModel(IContainer container) : base(container) { }

        #endregion
    }
}