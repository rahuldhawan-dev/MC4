using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class SearchInterconnectionTest : SearchSet<InterconnectionTest>
    {
        [SearchAlias("Facility.OperatingCenter", "State.Id"),
         DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [SearchAlias("Facility", "OperatingCenter.Id", Required = true), 
         DropDown("", "OperatingCenter", "ByStateId", DependsOn = nameof(State), PromptText = "Please select a State above."),
         EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        public DateRange InspectionDate { get; set; }

        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = nameof(OperatingCenter)), 
         EntityMap, EntityMustExist(typeof(Facility))]
        public int? Facility { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Employee))]
        public int? Employee { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Contractor))]
        public int? Contractor { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(InterconnectionInspectionRating)),
         View(InterconnectionTest.Display.INTERCONNECTION_INSPECTION_RATING)]
        public int? InterconnectionInspectionRating { get; set; }
    }
}