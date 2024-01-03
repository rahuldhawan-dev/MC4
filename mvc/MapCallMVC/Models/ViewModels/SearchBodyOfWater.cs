using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Models.ViewModels
{
    public class SearchBodyOfWater : SearchSet<BodyOfWater>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [Search(CanMap = false)]
        public int? State { get; set; }

        [RequiredWhen(nameof(State), ComparisonType.NotEqualTo, null)]
        [DropDown(Area = "", Controller = "OperatingCenter", Action = "ByStateIds", DependsOn = nameof(State), PromptText = "Please select a state above.")]
        public int? OperatingCenter { get; set; }

        public SearchString Name { get; set; }

        #endregion
    }
}