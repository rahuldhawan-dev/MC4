using MapCall.Common.Model.Entities;
using MMSINC.Utilities.ObjectMapping;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Engineering.Models.ViewModels.ArcFlash
{
    public class EditArcFlashStudy : ArcFlashStudyViewModel
    {
        [AutoMap(MapDirections.None)]
        public int? State { get; set; }

        public EditArcFlashStudy(IContainer container) : base(container) { }

        public override void Map(ArcFlashStudy entity)
        {
            base.Map(entity);
            State = entity.Facility?.OperatingCenter?.State?.Id;
        }
    }
}
