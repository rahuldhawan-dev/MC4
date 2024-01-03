using System;
using System.ServiceModel;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.service;

namespace MapCall.SAP.Model.Services
{
    public class CompleteWorkOrderInvoker : SAPServiceInvoker<SAPCompleteWorkOrder, CompleteWorkOrder_OB_SYNClient, CompleteWorkOrder_OB_SYN>
    {
        #region Constants

        public const string NO_DATA_FOUND = "No data found";

        #endregion

        #region Constructors

        public CompleteWorkOrderInvoker(ISAPHttpClient sapHttpClient) : base(sapHttpClient)
        {
            SendTimeOut = TimeSpan.FromSeconds(60);
        }

        #endregion

        #region Private Methods

        protected override SAPCompleteWorkOrder InvokeInternal(SAPCompleteWorkOrder sapCompleteWorkOrder, CompleteWorkOrder_OB_SYN client)
        {
            try
            {
                var completeWorkOrderRequest = sapCompleteWorkOrder.CompleteWorkOrderRequest();

                var retVal = client
                            .CompleteWorkOrder_OB_SYNAsync(
                                 new CompleteWorkOrder_OB_SYNRequest(completeWorkOrderRequest)).Result;
                if (retVal.CompleteWorkOrderResponse.Length > 0)
                {
                    sapCompleteWorkOrder.NotificationNumber = retVal.CompleteWorkOrderResponse[0]?.NotificationNumber;
                    sapCompleteWorkOrder.OrderNumber = retVal.CompleteWorkOrderResponse[0]?.OrderNumber;
                    sapCompleteWorkOrder.Status = retVal.CompleteWorkOrderResponse[0]?.Status;
                    sapCompleteWorkOrder.WBSElement = retVal.CompleteWorkOrderResponse[0]?.WBSElement;
                    sapCompleteWorkOrder.CostCenter = retVal.CompleteWorkOrderResponse[0]?.CostCenter;
                }
                else
                {
                    sapCompleteWorkOrder.Status = NO_DATA_FOUND;
                }
            }
            catch (FaultException e)
            {
                sapCompleteWorkOrder.Status = RETRY_ERROR_TEXT + e;
            }
            catch (Exception ex)
            {
                sapCompleteWorkOrder.Status = RETRY_ERROR_TEXT + ex;
            }

            return sapCompleteWorkOrder;
        }
      
        #endregion
    }
}
