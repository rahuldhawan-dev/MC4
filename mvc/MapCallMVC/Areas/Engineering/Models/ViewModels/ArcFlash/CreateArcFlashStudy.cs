using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Engineering.Models.ViewModels.ArcFlash
{
    public class CreateArcFlashStudy : CreateArcFlashStudyBase
    {
        #region Properties

        [Required, DropDown, AutoMap(MapDirections.None), EntityMustExist(typeof(State))]
        public int? State { get; set; }
        
        [Required, AutoMap(MapDirections.None), DropDown("", "OperatingCenter", "ActiveByStateIdOrAll", DependsOn = "State", PromptText = "Select a state above")]
        [EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        #endregion

        public CreateArcFlashStudy(IContainer container) : base(container) { }
    }

    public class CreateArcFlashStudyBase : ArcFlashStudyViewModel
    {
        #region Properties

        [Required, EntityMustExist(typeof(Facility)), EntityMap]
        [DropDown("", "Facility", "GetByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center.")]
        public int? Facility { get; set; }

        #endregion

        public CreateArcFlashStudyBase(IContainer container) : base(container) { }
    }
}
