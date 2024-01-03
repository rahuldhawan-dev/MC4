using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class FacilitySubAreaViewModel : ViewModel<FacilitySubArea>
    {
        #region Constructor

        public FacilitySubAreaViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required, StringLength(FacilitySubArea.MAX_DESCRIPTION_LENGTH)]
        public string Description { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(FacilityArea)), Required]
        public int? Area { get; set; }

        #endregion
    }

    public class CreateFacilitySubArea : FacilitySubAreaViewModel
    {
        public CreateFacilitySubArea(IContainer container) : base(container) {}
    }

    public class EditFacilitySubArea : FacilitySubAreaViewModel
    {
        public EditFacilitySubArea(IContainer container) : base(container) {}
    }

    public class SearchFacilitySubArea : SearchSet<FacilitySubArea> {}
}
