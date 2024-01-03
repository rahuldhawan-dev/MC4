using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WorkOrders.Model
{
    public class RequisitionRepository : WorkOrdersRepository<Requisition>
    {
        public static IEnumerable<Requisition> GetRequisitionsByWorkOrder(int workOrderID)
        {
            return (from r in DataTable
                    where r.WorkOrderID == workOrderID
                    orderby r.RequisitionID
                    select r);
        }

        public static void DeleteRequisition(int requisitionID)
        {
            var entity = GetEntity(requisitionID);
            Delete(entity);
        }

        public static void InsertRequisition(int workOrderID, int requisitionTypeID, string sapRequisitionNumber, int creatorId)
        {
            try
            {
                var req = new Requisition {
                    WorkOrderID = workOrderID,
                    RequisitionTypeID = requisitionTypeID,
                    SAPRequisitionNumber = sapRequisitionNumber,
                    CreatorID = creatorId
                };
                Insert(req);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void UpdateRequisition(int requisitionID, int requisitionTypeID, string sapRequisitionNumber)
        {
            var req = GetEntity(requisitionID);
            req.RequisitionTypeID = requisitionTypeID;
            req.SAPRequisitionNumber = sapRequisitionNumber;
            Update(req);
        }

        public object GetRequiredRequisions(int? operatingCenterID, int? requisitionTypeID, int? workOrderID)
        {
            return (from r in DataTable
                    where
                        (r.SAPRequisitionNumber == null || r.SAPRequisitionNumber == String.Empty) &&
                        (operatingCenterID == null || r.WorkOrder.OperatingCenterID == operatingCenterID) &&
                        (requisitionTypeID == null || r.RequisitionTypeID == requisitionTypeID) &&
                        (workOrderID == null || r.WorkOrderID == workOrderID)
                    select r);
        }
    }

    public class RequisitionTypeRepository : WorkOrdersRepository<RequisitionType>
    {
        
    }
}