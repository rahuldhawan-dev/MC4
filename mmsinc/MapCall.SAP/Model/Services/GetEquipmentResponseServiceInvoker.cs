using System;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.SAPEquipmentWS;

namespace MapCall.SAP.Model.Services
{
    public class
        GetEquipmentResponseServiceInvoker : SAPServiceInvoker<SAPEquipment, Equipments_OB_SYNClient, Equipments_OB_SYN>
    {
        #region Constructor

        public GetEquipmentResponseServiceInvoker(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Private Methods

        protected override SAPEquipment InvokeInternal(SAPEquipment sapEntity, Equipments_OB_SYN client)
        {
            var equipmentRequest = sapEntity.ToEquipmentsEquipments();
            var response =
                client.Equipments_OB_SYN(new Equipments_OB_SYNRequest {Equipments_Request = equipmentRequest});
            if (response.Equipments_Response.Length > 0)
            {
                // TODO: Why would we receive a null item in the Equipments_Response array?
                // TODO: If we're receiving an array, why are we only dealing with the first result?
                sapEntity.SAPEquipmentNumber = response.Equipments_Response[0]?.SAPEquipmentNo?.TrimStart('0');
                sapEntity.Status = response.Equipments_Response[0]?.Status;
            }

            return sapEntity;
        }

        protected override void OnException(SAPEquipment sapEntity, Exception ex)
        {
            // This override only exists to set the Status property. I do not know what
            // this property is used for other than to later set SAPErrorCode to the
            // same value.
            sapEntity.Status = RETRY_ERROR_TEXT + ex;
        }

        protected override void AfterInvoke(SAPEquipment sapEntity)
        {
            base.AfterInvoke(sapEntity);
            sapEntity.SAPErrorCode = sapEntity.Status;
        }

        #endregion
    }
}
