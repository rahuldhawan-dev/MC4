using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class CreateBusinessUnit : BusinessUnitViewModel
    {
        public CreateBusinessUnit(IContainer container) : base(container) { }
    }
}