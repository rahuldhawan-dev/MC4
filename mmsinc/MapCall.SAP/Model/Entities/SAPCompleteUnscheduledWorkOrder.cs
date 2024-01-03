using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.CustomerOrder;
using MapCall.SAP.CompleteScheduledUnscheduledWOWS;
using MMSINC.ClassExtensions.EnumExtensions;

namespace MapCall.SAP.Model.Entities
{
    public class SAPCompleteUnscheduledWorkOrder : SAPEntity
    {
        #region properties

        #region WebService Request Properties

        public virtual string WorkOrderNumber { get; set; }
        public virtual string DateCompleted { get; set; }
        public virtual string WorkOrderDescription { get; set; }
        public virtual string PlantMaintenanceActivityType { get; set; }
        public virtual string Finalize { get; set; }
        public virtual string Notes { get; set; }
        public IEnumerable<SAPEmployeeAssignments> SapEmployeeAssignments { get; set; }

        public virtual IEnumerable<SAPMaterialUsedForProductionWorkOrder> SapMaterialUsedForProductionWorkOrder
        {
            get;
            set;
        }

        public virtual string Capitalized { get; set; }

        #endregion

        #region WebService Response Properties

        public virtual string NotificationNumber { get; set; }
        public virtual string OrderNumber { get; set; }
        public virtual string Status { get; set; }
        public virtual string WbsElement { get; set; }
        public virtual string SAPErrorCode { get; set; }

        #endregion

        public virtual bool IsSAPEnabled { get; set; }

        #endregion

        #region Constructors

        public SAPCompleteUnscheduledWorkOrder() { }

        public SAPCompleteUnscheduledWorkOrder(ProductionWorkOrder productionWorkOrder)
        {
            WorkOrderNumber = productionWorkOrder.SAPWorkOrder;
            IsSAPEnabled = productionWorkOrder.OrderType.IsSAPEnabled;
            DateCompleted = productionWorkOrder.DateCompleted?.Date.ToString(SAP_DATE_FORMAT);
            //Employee = productionWorkOrder
            //  ActualWork   
            Finalize = productionWorkOrder.DateCompleted != null ? "X" : "";

            if (productionWorkOrder.EmployeeAssignments != null && productionWorkOrder.EmployeeAssignments.Any())
            {
                var employeeAssignments = from ca in productionWorkOrder.EmployeeAssignments
                                          where (ca.DateStarted != null && ca.DateEnded != null)
                                          orderby ca.DateStarted descending
                                          select new SAPEmployeeAssignments {
                                              DateStart = ca.DateStarted?.Date.ToString(SAP_DATE_FORMAT),
                                              DateEnd = ca.DateEnded?.Date.ToString(SAP_DATE_FORMAT),
                                              StartTime = ca.DateStarted?.ToString(SAP_TIME_FORMAT),
                                              EndTime = ca.DateEnded?.ToString(SAP_TIME_FORMAT),
                                              TotalManHours = ca.TotalManHours,
                                              // (ca.DateEnded?.Date.Subtract(ca.DateStarted?.Date)) * ca.EmployeesOnJob,
                                              EmployeeId = ca.AssignedTo?.EmployeeId
                                          };

                SapEmployeeAssignments = employeeAssignments.ToList();
            }

            WorkOrderDescription = productionWorkOrder.ProductionWorkDescription?.Description;
            Notes = productionWorkOrder.CancellationReason != null
                ? productionWorkOrder.CancellationReason?.Description
                : productionWorkOrder.OrderNotes;

            if (productionWorkOrder.ProductionWorkDescription?.PlantMaintenanceActivityType == null)
                PlantMaintenanceActivityType =
                    productionWorkOrder.ProductionWorkDescription?.PlantMaintenanceActivityType?.Code; //PMActType
            else
            {
                PlantMaintenanceActivityType =
                    productionWorkOrder.ProductionWorkDescription?.PlantMaintenanceActivityType?.Code;
                WbsElement = productionWorkOrder.WBSElement;
            }

            if (productionWorkOrder.ProductionWorkOrderMaterialUsed != null &&
                productionWorkOrder.ProductionWorkOrderMaterialUsed.Any() &&
                productionWorkOrder.MaterialsApprovedOn != null)
            {
                SapMaterialUsedForProductionWorkOrder = productionWorkOrder.ProductionWorkOrderMaterialUsed.Select(tc =>
                    new
                        SAPMaterialUsedForProductionWorkOrder {
                            Quantity =
                                tc.Quantity.ToString(),
                            StcokLocation =
                                tc.StockLocation
                                 ?.SAPStockLocation,
                            PlanningPlan =
                                tc.ProductionWorkOrder
                                 ?.PlanningPlant?.Code,
                            PartNumber =
                                tc.Material?.PartNumber,
                            Description =
                                tc.Material?.Description ??
                                tc.NonStockDescription,
                        }).ToArray();
            }

            Capitalized = string.IsNullOrEmpty(productionWorkOrder.CapitalizationReason) == false ? "Y" : "";
        }

        #endregion

        #region Exposed Methods

        public CompleteScheduledUnscheduledWO UnscheduledCompleteWorkOrderRequest()
        {
            CompleteScheduledUnscheduledWO request = new CompleteScheduledUnscheduledWO();
            request.UnscheduledWORequest = new CompleteScheduledUnscheduledWOUnscheduledWORequest();

            if (SapMaterialUsedForProductionWorkOrder != null && SapMaterialUsedForProductionWorkOrder.Any())
            {
                request.UnscheduledWORequest.GoodsIssue = SapMaterialUsedForProductionWorkOrder.Select(tc =>
                    new
                        CompleteScheduledUnscheduledWOUnscheduledWORequestGoodsIssue {
                            DocumentDate = DateCompleted,
                            PostingDate = DateCompleted,
                            PartNumber = tc.PartNumber,
                            Quantity = tc.Quantity,
                            SAPStockLocation = tc.StcokLocation,
                            OperatingCenterPlant = tc.PlanningPlan,
                            SAPOrderNumber = WorkOrderNumber
                        }).ToArray();
            }
            else
            {
                request.UnscheduledWORequest.ChangeOrder =
                    new CompleteScheduledUnscheduledWOUnscheduledWORequestChangeOrder[1];
                request.UnscheduledWORequest.ChangeOrder[0] =
                    new CompleteScheduledUnscheduledWOUnscheduledWORequestChangeOrder();
                request.UnscheduledWORequest.ChangeOrder[0].Notes = Notes;
                request.UnscheduledWORequest.ChangeOrder[0].PMActType = PlantMaintenanceActivityType;
                request.UnscheduledWORequest.ChangeOrder[0].WorkOrderDesc = WorkOrderDescription;

                if (SapEmployeeAssignments != null && SapEmployeeAssignments.Any())
                {
                    request.UnscheduledWORequest.TimeConfirmation =
                        new CompleteScheduledUnscheduledWOUnscheduledWORequestTimeConfirmation[1];
                    request.UnscheduledWORequest.TimeConfirmation[0] =
                        new CompleteScheduledUnscheduledWOUnscheduledWORequestTimeConfirmation();
                    request.UnscheduledWORequest.TimeConfirmation[0].Employee =
                        SapEmployeeAssignments.First().EmployeeId;
                    request.UnscheduledWORequest.TimeConfirmation[0].StartDate =
                        SapEmployeeAssignments.First().DateStart;
                    request.UnscheduledWORequest.TimeConfirmation[0].StartTime =
                        SapEmployeeAssignments.First().StartTime;
                    request.UnscheduledWORequest.TimeConfirmation[0].EndDate = SapEmployeeAssignments.First().DateEnd;
                    request.UnscheduledWORequest.TimeConfirmation[0].EndTime = SapEmployeeAssignments.First().EndTime;
                    request.UnscheduledWORequest.TimeConfirmation[0].ActualWork =
                        SapEmployeeAssignments.First().TotalManHours.ToString();
                    request.UnscheduledWORequest.TimeConfirmation[0].DateCompleted = DateCompleted;
                    request.UnscheduledWORequest.TimeConfirmation[0].Finalize = Finalize;
                    request.UnscheduledWORequest.TimeConfirmation[0].OrderNo = WorkOrderNumber;
                    request.UnscheduledWORequest.TimeConfirmation[0].Capitalized = Capitalized;
                }
            }

            return request;
        }

        #endregion
    }
}
