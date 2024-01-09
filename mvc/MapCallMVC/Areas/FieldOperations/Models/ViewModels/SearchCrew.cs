using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchCrew : SearchSet<Crew>, ISearchCrew
    {
        [DropDown]
        [EntityMap]
        [EntityMustExist(typeof(State))]
        [SearchAlias("OperatingCenter", "State.Id")]
        public int? State { get; set; }

        [DropDown("", nameof(OperatingCenter), "ByStateId", DependsOn = "State")]
        [EntityMap]
        [EntityMustExist(typeof(OperatingCenter))]
        public int OperatingCenter { get; set; }

        [View("Name")]
        public string Description { get; set; }

        [CheckBox]
        public bool? Active { get; set; }
    }
}