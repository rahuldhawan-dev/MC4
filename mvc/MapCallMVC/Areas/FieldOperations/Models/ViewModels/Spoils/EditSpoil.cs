using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Spoils
{
    public class EditSpoil : SpoilViewModel
    {
        #region Constructor

        public EditSpoil(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(Spoil entity)
        {
            base.Map(entity);
            OperatingCenter = entity.WorkOrder?.OperatingCenter?.Id;
        }

        #endregion
    }
}
