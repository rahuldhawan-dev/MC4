using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.MaterialsUsed
{
    public class CreateMaterialUsed : MaterialUsedViewModel
    {
        #region Constructor

        public CreateMaterialUsed(IContainer container) : base(container) { }

        #endregion
    }
}