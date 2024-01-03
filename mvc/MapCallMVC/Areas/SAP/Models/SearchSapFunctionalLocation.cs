using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.SAP.Models
{
    public class SearchSapFunctionalLocation
    {
        #region Properties
        [DropDown]
        public string PlanningPlant { get; set; }
        public int OperatingCenter { get; set; }
        [View(Description = "Use * for wildcards")]
        public string FunctionalLocation { get; set; }
        public string Description { get; set; }
        [DropDown]
        public string FunctionalLocationCategory { get; set; }
        [View(DisplayName="PWSID")]
        public string SortField { get; set; }
        [DropDown]
        public string TechnicalObjectType { get; set; }

        [Search(CanMap=false)]
        public string SearchUrl { get; set; }

        #endregion

        //SAP want's all strings so lets provide them with that.
        public MapCall.Common.Model.ViewModels.SearchSapFunctionalLocation ToSearchSapNotification()
        {
            return new MapCall.Common.Model.ViewModels.SearchSapFunctionalLocation
            {
                PlanningPlant = PlanningPlant,
                FunctionalLocation = FunctionalLocation,
                Description = Description,
                FunctionalLocationCategory = FunctionalLocationCategory,
                SortField = SortField,
                TechnicalObjectType = TechnicalObjectType
            };
        }
    }
}