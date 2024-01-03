using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class CreateMarkout : MarkoutViewModel
    {
        #region Constructors

        public CreateMarkout(IContainer container) : base(container) { }

        #endregion
    }
}
