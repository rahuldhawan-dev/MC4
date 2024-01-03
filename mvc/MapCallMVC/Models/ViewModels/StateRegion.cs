using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class StateRegionViewModel : ViewModel<StateRegion>
    {
        #region Properties

        [Required]
        public string Region { get; set; }
        [DropDown, Required, EntityMap]
        public int? State { get; set; }

        #endregion

        #region Constructors

        public StateRegionViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateStateRegion : StateRegionViewModel
    {
        #region Constructors

        public CreateStateRegion(IContainer container) : base(container) {}

        #endregion
    }

    public class EditStateRegion : StateRegionViewModel
    {
        #region Constructors

        public EditStateRegion(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchStateRegion : SearchSet<StateRegion>
    {
        #region Properties

        public SearchString Region { get; set; }
        [DropDown]
        public int? State { get; set; }

        #endregion
    }
}