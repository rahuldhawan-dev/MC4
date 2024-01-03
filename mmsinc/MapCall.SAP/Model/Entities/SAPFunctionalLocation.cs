using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.FunctionalLocationWS;
using MMSINC.Data;

namespace MapCall.SAP.Model.Entities
{
    public class SAPFunctionalLocation
    {
        #region Properties

        public string FunctionalLocation { get; set; }
        public string FunctionalLocationDescription { get; set; }
        public string SAPErrorCode { get; set; }

        #endregion

        public SAPFunctionalLocation(FunctionalLocationInfoRecord record)
        {
            FunctionalLocation = record.FunctionalLocation;
            FunctionalLocationDescription = record.FunctionalLocationDescription;
            SAPErrorCode = "Successful";
        }

        public SAPFunctionalLocation() { }

        public FunctionalLocationQuery FunctionalLocationRequest(SearchSapFunctionalLocation search)
        {
            FunctionalLocationQuery FunctionalLocationQuery = new FunctionalLocationQuery();

            FunctionalLocationQuery.FunctionalLocation = search.FunctionalLocation;
            FunctionalLocationQuery.FunctionalLocationCategory = search.FunctionalLocationCategory;
            FunctionalLocationQuery.FunctionalLocationDescription = search.Description;
            FunctionalLocationQuery.PlanningPlant = search.PlanningPlant;
            FunctionalLocationQuery.SortField = search.SortField;
            FunctionalLocationQuery.TechnicalObjectType = search.TechnicalObjectType;

            return FunctionalLocationQuery;
        }
    }
}
