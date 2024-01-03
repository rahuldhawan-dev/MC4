using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class EditBodyOfWater : BodyOfWaterViewModel
    {
        [EntityMap(MapDirections.None)]
        [DropDown, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        public EditBodyOfWater(IContainer container) : base(container) { }

        public override void Map(BodyOfWater entity)
        {
            base.Map(entity);
            State = entity.OperatingCenter.State.Id;
        }
    }
}
