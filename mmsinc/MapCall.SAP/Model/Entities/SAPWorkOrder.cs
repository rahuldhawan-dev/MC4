using MapCall.Common.Model.Entities;
using System;
using System.Linq;
using MapCall.SAP.service;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPWorkOrder : SAPEntity, ISAPServiceEntity
    {
        #region Properties

        #region WebService Request Properties

        public virtual string SAPNotificationNo { get; set; }
        public virtual string DocumentTitle { get; set; } //Order Number
        public string URL { get; private set; }
        public string AssetType { get; set; }
        public string PurposeCodeGroup { get; set; }
        public string Purpose { get; set; }
        public virtual string OrderType { get; set; }
        public virtual string Priority { get; set; }
        public virtual string FunctionalLocation { get; set; }
        public virtual string EquipmentNo { get; set; }
        public virtual string ShortText { get; set; } //Work Order Description
        public virtual string LongText { get; set; } //Notes
        public virtual string House { get; set; } //Street Number
        public virtual string Street1 { get; set; } //Street
        public virtual string Street3 { get; set; } //Nearest Cross Street
        public virtual string City { get; set; } //Town
        public virtual string PostalCode { get; set; } //Town.Zipcode
        public virtual string State { get; set; } //NJ
        public virtual string Country { get; set; } //US
        public virtual string SearchTerm1 { get; set; } //Latitude
        public virtual string SearchTerm2 { get; set; } //Longitude
        public virtual string MaintActivityType { get; set; } //PMActType
        public virtual string DateReceived { get; set; } //Date Received
        public virtual string BasicStart { get; set; } //Date Received
        public virtual string BasicFinish { get; set; } //PlannedCompletionDate
        public virtual string FunctionalLoc { get; set; } //SAP Functional Location
        public virtual string Equipment { get; set; } //SAP Equipment
        public virtual string Operation { get; set; } //Work Order Description
        public virtual string OperationDuration { get; set; } //Time To Complete
        public virtual string PlanningPlant { get; set; } //Plant
        public virtual string MainWorkCenter { get; set; } //WorkCenter
        public virtual string PremiseNumber { get; set; }
        public virtual string RequestedBy { get; set; }

        public virtual string AccountCharged { get; set; }

        //phase 2 - added as part of technical master integration interface
        public virtual string Installation { get; set; }

        //New field User Id included as per change request
        public virtual string UserID { get; set; }

        #endregion

        #region WebService Response Properties

        //SAP order number
        public virtual string OrderNumber { get; set; } //SAP Order Number
        public virtual string NotificationNumber { get; set; } //SAP Notification Number
        public virtual string WBSElement { get; set; } //Account
        public virtual string SAPErrorCode { get; set; }
        public virtual string CostCenter { get; set; }

        #endregion

        #endregion

        #region Constructors

        public SAPWorkOrder() { }

        public SAPWorkOrder(WorkOrder workOrder)
        {
            SAPNotificationNo = workOrder.SAPNotificationNumber?.ToString();
            DocumentTitle = workOrder.Id.ToString() + " " + workOrder.WorkDescription?.Description?.ToString();
            URL = GetShowUrl("WorkOrder", workOrder.Id);
            AssetType = workOrder.AssetType?.Description.ToUpper();
            Purpose = workOrder.Purpose?.SapCode;
            //new purpose code group added for NSI interface
            PurposeCodeGroup = workOrder.Purpose?.Description == "Construction Project" ? "N-D-PUR2" : "N-D-PUR1";
            Priority = workOrder.Priority?.Description.ToString();
            AccountCharged = workOrder.AccountCharged;
            //New field User Id included as per change request
            UserID = workOrder.UserId;
            ShortText = workOrder.WorkDescription?.Description?.ToString();
            LongText = workOrder.Notes?.ToString();
            House = workOrder.StreetNumber;
            Street1 = workOrder.Street?.FullStName;
            Street3 = workOrder.NearestCrossStreet?.FullStName?.ToString();
            City = workOrder.Town?.ShortName;
            PostalCode = workOrder.Town?.Zip;
            State = workOrder?.Town?.State?.Abbreviation;
            Country = "US";
            SearchTerm1 = workOrder.Latitude?.ToString();
            SearchTerm2 = workOrder.Longitude?.ToString();
            RequestedBy = workOrder.RequestedBy?.Description;

            MaintActivityType = workOrder.PlantMaintenanceActivityType?.Code;
            if (workOrder.PlantMaintenanceActivityTypeOverride != null)
            {
                WBSElement = workOrder.AccountCharged;
            }

            DateReceived = (workOrder.DateReceived.HasValue && workOrder.DateReceived?.Year < 2010)
                ? "20100101"
                : workOrder.DateReceived?.Date.ToString(SAP_DATE_FORMAT);
            Operation = workOrder.WorkDescription?.Description?.ToString();
            OperationDuration = workOrder.WorkDescription?.TimeToComplete.ToString();
            BasicFinish = workOrder.PlannedCompletionDate?.Date.ToString(SAP_DATE_FORMAT);

            switch (AssetType)
            {
                case "HYDRANT":
                    MapFromHydrant(workOrder);
                    break;
                case "VALVE":
                    MapFromValve(workOrder);
                    break;
                case "SEWER OPENING":
                    MapFromSewerOpening(workOrder);
                    break;
                case "MAIN":
                    MapFromMain(workOrder);
                    break;
                case "SEWER MAIN":
                    MapFromSewerMain(workOrder);
                    break;
                case "SERVICE":
                    MapFromService(workOrder);
                    break;
                case "SEWER LATERAL":
                    MapFromSewerLateral(workOrder);
                    break;
                case "MAIN CROSSING":
                    MapFromMainCrossing(workOrder);
                    break;
            }
        }

        #endregion
        
        #region Private Methods

        private void MapFromHydrant(WorkOrder workOrder)
        {
            FunctionalLocation = workOrder.Hydrant?.FunctionalLocation?.Description;
            EquipmentNo = workOrder.Hydrant?.SAPEquipmentNumber;
            FunctionalLoc = workOrder.Hydrant?.FunctionalLocation?.Description;
            Equipment = workOrder.Hydrant?.SAPEquipmentNumber;
            MainWorkCenter = "TD";
            PlanningPlant = workOrder.DistributionPlanningPlant?.Code;
        }

        private void MapFromValve(WorkOrder workOrder)
        {
            FunctionalLocation = workOrder.Valve?.FunctionalLocation?.Description;
            EquipmentNo = workOrder.Valve?.SAPEquipmentNumber;
            FunctionalLoc = workOrder.Valve?.FunctionalLocation?.Description;
            Equipment = workOrder.Valve?.SAPEquipmentNumber;
            MainWorkCenter = "TD";
            PlanningPlant = workOrder.DistributionPlanningPlant?.Code;
        }

        private void MapFromSewerOpening(WorkOrder workOrder)
        {
            FunctionalLocation = workOrder.SewerOpening?.FunctionalLocation?.Description;
            EquipmentNo = workOrder.SewerOpening?.SAPEquipmentNumber;
            FunctionalLoc = workOrder.SewerOpening?.FunctionalLocation?.Description;
            Equipment = workOrder.SewerOpening?.SAPEquipmentNumber;
            MainWorkCenter = "SEW_C";
            PlanningPlant = workOrder.SewerPlanningPlant?.Code;
        }

        private void MapFromMain(WorkOrder workOrder)
        {
            FunctionalLocation = workOrder.MainSAPFunctionalLocation?.Description;
            EquipmentNo = workOrder.MainSAPEquipmentId?.ToString();
            FunctionalLoc = workOrder.MainSAPFunctionalLocation?.Description;
            Equipment = workOrder.MainSAPEquipmentId?.ToString();
            MainWorkCenter = "TD";
            PlanningPlant = workOrder.DistributionPlanningPlant?.Code;
        }

        private void MapFromSewerMain(WorkOrder workOrder)
        {
            FunctionalLocation = workOrder.SewerMainSAPFunctionalLocation?.Description;
            EquipmentNo = workOrder.SewerMainSAPEquipmentId?.ToString();
            FunctionalLoc = workOrder.SewerMainSAPFunctionalLocation?.Description;
            Equipment = workOrder.SewerMainSAPEquipmentId?.ToString();
            MainWorkCenter = "SEW_C";
            PlanningPlant = workOrder.SewerPlanningPlant?.Code;
        }

        private void MapFromService(WorkOrder workOrder)
        {
            PremiseNumber = workOrder.PremiseNumber;
            MainWorkCenter = "TD";
            PlanningPlant = workOrder.DistributionPlanningPlant?.Code;
            //phase 2 changes, technical data interface impact 
            FunctionalLocation = workOrder.DeviceLocation?.ToString();
            Equipment = workOrder.SAPEquipmentNumber?.ToString();
            Installation = workOrder.Installation?.ToString();

            if (ShortText.Contains("NO PREMISE"))
            {
                // SAP doesn't actually keep track of these particular services, so we have to trick them
                // into associating the order with a main so we don't get rejected
                AssetType = "MAIN";
                var oct = workOrder.OperatingCenter.OperatingCenterTowns
                                   .SingleOrDefault(x => x.Town.Id == workOrder.Town.Id);
                FunctionalLocation = FunctionalLocation ??
                                     oct?.MainSAPFunctionalLocation?.ToString() ??
                                     workOrder.TownSection?.MainSAPFunctionalLocation?.ToString();
                Equipment = Equipment ??
                            oct?.MainSAPEquipmentId?.ToString() ??
                            workOrder.TownSection?.MainSAPEquipmentId?.ToString();
            }

            FunctionalLoc = FunctionalLocation;
            EquipmentNo = Equipment;
        }

        private void MapFromSewerLateral(WorkOrder workOrder)
        {
            PremiseNumber = workOrder.PremiseNumber;
            MainWorkCenter = "SEW_C";
            PlanningPlant = workOrder.SewerPlanningPlant?.Code;
            //phase 2 changes, technical data interface impact 
            FunctionalLoc = workOrder.DeviceLocation?.ToString();
            Equipment = workOrder.SAPEquipmentNumber?.ToString();
            Installation = workOrder.Installation?.ToString();

            FunctionalLocation = workOrder.DeviceLocation?.ToString();
            EquipmentNo = workOrder.SAPEquipmentNumber?.ToString();
        }

        private void MapFromMainCrossing(WorkOrder workOrder)
        {
            AssetType = "MAIN";
            FunctionalLocation = workOrder.MainSAPFunctionalLocation?.Description;
            EquipmentNo = workOrder.MainSAPEquipmentId?.ToString();
        }
        
        #endregion

        #region Exposed Methods

        public CreateWorkOrderWorkOrder[] WorkOrderRequest()
        {
            CreateWorkOrderWorkOrder[] Request = new CreateWorkOrderWorkOrder[1];

            Request[0] = new CreateWorkOrderWorkOrder();
            Request[0].SAPNotificationNo = SAPNotificationNo;
            Request[0].DocumentTitle = DocumentTitle;
            Request[0].URL = URL;
            Request[0].OrderType = OrderType;
            Request[0].AssetType = AssetType;
            Request[0].Priority = SAPPriority(Priority);
            Request[0].FunctionalLocation = FunctionalLocation;
            Request[0].EquipmentNo = EquipmentNo;
            Request[0].ShortText = ShortText;
            Request[0].LongText = LongText;
            Request[0].PurposeCode = Purpose;
            Request[0].PurposeGroup = PurposeCodeGroup;
            Request[0].Premise = PremiseNumber;
            Request[0].RequestedBy = RequestedBy;
            Request[0].AccountCharged = AccountCharged;
            Request[0].Installation = Installation;
            //New field User Id included as per change request
            Request[0].UserID = UserID;

            Request[0].HeaderData = new CreateWorkOrderWorkOrderHeaderData();
            Request[0].HeaderData.BasicFinish = BasicFinish;
            Request[0].HeaderData.FunctionalLoc = FunctionalLoc;
            Request[0].HeaderData.Equipment = Equipment;
            Request[0].HeaderData.MaintActivityType = MaintActivityType;
            Request[0].HeaderData.BasicStart = DateReceived;
            Request[0].HeaderData.Operation = Operation;
            Request[0].HeaderData.OperationDuration = OperationDuration;
            Request[0].HeaderData.MaintWorkCenter = MainWorkCenter;
            Request[0].HeaderData.Plant = PlanningPlant;

            Request[0].OrderAddress = new CreateWorkOrderWorkOrderOrderAddress();
            Request[0].OrderAddress.House = House;
            Request[0].OrderAddress.Street1 = Street1;
            Request[0].OrderAddress.Street3 = Street3;
            Request[0].OrderAddress.City = City;
            Request[0].OrderAddress.PostalCode = PostalCode;
            Request[0].OrderAddress.State = State;
            Request[0].OrderAddress.Country = Country;
            Request[0].OrderAddress.SearchTerm1 = SearchTerm1;
            Request[0].OrderAddress.SearchTerm2 = SearchTerm2;
            Request[0].UserID = UserID;

            return Request;
        }

        public void MapToWorkOrder(ISapWorkOrder workOrder)
        {
            if (!string.IsNullOrEmpty(OrderNumber))
                workOrder.SAPWorkOrderNumber = long.Parse(OrderNumber);
            if (!string.IsNullOrEmpty(WBSElement))
                workOrder.AccountCharged = WBSElement;
            if ((!workOrder.SAPNotificationNumber.HasValue || workOrder.SAPNotificationNumber == 0) &&
                !string.IsNullOrEmpty(NotificationNumber))
                workOrder.SAPNotificationNumber = long.Parse(NotificationNumber);
            workOrder.SAPErrorCode = SAPErrorCode;
        }

        #endregion
    }
}
