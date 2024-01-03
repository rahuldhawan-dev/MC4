using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class EditMarkout : MarkoutViewModel
    {
        #region Constructors

        public EditMarkout(IContainer container) : base(container) { }

        #endregion
    }
}
