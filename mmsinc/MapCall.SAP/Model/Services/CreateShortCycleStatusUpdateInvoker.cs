using System;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.WorkOrderStatusUpdateWS;

namespace MapCall.SAP.Model.Services
{
    public class CreateShortCycleStatusUpdateInvoker : SAPServiceInvoker<SAPWorkOrderStatusUpdateRequest,
        WO_StatusUpdate_OB_SYCClient, WO_StatusUpdate_OB_SYC>
    {
        public CreateShortCycleStatusUpdateInvoker(ISAPHttpClient sapHttpClient) : base(sapHttpClient)
        {
            SendTimeOut = TimeSpan.FromSeconds(10);
        }

        protected override SAPWorkOrderStatusUpdateRequest InvokeInternal(SAPWorkOrderStatusUpdateRequest sapEntity,
            WO_StatusUpdate_OB_SYC client)
        {
            //_sapHttpClient.SendTimeOut = new TimeSpan(0,0,10);
            var WorkOrderStatusUpdate_Request = sapEntity.WorkOrderStatusUpdateRequest();

            var retVal = client.WO_StatusUpdate_OB_SYC(new WO_StatusUpdate_OB_SYCRequest {
                WOStatusUpdate_Request = WorkOrderStatusUpdate_Request
            });

            sapEntity.SAPStatus = retVal?.WOStatusUpdate_Response?.SAPStatus ??
                                  "No records found, null response or status";

            return sapEntity;
        }

        protected override void OnException(SAPWorkOrderStatusUpdateRequest sapEntity, Exception ex)
        {
            base.OnException(sapEntity, ex);
            if (!string.IsNullOrWhiteSpace(sapEntity.SAPErrorCode) && string.IsNullOrWhiteSpace(sapEntity.SAPStatus))
            {
                sapEntity.SAPStatus = sapEntity.SAPErrorCode;
            }
        }
    }
}
