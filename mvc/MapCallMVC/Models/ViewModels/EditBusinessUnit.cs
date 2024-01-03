using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class EditBusinessUnit : BusinessUnitViewModel
    {
        public EditBusinessUnit(IContainer container) : base(container) { }
    }
}