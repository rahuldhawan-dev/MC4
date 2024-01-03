using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Spoils
{
    public class CreateSpoil : SpoilViewModel
    {
        #region Constructor

        public CreateSpoil(IContainer container) : base(container) { }

        #endregion
    }
}
