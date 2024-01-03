using System;
using System.ServiceModel;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.service;

namespace MapCall.SAP.Model.Services
{
    public class InspectionRecordInvoker : SAPServiceInvoker<SAPInspection, InspectionRecord_Create_OB_SYNClient,
        InspectionRecord_Create_OB_SYN>
    {
        #region Constants

        public const string NO_DATA_FOUND = "No Data Found";

        #endregion

        #region Constructors

        public InspectionRecordInvoker(ISAPHttpClient sapHttpClient) : base(sapHttpClient)
        {
            SendTimeOut = TimeSpan.FromSeconds(60);
        }

        #endregion

        #region Private Methods

        protected override SAPInspection InvokeInternal(SAPInspection sapInspection,
            InspectionRecord_Create_OB_SYN client)
        {
            try
            {
                var inspectionRequest = sapInspection.InspectionRecordCreate();
                var response = client
                              .InspectionRecord_Create_OB_SYNAsync(
                                   new InspectionRecord_Create_OB_SYNRequest(inspectionRequest)).Result;

                if (response.SAPNotificationNumber_Res.Length > 0)
                {
                    sapInspection.SAPNotificationNumber = response.SAPNotificationNumber_Res[0]?.SAPNotificationNumber;
                    sapInspection.Status = response.SAPNotificationNumber_Res[0]?.Status;
                    sapInspection.SAPErrorCode = response.SAPNotificationNumber_Res[0]?.Status;
                    sapInspection.CostCenter = response.SAPNotificationNumber_Res[0]?.CostCenter;
                }
                else
                {
                    sapInspection.Status = NO_DATA_FOUND;
                }
            }
            catch (FaultException e)
            {
                sapInspection.Status = RETRY_ERROR_TEXT + e;
            }
            catch (Exception ex)
            {
                sapInspection.Status = RETRY_ERROR_TEXT + ex;
            }

            return sapInspection;
        }

        #endregion
    }
}
