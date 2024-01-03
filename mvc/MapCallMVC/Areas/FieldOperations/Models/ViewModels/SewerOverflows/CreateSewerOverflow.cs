using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerOverflows
{
    public class CreateSewerOverflow : SewerOverflowViewModel
    {
        #region Constructors

        public CreateSewerOverflow(IContainer container) : base(container) { }

        #endregion
    }
}