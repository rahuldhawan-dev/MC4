using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WorkOrders.Model
{
    public class MainBreakRepository : SapWorkOrdersBaseRepository<MainBreak>
    {
        public static IEnumerable<MainBreak> GetMainBreaksByWorkOrder(int workOrderID)
        {
            return (from m in DataTable
                    where m.WorkOrderID == workOrderID
                    select m);
        }

        public static void DeleteMainBreak(int mainBreakID)
        {
            var mainBreak = GetEntity(mainBreakID);
            var workOrder = mainBreak.WorkOrder;
            Delete(mainBreak);
            UpdateSAPWorkOrderStatic(workOrder);
        }

        public static void InsertMainBreak(int workOrderID, int mainBreakMaterialID, int mainConditionID, int mainFailureTypeID, decimal depth, int mainBreakSoilConditionID,
            int customersAffected, decimal shutdownTime, int mainBreakDisinfectionMethodID, int mainBreakFlushMethodID, decimal? chlorineResidual,
            bool boilAlertIssued, int serviceSizeID, int? footageReplaced, int? replacedWithId)
        {
            try
            {
                var mo = new MainBreak {
                    WorkOrderID = workOrderID,
                    MainBreakMaterialID = mainBreakMaterialID,
                    MainConditionID = mainConditionID,
                    MainFailureTypeID = mainFailureTypeID,
                    Depth = depth,
                    MainBreakSoilConditionID = mainBreakSoilConditionID,
                    CustomersAffected = customersAffected,
                    ShutdownTime = shutdownTime,
                    MainBreakDisinfectionMethodID = mainBreakDisinfectionMethodID,
                    MainBreakFlushMethodID = mainBreakFlushMethodID,
                    ChlorineResidual = chlorineResidual,
                    BoilAlertIssued = boilAlertIssued,
                    ServiceSizeID = serviceSizeID, 
                    FootageReplaced = footageReplaced,
                    ReplacedWithId = replacedWithId
                };
                Insert(mo);
                UpdateSAPWorkOrderStatic(WorkOrderRepository.GetEntity(workOrderID));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void UpdateMainBreak(int MainBreakMaterialID, int MainConditionID, int MainFailureTypeID, decimal Depth, int MainBreakSoilConditionID,
            int CustomersAffected, decimal ShutdownTime, int MainBreakDisinfectionMethodID, int MainBreakFlushMethodID, decimal ChlorineResidual,
            bool BoilAlertIssued, int MainBreakID, int serviceSizeID, int? footageReplaced, int? replacedWithId)
        {
            var item = GetEntity(MainBreakID);
            item.MainBreakMaterialID = MainBreakMaterialID;
            item.MainConditionID = MainConditionID;
            item.MainFailureTypeID = MainFailureTypeID;
            item.Depth = Depth;
            item.MainBreakSoilConditionID = MainBreakSoilConditionID;
            item.CustomersAffected = CustomersAffected;
            item.ShutdownTime = ShutdownTime;
            item.MainBreakDisinfectionMethodID = MainBreakDisinfectionMethodID;
            item.MainBreakFlushMethodID = MainBreakFlushMethodID;
            item.ChlorineResidual = ChlorineResidual;
            item.BoilAlertIssued = BoilAlertIssued;
            item.ServiceSizeID = serviceSizeID;
            item.FootageReplaced = footageReplaced;
            item.ReplacedWithId = replacedWithId;
            Update(item);
            UpdateSAPWorkOrderStatic(item.WorkOrder);
        }
    }
}
