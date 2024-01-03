using MapCall.SAP.GetPMOrderWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.SAP.Model.Entities
{
    public class SAPCreatePreventiveWorkOrder
    {
        #region properties

        #region Request Properoties

        public virtual string PlanningPlant { get; set; }
        public virtual string OrderType { get; set; }
        public virtual string CreatedOn { get; set; }
        public virtual string CompanyCode { get; set; }
        public virtual string LastRunTime { get; set; }
        public virtual string OrderNumber { get; set; }

        #endregion

        #region Response Properoties

        public virtual string ShortText { get; set; }
        public virtual string LongText { get; set; }
        public virtual string MaintenanceActivityType { get; set; }
        public virtual string MaintenancePlant { get; set; }
        public virtual string MaintenanceWorkcenter { get; set; }
        public virtual string BasicStart { get; set; }
        public virtual string BasicFinish { get; set; }
        public virtual string Priority { get; set; }
        public virtual string FunctionalLocation { get; set; }
        public virtual string Equipment { get; set; }
        public virtual string SAPNotificationNumber { get; set; }
        public virtual string Address { get; set; }
        public virtual string SearchTerm { get; set; }
        public virtual string Category { get; set; }
        public virtual string PlannerGroup { get; set; }
        public virtual string SettlementReceiver { get; set; }
        public virtual string TaskListType { get; set; }
        public virtual string TaskListGroup { get; set; }
        public virtual string TaskListCounter { get; set; }
        public virtual string ListSAPWorkOrder { get; set; }
        public IEnumerable<SAPWorkOrderOperation> SapWorkOrderOperation { get; set; }
        public IEnumerable<SAPWorkOrderComponent> SapWorkOrderComponent { get; set; }
        public IEnumerable<SAPWorkOrderObjectList> SapWorkOrderObjectList { get; set; }
        public IEnumerable<SAPWorkOrderPermits> SapWorkOrderPermits { get; set; }
        public virtual string SAPErrorCode { get; set; }

        #endregion

        #endregion

        public SAPCreatePreventiveWorkOrder() { }

        public SAPCreatePreventiveWorkOrder(PMOrdersInfoRecord PmOrdersInfoRecord)
        {
            Address = PmOrdersInfoRecord.Address;
            BasicFinish = PmOrdersInfoRecord.BasicFinish;
            BasicStart = PmOrdersInfoRecord.BasicStart;
            Category = PmOrdersInfoRecord.Category;

            if (PmOrdersInfoRecord.ComponentList != null)
            {
                SapWorkOrderComponent = from c in PmOrdersInfoRecord.ComponentList
                                        select new SAPWorkOrderComponent {
                                            Component = c.OrderComponent,
                                            Desciption = c.Desciption,
                                            ItemCategory = c.ItemCategory,
                                            OperationNumber = c.OrderOperationno,
                                            QtyRequired = c.QtyRequired,
                                            StorageLocation = c.StorageLocation,
                                            UnitOfMeasurement = c.UOM,
                                            OrderPlantDate = c.Orderplantdate,
                                            OrderItemNo = c.OrderItemNo
                                        };
            }

            Equipment = PmOrdersInfoRecord.Equipment;
            FunctionalLocation = PmOrdersInfoRecord.FunctionalLocation;
            ListSAPWorkOrder = PmOrdersInfoRecord.ListSAPWO;
            LongText = PmOrdersInfoRecord.LongText;
            MaintenanceActivityType = PmOrdersInfoRecord.MaintActivitytype;
            MaintenancePlant = PmOrdersInfoRecord.MaintPlan;
            MaintenanceWorkcenter = PmOrdersInfoRecord.MaintWorkcenter;

            if (PmOrdersInfoRecord.ObjectList != null)
            {
                SapWorkOrderObjectList = from o in PmOrdersInfoRecord.ObjectList
                                         select new SAPWorkOrderObjectList {
                                             SAPEquipmentNumber = o.SAPEquipmentNo,
                                             EquipmentDescription = o.EquipmentDesc,
                                             Counter = o.Counter
                                         };
            }

            if (PmOrdersInfoRecord.OperationList != null)
            {
                SapWorkOrderOperation = from p in PmOrdersInfoRecord.OperationList
                                        select new SAPWorkOrderOperation {
                                            NumberOfPersons = p.NumberOfPersons,
                                            OperationDuration = p.OperationDuration,
                                            OperationNumber = p.OperationNo,
                                            OperationShortText = p.ShortText,
                                            Plant = p.Plant,
                                            StdTextKey = p.StdTextKey,
                                            Unit = p.Unit,
                                            WorkCenter = p.WorkCenter,
                                            WorkDuration = p.WorkDuration
                                        };
            }

            OrderNumber = PmOrdersInfoRecord.OrderNo;
            OrderType = PmOrdersInfoRecord.OrderType;
            if (PmOrdersInfoRecord.Permits != null)
            {
                SapWorkOrderPermits = from r in PmOrdersInfoRecord.Permits
                                      select new SAPWorkOrderPermits {
                                          CounterForPermit = r.Counter,
                                          Permit = r.Permit,
                                          PermitCategory = r.PermitCategory
                                      };
            }

            PlannerGroup = PmOrdersInfoRecord.PlannerGroup;
            PlanningPlant = PmOrdersInfoRecord.PlanningPlant;
            Priority = PmOrdersInfoRecord.Priority;
            SAPNotificationNumber = PmOrdersInfoRecord.SAPNotificationNo;
            SearchTerm = PmOrdersInfoRecord.Searchterm1_2;
            SettlementReceiver = PmOrdersInfoRecord.SettlementReceiver;
            ShortText = PmOrdersInfoRecord.ShortText;
            TaskListCounter = PmOrdersInfoRecord.TaskListCounter;
            TaskListGroup = PmOrdersInfoRecord.TaskListGroup;
            TaskListType = PmOrdersInfoRecord.TaskListType;

            SAPErrorCode = "Successful";
        }

        public PMOrdersQuery Request()
        {
            var queryRequest = new PMOrdersQuery();
            queryRequest.CompanyCode = CompanyCode;
            queryRequest.CreatedOn = CreatedOn;
            queryRequest.OrderType = OrderType;
            queryRequest.PlanningPlant = PlanningPlant;
            queryRequest.LastRunTime = LastRunTime;
            queryRequest.Order = OrderNumber;
            return queryRequest;
        }
    }

    public class SAPWorkOrderOperation
    {
        public virtual string OperationNumber { get; set; }
        public virtual string OperationShortText { get; set; }
        public virtual string WorkCenter { get; set; }
        public virtual string Plant { get; set; }
        public virtual string OperationDuration { get; set; }
        public virtual string NumberOfPersons { get; set; }
        public virtual string WorkDuration { get; set; }
        public virtual string StdTextKey { get; set; }
        public virtual string Unit { get; set; }
    }

    public class SAPWorkOrderComponent
    {
        public virtual string OperationNumber { get; set; }
        public virtual string Component { get; set; }
        public virtual string Desciption { get; set; }
        public virtual string QtyRequired { get; set; }
        public virtual string UnitOfMeasurement { get; set; }
        public virtual string ItemCategory { get; set; }
        public virtual string StorageLocation { get; set; }
        public virtual string OrderPlantDate { get; set; }
        public virtual string OrderItemNo { get; set; }
    }

    public class SAPWorkOrderObjectList
    {
        public virtual string Counter { get; set; }
        public virtual string SAPEquipmentNumber { get; set; }
        public virtual string EquipmentDescription { get; set; }
    }

    public class SAPWorkOrderPermits
    {
        public virtual string CounterForPermit { get; set; }
        public virtual string Permit { get; set; }
        public virtual string PermitCategory { get; set; }
    }
}
