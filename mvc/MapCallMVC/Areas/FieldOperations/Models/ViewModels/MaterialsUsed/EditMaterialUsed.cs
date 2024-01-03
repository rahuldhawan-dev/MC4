using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.MaterialsUsed
{
    public class EditMaterialUsed : MaterialUsedViewModel
    {
        #region Constructor

        public EditMaterialUsed(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void Map(MaterialUsed entity)
        {
            base.Map(entity);
            OperatingCenter = entity.WorkOrder?.OperatingCenter?.Id;
        }

        #endregion
    }
}