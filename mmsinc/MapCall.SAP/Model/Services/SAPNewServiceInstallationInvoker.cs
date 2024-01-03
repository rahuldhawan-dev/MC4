using System;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.service;

namespace MapCall.SAP.Model.Services
{
    public class SAPNewServiceInstallationInvoker : SAPServiceInvoker<SAPNewServiceInstallation,
        W1v_New_ServiceInstallation_Get_OB_SYNClient, W1v_New_ServiceInstallation_Get_OB_SYN>
    {
        #region Constructors

        public SAPNewServiceInstallationInvoker(ISAPHttpClient sapHttpClient) : base(sapHttpClient)
        {
            SendTimeOut = TimeSpan.FromSeconds(30);
        }

        #endregion

        #region Private Methods

        protected override SAPNewServiceInstallation InvokeInternal(SAPNewServiceInstallation sapEntity,
            W1v_New_ServiceInstallation_Get_OB_SYN client)
        {
            var newServiceInstallationRequest = sapEntity.NewServiceInstallationRequest();
            var retVal = client.W1v_New_ServiceInstallation_Get_OB_SYNAsync(new W1v_New_ServiceInstallation_Get_OB_SYNRequest {
                W1v_New_ServiceInstallation_Request = newServiceInstallationRequest
            }).Result;

            if (retVal.W1v_New_ServiceInstallation_Response != null && retVal.W1v_New_ServiceInstallation_Response.Length > 0)
            {
                sapEntity.SAPStatus = retVal.W1v_New_ServiceInstallation_Response[0].SAPStatus;
                sapEntity.WorkOrderNumber = retVal.W1v_New_ServiceInstallation_Response[0].WorkOrderNumber;
            }
            else
            {
                sapEntity.SAPStatus = "No record was returned from the SAP Web Service";
            }

            return sapEntity;
        }

        #endregion
    }
}
