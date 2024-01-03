using MMSINC.Metadata;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerMainCleanings
{
    public class EditSewerMainCleaning : SewerMainCleaningViewModel
    {
        #region Properties

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public override int? Street { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public override int? CrossStreet { get; set; }

        [DropDown("FieldOperations", "Hydrant", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public override int? HydrantUsed { get; set; }

        #endregion

        #region Constructors

        public EditSewerMainCleaning(IContainer container) : base(container) {}

        #endregion
    }
}