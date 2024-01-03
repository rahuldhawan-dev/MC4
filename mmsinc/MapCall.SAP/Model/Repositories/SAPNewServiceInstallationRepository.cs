using MapCall.SAP.Model.Entities;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPNewServiceInstallationRepository : SAPRepositoryBase, ISAPNewServiceInstallationRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE = "W1v_NewServiceInstallation",
                            SAP_INTERFACE = "http://amwater.com/EAM/0019/MAPCALL/W1v_GetNewServiceInstallation";

        #endregion

        #region Constructors

        public SAPNewServiceInstallationRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public virtual SAPNewServiceInstallation Save(SAPNewServiceInstallation entity)
        {
            SAPHttpClient.SAPInterface = SAP_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                entity.SAPStatus = ERROR_NO_SITE_CONNECTION;
                return entity;
            }

            return SAPHttpClient.SaveNewServiceInstallation(entity);
        }

        public virtual SAPNewServiceInstallation SaveService(SAPNewServiceInstallation entity)
        {
            SAPHttpClient.SAPInterface = SAP_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                entity.SAPStatus = ERROR_NO_SITE_CONNECTION;
                return entity;
            }

            return SAPHttpClient.SaveService(entity);
        }

        #endregion
    }

    public interface ISAPNewServiceInstallationRepository
    {
        #region Abstract Methods

        SAPNewServiceInstallation Save(SAPNewServiceInstallation entity);
        SAPNewServiceInstallation SaveService(SAPNewServiceInstallation entity);

        #endregion
    }
}
