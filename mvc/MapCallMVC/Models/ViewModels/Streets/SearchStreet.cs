using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Models.ViewModels.Streets
{
    public class SearchStreet : SearchSet<Street>
    {
        [DropDown, EntityMustExist(typeof(State))]
        [SearchAlias("Town", "State.Id")]
        public int? State { get; set; }

        [DropDown("", "County", "ByStateId", DependsOn = "State"), EntityMustExist(typeof(County))]
        [SearchAlias("Town", "County.Id")]
        public int? County { get; set; }

        [EntityMap, DropDown("", "Town", "ByCountyId", DependsOn = "County"), EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        public bool? IsActive { get; set; }

        public SearchString FullStName { get; set; }
    }
}