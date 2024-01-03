using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.SAP.ProgressScheduledUnscheduledWOWS;

namespace MapCall.SAP.Model.Entities
{
    /// <summary>
    /// This class takes a MapCall ProductionWorkOrder and turns it into an SAPProgressUnscheduledWorkOrder that
    /// SAP can use. The constructor takes everything from MapCall's object and stores it in properties that are then 
    /// translated out into what SAP needs that it gets from UnscheduledProgressWorkOrderRequest()
    /// </summary>
    public class SAPProgressUnscheduledWorkOrder : SAPEntity
    {
        #region Properties

        #region WebService Request Properties

        public virtual string OperatingCenter { get; set; }
        public virtual string SAPNotificationNo { get; set; }
        public virtual string SAPWorkOrderNo { get; set; }
        public virtual string SAPFunctionalLoc { get; set; }
        public virtual string SAPEquipmentNo { get; set; }
        public virtual string RequestedBY { get; set; }
        public virtual string PurposeGroup { get; set; }
        public virtual string Purpose { get; set; }
        public virtual string Priority { get; set; }
        public virtual string Notes { get; set; }
        public virtual string AccountCharged { get; set; }
        public virtual string CancelOrder { get; set; }
        public virtual string CancellationReason { get; set; }
        public virtual string WorkDescription { get; set; }
        public virtual string PMActType { get; set; }
        public virtual string BreakDownIndicator { get; set; }
        public virtual string TimeToComplete { get; set; }
        public virtual string Permits { get; set; }
        public virtual IEnumerable<SAPEmployeeAssignments> SapEmployeeAssignments { get; set; }

        public virtual IEnumerable<SAPMaterialUsedForProductionWorkOrder> SapMaterialUsedForProductionWorkOrder
        {
            get;
            set;
        }

        public virtual IEnumerable<IList<Employee>> SapEmployees { get; set; }

        public virtual string Capitalized { get; set; }

        //added for preventive work order
        public virtual IList<SAPProductionWorkOrderChildNotification> SapProductionWorkOrderChildNotification
        {
            get;
            set;
        }

        public virtual IList<SAPProductionWorkOrderDependencies> SapProductionWorkOrderDependencies { get; set; }
        public virtual IList<SAPProductionWorkOrderActions> SapProductionWorkOrderActions { get; set; }
        public virtual IList<SAPProductionWorkOrderMeasuringPoints> SapProductionWorkOrderMeasuringPoints { get; set; }

        #endregion

        #region WebService Response Properties

        public virtual string NotificationNumber { get; set; }
        public virtual string OrderNumber { get; set; }
        public virtual string Status { get; set; }
        public virtual string WBSElement { get; set; }
        public virtual string SAPErrorCode { get; set; }

        #endregion

        public virtual bool IsSAPEnabled { get; set; }

        #endregion

        #region Constructors

        public SAPProgressUnscheduledWorkOrder() { }

        public SAPProgressUnscheduledWorkOrder(ProductionWorkOrder productionWorkOrder)
        {
            OperatingCenter = productionWorkOrder.PlanningPlant?.Code;
            SAPNotificationNo = productionWorkOrder.SAPNotificationNumber.ToString();
            SAPWorkOrderNo = productionWorkOrder.SAPWorkOrder;
            SAPFunctionalLoc = productionWorkOrder.FunctionalLocation;
            SAPEquipmentNo = productionWorkOrder.Equipment?.SAPEquipmentId?.ToString();
            RequestedBY = productionWorkOrder.RequestedBy?.FirstName + " " +
                          productionWorkOrder.RequestedBy?.MiddleName + " " + productionWorkOrder.RequestedBy?.LastName;
            Priority = productionWorkOrder.Priority?.Description;
            Notes = productionWorkOrder.CancellationReason != null
                ? productionWorkOrder.CancellationReason?.Description
                : productionWorkOrder.OrderNotes;
            WorkDescription = productionWorkOrder.ProductionWorkDescription?.Description;
            IsSAPEnabled = productionWorkOrder.OrderType.IsSAPEnabled;

            if (productionWorkOrder.ProductionWorkOrderChildNotification == null)
                BreakDownIndicator = productionWorkOrder.ProductionWorkDescription?.BreakdownIndicator == true
                    ? "Y"
                    : "N";
            CancelOrder = productionWorkOrder.DateCancelled != null ? "X" : null;
            CancellationReason = productionWorkOrder.CancellationReason?.SAPCode;

            if (productionWorkOrder.ProductionWorkDescription?.PlantMaintenanceActivityType != null)
            {
                WBSElement = productionWorkOrder.WBSElement;
            }

            PMActType = productionWorkOrder.ProductionWorkDescription?.PlantMaintenanceActivityType?.Code;

            if (productionWorkOrder.ProductionWorkOrderMaterialUsed != null &&
                productionWorkOrder.ProductionWorkOrderMaterialUsed.Any() &&
                productionWorkOrder.MaterialsPlannedOn != null && productionWorkOrder.DateCompleted == null)
            {
                var MaterialsUsed = from m in productionWorkOrder.ProductionWorkOrderMaterialUsed
                                    select new SAPMaterialUsedForProductionWorkOrder {
                                        PartNumber = m.Material?.PartNumber,
                                        Description = m.Material?.Description ?? m.NonStockDescription,
                                        Quantity = m.Quantity.ToString(),
                                        StcokLocation = m.StockLocation?.SAPStockLocation,
                                        PlanningPlan = m.ProductionWorkOrder?.PlanningPlant?.Code,
                                    };

                SapMaterialUsedForProductionWorkOrder = MaterialsUsed.ToList();
            }

            if (productionWorkOrder.EmployeeAssignments != null && productionWorkOrder.EmployeeAssignments.Any())
            {
                var EmployeeAssignments = from e in productionWorkOrder.EmployeeAssignments
                                          where (e.DateStarted != null)
                                          orderby e.DateStarted descending
                                          select new SAPEmployeeAssignments {
                                              CrewAssign = e.AssignedOn.Date.ToString(SAP_DATE_FORMAT),
                                              DateStart = e.DateStarted?.Date.ToString(SAP_DATE_FORMAT),
                                              DateEnd = e.DateEnded?.Date.ToString(SAP_DATE_FORMAT),
                                              DateCompleted =
                                                  productionWorkOrder.DateCompleted?.Date.ToString(SAP_DATE_FORMAT),
                                              TotalManHours = e.TotalManHours,
                                              EmployeeId = e.AssignedTo?.EmployeeId
                                          };

                //if start dates are not avaliable then sort data based on assignment date.
                if (EmployeeAssignments.ToList().Count == 0)
                {
                    EmployeeAssignments = from c in productionWorkOrder.EmployeeAssignments
                                          orderby c.AssignedOn descending
                                          select new SAPEmployeeAssignments {
                                              CrewAssign = c.AssignedOn.Date.ToString(SAP_DATE_FORMAT),
                                              EmployeeId = c.AssignedTo?.EmployeeId
                                          };
                }

                SapEmployeeAssignments = EmployeeAssignments.ToList();
            }

            Capitalized = string.IsNullOrEmpty(productionWorkOrder.CapitalizationReason) == false ? "Y" : "";

            if (productionWorkOrder.ProductionWorkOrderDependencies != null &&
                productionWorkOrder.ProductionWorkOrderDependencies.Any())
            {
                var Dependencies =
                    from d in productionWorkOrder.ProductionWorkOrderDependencies
                    select new SAPProductionWorkOrderDependencies {
                        Code = d.Code,
                        CodeGroup = d.CodeGroup
                    };
                SapProductionWorkOrderDependencies = Dependencies.ToList();
            }

            if (productionWorkOrder.ProductionWorkOrderActions != null &&
                productionWorkOrder.ProductionWorkOrderActions.Any())
            {
                var Actions =
                    from a in productionWorkOrder.ProductionWorkOrderActions
                    select new SAPProductionWorkOrderActions {
                        Code = a.Code,
                        CodeGroup = a.CodeGroup
                    };
                SapProductionWorkOrderActions = Actions.ToList();
            }

            SapProductionWorkOrderChildNotification = MapChildNotifications(productionWorkOrder);

            // are we completing notifications / measurement points?
            if (productionWorkOrder.CompleteMeasurementPoints)
            {
                foreach (var pwoe in productionWorkOrder.Equipments)
                {
                    // add if there's a notification number and it hasn't been completed.
                    if (pwoe.SAPNotificationNumber.HasValue && !pwoe.CompletedOn.HasValue)
                    {
                        foreach (var notification in SapProductionWorkOrderChildNotification)
                        {
                            if (notification.SAPEquipmentNumber ==
                                pwoe.Equipment.SAPEquipmentId.ToString().TrimStart('0'))
                            {
                                notification.CompleteFlag = "Y";
                                foreach (var point in notification.SapProductionWorkOrderMeasuringPoints)
                                {
                                    point.CompleteNotification = "Y";
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private IList<SAPProductionWorkOrderChildNotification> MapChildNotifications(
            ProductionWorkOrder productionWorkOrder)
        {
            var ret = new List<SAPProductionWorkOrderChildNotification>();
            foreach (var notification in productionWorkOrder.Equipments)
            {
                var childNotification = new SAPProductionWorkOrderChildNotification {
                    SAPEquipmentNumber = notification.Equipment?.SAPEquipmentId?.ToString(),
                    NotificationNumber = notification.SAPNotificationNumber?.ToString(),
                    NotificationType = "11"
                };
                // add any child measurement points
                foreach (var mp in productionWorkOrder.ProductionWorkOrderMeasurementPointValues.Where(z =>
                    z.Equipment == notification.Equipment))
                {
                    childNotification.SapProductionWorkOrderMeasuringPoints.Add(
                        new SAPProductionWorkOrderMeasuringPoints {
                            MeasuringReading1 = mp.Value,
                            Unit1 = mp.MeasurementPointEquipmentType?.UnitOfMeasure?.SAPCode,
                            MeasuringPoint1 = mp.MeasurementPointEquipmentType?.Description,
                            MeasuringDocument = mp.MeasurementDocId?.ToString()
                        });
                }

                ret.Add(childNotification);
            }

            return ret;
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// This is the object that gets passed to the SAP Webservice through HttpClient.
        /// </summary>
        /// <returns></returns>
        public ProgressScheduledUnscheduledWO UnscheduledProgressWorkOrderRequest()
        {
            ProgressScheduledUnscheduledWO request = new ProgressScheduledUnscheduledWO();
            request.UnscheduledWORequest = new ProgressScheduledUnscheduledWOChangeOrder[1];

            var Materials = new ProgressScheduledUnscheduledWOChangeOrderMaterials[1];
            var EmployeeAssignment = new ProgressScheduledUnscheduledWOChangeOrderCrewAssignment[1];
            var MultipleEmployeeAssignment = new ProgressScheduledUnscheduledWOChangeOrderEmployeeAssignment[1];

            request.UnscheduledWORequest[0] = new ProgressScheduledUnscheduledWOChangeOrder();
            request.UnscheduledWORequest[0].AccountCharged = AccountCharged;
            request.UnscheduledWORequest[0].BreakDownIndicator = BreakDownIndicator;
            request.UnscheduledWORequest[0].CancellationReason = CancellationReason;
            request.UnscheduledWORequest[0].CancelOrder = CancelOrder;
            request.UnscheduledWORequest[0].Permit = Permits;

            if (SapMaterialUsedForProductionWorkOrder != null && SapMaterialUsedForProductionWorkOrder.Any())
            {
                Materials = new ProgressScheduledUnscheduledWOChangeOrderMaterials[SapMaterialUsedForProductionWorkOrder
                   .ToList().Count];

                for (int i = 0; i < SapMaterialUsedForProductionWorkOrder.ToList().Count; i++)
                {
                    Materials[i] = new ProgressScheduledUnscheduledWOChangeOrderMaterials();
                    Materials[i].PartNumber = SapMaterialUsedForProductionWorkOrder.ToList()[i].PartNumber;
                    Materials[i].Description = SapMaterialUsedForProductionWorkOrder.ToList()[i].Description;
                    Materials[i].Quantity = SapMaterialUsedForProductionWorkOrder.ToList()[i].Quantity;
                    Materials[i].StcokLocation = SapMaterialUsedForProductionWorkOrder.ToList()[i].StcokLocation;
                    Materials[i].OperatingCenterCode = SapMaterialUsedForProductionWorkOrder.ToList()[i].PlanningPlan;
                    Materials[i].ItemCategory = SapMaterialUsedForProductionWorkOrder.ToList()[i].ItemCategory;
                }

                request.UnscheduledWORequest[0].Materials = Materials;
            }

            //latest employee assignment
            if (SapEmployeeAssignments != null && SapEmployeeAssignments.Any())
            {
                EmployeeAssignment = new ProgressScheduledUnscheduledWOChangeOrderCrewAssignment[1];
                EmployeeAssignment[0] = new ProgressScheduledUnscheduledWOChangeOrderCrewAssignment();
                EmployeeAssignment[0].CrewAssign = SapEmployeeAssignments.ToList()[0].CrewAssign;
                EmployeeAssignment[0].DateStart = SapEmployeeAssignments.ToList()[0].DateStart;
                EmployeeAssignment[0].DateEnd = SapEmployeeAssignments.ToList()[0].DateEnd;
                EmployeeAssignment[0].DateCompleted = SapEmployeeAssignments.ToList()[0].DateCompleted;
                request.UnscheduledWORequest[0].Employee = SapEmployeeAssignments.ToList()[0].EmployeeId;
                request.UnscheduledWORequest[0].TimeToComplete =
                    SapEmployeeAssignments.ToList()[0].TotalManHours?.ToString();
            }

            request.UnscheduledWORequest[0].CrewAssignment = EmployeeAssignment[0];
            request.UnscheduledWORequest[0].Notes = Notes;
            request.UnscheduledWORequest[0].OperatingCenter = OperatingCenter;
            request.UnscheduledWORequest[0].PMActType = PMActType;
            request.UnscheduledWORequest[0].Priority = SAPPriority(Priority);
            request.UnscheduledWORequest[0].PurposeCode = GetPurposeCode(Purpose);
            request.UnscheduledWORequest[0].PurposeGroup = PurposeGroup;
            request.UnscheduledWORequest[0].RequestedBY = RequestedBY;
            request.UnscheduledWORequest[0].SAPEquipmentNo = SAPEquipmentNo;
            request.UnscheduledWORequest[0].SAPFunctionalLoc = SAPFunctionalLoc;
            request.UnscheduledWORequest[0].SAPNotificationNo = SAPNotificationNo;
            request.UnscheduledWORequest[0].SAPWorkOrderNo = SAPWorkOrderNo;
            request.UnscheduledWORequest[0].WorkDescription = WorkDescription;
            request.UnscheduledWORequest[0].Capitalized = Capitalized;

            if (SapProductionWorkOrderChildNotification != null && SapProductionWorkOrderChildNotification.Any())
            {
                var existingChildNotifications = SapProductionWorkOrderChildNotification.ToList();
                var sapChildNotifications =
                    new ProgressScheduledUnscheduledWOChangeOrderChildNotification[existingChildNotifications.Count];

                for (int i = 0; i < existingChildNotifications.Count; i++)
                {
                    // add a notification if there's no notification number
                    // add notification if it has a notification number already and has measurement points without doc ids
                    // add notification if we are trying to reverse/cancel it
                    if (string.IsNullOrWhiteSpace(existingChildNotifications[i].NotificationNumber)
                        ||
                        (!string.IsNullOrWhiteSpace(existingChildNotifications[i].NotificationNumber) &&
                         existingChildNotifications[i].SapProductionWorkOrderMeasuringPoints
                                                      .Count(mp => string.IsNullOrWhiteSpace(mp.MeasuringDocument)) > 0)
                        ||
                        existingChildNotifications[i].SapProductionWorkOrderMeasuringPoints
                                                     .Any(mp => mp.CancellationFlag)
                        ||
                        existingChildNotifications[i].CompleteFlag == "Y")
                    {
                        sapChildNotifications[i] = new ProgressScheduledUnscheduledWOChangeOrderChildNotification();
                        sapChildNotifications[i].CodeGroup = existingChildNotifications[i].CodeGroup;
                        sapChildNotifications[i].CompleteFlag = existingChildNotifications[i].CompleteFlag;
                        sapChildNotifications[i].NotificationLongText =
                            existingChildNotifications[i].NotificationLongText;
                        sapChildNotifications[i].NotificationNumber = existingChildNotifications[i].NotificationNumber;
                        sapChildNotifications[i].NotificationType = existingChildNotifications[i].NotificationType;
                        sapChildNotifications[i].PurposeCode = existingChildNotifications[i].PurposeCode;
                        sapChildNotifications[i].ReqStartDate = existingChildNotifications[i].ReqStartDate;
                        sapChildNotifications[i].SAPEquipmentNumber = existingChildNotifications[i].SAPEquipmentNumber;
                        sapChildNotifications[i].SAPFunctionalLocation =
                            existingChildNotifications[i].SAPFunctionalLocation;

                        if (existingChildNotifications[i].SapProductionWorkOrderActions != null)
                        {
                            sapChildNotifications[i].Actions =
                                new ProgressScheduledUnscheduledWOChangeOrderChildNotificationActions
                                    [existingChildNotifications[i].SapProductionWorkOrderActions.ToList().Count];
                            for (int j = 0;
                                 j < existingChildNotifications[i].SapProductionWorkOrderActions.ToList().Count;
                                 j++)
                            {
                                sapChildNotifications[i].Actions[j] =
                                    new ProgressScheduledUnscheduledWOChangeOrderChildNotificationActions();
                                sapChildNotifications[i].Actions[j].Code =
                                    existingChildNotifications[i].SapProductionWorkOrderActions.ToList()[j].Code;
                                sapChildNotifications[i].Actions[j].CodeGroup =
                                    existingChildNotifications[i].SapProductionWorkOrderActions.ToList()[j].CodeGroup;
                            }
                        }

                        if (existingChildNotifications[i].SapProductionWorkOrderDependencies != null)
                        {
                            sapChildNotifications[i].Dependencies =
                                new ProgressScheduledUnscheduledWOChangeOrderChildNotificationDependencies
                                    [existingChildNotifications[i].SapProductionWorkOrderDependencies.ToList().Count];
                            for (int l = 0;
                                 l < existingChildNotifications[i].SapProductionWorkOrderDependencies.ToList().Count;
                                 l++)
                            {
                                sapChildNotifications[i].Dependencies[l] =
                                    new ProgressScheduledUnscheduledWOChangeOrderChildNotificationDependencies();
                                sapChildNotifications[i].Dependencies[l].Code = existingChildNotifications[i]
                                   .SapProductionWorkOrderDependencies.ToList()[l].Code;
                                sapChildNotifications[i].Dependencies[l].CodeGroup = existingChildNotifications[i]
                                   .SapProductionWorkOrderDependencies.ToList()[l].CodeGroup;
                            }
                        }

                        if (existingChildNotifications[i].SapProductionWorkOrderMeasuringPoints != null)
                        {
                            sapChildNotifications[i].MeasuringPoints =
                                new ProgressScheduledUnscheduledWOChangeOrderChildNotificationMeasuringPoints
                                [existingChildNotifications[i].SapProductionWorkOrderMeasuringPoints.ToList()
                                                              .Count];
                            for (int k = 0;
                                 k < existingChildNotifications[i].SapProductionWorkOrderMeasuringPoints.ToList().Count;
                                 k++)
                            {
                                // only add a measurement point if it has NO document id, or we are cancelling it, or we are completing it
                                var currentMeasurementPoint = existingChildNotifications[i]
                                                             .SapProductionWorkOrderMeasuringPoints.ToList()[k];
                                if (string.IsNullOrWhiteSpace(currentMeasurementPoint.MeasuringDocument) ||
                                    currentMeasurementPoint.CancellationFlag)
                                {
                                    sapChildNotifications[i].MeasuringPoints[k] =
                                        new ProgressScheduledUnscheduledWOChangeOrderChildNotificationMeasuringPoints();
                                    sapChildNotifications[i].MeasuringPoints[k].MeasuringPoint1 =
                                        currentMeasurementPoint.MeasuringPoint1;
                                    sapChildNotifications[i].MeasuringPoints[k].MeasuringReading1 =
                                        currentMeasurementPoint.MeasuringReading1;
                                    sapChildNotifications[i].MeasuringPoints[k].NoReadingTakenFlag =
                                        currentMeasurementPoint.NoReadingTakenFlag;
                                    sapChildNotifications[i].MeasuringPoints[k].Unit1 = currentMeasurementPoint.Unit1;
                                    //Code added for Measuring Point change
                                    sapChildNotifications[i].MeasuringPoints[k].MeasuringDocument =
                                        currentMeasurementPoint.MeasuringDocument;
                                    sapChildNotifications[i].MeasuringPoints[k].CancellationFlag =
                                        currentMeasurementPoint.CancellationFlag ? "X" : string.Empty;
                                }
                            }
                        }
                    }
                }

                request.UnscheduledWORequest[0].ChildNotification = sapChildNotifications;
            }

            if (SapProductionWorkOrderActions != null)
            {
                var Actions =
                    new ProgressScheduledUnscheduledWOChangeOrderActions[SapProductionWorkOrderActions.ToList().Count];
                for (int j = 0; j < SapProductionWorkOrderActions.ToList().Count; j++)
                {
                    Actions[j] = new ProgressScheduledUnscheduledWOChangeOrderActions();
                    Actions[j].Code = SapProductionWorkOrderActions.ToList()[j].Code;
                    Actions[j].CodeGroup = SapProductionWorkOrderActions.ToList()[j].CodeGroup;
                }

                request.UnscheduledWORequest[0].Actions = Actions;
            }

            if (SapProductionWorkOrderDependencies != null)
            {
                var Dependencies =
                    new ProgressScheduledUnscheduledWOChangeOrderDependencies[SapProductionWorkOrderDependencies
                                                                             .ToList().Count];
                for (int l = 0; l < SapProductionWorkOrderDependencies.ToList().Count; l++)
                {
                    Dependencies[l] = new ProgressScheduledUnscheduledWOChangeOrderDependencies();
                    Dependencies[l].Code = SapProductionWorkOrderDependencies.ToList()[l].Code;
                    Dependencies[l].CodeGroup = SapProductionWorkOrderDependencies.ToList()[l].CodeGroup;
                }

                request.UnscheduledWORequest[0].Dependencies = Dependencies;
            }

            // Add Parent Measurement Points
            if (SapProductionWorkOrderMeasuringPoints != null && SapProductionWorkOrderMeasuringPoints.Any())
            {
                request.UnscheduledWORequest[0].MeasuringPoints = SapProductionWorkOrderMeasuringPoints.Select(x =>
                    new ProgressScheduledUnscheduledWOChangeOrderMeasuringPoints {
                        MeasuringPoint1 = x.MeasuringPoint1,
                        Unit1 = x.Unit1,
                        MeasuringReading1 = x.MeasuringReading1
                    }).ToArray();
            }
            // Add Child Measurement Points

            //Add multiple employee assignments
            //latest employee assignment
            if (SapEmployeeAssignments != null && SapEmployeeAssignments.Any())
            {
                request.UnscheduledWORequest[0].EmployeeAssignment = SapEmployeeAssignments.Select(x =>
                    new
                        ProgressScheduledUnscheduledWOChangeOrderEmployeeAssignment {
                            Person = x.EmployeeId,
                            StartDate = x.DateStart,
                            StartTime = x.StartTime,
                            EndDate = x.DateEnd,
                            EndTime = x.EndTime,
                            TimetoComplete = x.TotalManHours.ToString(),
                            TimetoCompleteUnit = string.Empty
                        }).ToArray();
            }

            return request;
        }

        #endregion
    }
}
