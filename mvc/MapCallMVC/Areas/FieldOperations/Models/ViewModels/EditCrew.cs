using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class EditCrew : CrewViewModel
    {
        #region Constructors
        public EditCrew(IContainer container) : base(container) { }

        #endregion
    }
}