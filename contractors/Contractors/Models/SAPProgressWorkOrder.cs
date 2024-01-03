using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;

namespace Contractors.Models
{
    //Contractor version of this class that accepts a contractor work order
    public class SapProgressWorkOrder : SAPProgressWorkOrder
    {
        #region Constructors

        /// <summary>
        /// TODO: Shouldn't this just be a view model? Is the conversion of strings throwing that much off?
        /// Or is that all the child properties are in the common vs contractor namespace?
        /// 
        /// This is a copy of the SAPProgressWorkOrder constructor that takes a common work order object
        /// </summary>
        /// <param name="entity"></param>
        public SapProgressWorkOrder(WorkOrder workOrder)
        {
            SAPNotificationNo = workOrder.SAPNotificationNumber?.ToString();
            SAPWorkOrderNo = workOrder.SAPWorkOrderNumber?.ToString();
            HouseNo = workOrder.StreetNumber;
            Street = workOrder.Street?.FullStName;
            CrossStreet = workOrder.NearestCrossStreet?.FullStName;
            Town = workOrder.Town?.ToString();
            TownSection = workOrder.TownSection?.Description;
            Zipcode = workOrder.Town?.Zip;
            State = workOrder.Town?.County?.State?.Abbreviation;
            Country = "US";
            Latitude = workOrder.Latitude.ToString();
            Longitude = workOrder.Longitude.ToString();
            AssetType = workOrder.AssetType?.Description.ToUpper();
            if (AssetType == "HYDRANT")
            {
                SAPFunctionalLoc = workOrder.Hydrant?.FunctionalLocation?.Description;
                SAPEquipmentNo = workOrder.Hydrant?.SAPEquipmentNumber;
                OperatingCenter = workOrder.OperatingCenter?.DistributionPlanningPlant?.Code;
            }
            else if (AssetType == "VALVE")
            {
                SAPFunctionalLoc = workOrder.Valve?.FunctionalLocation?.Description;
                SAPEquipmentNo = workOrder.Valve?.SAPEquipmentNumber;
                OperatingCenter = workOrder.OperatingCenter?.DistributionPlanningPlant?.Code;
            }
            else if (AssetType == "SEWER OPENING")
            {
                SAPFunctionalLoc = workOrder.SewerOpening?.FunctionalLocation?.Description;
                SAPEquipmentNo = workOrder.SewerOpening?.SAPEquipmentNumber;
                OperatingCenter = workOrder.OperatingCenter?.SewerPlanningPlant?.Code;
            }
            else if (AssetType == "MAIN")
            {
                SAPFunctionalLoc = workOrder.MainSAPFunctionalLocation?.Description;
                SAPEquipmentNo = workOrder.MainSAPEquipmentId.ToString();
                OperatingCenter = workOrder.OperatingCenter?.DistributionPlanningPlant?.Code;
            }
            else if (AssetType == "SEWER MAIN")
            {
                SAPFunctionalLoc = workOrder.SewerMainSAPFunctionalLocation?.Description;
                SAPEquipmentNo = workOrder.SewerMainSAPEquipmentId.ToString();
                OperatingCenter = workOrder.OperatingCenter?.SewerPlanningPlant?.Code;
            }
            else if (AssetType == "SERVICE")
            {
                Premise = workOrder.PremiseNumber;
                OperatingCenter = workOrder.OperatingCenter?.DistributionPlanningPlant?.Code;
                //phase 2 changes, technical data interface impact 
                SAPFunctionalLoc = workOrder.DeviceLocation?.ToString();
                SAPEquipmentNo = workOrder.SAPEquipmentNumber?.ToString();
                Installation = workOrder.Installation?.ToString();
            }
            else if (AssetType == "SEWER LATERAL")
            {
                Premise = workOrder.PremiseNumber;
                OperatingCenter = workOrder.OperatingCenter?.SewerPlanningPlant?.Code;
                //phase 2 changes, technical data interface impact 
                SAPFunctionalLoc = workOrder.DeviceLocation?.ToString();
                SAPEquipmentNo = workOrder.SAPEquipmentNumber?.ToString();
                Installation = workOrder.Installation?.ToString();
            }
            else if (AssetType == "MAIN CROSSING")
            {
                AssetType = "MAIN";
                SAPFunctionalLoc = workOrder.MainSAPFunctionalLocation?.Description;
                SAPEquipmentNo = workOrder.MainSAPEquipmentId?.ToString();
            }
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
            BasicFinish = workOrder.PlannedCompletionDate?.Date.ToString(SAP_DATE_FORMAT);
            Location = workOrder.MeterLocation?.SAPCode;

            if (workOrder.PlantMaintenanceActivityTypeOverride == null)
            {
                PMActType = workOrder.WorkDescription?.PlantMaintenanceActivityType?.Code; //PMActType
            }
            else
            {
                PMActType = workOrder.PlantMaintenanceActivityTypeOverride.Code;
                WBSElement = workOrder.AccountCharged;
            }

            TimeToComplete = workOrder.WorkDescription?.TimeToComplete.ToString();
            MaterialPlanningCompletedOn = workOrder.MaterialPlanningCompletedOn?.Date.ToString(SAP_DATE_FORMAT);

            if (PMActType == "DVA" || PMActType == "RBS" || PMActType == "RPS" || PMActType == "RPT" || PMActType == "BRG")
                AccountCharged = workOrder.AccountCharged?.ToString();

            if (workOrder.MaterialsUsed != null && workOrder.MaterialsUsed.Any() && workOrder.MaterialPlanningCompletedOn != null)
            {
                var MaterialsUsed = from m in workOrder.MaterialsUsed
                                    select new SAPProductionWorkOrderMaterialUsed {
                                        Quantity = m.Quantity.ToString(),
                                        StcokLocation = m.StockLocation?.SAPStockLocation,
                                        PlanningPlan = PlanningPlan(m),
                                        PartNumber = m.Material?.PartNumber,
                                        Description = m.Material?.Description ?? m.NonStockDescription
                                    };

                sapMaterialsUsed = MaterialsUsed.ToList();
            }
            //workOrder.ApprovedOn is supervior approval date, added to avoid setMeter = Y or N value
            if (!workOrder.DateRejected.HasValue && !workOrder.ApprovedOn.HasValue 
                                                 && !workOrder.CancelledAt.HasValue 
                                                 && workOrder.CrewAssignments != null && workOrder.CrewAssignments.Any())
            {
                // new field added as part of NSI interface
                // WARNING: YOU MAY HAVE TO CHANGE THE MVC CONTROLLER TOO
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

                //bug-3689 - data sorting based on datestarted instead of assignedon
                //if start dates are avaliable then sort data based on start date

                var CrewAssignments = from c in workOrder.CrewAssignments
                                      where (c.DateStarted != null)
                                      orderby c.DateStarted descending
                                      select new SAPCrewAssignment()

                                      {
                                          CrewAssign = c.AssignedOn.Date.ToString(SAP_DATE_FORMAT),
                                          DateStart = c.DateStarted?.Date.ToString(SAP_DATE_FORMAT),
                                          DateEnd = c.DateEnded?.Date.ToString(SAP_DATE_FORMAT),
                                          DateCompleted = workOrder.DateCompleted?.Date.ToString(SAP_DATE_FORMAT),
                                          TotalManHours = c.TotalManHours,

                                      };
                //if start dates are not avaliable then sort data based on assignment date.
                if (CrewAssignments.ToList().Count == 0)
                {
                    CrewAssignments = from c in workOrder.CrewAssignments
                                      orderby c.AssignedOn descending
                                      select new SAPCrewAssignment()
                                      {
                                          CrewAssign = c.AssignedOn.Date.ToString(SAP_DATE_FORMAT),
                                      };

                }

                sapCrewAssignments = CrewAssignments.ToList();
            }
        }

        private string PlanningPlan(MaterialUsed materialUsed)
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
    }
}