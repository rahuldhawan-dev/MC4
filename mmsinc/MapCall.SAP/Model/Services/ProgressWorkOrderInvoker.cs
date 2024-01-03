using System;
using System.ServiceModel;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.service;

namespace MapCall.SAP.Model.Services
{
    public class ProgressWorkOrderInvoker : SAPServiceInvoker<SAPProgressWorkOrder, ProgressWorkOrder_OB_SYNClient, ProgressWorkOrder_OB_SYN>
    {
        #region Constants

        public const string NO_DATA_FOUND = "No data found";

        #endregion

        #region Constructors

        public ProgressWorkOrderInvoker(ISAPHttpClient sapHttpClient) : base(sapHttpClient)
        {
            SendTimeOut = TimeSpan.FromSeconds(60);
        }

        #endregion

        #region Private Methods

        protected override SAPProgressWorkOrder InvokeInternal(SAPProgressWorkOrder sapProgressWorkOrder, ProgressWorkOrder_OB_SYN client)
        {
            try
            {
                var processWorkOrderRequest = sapProgressWorkOrder.ProcessWorkOrderRequest();

                var retVal = client.ProgressWorkOrder_OB_SYNAsync(new ProgressWorkOrder_OB_SYNRequest(processWorkOrderRequest)).Result;
                if (retVal.ProgressWorkOrderResponse.Length > 0)
                {
                    sapProgressWorkOrder.MaterialDocument = retVal.ProgressWorkOrderResponse[0]?.MaterialDocument;
                    sapProgressWorkOrder.NotificationNumber = retVal.ProgressWorkOrderResponse[0]?.NotificationNumber;
                    sapProgressWorkOrder.OrderNumber = retVal.ProgressWorkOrderResponse[0]?.OrderNumber;
                    sapProgressWorkOrder.Status = retVal.ProgressWorkOrderResponse[0]?.Status;
                    sapProgressWorkOrder.WBSElement = retVal.ProgressWorkOrderResponse[0]?.WBSElement;
                    sapProgressWorkOrder.CostCenter = retVal.ProgressWorkOrderResponse[0].CostCenter;
                }
                else
                {
                    sapProgressWorkOrder.Status = NO_DATA_FOUND;
                }
            }
            catch (FaultException e)
            {
                sapProgressWorkOrder.Status = RETRY_ERROR_TEXT + e;
            }
            catch (Exception ex)
            {
                sapProgressWorkOrder.Status = RETRY_ERROR_TEXT + ex;
            }

            return sapProgressWorkOrder;
        }

        #endregion
    }
}
