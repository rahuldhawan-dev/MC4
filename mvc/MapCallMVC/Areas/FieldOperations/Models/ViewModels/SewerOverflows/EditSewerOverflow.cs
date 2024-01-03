using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerOverflows
{
    public class EditSewerOverflow : SewerOverflowViewModel
    {
        #region Constructors

        public EditSewerOverflow(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(SewerOverflow sewerOverflow)
        {
            base.Map(sewerOverflow);

            State = sewerOverflow.OperatingCenter?.State?.Id;
        }

        #endregion
    }
}