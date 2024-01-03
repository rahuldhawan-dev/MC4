using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class CreateBodyOfWater : BodyOfWaterViewModel
    {
        [EntityMap(MapDirections.None)]
        [DropDown, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        public CreateBodyOfWater(IContainer container) : base(container) { }
    }
}
