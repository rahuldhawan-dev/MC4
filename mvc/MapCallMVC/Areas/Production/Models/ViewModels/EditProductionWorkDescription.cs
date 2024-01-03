using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class EditProductionWorkDescription : ProductionWorkDescriptionViewModel
    {
        public EditProductionWorkDescription(IContainer container) : base(container) { }
    }
}
