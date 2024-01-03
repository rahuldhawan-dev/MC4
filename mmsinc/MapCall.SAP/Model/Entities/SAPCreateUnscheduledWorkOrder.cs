using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.CreateUnscheduledWOWS;
using MapCall.SAP.CustomerOrder;

namespace MapCall.SAP.Model.Entities
{
    public class SAPCreateUnscheduledWorkOrder : SAPEntity
    {
        #region properties

        #region WebService Request Properties

        public virtual string OrderType { get; set; }
        public virtual string Priority { get; set; }
        public virtual string FunctionalLocation { get; set; }
        public virtual string Equipment { get; set; }
        public virtual string ShortText { get; set; }
        public virtual string LongText { get; set; }
        public virtual string MaintenanceActivityType { get; set; }
        public virtual string BasicStartDate { get; set; }
        public virtual string Operation { get; set; }
        public virtual float? OperationDuration { get; set; }
        public virtual string Purpose { get; set; }
        public virtual string PurposeGroup { get; set; }
        public virtual string Permits { get; set; }
        public virtual string BreakDownIndicator { get; set; }
        public virtual string SettlementReceiverField { get; set; }
        public virtual IEnumerable<SAPEmployeeAssignments> SapEmployeeAssignment { get; set; }

        #endregion

        #region WebService Response Properties

        public virtual string NotificationNumber { get; set; }
        public virtual string OrderNumber { get; set; }
        public virtual string Status { get; set; }
        public virtual string WBSElement { get; set; }
        public virtual string SAPErrorCode { get; set; }

        #endregion

        #region Logical properties

        #endregion

        public virtual bool IsSAPEnabled { get; set; }

        #endregion

        #region Exposed methods

        public SAPCreateUnscheduledWorkOrder(ProductionWorkOrder productionWorkOrder)
        {
            OrderType = productionWorkOrder.ProductionWorkDescription?.OrderType?.SAPCode;
            IsSAPEnabled = productionWorkOrder.OrderType.IsSAPEnabled;
            Priority = productionWorkOrder.Priority?.Description;
            FunctionalLocation = productionWorkOrder.FunctionalLocation;
            Equipment = productionWorkOrder.Equipment?.SAPEquipmentId?.ToString();
            ShortText = productionWorkOrder.ProductionWorkDescription?.Description;

            if (productionWorkOrder.ProductionWorkDescription?.PlantMaintenanceActivityType == null)
            {
                MaintenanceActivityType =
                    productionWorkOrder.ProductionWorkDescription?.PlantMaintenanceActivityType?.Code; //PMActType
            }
            else
            {
                MaintenanceActivityType =
                    productionWorkOrder.ProductionWorkDescription?.PlantMaintenanceActivityType?.Code;
                WBSElement = productionWorkOrder.WBSElement;
            }

            BasicStartDate = productionWorkOrder.DateReceived?.Date.ToString(SAP_DATE_FORMAT);
            //wrong value sending becuase of SAP issue. Need to correct once SAP fix this issue
            Operation = productionWorkOrder.ProductionWorkDescription?.Description;
            SettlementReceiverField = productionWorkOrder.WBSElement;

            //PurposeGroup = productionWorkOrder.Purpose?.Description == "Construction Project" ? "N-D-PUR2" : "N-D-PUR1";
            //Purpose = productionWorkOrder.Purpose.Description;

            BreakDownIndicator = productionWorkOrder.ProductionWorkDescription?.BreakdownIndicator == true ? "Y" : "N";
            LongText = productionWorkOrder.CapitalizationReason != ""
                ? productionWorkOrder.CapitalizationReason
                : productionWorkOrder.OrderNotes;
            if (productionWorkOrder.EmployeeAssignments != null)
            {
                var EmployeeAssignments =
                    from c in productionWorkOrder.EmployeeAssignments
                    orderby c.AssignedOn descending
                    select new SAPEmployeeAssignments {
                        TotalManHours = c.TotalManHours,
                        EmployeeId = c.AssignedTo?.EmployeeId
                    };
                SapEmployeeAssignment = EmployeeAssignments.ToList();
            }
        }

        public SAPCreateUnscheduledWorkOrder() { }

        public CreateUnscheduledWOWorkOrder[] UnscheduledWorkOrderRequest()
        {
            CreateUnscheduledWOWorkOrder[] UnscheduledWORequest = new CreateUnscheduledWOWorkOrder[1];
            UnscheduledWORequest[0] = new CreateUnscheduledWOWorkOrder();
            UnscheduledWORequest[0].BasicStartDate = BasicStartDate;
            UnscheduledWORequest[0].BreakDownIndicator = BreakDownIndicator;
            UnscheduledWORequest[0].Equipment = Equipment;
            UnscheduledWORequest[0].FunctionalLocation = FunctionalLocation;
            UnscheduledWORequest[0].LongText = LongText;
            UnscheduledWORequest[0].MaintenanceActivityType = MaintenanceActivityType;
            UnscheduledWORequest[0].Operation = Operation;
            UnscheduledWORequest[0].OperationDuration = OperationDuration?.ToString();
            UnscheduledWORequest[0].OrderType = OrderType;
            UnscheduledWORequest[0].Permits = Permits;
            UnscheduledWORequest[0].Priority = SAPPriority(Priority);
            UnscheduledWORequest[0].Purpose = PurposeGroup;
            UnscheduledWORequest[0].PurposeCode = GetPurposeCode(Purpose);
            UnscheduledWORequest[0].ShortText = ShortText;

            //latest employee assignment
            if (SapEmployeeAssignment != null && SapEmployeeAssignment.Any())
            {
                UnscheduledWORequest[0].OperationDuration =
                    SapEmployeeAssignment.ToList()[0].TotalManHours?.ToString();
                UnscheduledWORequest[0].Employee = SapEmployeeAssignment.ToList()[0].EmployeeId;
            }

            return UnscheduledWORequest;
        }

        #endregion
    }
}
