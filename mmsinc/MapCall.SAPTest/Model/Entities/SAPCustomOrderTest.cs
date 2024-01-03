using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SAP.DataTest.Model.Entities
{
    [TestClass()]
    public class SAPCustomOrderTest
    {
        #region Private Methods

        public SearchSapCustomerOrder SetCustomerOrderRequest()
        {
            var searchSapCustomerOrder = new SearchSapCustomerOrder();
            searchSapCustomerOrder.FSR_ID = "";
            searchSapCustomerOrder.WorkOrder = "500000241";

            return searchSapCustomerOrder;
        }

        public SearchSapCustomerOrder SetCustomerOrderRequestForFRS()
        {
            var searchSapCustomerOrder = new SearchSapCustomerOrder();
            searchSapCustomerOrder.FSR_ID = "18721391";
            searchSapCustomerOrder.WorkOrder = "";

            return searchSapCustomerOrder;
        }

        #endregion
    }
}
