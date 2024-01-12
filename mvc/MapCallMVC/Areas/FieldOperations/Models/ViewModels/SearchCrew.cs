using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using System.Collections.Generic;

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
        public int? OperatingCenter { get; set; }

        [View("Name")]
        public string Description { get; set; }

        [CheckBox]
        public bool? Active { get; set; }
    }

    public class SearchCrewForWorkOrders : SearchSet<Crew>, ISearchCrewForWorkOrders
    {
        #region Properties

        [DoesNotAutoMap, Search(CanMap = false)]
        public Crew Crew { get; set; }

        [DoesNotAutoMap, Search(CanMap = false)]
        public IEnumerable<CrewWorkOrderDetails> CrewWorkOrders { get; set; }

        public int? Id { get; set; }

        #endregion
    }
}