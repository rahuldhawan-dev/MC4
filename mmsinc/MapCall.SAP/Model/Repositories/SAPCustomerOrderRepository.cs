using MapCall.Common.Model.ViewModels;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;

namespace SAP.DataTest.Model.Repositories // <-- What is this namespace about? -Ross 4/6/2020
{
    public class SAPCustomerOrderRepository : SAPRepositoryBase, ISAPCustomerOrderRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE = "GetCustomerOrderDetails",
                            SAP_INTERFACE = "http://amwater.com/EAM/0022/MAPCALL/GetCustomerOrderDetails";

        #endregion

        #region Constructors

        public SAPCustomerOrderRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public virtual SAPCustomerOrderCollection Search(SearchSapCustomerOrder entity)
        {
            SAPHttpClient.SAPInterface = SAP_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                SAPCustomerOrder sapCustomerOrder = new SAPCustomerOrder();
                sapCustomerOrder.SAPErrorCode = ERROR_NO_SITE_CONNECTION;
                var sapCustomerOrderCollection = new SAPCustomerOrderCollection();
                sapCustomerOrderCollection.Items.Add(sapCustomerOrder);
                return sapCustomerOrderCollection;
            }

            return SAPHttpClient.GetCustomerOrder(entity);
        }

        #endregion
    }

    public interface ISAPCustomerOrderRepository
    {
        #region Abstract Methods

        SAPCustomerOrderCollection Search(SearchSapCustomerOrder entity);

        #endregion
    }
}
