using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class CreateCrew : CrewViewModel
    {
        #region Constructors
        public CreateCrew(IContainer container) : base(container) { }

        #endregion
    }
}