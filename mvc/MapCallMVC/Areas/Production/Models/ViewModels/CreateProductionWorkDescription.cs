using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class CreateProductionWorkDescription : ProductionWorkDescriptionViewModel
    {
        public CreateProductionWorkDescription(IContainer container) : base(container) { }
    }
}
