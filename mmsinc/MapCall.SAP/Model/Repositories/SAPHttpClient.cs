using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.CreateUnscheduledWOWS;
using MapCall.SAP.CustomerOrder;
using MapCall.SAP.FunctionalLocationWS;
using MapCall.SAP.service;
using MapCall.SAP.GoodsIssueWS;
using MapCall.SAP.ManufacturerLookupWS;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.NotificationStatusWS;
using MapCall.SAP.ProgressScheduledUnscheduledWOWS;
using MapCall.SAP.TechnicalMasterWS;
using MapCall.SAP.WBSElementWS;
using MapCall.SAP.CompleteScheduledUnscheduledWOWS;
using MapCall.SAP.MaintenancePlanLookupWS;
using MapCall.SAP.MaintenancePlanUpdateWS;
using MapCall.SAP.GetPMOrderWS;
using MapCall.SAP.Model.Services;
using MapCall.SAP.PreDispatchWS;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPHttpClient : ISAPHttpClient
    {
        #region Constants

        public const string
            RETRY_ERROR_TEXT = "RETRY::",
            DEV_QUERY_PATH =
                "?senderParty=&senderService=MapCall&receiverParty=&receiverService=&interface={0}&interfaceNamespace={1}",
            DEVELOPMENT_USER_NAME = "sys_mapcall",
            DEVELOPMENT_PASSWORD = "Amwater11",
            DEV_URL_INTERNAL = "https://gateway-dev.amwater.com/qa2/XISOAPAdapter/MessageServlet";

        #endregion

        #region Private Members

        private bool? _isSiteRunning;
        private Uri _baseAddress;
        private string _userName, _password, _queryPath;
        private TimeSpan? _timeOut;

        #endregion

        #region Properties

        public Uri BaseAddress
        {
            get { return _baseAddress ?? (_baseAddress = new Uri(DEV_URL_INTERNAL)); }
            set { _baseAddress = value; }
        }

        public string QueryPath
        {
            get
            {
                _queryPath = string.Format(DEV_QUERY_PATH, SAPInterface, SAPInterfaceNamespace);
                return _queryPath;
            }
            set { _queryPath = value; }
        }

        public string UserName
        {
            get { return string.IsNullOrWhiteSpace(_userName) ? _userName = DEVELOPMENT_USER_NAME : _userName; }
            set { _userName = value; }
        }

        public string Password
        {
            get { return string.IsNullOrWhiteSpace(_password) ? _password = DEVELOPMENT_PASSWORD : _password; }
            set { _password = value; }
        }

        public string SAPInterface { get; set; }
        public string SAPInterfaceNamespace { get; set; }

        public TimeSpan? SendTimeOut
        {
            get => _timeOut ?? (_timeOut = new TimeSpan(0, 10, 0));
            set => _timeOut = value;
        }

        #endregion

        #region Exposed methods

        public bool IsSiteRunning
        {
            get
            {
                if (_isSiteRunning == null)
                    _isSiteRunning = CheckSite();
                return _isSiteRunning.Value;
            }
            set { _isSiteRunning = value; }
        }

        /// <summary>
        /// Returns a service client with the binding, address, and credentials already created. Callers are
        /// responsible for disposing this instance!
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <returns></returns>
        public TClient CreateServiceClient<TClient, TChannel>()
            where TClient : ClientBase<TChannel>
            where TChannel : class
        {
            var binding = Bindings();
            binding.SendTimeout = SendTimeOut.Value;
            var address = new EndpointAddress(BaseAddress + SAPInterface);

            var serviceClient = (TClient)Activator.CreateInstance(typeof(TClient), binding, address);
            Credentials(serviceClient.ClientCredentials.UserName);

            return serviceClient;
        }

        /// <summary>
        /// Create/Update equipment in SAP
        /// Valves, Hydrants, Etc.
        /// 
        /// </summary>
        /// <param name="sapEquipment"></param>
        /// <returns></returns>
        public SAPEquipment GetEquipmentResponse(SAPEquipment sapEquipment)
        {
            var invoker = new GetEquipmentResponseServiceInvoker(this);
            return invoker.Invoke(sapEquipment);
        }

        /// <summary>
        /// Create/Update inspection items in SAP
        /// Valves, Hydrants, etc.
        /// http://localhost:15765/FieldOperations/ValveInspection/New/43748
        /// </summary>
        /// <param name="sapInspection"></param>
        /// <returns></returns>
        public SAPInspection GetInspectionResponse(SAPInspection sapInspection)
        {
            var invoker = new InspectionRecordInvoker(this);
            return invoker.Invoke(sapInspection);
        }

        /// <summary>
        /// Get notifcation from SAP based on search criteria
        /// http://localhost:15765/FieldOperations/SapNotification
        /// </summary>
        /// <param name="searchSapNotification"></param>
        /// <returns></returns>
        public SAPNotificationCollection GetNotificationResponse(SearchSapNotification searchSapNotification)
        {
            var notificationAggregate = new GetNotificationAggregate {
                SearchSapNotifications = searchSapNotification
            };
            var invoker = new GetNotificationInvoker(this);

            return invoker.Invoke(notificationAggregate).SAPNotificationCollections;
        }

        /// <summary>
        /// Get notification details based on notification number from SAP
        /// </summary>
        /// <param name="sapNotification"></param>
        /// <returns></returns>
        public SAPNotificationCollection GetWorkOrder(SAPNotification sapNotification)
        {
            var notificationAggregate = new GetNotificationAggregate {
                SAPNotification = sapNotification
            };
            var invoker = new GetWorkOrderInvoker(this);

            return invoker.Invoke(notificationAggregate).SAPNotificationCollections;
        }

        /// <summary>
        /// Cancel/Complete notifications in SAP
        /// http://localhost:15765/FieldOperations/SapNotification
        /// </summary>
        /// <param name="sapNotificationStatus"></param>
        /// <returns></returns>
        public SAPNotificationStatus GetNotificationStatusUpdate(SAPNotificationStatus sapNotificationStatus)
        {
            try
            {
                var NotificationStatusRecord = sapNotificationStatus.UpdateNotificationStatus(sapNotificationStatus);
                CustomBinding binding = Bindings();

                sapNotificationStatus = new SAPNotificationStatus();
                var address = new EndpointAddress(BaseAddress + SAPInterface);
                using (var client = new NotificationStatus_OB_SYNClient(binding, address))
                {
                    Credentials(client.ClientCredentials.UserName);

                    NotificationStatus_OB_SYNResponse retVal =
                        ((NotificationStatus_OB_SYN)(client)).NotificationStatus_OB_SYN(
                            new NotificationStatus_OB_SYNRequest {NotificationStatus_Request = NotificationStatusRecord});

                    if (retVal.NotificationStatus_Response.Length > 0)
                        sapNotificationStatus = new SAPNotificationStatus(retVal.NotificationStatus_Response[0]);
                    else
                        sapNotificationStatus = new SAPNotificationStatus {SAPMessage = "Response not yet configure"};
                    client.Close();
                }
            }
            catch (FaultException e)
            {
                sapNotificationStatus = new SAPNotificationStatus {SAPMessage = RETRY_ERROR_TEXT + e};
            }
            catch (Exception ex)
            {
                sapNotificationStatus = new SAPNotificationStatus {SAPMessage = RETRY_ERROR_TEXT + ex};
            }

            return sapNotificationStatus;
        }

        /// <summary>
        /// Creates a T&D work order in SAP.
        /// MapCallMVC - FieldOperations/WorkOrder/New
        /// Also still lingering in mapcall_workorder for functional tests only
        /// </summary>
        /// <param name="sapWorkOrder"></param>
        /// <returns></returns>
        public SAPWorkOrder CreateWorkOrder(SAPWorkOrder sapWorkOrder)
        {
            var invoker = new CreateWorkOrderInvoker(this);
            return invoker.Invoke(sapWorkOrder);
        }

        public SAPProgressWorkOrder ProgressWorkOrder(SAPProgressWorkOrder sapProgressWorkorder)
        {
            var invoker = new ProgressWorkOrderInvoker(this);
            return invoker.Invoke(sapProgressWorkorder);
        }

        public SAPCompleteWorkOrder CompleteWorkOrder(SAPCompleteWorkOrder sapCompleteWorkOrder)
        {
            var invoker = new CompleteWorkOrderInvoker(this);
            return invoker.Invoke(sapCompleteWorkOrder);
        }

        /// <summary>
        /// This is getting call from stock approval
        /// </summary>
        /// <param name="sapGoodIssue"></param>
        /// <returns></returns>
        public SAPGoodsIssueCollection ApproveGoodsIssue(SAPGoodsIssue sapGoodIssue)
        {
            var sapGoodsIssueCollection = new SAPGoodsIssueCollection();

            try
            {
                GoodsIssueGoodsIssue[] goodsIssueRequest = sapGoodIssue.ApproveGoodIssue();

                CustomBinding binding = Bindings();

                var address = new EndpointAddress(BaseAddress + SAPInterface);

                using (var client = new GoodsIssue_OB_SYNClient(binding, address))
                {
                    Credentials(client.ClientCredentials.UserName);

                    GoodsIssue_OB_SYNResponse retVal =
                        ((GoodsIssue_OB_SYN)(client)).GoodsIssue_OB_SYN(new GoodsIssue_OB_SYNRequest
                            {GoodsIssueRequest = goodsIssueRequest});
                    if (retVal.GoodsIssueResponse.Length > 0)
                    {
                        for (int i = 0; i < retVal.GoodsIssueResponse.Length; i++)
                        {
                            if (retVal.GoodsIssueResponse[i].OrderNumber != null)
                                sapGoodIssue = new SAPGoodsIssue(retVal.GoodsIssueResponse[i]);
                            else
                                sapGoodIssue = new SAPGoodsIssue {
                                    Status = retVal.GoodsIssueResponse[i]?.Status,
                                    MaterialDocument = retVal.GoodsIssueResponse[i]?.MaterialDocument
                                };
                            sapGoodsIssueCollection.Items.Add(sapGoodIssue);
                        }
                    }
                    else
                        sapGoodIssue.Status = "No data found";

                    client.Close();
                }
            }
            catch (FaultException e)
            {
                sapGoodIssue.Status = RETRY_ERROR_TEXT + e;
                sapGoodsIssueCollection.Items.Add(sapGoodIssue);
            }
            catch (Exception ex)
            {
                sapGoodIssue.Status = RETRY_ERROR_TEXT + ex;
                sapGoodsIssueCollection.Items.Add(sapGoodIssue);
            }

            return sapGoodsIssueCollection;
        }

        /// <summary>
        /// This method is used to pull back deatils about a premise/installation
        /// This is used in the MVC project.
        /// http://localhost:15765/Customer/SAPTechnicalMasterAccount?Equipment=&InstallationType=&PremiseNumber=9240690201 
        /// </summary>
        /// <param name="sapTechnicalMasterSearch"></param>
        /// <returns></returns>
        public SAPTechnicalMasterAccountCollection GetTechnicalMasterAccountResponse(
            SearchSapTechnicalMaster sapTechnicalMasterSearch)
        {
            var sapTechnicalMaster = new SAPTechnicalMasterAccount();

            var sapTechnicalMasterAccountCollection = new SAPTechnicalMasterAccountCollection();

            try
            {
                TechnicalMaster_AccountDetailsQuery technicalMasterDataRequest =
                    TechnicalDataRequest(sapTechnicalMasterSearch);

                CustomBinding binding = Bindings();

                var address = new EndpointAddress(BaseAddress + SAPInterface);

                using (var client = new TechnicalMaster_AccountDetails_Get_OB_SYNClient(binding, address))
                {
                    Credentials(client.ClientCredentials.UserName);

                    TechnicalMaster_AccountDetails_Get_OB_SYNResponse retVal =
                        ((TechnicalMaster_AccountDetails_Get_OB_SYN)(client)).TechnicalMaster_AccountDetails_Get_OB_SYN(
                            new TechnicalMaster_AccountDetails_Get_OB_SYNRequest
                                {TechnicalMaster_AccountDetails_Request = technicalMasterDataRequest});

                    if (retVal.TechnicalMaster_AccountDetails_Response != null &&
                        retVal.TechnicalMaster_AccountDetails_Response.Record?.Length >= 0)
                    {
                        for (int i = 0; i < retVal.TechnicalMaster_AccountDetails_Response.Record.Length; i++)
                        {
                            sapTechnicalMaster =
                                new SAPTechnicalMasterAccount(retVal.TechnicalMaster_AccountDetails_Response.Record[i]);
                            sapTechnicalMasterAccountCollection.Items.Add(sapTechnicalMaster);
                        }
                    }
                    else
                    {
                        sapTechnicalMaster = new SAPTechnicalMasterAccount
                            {SAPError = retVal.TechnicalMaster_AccountDetails_Response.Exception?.ToString()};
                        sapTechnicalMasterAccountCollection.Items.Add(sapTechnicalMaster);
                    }

                    client.Close();
                }
            }

            catch (FaultException e)
            {
                sapTechnicalMasterAccountCollection.Items.Add(new SAPTechnicalMasterAccount
                    {SAPError = RETRY_ERROR_TEXT + e});
            }
            catch (Exception ex)
            {
                sapTechnicalMasterAccountCollection.Items.Add(new SAPTechnicalMasterAccount
                    {SAPError = RETRY_ERROR_TEXT + ex});
            }

            return sapTechnicalMasterAccountCollection;
        }

        /// <summary>
        /// http://localhost:15765/FieldOperations/Service/New
        /// For selecting and validating correct WBS Numbers from SAP
        /// </summary>
        /// <param name="sapWBSElement">WBSNumber E17-1420-171121</param>
        /// <returns></returns>
        public SAPWBSElementCollection GetWBSElement(SAPWBSElement sapWBSElement)
        {
            SAPWBSElementCollection sapWBSElementCollection = new SAPWBSElementCollection();

            try
            {
                var wbsElementRequest = sapWBSElement.WBSElementRequest();

                CustomBinding binding = Bindings();

                var address = new EndpointAddress(BaseAddress + SAPInterface);

                using (var client = new WBSElement_Get_OB_SYNClient(binding, address))
                {
                    Credentials(client.ClientCredentials.UserName);

                    WBSElement_Get_OB_SYNResponse retVal =
                        ((WBSElement_Get_OB_SYN)(client)).WBSElement_Get_OB_SYN(new WBSElement_Get_OB_SYNRequest
                            {WBSElementRequest = wbsElementRequest});
                    if (retVal.WBSElementResponse.Record != null)
                    {
                        for (int i = 0; i < retVal.WBSElementResponse.Record.Length; i++)
                        {
                            if (retVal.WBSElementResponse.Record[i].WBSElement != null)
                                sapWBSElement = new SAPWBSElement(retVal.WBSElementResponse.Record[i]);
                            else
                                sapWBSElement = new SAPWBSElement {SAPErrorCode = retVal.WBSElementResponse?.SAPStatus};
                            sapWBSElementCollection.Items.Add(sapWBSElement);
                        }
                    }
                    else
                    {
                        sapWBSElement = new SAPWBSElement {
                            SAPErrorCode = retVal.WBSElementResponse?.SAPStatus != null
                                ? retVal.WBSElementResponse?.SAPStatus.ToString()
                                : "No record was returned from the SAP Web Service"
                        };
                        sapWBSElementCollection.Items.Add(sapWBSElement);
                    }

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                sapWBSElement = new SAPWBSElement {SAPErrorCode = RETRY_ERROR_TEXT + ex};
                sapWBSElementCollection.Items.Add(sapWBSElement);
            }

            return sapWBSElementCollection;
        }

        /// <summary>
        /// Used to pull SAP fucntional location information from SAP
        /// Can be tested in mvc project http://localhost:15765/Equipment/Search
        /// </summary>
        /// <param name="search">OperatingCenter: NJ6, planningPlant:P216, TechincalObjectType: Administration</param>
        /// <returns></returns>
        public SAPFunctionalLocationCollection GetFunctionalLocation(SearchSapFunctionalLocation search)
        {
            SAPFunctionalLocationCollection sapFunctionalLocationCollection = new SAPFunctionalLocationCollection();
            SAPFunctionalLocation sapFunctionalLocation = new SAPFunctionalLocation();
            try
            {
                var FunctionalLocationRequest = sapFunctionalLocation.FunctionalLocationRequest(search);

                CustomBinding binding = Bindings();

                var address = new EndpointAddress(BaseAddress + SAPInterface);

                using (var client = new FunctionalLocation_Get_OB_SYNClient(binding, address))
                {
                    Credentials(client.ClientCredentials.UserName);

                    FunctionalLocation_Get_OB_SYNResponse retVal =
                        ((FunctionalLocation_Get_OB_SYN)(client)).FunctionalLocation_Get_OB_SYN(
                            new FunctionalLocation_Get_OB_SYNRequest
                                {FunctionalLocationRequest = FunctionalLocationRequest});
                    if (retVal.FunctionalLocationResponse.Record != null)
                    {
                        for (int i = 0; i < retVal.FunctionalLocationResponse.Record.Length; i++)
                        {
                            if (retVal.FunctionalLocationResponse.Record[i].FunctionalLocation != null)
                                sapFunctionalLocation =
                                    new SAPFunctionalLocation(retVal.FunctionalLocationResponse.Record[i]);
                            else
                                sapFunctionalLocation = new SAPFunctionalLocation
                                    {SAPErrorCode = retVal.FunctionalLocationResponse?.SAPStatus};
                            sapFunctionalLocationCollection.Items.Add(sapFunctionalLocation);
                        }
                    }
                    else
                    {
                        sapFunctionalLocation = new SAPFunctionalLocation {
                            SAPErrorCode = retVal.FunctionalLocationResponse?.SAPStatus != null
                                ? retVal.FunctionalLocationResponse?.SAPStatus.ToString()
                                : "No record was returned from the SAP Web Service"
                        };
                        sapFunctionalLocationCollection.Items.Add(sapFunctionalLocation);
                    }

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                sapFunctionalLocation = new SAPFunctionalLocation {SAPErrorCode = RETRY_ERROR_TEXT + ex};
                sapFunctionalLocationCollection.Items.Add(sapFunctionalLocation);
            }

            return sapFunctionalLocationCollection;
        }

        /// <summary>
        /// Used for looking up details on a short cycle (customer) order
        /// http://localhost:15765/SAP/SAPCustomerOrder/Search
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public SAPCustomerOrderCollection GetCustomerOrder(SearchSapCustomerOrder search)
        {
            SAPCustomerOrderCollection sapCustomerOrderCollection = new SAPCustomerOrderCollection();
            SAPCustomerOrder sapCustomerOrder = new SAPCustomerOrder();
            try
            {
                var CustomerOrderRequest = sapCustomerOrder.CustomerOrderRequest(search);

                CustomBinding binding = Bindings();

                var address = new EndpointAddress(BaseAddress + SAPInterface);

                using (var client = new CustomerOrder_FSR_Get_OB_SYNClient(binding, address))
                {
                    Credentials(client.ClientCredentials.UserName);

                    CustomerOrder_FSR_Get_OB_SYNResponse retVal =
                        ((CustomerOrder_FSR_Get_OB_SYN)(client)).CustomerOrder_FSR_Get_OB_SYN(
                            new CustomerOrder_FSR_Get_OB_SYNRequest {CustomerOrder_FSRRequest = CustomerOrderRequest});
                    if (retVal.CustomerOrder_FSRResponse != null &&
                        retVal.CustomerOrder_FSRResponse.MapCall_Response != null)
                    {
                        if (retVal.CustomerOrder_FSRResponse.MapCall_Response.Response_FSR != null)
                        {
                            for (int i = 0;
                                 i < retVal.CustomerOrder_FSRResponse.MapCall_Response.Response_FSR.Length;
                                 i++)
                            {
                                sapCustomerOrder =
                                    new SAPCustomerOrder(
                                        retVal.CustomerOrder_FSRResponse.MapCall_Response.Response_FSR[i]);
                                sapCustomerOrderCollection.Items.Add(sapCustomerOrder);
                            }
                        }
                        else if (retVal.CustomerOrder_FSRResponse.MapCall_Response.Response_WorkOrder != null)
                        {
                            sapCustomerOrder =
                                new SAPCustomerOrder(retVal.CustomerOrder_FSRResponse.MapCall_Response
                                                           .Response_WorkOrder[0]);
                            sapCustomerOrderCollection.Items.Add(sapCustomerOrder);
                        }
                    }
                    else
                    {
                        sapCustomerOrder = new SAPCustomerOrder {
                            SAPErrorCode = retVal.CustomerOrder_FSRResponse?.SAPStatus != null
                                ? retVal.CustomerOrder_FSRResponse?.SAPStatus.ToString()
                                : "No record was returned from the SAP Web Service"
                        };
                        sapCustomerOrderCollection.Items.Add(sapCustomerOrder);
                    }

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                sapCustomerOrder = new SAPCustomerOrder {SAPErrorCode = RETRY_ERROR_TEXT + ex};
                sapCustomerOrderCollection.Items.Add(sapCustomerOrder);
            }

            return sapCustomerOrderCollection;
        }

        /// <summary>
        /// Used for looking up the correct equipment type from SAP
        /// Populated SAP Equipment Manufacturer drop down
        /// http://localhost:15765/Equipment/New
        /// </summary>
        /// <param name="sapManufacturer"></param>
        /// <returns></returns>
        public SAPManufacturerCollection GetManufacturer(SAPManufacturer sapManufacturer)
        {
            SAPManufacturerCollection sapManufacturerCollection = new SAPManufacturerCollection();

            try
            {
                var Manufacturer_Request = sapManufacturer.ManufacturerRequest(sapManufacturer);

                CustomBinding binding = Bindings();

                var address = new EndpointAddress(BaseAddress + SAPInterface);

                using (var client = new ManufacturerLookup_OB_SYNClient(binding, address))
                {
                    Credentials(client.ClientCredentials.UserName);

                    ManufacturerLookup_OB_SYNResponse retVal =
                        ((ManufacturerLookup_OB_SYN)(client)).ManufacturerLookup_OB_SYN(
                            new ManufacturerLookup_OB_SYNRequest {Manufacturer_Request = Manufacturer_Request});
                    if (retVal.Manufacturer_Response != null)
                    {
                        for (int i = 0; i < retVal.Manufacturer_Response.Manufacturer.Length; i++)
                        {
                            sapManufacturer = new SAPManufacturer(retVal.Manufacturer_Response.Manufacturer[i]);
                            sapManufacturerCollection.Items.Add(sapManufacturer);
                        }
                    }
                    else
                    {
                        sapManufacturer = new SAPManufacturer {
                            SAPErrorCode = retVal.Manufacturer_Response?.SAPStatus != null
                                ? retVal.Manufacturer_Response?.SAPStatus.ToString()
                                : "No record was returned from the SAP Web Service"
                        };
                        sapManufacturerCollection.Items.Add(sapManufacturer);
                    }

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                sapManufacturer = new SAPManufacturer {SAPErrorCode = RETRY_ERROR_TEXT + ex};
                sapManufacturerCollection.Items.Add(sapManufacturer);
            }

            return sapManufacturerCollection;
        }

        /// <summary>
        /// Production Work Order - Create
        /// Production/ProductionWorkOrder/New
        /// </summary>
        /// <param name="sapCreateUnscheduledWorkOrder"></param>
        /// <returns></returns>
        public SAPCreateUnscheduledWorkOrder CreateUnscheduleWorkOrder(
            SAPCreateUnscheduledWorkOrder sapCreateUnscheduledWorkOrder)
        {
            try
            {
                var UnscheduledWORequest = sapCreateUnscheduledWorkOrder.UnscheduledWorkOrderRequest();

                CustomBinding binding = Bindings();

                var address = new EndpointAddress(BaseAddress + SAPInterface);

                using (var client = new CreateUnscheduledWO_OB_SYNClient(binding, address))
                {
                    Credentials(client.ClientCredentials.UserName);

                    CreateUnscheduledWO_OB_SYNResponse retVal =
                        ((CreateUnscheduledWO_OB_SYN)(client)).CreateUnscheduledWO_OB_SYN(
                            new CreateUnscheduledWO_OB_SYNRequest {UnscheduledWORequest = UnscheduledWORequest});
                    if (retVal.UnscheduledWOResponse != null && retVal.UnscheduledWOResponse.Status != null)
                    {
                        sapCreateUnscheduledWorkOrder.NotificationNumber =
                            retVal.UnscheduledWOResponse.Status[0]?.SAPNotificationNumber;
                        sapCreateUnscheduledWorkOrder.OrderNumber =
                            retVal.UnscheduledWOResponse.Status[0]?.SAPOrderNumber;
                        sapCreateUnscheduledWorkOrder.SAPErrorCode = retVal.UnscheduledWOResponse.SAPStatus;
                        sapCreateUnscheduledWorkOrder.WBSElement = retVal.UnscheduledWOResponse.Status[0]?.WBSElement;
                    }
                    else
                        sapCreateUnscheduledWorkOrder.SAPErrorCode = retVal.UnscheduledWOResponse.SAPStatus;

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                sapCreateUnscheduledWorkOrder.SAPErrorCode = RETRY_ERROR_TEXT + ex.Message;
            }

            return sapCreateUnscheduledWorkOrder;
        }

        public SAPProgressUnscheduledWorkOrder ProgressUnscheduleWorkOrder(
            SAPProgressUnscheduledWorkOrder sapProgressUnscheduledWorkOrder)
        {
            SAPProgressUnscheduledWorkOrder progressUnscheduleWorkOrder = new SAPProgressUnscheduledWorkOrder();
            try
            {
                //request
                var ProgressScheduledUnscheduledWORequest =
                    sapProgressUnscheduledWorkOrder.UnscheduledProgressWorkOrderRequest();

                CustomBinding binding = Bindings();

                var address = new EndpointAddress(BaseAddress + SAPInterface);

                using (var client = new ProgressScheduledUnscheduledWO_OB_SYNClient(binding, address))
                {
                    Credentials(client.ClientCredentials.UserName);

                    ProgressScheduledUnscheduledWO_OB_SYNResponse retVal =
                        ((ProgressScheduledUnscheduledWO_OB_SYN)(client)).ProgressScheduledUnscheduledWO_OB_SYN(
                            new ProgressScheduledUnscheduledWO_OB_SYNRequest {
                                ProgressScheduledUnscheduledWORequest = ProgressScheduledUnscheduledWORequest
                            });
                    if (retVal.ProgressScheduledUnscheduledWOResponse != null &&
                        retVal.ProgressScheduledUnscheduledWOResponse.UnscheduledWOResponse != null)
                    {
                        progressUnscheduleWorkOrder.NotificationNumber = retVal.ProgressScheduledUnscheduledWOResponse
                           .UnscheduledWOResponse[0]?.NotificationNumber;
                        progressUnscheduleWorkOrder.OrderNumber = retVal.ProgressScheduledUnscheduledWOResponse
                                                                        .UnscheduledWOResponse[0]?.OrderNumber;
                        progressUnscheduleWorkOrder.SAPErrorCode = retVal.ProgressScheduledUnscheduledWOResponse
                                                                         .UnscheduledWOResponse[0]?.SAPStatus;
                        progressUnscheduleWorkOrder.WBSElement = retVal.ProgressScheduledUnscheduledWOResponse
                                                                       .UnscheduledWOResponse[0]?.WBSElement;

                        if (retVal.ProgressScheduledUnscheduledWOResponse.UnscheduledWOResponse[0]
                                  .ChildNotification_Equipment != null &&
                            retVal.ProgressScheduledUnscheduledWOResponse.UnscheduledWOResponse[0]
                                  .ChildNotification_Equipment.Any())
                        {
                            progressUnscheduleWorkOrder.SapProductionWorkOrderChildNotification =
                                new SAPProductionWorkOrderChildNotification[retVal
                                                                           .ProgressScheduledUnscheduledWOResponse
                                                                           .UnscheduledWOResponse[0]
                                                                           .ChildNotification_Equipment.ToList().Count];
                            for (int i = 0;
                                 i < retVal.ProgressScheduledUnscheduledWOResponse.UnscheduledWOResponse[0]
                                           .ChildNotification_Equipment.ToList().Count;
                                 i++)
                            {
                                var thing = retVal.ProgressScheduledUnscheduledWOResponse.UnscheduledWOResponse[0]
                                                  .ChildNotification_Equipment[i];

                                var childNotification = new SAPProductionWorkOrderChildNotification {
                                    NotificationNumber = thing.NotificationNo,
                                    SAPEquipmentNumber = thing.EquipmentNo
                                };

                                //To add MeasuringPoint response
                                if (thing.MeasuringPoint != null && thing.MeasuringPoint.Any())
                                {
                                    childNotification.SapProductionWorkOrderMeasuringPoints =
                                        new SAPProductionWorkOrderMeasuringPoints[thing.MeasuringPoint.ToList().Count];

                                    for (int k = 0; k < thing.MeasuringPoint.ToList().Count; k++)
                                    {
                                        var measuringPoint = thing.MeasuringPoint[k];

                                        var measuringPoint1 = new SAPProductionWorkOrderMeasuringPoints();
                                        measuringPoint1.MeasuringPointStatus = measuringPoint.MeasuringPointStatus;
                                        measuringPoint1.MeasuringDocument = measuringPoint.MeasuringDocument;
                                        measuringPoint1.Unit1 = measuringPoint.MeasuringPoint1;
                                        childNotification.SapProductionWorkOrderMeasuringPoints[k] =
                                            new SAPProductionWorkOrderMeasuringPoints();
                                        childNotification.SapProductionWorkOrderMeasuringPoints[k] = measuringPoint1;
                                    }
                                }

                                progressUnscheduleWorkOrder.SapProductionWorkOrderChildNotification[i] =
                                    new SAPProductionWorkOrderChildNotification();
                                progressUnscheduleWorkOrder.SapProductionWorkOrderChildNotification[i] =
                                    childNotification;
                            }
                        }
                    }

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                progressUnscheduleWorkOrder.SAPErrorCode = RETRY_ERROR_TEXT + ex.Message;
            }

            return progressUnscheduleWorkOrder;
        }

        public SAPCompleteUnscheduledWorkOrder CompleteUnscheduleWorkOrder(
            SAPCompleteUnscheduledWorkOrder sapCompleteUnscheduledWorkOrder)
        {
            try
            {
                var request = sapCompleteUnscheduledWorkOrder.UnscheduledCompleteWorkOrderRequest();

                CustomBinding binding = Bindings();

                var address = new EndpointAddress(BaseAddress + SAPInterface);

                using (var client = new CompleteScheduledUnscheduledWO_OB_SYNClient(binding, address))
                {
                    Credentials(client.ClientCredentials.UserName);

                    CompleteScheduledUnscheduledWO_OB_SYNResponse retVal =
                        ((CompleteScheduledUnscheduledWO_OB_SYN)(client)).CompleteScheduledUnscheduledWO_OB_SYN(
                            new CompleteScheduledUnscheduledWO_OB_SYNRequest
                                {CompleteScheduledUnscheduledWORequest = request});
                    if (retVal.CompleteScheduledUnscheduledWOResponse != null &&
                        retVal.CompleteScheduledUnscheduledWOResponse.Length > 0)
                    {
                        sapCompleteUnscheduledWorkOrder.NotificationNumber =
                            retVal.CompleteScheduledUnscheduledWOResponse[0]?.NotificationNumber;
                        sapCompleteUnscheduledWorkOrder.OrderNumber =
                            retVal.CompleteScheduledUnscheduledWOResponse[0]?.OrderNumber;
                        sapCompleteUnscheduledWorkOrder.SAPErrorCode =
                            retVal.CompleteScheduledUnscheduledWOResponse[0]?.SAPStatus;
                        sapCompleteUnscheduledWorkOrder.WbsElement =
                            retVal.CompleteScheduledUnscheduledWOResponse[0]?.WBSElement;
                    }

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                sapCompleteUnscheduledWorkOrder.SAPErrorCode = RETRY_ERROR_TEXT + ex.Message;
            }

            return sapCompleteUnscheduledWorkOrder;
        }

        /// <summary>
        /// Looks up maintenance plans for production work orders
        /// http://localhost:15765/SAP/SAPMaintenancePlan/Search
        /// </summary>
        /// <param name="sapMaintenancePlanLookup"></param>
        /// <returns></returns>
        public SAPMaintenancePlanLookupCollection GetMaintenancePlan(SAPMaintenancePlanLookup sapMaintenancePlanLookup)
        {
            var SapMaintenancePlanLookupCollection = new SAPMaintenancePlanLookupCollection();
            try
            {
                var MaintenancePlanLookup_Request = sapMaintenancePlanLookup.Request();

                CustomBinding binding = Bindings();

                var address = new EndpointAddress(BaseAddress + SAPInterface);

                using (var client = new MaintenancePlanLookup_OB_SYNClient(binding, address))
                {
                    Credentials(client.ClientCredentials.UserName);

                    MaintenancePlanLookup_OB_SYNResponse retVal =
                        ((MaintenancePlanLookup_OB_SYN)(client)).MaintenancePlanLookup_OB_SYN(
                            new MaintenancePlanLookup_OB_SYNRequest
                                {MaintenancePlanLookup_Request = MaintenancePlanLookup_Request});
                    if (retVal.MaintenancePlanLookup_Response != null &&
                        retVal.MaintenancePlanLookup_Response.Record?.Length > 0)
                    {
                        for (int i = 0; i < retVal.MaintenancePlanLookup_Response.Record.Length; i++)
                        {
                            sapMaintenancePlanLookup =
                                new SAPMaintenancePlanLookup(retVal.MaintenancePlanLookup_Response.Record[i]);
                            SapMaintenancePlanLookupCollection.Items.Add(sapMaintenancePlanLookup);
                        }
                    }
                    else
                    {
                        sapMaintenancePlanLookup = new SAPMaintenancePlanLookup
                            {SAPErrorCode = retVal.MaintenancePlanLookup_Response?.SAPStatus};
                        SapMaintenancePlanLookupCollection.Items.Add(sapMaintenancePlanLookup);
                    }

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                sapMaintenancePlanLookup.SAPErrorCode = RETRY_ERROR_TEXT + ex.Message;
                SapMaintenancePlanLookupCollection.Items.Add(sapMaintenancePlanLookup);
            }

            return SapMaintenancePlanLookupCollection;
        }

        /// <summary>
        /// Updates details about a Production Maintenance Plan
        /// http://localhost:15765/SAP/SAPMaintenancePlan/Search
        /// </summary>
        /// <param name="sapMaintenancePlanUpdate"></param>
        /// <returns></returns>
        public SAPMaintenancePlanUpdateCollection UpdateMaintenancePlan(
            SAPMaintenancePlanUpdate sapMaintenancePlanUpdate)
        {
            var SapMaintenancePlanUpdateCollection = new SAPMaintenancePlanUpdateCollection();
            try
            {
                var Request = sapMaintenancePlanUpdate.Request();

                CustomBinding binding = Bindings();

                var address = new EndpointAddress(BaseAddress + SAPInterface);

                using (var client = new MaintenancePlanUpdate_OB_SYNClient(binding, address))
                {
                    Credentials(client.ClientCredentials.UserName);

                    MaintenancePlanUpdate_OB_SYNResponse retVal =
                        ((MaintenancePlanUpdate_OB_SYN)(client)).MaintenancePlanUpdate_OB_SYN(
                            new MaintenancePlanUpdate_OB_SYNRequest {MaintenancePlanUpdate_Request = Request});
                    if (retVal.MaintenancePlanUpdate_Response != null &&
                        retVal.MaintenancePlanUpdate_Response.Length > 0)
                    {
                        for (int i = 0; i < retVal.MaintenancePlanUpdate_Response.Length; i++)
                        {
                            sapMaintenancePlanUpdate = new SAPMaintenancePlanUpdate {
                                MaintenancePlan = retVal.MaintenancePlanUpdate_Response[0].MaintenancePlan,
                                SAPErrorCode = retVal.MaintenancePlanUpdate_Response[0].SAPStatus
                            };
                            SapMaintenancePlanUpdateCollection.Items.Add(sapMaintenancePlanUpdate);
                        }
                    }
                    else
                    {
                        sapMaintenancePlanUpdate = new SAPMaintenancePlanUpdate
                            {SAPErrorCode = retVal.MaintenancePlanUpdate_Response[0].SAPStatus};
                        SapMaintenancePlanUpdateCollection.Items.Add(sapMaintenancePlanUpdate);
                    }

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                sapMaintenancePlanUpdate.SAPErrorCode = RETRY_ERROR_TEXT + ex.Message;
                SapMaintenancePlanUpdateCollection.Items.Add(sapMaintenancePlanUpdate);
            }

            return SapMaintenancePlanUpdateCollection;
        }

        /// <summary>
        /// Gets Production Work Orders from SAP, used in the scheduler
        /// SapScheduledProductionWorkOrderService/Job
        /// </summary>
        /// <param name="SapCreatePreventiveWorkOrder"></param>
        /// <returns></returns>
        public SAPCreatePreventiveWorkOrderCollection CreatePreventiveWorkOrder(
            SAPCreatePreventiveWorkOrder SapCreatePreventiveWorkOrder)
        {
            var SapCreatePreventiveWorkOrderCollection = new SAPCreatePreventiveWorkOrderCollection();
            try
            {
                var Request = SapCreatePreventiveWorkOrder.Request();

                CustomBinding binding = Bindings();

                var address = new EndpointAddress(BaseAddress + SAPInterface);

                using (var client = new PMOrders_Get_OB_SYNClient(binding, address))
                {
                    Credentials(client.ClientCredentials.UserName);

                    PMOrders_Get_OB_SYNResponse retVal =
                        ((PMOrders_Get_OB_SYN)(client)).PMOrders_Get_OB_SYN(new PMOrders_Get_OB_SYNRequest
                            {PMOrders_Request = Request});
                    if (retVal.PMOrders_Response != null && retVal.PMOrders_Response.Record?.Length > 0)
                    {
                        for (int i = 0; i < retVal.PMOrders_Response.Record.Length; i++)
                        {
                            SapCreatePreventiveWorkOrder =
                                new SAPCreatePreventiveWorkOrder(retVal.PMOrders_Response.Record[i]);
                            SapCreatePreventiveWorkOrderCollection.Items.Add(SapCreatePreventiveWorkOrder);
                        }
                    }
                    else
                    {
                        SapCreatePreventiveWorkOrder = new SAPCreatePreventiveWorkOrder
                            {SAPErrorCode = "SAP Response was null"};
                        SapCreatePreventiveWorkOrderCollection.Items.Add(SapCreatePreventiveWorkOrder);
                    }

                    client.Close();
                }
            }
            catch (Exception e)
            {
                SapCreatePreventiveWorkOrder.SAPErrorCode = RETRY_ERROR_TEXT + e.Message;
            }

            return SapCreatePreventiveWorkOrderCollection;
        }

        #region T&D Shared Services

        /// <summary>
        /// used for short cycle
        /// </summary>
        /// <param name="sapNewServiceInstallation"></param>
        /// <returns></returns>
        public SAPNewServiceInstallation SaveNewServiceInstallation(SAPNewServiceInstallation sapNewServiceInstallation)
        {
            var invoker = new SAPNewServiceInstallationInvoker(this);
            return invoker.Invoke(sapNewServiceInstallation);
        }

        /// <summary>
        /// does not appear to be used for short cycle, t&d
        /// </summary>
        /// <param name="sapNewServiceInstallation"></param>
        /// <returns></returns>
        public SAPNewServiceInstallation SaveService(SAPNewServiceInstallation sapNewServiceInstallation)
        {
            try
            {
                var new_ServiceInstallation_Request = sapNewServiceInstallation.ServiceRequest();

                CustomBinding binding = Bindings();

                var address = new EndpointAddress(BaseAddress + SAPInterface);

                using (var client = new W1v_New_ServiceInstallation_Get_OB_SYNClient(binding, address))
                {
                    Credentials(client.ClientCredentials.UserName);

                    W1v_New_ServiceInstallation_Get_OB_SYNResponse retVal =
                        ((W1v_New_ServiceInstallation_Get_OB_SYN)(client)).W1v_New_ServiceInstallation_Get_OB_SYNAsync(
                            new W1v_New_ServiceInstallation_Get_OB_SYNRequest
                                { W1v_New_ServiceInstallation_Request = new_ServiceInstallation_Request}).Result;
                    if (retVal.W1v_New_ServiceInstallation_Response != null &&
                        retVal.W1v_New_ServiceInstallation_Response.Length > 0)
                    {
                        sapNewServiceInstallation.SAPStatus = retVal.W1v_New_ServiceInstallation_Response[0].SAPStatus;
                        sapNewServiceInstallation.WorkOrderNumber =
                            retVal.W1v_New_ServiceInstallation_Response[0].WorkOrderNumber;
                    }
                    else
                        sapNewServiceInstallation.SAPStatus = "No record was returned from the SAP Web Service";

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                sapNewServiceInstallation.SAPStatus = RETRY_ERROR_TEXT + ex;
            }

            return sapNewServiceInstallation;
        }

        #endregion

        #region ShortCycle

        /// <summary>
        /// This method pull the current details for a short cycle work order from SAP.
        /// It's used in the API project to refresh the current order in MapCall
        /// WO_Predispatch_Pull can be tested with post action in API project:
        /// http://localhost:55667/UnscheduledOrder/Update/WorkOrder=519710480
        /// </summary>
        /// <param name="search">Will include a work order number</param>
        /// <returns></returns>
        public WO_Predispatch_PULL_StatusRecord[] SearchShortCycleWorkOrders(WO_Predispatch_PULL_QueryRecord[] search)
        {
            WO_Predispatch_PULL_StatusRecord[] results = null;

            try
            {
                CustomBinding binding = Bindings();
                var address = new EndpointAddress(BaseAddress + SAPInterface);
                using (var client = new WO_Predispatch_PULL_OB_SYCClient(binding, address))
                {
                    Credentials(client.ClientCredentials.UserName);
                    results = client.WO_Predispatch_PULL_OB_SYC(search);
                    client.Close();
                }
            }
            catch (FaultException ex) { }

            return results;
        }

        /// <summary>ShortCycleWorkOrderStatusUpdate</summary>
        public SAPWorkOrderStatusUpdateRequest WorkOrderStatusUpdate(
            SAPWorkOrderStatusUpdateRequest sapWorkOrderStatusUpdateRequest)
        {
            var invoker = new CreateShortCycleStatusUpdateInvoker(this);
            return invoker.Invoke(sapWorkOrderStatusUpdateRequest);
        }

        #endregion

        #endregion

        #region Private Method

        private TechnicalMaster_AccountDetailsQuery TechnicalDataRequest(
            SearchSapTechnicalMaster sapTechnicalMasterSearch)
        {
            TechnicalMaster_AccountDetailsQuery technicalMasterDataRequest = new TechnicalMaster_AccountDetailsQuery();

            technicalMasterDataRequest = new TechnicalMaster_AccountDetailsQuery();
            technicalMasterDataRequest.Equipment = sapTechnicalMasterSearch.Equipment;
            technicalMasterDataRequest.PremiseNumber = sapTechnicalMasterSearch.PremiseNumber;
            technicalMasterDataRequest.InstallationType = sapTechnicalMasterSearch.InstallationType;

            return technicalMasterDataRequest;
        }

        private bool CheckSite()
        {
            var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                s.Connect(Dns.GetHostAddresses(BaseAddress.DnsSafeHost), BaseAddress.Port);
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
            finally
            {
                s.Dispose();
            }
        }

        private void Credentials(UserNamePasswordClientCredential credential)
        {
            credential.UserName = UserName;
            credential.Password = Password;
        }

        protected CustomBinding Bindings()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            CustomBinding binding = new CustomBinding();
            binding.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8));
            binding.SendTimeout = SendTimeOut.Value;

            HttpsTransportBindingElement transportHttps = new HttpsTransportBindingElement {
                AuthenticationScheme = AuthenticationSchemes.Basic,
                Realm = "XISOAPApps",
                MaxReceivedMessageSize = 16553600,
                MaxBufferPoolSize = 524288,
            };

            binding.Elements.Add(transportHttps);

            return binding;
        }

        #endregion
    }
}