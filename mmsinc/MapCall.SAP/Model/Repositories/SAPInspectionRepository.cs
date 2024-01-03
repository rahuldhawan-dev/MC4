using MapCall.SAP.Model.Entities;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPInspectionRepository : SAPRepositoryBase, ISAPInspectionRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE = "InspectionRecord_Create",
                            SAP_INTERFACE = "http://amwater.com/EAM/0010/MAPCALL/CreateInspectionRecord";

        #endregion

        #region Constructors

        public SAPInspectionRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public SAPInspection Save(SAPInspection entity)
        {
            SAPHttpClient.SAPInterface = SAP_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                entity.SAPErrorCode = ERROR_NO_SITE_CONNECTION;
                return entity;
            }

            return SAPHttpClient.GetInspectionResponse(entity);
        }

        #endregion
    }

    public interface ISAPInspectionRepository
    {
        #region Abstract Methods

        SAPInspection Save(SAPInspection entity);

        #endregion
    }
}
