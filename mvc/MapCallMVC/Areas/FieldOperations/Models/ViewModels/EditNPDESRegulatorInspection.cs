using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class EditNpdesRegulatorInspection : NpdesRegulatorInspectionViewModel
    {
        #region Constructors

        public EditNpdesRegulatorInspection(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void Map(NpdesRegulatorInspection entity)
        {
            base.Map(entity);
            InspectedBy = entity.InspectedBy.UserName;
        }

        #endregion
    }
}
