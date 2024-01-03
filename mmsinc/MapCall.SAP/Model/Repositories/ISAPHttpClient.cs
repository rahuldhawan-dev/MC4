using System;
using System.ServiceModel;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Services;
using MapCall.SAP.PreDispatchWS;

namespace MapCall.SAP.Model.Repositories
{
    public interface ISAPHttpClient
    {
        bool IsSiteRunning { get; set; }
        SAPEquipment GetEquipmentResponse(SAPEquipment sapEquipment);
        string UserName { get; set; }
        string Password { get; set; }
        SAPInspection GetInspectionResponse(SAPInspection sapInspection);
        string SAPInterface { get; set; }
        string SAPInterfaceNamespace { get; set; }
        TimeSpan? SendTimeOut { get; set; }

        TClient CreateServiceClient<TClient, TChannel>()
            where TClient : ClientBase<TChannel>
            where TChannel : class;

        SAPNotificationCollection GetNotificationResponse(SearchSapNotification searchSapNotification);
        SAPNotificationStatus GetNotificationStatusUpdate(SAPNotificationStatus notificationStatus);
        SAPWorkOrder CreateWorkOrder(SAPWorkOrder sapWorkOrder);
        SAPProgressWorkOrder ProgressWorkOrder(SAPProgressWorkOrder sapProgressWorkOrder);
        SAPNotificationCollection GetWorkOrder(SAPNotification sapNotification);
        SAPCompleteWorkOrder CompleteWorkOrder(SAPCompleteWorkOrder sapCompleteWorkOrder);
        SAPGoodsIssueCollection ApproveGoodsIssue(SAPGoodsIssue sapGoodIssue);

        SAPTechnicalMasterAccountCollection GetTechnicalMasterAccountResponse(
            SearchSapTechnicalMaster sapTechnicalMasterSearch);

        SAPWBSElementCollection GetWBSElement(SAPWBSElement sapWBSElement);
        SAPNewServiceInstallation SaveNewServiceInstallation(SAPNewServiceInstallation sapNewServiceInstallation);
        SAPNewServiceInstallation SaveService(SAPNewServiceInstallation sapNewServiceInstallation);
        SAPFunctionalLocationCollection GetFunctionalLocation(SearchSapFunctionalLocation search);
        SAPCustomerOrderCollection GetCustomerOrder(SearchSapCustomerOrder search);
        SAPManufacturerCollection GetManufacturer(SAPManufacturer sapManufacturer);

        SAPCreateUnscheduledWorkOrder CreateUnscheduleWorkOrder(
            SAPCreateUnscheduledWorkOrder sapCreateUnscheduledWorkOrder);

        SAPProgressUnscheduledWorkOrder ProgressUnscheduleWorkOrder(
            SAPProgressUnscheduledWorkOrder sapProgressUnscheduledWorkOrder);

        SAPCompleteUnscheduledWorkOrder CompleteUnscheduleWorkOrder(
            SAPCompleteUnscheduledWorkOrder sapCompleteUnscheduledWorkOrder);

        SAPMaintenancePlanLookupCollection GetMaintenancePlan(SAPMaintenancePlanLookup sapMaintenancePlanLookup);
        SAPMaintenancePlanUpdateCollection UpdateMaintenancePlan(SAPMaintenancePlanUpdate sapMaintenancePlanUpdate);

        SAPCreatePreventiveWorkOrderCollection CreatePreventiveWorkOrder(
            SAPCreatePreventiveWorkOrder SapCreatePreventiveWorkOrder);

        SAPWorkOrderStatusUpdateRequest WorkOrderStatusUpdate(
            SAPWorkOrderStatusUpdateRequest SapWorkOrderStatusUpdateRequest);

        WO_Predispatch_PULL_StatusRecord[] SearchShortCycleWorkOrders(WO_Predispatch_PULL_QueryRecord[] search);
    }
}
