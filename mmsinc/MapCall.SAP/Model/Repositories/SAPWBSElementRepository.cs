using MapCall.SAP.Model.Entities;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPWBSElementRepository : SAPRepositoryBase, ISAPWBSElementRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE = "GetWBSElement",
                            SAP_INTERFACE = "http://amwater.com/EAM/0020/MAPCALL/GetWBSElement";

        #endregion

        #region Constructors

        public SAPWBSElementRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public virtual SAPWBSElementCollection Search(SAPWBSElement entity)
        {
            SAPHttpClient.SAPInterface = SAP_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                SAPWBSElement sapWBSElement = new SAPWBSElement();
                sapWBSElement.SAPErrorCode = ERROR_NO_SITE_CONNECTION;
                var sapWBSElementCollection = new SAPWBSElementCollection();
                sapWBSElementCollection.Items.Add(sapWBSElement);
                return sapWBSElementCollection;
            }

            return SAPHttpClient.GetWBSElement(entity);
        }

        #endregion
    }

    public interface ISAPWBSElementRepository
    {
        #region Abstract Methods

        SAPWBSElementCollection Search(SAPWBSElement entity);

        #endregion
    }
}
