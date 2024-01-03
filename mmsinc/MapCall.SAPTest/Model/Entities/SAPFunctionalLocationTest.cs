using MapCall.Common.Model.ViewModels;

namespace MapCall.SAP.Model.Entities.Tests
{
    public class SAPFunctionalLocationTest
    {
        #region Private Methods

        public SearchSapFunctionalLocation SetFunctionalLoactionSearchValues()
        {
            var SearchsapFunctionalLocation = new SearchSapFunctionalLocation();
            SearchsapFunctionalLocation.PlanningPlant = "";
            SearchsapFunctionalLocation.FunctionalLocation = "";
            SearchsapFunctionalLocation.FunctionalLocationCategory = "";
            SearchsapFunctionalLocation.Description = "";
            SearchsapFunctionalLocation.SortField = "134500167965AKASH";
            SearchsapFunctionalLocation.TechnicalObjectType = "";
            return SearchsapFunctionalLocation;
        }

        #endregion
    }
}
