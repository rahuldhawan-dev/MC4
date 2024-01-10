using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchReportUse : SearchSet<ReportViewing>
    {
        #region Properties

        [EntityMap, EntityMustExist(typeof(State))]
        [DropDown, SearchAlias("u.DefaultOperatingCenter", "State.Id")]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "ByStateIds", DependsOn = "State", PromptText = "Select a state above")]
        [SearchAlias("User", "u", "DefaultOperatingCenter.Id", Required = true)]
        public int? OperatingCenter { get; set; }

        [DropDown("", "User", "GetAllByStateOrOperatingCenterId", DependsOn = "State, OperatingCenter", DependentsRequired = DependentRequirement.None)]
        public int? User { get; set; }
        
        public DateRange DateViewed { get; set; }

        #endregion
    }
}
