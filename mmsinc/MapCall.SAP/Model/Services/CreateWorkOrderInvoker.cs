using System;
using System.ServiceModel;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.service;

namespace MapCall.SAP.Model.Services
{
    public class CreateWorkOrderInvoker : SAPServiceInvoker<SAPWorkOrder, CreateWorkOrder_OB_SYNClient, CreateWorkOrder_OB_SYN>
    {
        #region Constants

        public const string NO_DATA_FOUND = "No data found";

        #endregion

        #region Constructors

        public CreateWorkOrderInvoker(ISAPHttpClient sapHttpClient) : base(sapHttpClient)
        {
            SendTimeOut = TimeSpan.FromSeconds(60);
        }

        #endregion

        #region Private Methods

        protected override SAPWorkOrder InvokeInternal(SAPWorkOrder sapWorkOrder, CreateWorkOrder_OB_SYN client)
        {
            try
            {
                var workOrderRequest = sapWorkOrder.WorkOrderRequest();

                var retVal = client.CreateWorkOrder_OB_SYNAsync(new CreateWorkOrder_OB_SYNRequest(workOrderRequest))
                                   .Result;
                if (retVal.WorkOrderResponse.Length > 0)
                {
                    sapWorkOrder.OrderNumber = retVal.WorkOrderResponse[0]?.OrderNumber;
                    if (!string.IsNullOrWhiteSpace(retVal.WorkOrderResponse[0].EquipmentNo))
                    {
                        sapWorkOrder.EquipmentNo = retVal.WorkOrderResponse[0].EquipmentNo;
                    }
                    sapWorkOrder.NotificationNumber = retVal.WorkOrderResponse[0]?.NotificationNumber;
                    sapWorkOrder.SAPErrorCode = retVal.WorkOrderResponse[0]?.Status;
                    sapWorkOrder.WBSElement = retVal.WorkOrderResponse[0]?.WBSElement;
                    sapWorkOrder.CostCenter = retVal.WorkOrderResponse[0].CostCenter;
                }
                else
                {
                    sapWorkOrder.SAPErrorCode = NO_DATA_FOUND;
                }
            }
            catch (FaultException e)
            {
                sapWorkOrder.SAPErrorCode = RETRY_ERROR_TEXT + e;
            }
            catch (Exception ex)
            {
                sapWorkOrder.SAPErrorCode = RETRY_ERROR_TEXT + ex;
            }

            return sapWorkOrder;
        }

        #endregion
    }
}