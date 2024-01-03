using MapCall.Common.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.SAP.service;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPProgressWorkOrder : SAPEntity, ISAPServiceEntity
    {
        #region Properties

        #region Logical properties

        public string PlanningPlan(MaterialUsed materialUsed)
        {
            switch (AssetType)
            {
                case "HYDRANT":
                case "VALVE":
                case "MAIN":
                case "SERVICE":
                    return materialUsed.StockLocation?.OperatingCenter?.DistributionPlanningPlant?.Code;
                case "SEWER OPENING":
                case "SEWER MAIN":
                case "SEWER LATERAL":
                    return materialUsed.StockLocation?.OperatingCenter?.SewerPlanningPlant?.Code;
                default:
                    return string.Empty;
            }
        }

        #endregion

        public string SAPErrorCode { get; set; }
        
        #region WebService Request Properties

        public virtual string OperatingCenter { get; set; }
        public virtual string SAPNotificationNo { get; set; }
        public virtual string SAPWorkOrderNo { get; set; }
        public virtual string HouseNo { get; set; }
        public virtual string Street { get; set; }
        public virtual string CrossStreet { get; set; }
        public virtual string Town { get; set; }
        public virtual string TownSection { get; set; }
        public virtual string Zipcode { get; set; }
        public virtual string State { get; set; }
        public virtual string Country { get; set; }
        public virtual string Latitude { get; set; }
        public virtual string Longitude { get; set; }
        public virtual string AssetType { get; set; }
        public virtual string SAPFunctionalLoc { get; set; }
        public virtual string SAPEquipmentNo { get; set; }
        public virtual string RequestedBY { get; set; }
        public virtual string Customer { get; set; }
        public virtual string PurposeGroup { get; set; }
        public virtual string MapCallPurpose { get; set; }
        public virtual string Priority { get; set; }
        public virtual string Premise { get; set; }
        public virtual string Notes { get; set; }
        public virtual string AccountCharged { get; set; }
        public virtual string CancelOrder { get; set; }
        public virtual string CancellationReason { get; set; }
        public virtual string WorkDescription { get; set; }
        public virtual string PMActType { get; set; }
        public virtual string TimeToComplete { get; set; }
        public string MaterialPlanningCompletedOn { get; set; }
        public IEnumerable<SAPProductionWorkOrderMaterialUsed> sapMaterialsUsed { get; set; }

        public IEnumerable<SAPCrewAssignment> sapCrewAssignments { get; set; }

        //phase 2 - added as part of technical master integration interface
        public virtual string Installation { get; set; }

        public virtual string SetMeter { get; set; }

        //New field User Id included as per change request
        public virtual string UserId { get; set; }

        public virtual string BasicFinish { get; set; }
        
        public virtual string Location { get; set; }
        
        #endregion

        #region WebService Response Properties

        public virtual string MaterialDocument { get; set; }
        public virtual string NotificationNumber { get; set; }
        public virtual string OrderNumber { get; set; }
        public virtual string Status { get; set; }
        public virtual string WBSElement { get; set; }
        public virtual string CostCenter { get; set; }

        #endregion

        #endregion

        #region Constructors

        public SAPProgressWorkOrder() { }

        public SAPProgressWorkOrder(WorkOrder workOrder)
        {
            //OperatingCenter = workOrder.OperatingCenter?.OperatingCenterCode;
            SAPNotificationNo = workOrder.SAPNotificationNumber?.ToString();
            SAPWorkOrderNo = workOrder.SAPWorkOrderNumber?.ToString();
            //New field Employee ID included as per change request
            UserId = workOrder.UserId;
            HouseNo = workOrder.StreetNumber;
            Street = workOrder.Street?.FullStName;
            CrossStreet = workOrder.NearestCrossStreet?.FullStName;
            Town = workOrder.Town?.ToString();
            TownSection = workOrder.TownSection?.Description;
            Zipcode = workOrder.Town?.Zip;
            State = workOrder.Town?.State?.Abbreviation;
            Country = "US";
            Latitude = workOrder.Latitude?.ToString();
            Longitude = workOrder.Longitude?.ToString();
            AssetType = workOrder.AssetType?.Description.ToUpper();

            RequestedBY = workOrder.RequestedBy?.Description;
            Customer = workOrder.CustomerName?.ToString();
            MapCallPurpose = workOrder.Purpose?.SapCode;
            //new purpose code group added for NSI interface
            PurposeGroup = workOrder.Purpose?.Description == "Construction Project" ? "N-D-PUR2" : "N-D-PUR1";
            Priority = workOrder.Priority?.Description.ToString();
            Premise = workOrder.PremiseNumber;
            Notes = workOrder.Notes;
            CancelOrder = workOrder.CancelledAt != null ? "X" : "";
            CancellationReason = workOrder.WorkOrderCancellationReason?.Status;
            WorkDescription = workOrder.WorkDescription?.Description;

            PMActType = workOrder.PlantMaintenanceActivityType?.Code;
            if (workOrder.PlantMaintenanceActivityTypeOverride != null &&
                PlantMaintenanceActivityType.GetOverrideCodesRequiringWBSNumber()
                                            .Contains(workOrder.PlantMaintenanceActivityTypeOverride.Id))
            {
                WBSElement = workOrder.AccountCharged;
            }

            TimeToComplete = workOrder.WorkDescription?.TimeToComplete.ToString();
            MaterialPlanningCompletedOn = workOrder.MaterialPlanningCompletedOn?.Date.ToString(SAP_DATE_FORMAT);
            BasicFinish = workOrder.PlannedCompletionDate?.Date.ToString(SAP_DATE_FORMAT);
            Location = workOrder.MeterLocation?.SAPCode;
            
            if (PMActType == "DVA" || PMActType == "RBS" || PMActType == "RPS" || PMActType == "RPT" ||
                PMActType == "BRG")
                AccountCharged = workOrder.AccountCharged?.ToString();

            if (workOrder.MaterialsUsed != null && workOrder.MaterialsUsed.Any() &&
                workOrder.MaterialPlanningCompletedOn != null)
            {
                var MaterialsUsed = from m in workOrder.MaterialsUsed
                                    select new SAPProductionWorkOrderMaterialUsed {
                                        Quantity = m.Quantity.ToString(),
                                        StcokLocation = m.StockLocation?.SAPStockLocation,
                                        PlanningPlan =
                                            PlanningPlan(m), //m.StockLocation?.OperatingCenter?.OperatingCenterCode,
                                        PartNumber = m.Material?.PartNumber,
                                        Description = m.Material?.Description ?? m.NonStockDescription,
                                    };

                sapMaterialsUsed = MaterialsUsed.ToList();
            }

            //workOrder.ApprovedOn is supervior approval date, added to avoid setMeter = Y or N value
            if (!workOrder.DateRejected.HasValue && !workOrder.ApprovedOn.HasValue 
                                                 && !workOrder.CancelledAt.HasValue 
                                                 && workOrder.CrewAssignments != null && workOrder.CrewAssignments.Any())
            {
                // new field added as part of NSI interface
                // WARNING YOU MAY HAVE TO CHANGE THE CONTRACTORS CONTROLLER TOO
                if (workOrder.WorkDescription != null && MapCall.Common.Model.Entities.WorkDescription.NEW_SERVICE_INSTALLATION.Contains(workOrder.WorkDescription.Id))
                {
                    if (workOrder.ServiceInstallations != null && workOrder.ServiceInstallations.Any())
                    {
                        SetMeter = workOrder.DateCompleted != null ? "Y" : "";
                    }
                    else
                    {
                        SetMeter = workOrder.DateCompleted != null ? "N" : "";
                    }
                }

                //bug-3689 - data sorting based on DateStarted instead of AssignedOn
                //if start dates are available then sort data based on start date

                var crewAssignments =
                    (from c in workOrder.CrewAssignments
                     where c.DateStarted != null
                     orderby c.DateStarted descending
                     select new SAPCrewAssignment {
                         CrewAssign = c.AssignedOn.Date.ToString(SAP_DATE_FORMAT),
                         DateStart = c.DateStarted?.Date.ToString(SAP_DATE_FORMAT),
                         DateEnd = c.DateEnded?.Date.ToString(SAP_DATE_FORMAT),
                         DateCompleted = workOrder.DateCompleted?.Date.ToString(SAP_DATE_FORMAT),
                         TotalManHours = c.TotalManHours,
                     }).ToList();
                //if start dates are not avaliable then sort data based on assignment date.
                if (crewAssignments.Count == 0)
                {
                    crewAssignments =
                        (from c in workOrder.CrewAssignments
                         orderby c.AssignedOn descending
                         select new SAPCrewAssignment {
                             CrewAssign = c.AssignedOn.Date.ToString(SAP_DATE_FORMAT),
                         }).ToList();
                }

                sapCrewAssignments = crewAssignments;
            }
            else if (workOrder.CrewAssignments == null || !workOrder.CrewAssignments.Any())
            {
                if (workOrder.DateCompleted.HasValue)
                {
                    sapCrewAssignments = new[] {
                        new SAPCrewAssignment
                            {DateCompleted = workOrder.DateCompleted?.Date.ToString(SAP_DATE_FORMAT)}
                    };
                }
            }

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

        private void MapFromMainCrossing(WorkOrder workOrder)
        {
            AssetType = "MAIN";
            SAPFunctionalLoc = workOrder.MainSAPFunctionalLocation?.Description;
            SAPEquipmentNo = workOrder.MainSAPEquipmentId?.ToString();
        }

        private void MapFromSewerLateral(WorkOrder workOrder)
        {
            Premise = workOrder.PremiseNumber;
            OperatingCenter = workOrder.OperatingCenter?.SewerPlanningPlant?.Code;
            //phase 2 changes, technical data interface impact 
            SAPFunctionalLoc = workOrder.DeviceLocation?.ToString();
            SAPEquipmentNo = workOrder.SAPEquipmentNumber?.ToString();
            Installation = workOrder.Installation?.ToString();
        }

        private void MapFromService(WorkOrder workOrder)
        {
            Premise = workOrder.PremiseNumber;
            OperatingCenter = workOrder.DistributionPlanningPlant?.Code;
            //phase 2 changes, technical data interface impact 
            SAPFunctionalLoc = workOrder.DeviceLocation?.ToString();
            SAPEquipmentNo = workOrder.SAPEquipmentNumber?.ToString();
            Installation = workOrder.Installation?.ToString();

            if (WorkDescription.Contains("NO PREMISE"))
            {
                // SAP doesn't actually keep track of these particular services, so we have to trick them
                // into associating the order with a main so we don't get rejected
                AssetType = "MAIN";
                var oct = workOrder.OperatingCenter.OperatingCenterTowns
                                   .SingleOrDefault(x => x.Town.Id == workOrder.Town.Id);
                SAPFunctionalLoc = SAPFunctionalLoc ??
                                   oct?.MainSAPFunctionalLocation?.ToString() ??
                                   workOrder.TownSection?.MainSAPFunctionalLocation?.ToString();
                SAPEquipmentNo = SAPEquipmentNo ??
                                 oct?.MainSAPEquipmentId?.ToString() ??
                                 workOrder.TownSection?.MainSAPEquipmentId?.ToString();
            }
        }

        private void MapFromSewerMain(WorkOrder workOrder)
        {
            SAPFunctionalLoc = workOrder.SewerMainSAPFunctionalLocation?.Description;
            SAPEquipmentNo = workOrder.SewerMainSAPEquipmentId?.ToString();
            OperatingCenter = workOrder.OperatingCenter?.SewerPlanningPlant?.Code;
        }

        private void MapFromMain(WorkOrder workOrder)
        {
            SAPFunctionalLoc = workOrder.MainSAPFunctionalLocation?.Description;
            SAPEquipmentNo = workOrder.MainSAPEquipmentId?.ToString();
            OperatingCenter = workOrder.DistributionPlanningPlant?.Code;
        }

        private void MapFromSewerOpening(WorkOrder workOrder)
        {
            SAPFunctionalLoc = workOrder.SewerOpening?.FunctionalLocation?.Description;
            SAPEquipmentNo = workOrder.SewerOpening?.SAPEquipmentNumber;
            OperatingCenter = workOrder.OperatingCenter?.SewerPlanningPlant?.Code;
        }

        private void MapFromValve(WorkOrder workOrder)
        {
            SAPFunctionalLoc = workOrder.Valve?.FunctionalLocation?.Description;
            SAPEquipmentNo = workOrder.Valve?.SAPEquipmentNumber;
            OperatingCenter = workOrder.DistributionPlanningPlant?.Code;
        }

        private void MapFromHydrant(WorkOrder workOrder)
        {
            SAPFunctionalLoc = workOrder.Hydrant?.FunctionalLocation?.Description;
            SAPEquipmentNo = workOrder.Hydrant?.SAPEquipmentNumber;
            OperatingCenter = workOrder.DistributionPlanningPlant?.Code;
        }

        #endregion

        #region Exposed Methods

        public ProgressWorkOrder ProcessWorkOrderRequest()
        {
            ProgressWorkOrderWorkOrderMaterials[] Materials = new ProgressWorkOrderWorkOrderMaterials[1];
            ProgressWorkOrderWorkOrderCrewAssignment[] CrewAssignment = new ProgressWorkOrderWorkOrderCrewAssignment[1];

            ProgressWorkOrderWorkOrderChangeOrder[] changeOrder = new ProgressWorkOrderWorkOrderChangeOrder[1];

            changeOrder[0] = new ProgressWorkOrderWorkOrderChangeOrder();
            changeOrder[0].OperatingCenter = OperatingCenter;
            changeOrder[0].SAPNotificationNo = SAPNotificationNo;
            changeOrder[0].SAPWorkOrderNo = SAPWorkOrderNo;
            //New field User Id included as per change request
            changeOrder[0].UserId = UserId;
            changeOrder[0].HouseNo = HouseNo;
            changeOrder[0].Street = Street;
            changeOrder[0].CrossStreet = CrossStreet;
            changeOrder[0].Town = Town;
            changeOrder[0].TownSection = TownSection;
            changeOrder[0].Zipcode = Zipcode;
            changeOrder[0].State = State;
            changeOrder[0].Country = Country;
            changeOrder[0].Latitude = Latitude;
            changeOrder[0].Longitude = Longitude;
            changeOrder[0].AssetType = AssetType;
            changeOrder[0].SAPFunctionalLoc = SAPFunctionalLoc;
            changeOrder[0].SAPEquipmentNo = SAPEquipmentNo;
            changeOrder[0].RequestedBY = RequestedBY;
            changeOrder[0].Customer = Customer;
            changeOrder[0].PurposeGroup = PurposeGroup;
            changeOrder[0].PurposeCode = MapCallPurpose;
            changeOrder[0].Priority = SAPPriority(Priority);
            changeOrder[0].Premise = Premise;
            changeOrder[0].Notes = Notes;
            changeOrder[0].AccountCharged = AccountCharged;
            changeOrder[0].CancelOrder = CancelOrder;
            changeOrder[0].CancellationReason = CancellationReason;
            changeOrder[0].WorkDescription = WorkDescription;
            changeOrder[0].PMActType = PMActType;
            changeOrder[0].Installation = Installation;
            changeOrder[0].BasicFinish = BasicFinish;
            changeOrder[0].Location = Location;

            //new field added as part of NSI interface
            changeOrder[0].SetMeter = SetMeter;

            if (sapMaterialsUsed != null && sapMaterialsUsed.Any())
            {
                Materials = new ProgressWorkOrderWorkOrderMaterials[sapMaterialsUsed.ToList().Count];

                for (int i = 0; i < sapMaterialsUsed.ToList().Count; i++)
                {
                    Materials[i] = new ProgressWorkOrderWorkOrderMaterials();
                    Materials[i].PartNumber = sapMaterialsUsed.ToList()[i].PartNumber;
                    Materials[i].Description = sapMaterialsUsed.ToList()[i].Description;
                    Materials[i].Quantity = sapMaterialsUsed.ToList()[i].Quantity;
                    Materials[i].StcokLocation = sapMaterialsUsed.ToList()[i].StcokLocation;
                    Materials[i].OperatingCenterCode = sapMaterialsUsed.ToList()[i].PlanningPlan;
                    Materials[i].ItemCategory = sapMaterialsUsed.ToList()[i].ItemCategory;
                }
            }

            if (sapCrewAssignments != null && sapCrewAssignments.Any())
            {
                CrewAssignment = new ProgressWorkOrderWorkOrderCrewAssignment[1];

                CrewAssignment[0] = new ProgressWorkOrderWorkOrderCrewAssignment();

                CrewAssignment[0].CrewAssign = sapCrewAssignments.ToList()[0].CrewAssign;
                CrewAssignment[0].DateStart = sapCrewAssignments.ToList()[0].DateStart;
                CrewAssignment[0].DateEnd = sapCrewAssignments.ToList()[0].DateEnd;
                CrewAssignment[0].DateCompleted = sapCrewAssignments.ToList()[0].DateCompleted;
            }

            ProgressWorkOrder ProgressWorkOrderRequest = new ProgressWorkOrder {
                WorkOrder = new ProgressWorkOrderWorkOrder {
                    ChangeOrder = changeOrder,
                    Materials = Materials.ToArray(),
                    CrewAssignment = CrewAssignment
                }
            };

            return ProgressWorkOrderRequest;
        }

        public void MapToWorkOrder(ISapWorkOrder workOrder)
        {
            if (!string.IsNullOrEmpty(OrderNumber))
            {
                workOrder.SAPWorkOrderNumber = long.Parse(OrderNumber);
            }
            if (!string.IsNullOrEmpty(WBSElement))
            {
                workOrder.AccountCharged = WBSElement;
            }
            if ((!workOrder.SAPNotificationNumber.HasValue || workOrder.SAPNotificationNumber == 0) &&
                !string.IsNullOrEmpty(NotificationNumber))
            {
                workOrder.SAPNotificationNumber = long.Parse(NotificationNumber);
            }
            if (!string.IsNullOrEmpty(MaterialDocument))
            {
                workOrder.MaterialsDocID = MaterialDocument;
            }
            if (!string.IsNullOrEmpty(CostCenter))
            {
                workOrder.BusinessUnit = CostCenter;
            }
            workOrder.SAPErrorCode = Status;
        }

        #endregion
    }

    [Serializable]
    public class SAPProductionWorkOrderMaterialUsed
    {
        #region properties

        #region Logic properties

        public virtual string ItemCategory => PartNumber != null ? "L" : "T";

        #endregion

        public virtual string PartNumber { get; set; }
        public virtual string Description { get; set; }
        public virtual string Quantity { get; set; }
        public virtual string StcokLocation { get; set; }
        public virtual string PlanningPlan { get; set; }

        #endregion
    }

    [Serializable]
    public class SAPCrewAssignment
    {
        #region properties

        public virtual string CrewAssign { get; set; }
        public virtual string DateStart { get; set; }
        public virtual string DateEnd { get; set; }
        public virtual string DateCompleted { get; set; }
        public virtual string StartTime { get; set; }
        public virtual string EndTime { get; set; }
        public virtual double? TotalManHours { get; set; }

        #endregion
    }
}
