using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WorkOrders.Model
{
    public class ScheduleOfValueRepository : WorkOrdersRepository<ScheduleOfValue>
    {
        public static IEnumerable<ScheduleOfValue> GetScheduleOfValues()
        {
            return from m in DataTable orderby m.Description select m;
        }

        public static IEnumerable<ScheduleOfValue> SelectByScheduleOfValueCategory(ScheduleOfValueCategory category)
        {
            return category.ScheduleOfValues;
        }
    }

    public class WorkOrderScheduleOfValueRepository : WorkOrdersRepository<WorkOrderScheduleOfValue>
    {
        public static IEnumerable<WorkOrderScheduleOfValue> GetScheduleOfValuesByWorkOrder(int workOrderID)
        {
            return from m in DataTable
                where m.WorkOrderID == workOrderID
                select m;
        }

        public static void DeleteWorkOrderScheduleOfValue(int id)
        {
            Delete(GetEntity(id));
        }

        public static void InsertWorkOrderScheduleOfValue(int workOrderID, int scheduleOfValueID, decimal total, bool isOvertime, string otherDescription)
        {
            try
            {
                var li = new WorkOrderScheduleOfValue
                {
                    WorkOrderID = workOrderID,
                    ScheduleOfValueID = scheduleOfValueID,
                    Total = total,
                    IsOvertime = isOvertime,
                    OtherDescription = otherDescription,
                    LaborUnitCost = GetLaborUnitCostForScheduleOfValue(scheduleOfValueID)
                };
                Insert(li);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void UpdateWorkOrderScheduleOfValue(int scheduleOfValueID, decimal total, bool isOvertime, int id, string otherDescription)
        {
            try
            {
                var li = GetEntity(id);
                if (li != null)
                {
                    li.Total = total;
                    li.IsOvertime = isOvertime;
                    li.ScheduleOfValueID = scheduleOfValueID;
                    li.OtherDescription = otherDescription;
                    li.LaborUnitCost = GetLaborUnitCostForScheduleOfValue(scheduleOfValueID);
                    Update(li);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private static decimal GetLaborUnitCostForScheduleOfValue(int scheduleOfValueId)
        {
            var scheduleOfValue = ScheduleOfValueRepository.GetEntity(scheduleOfValueId);
            return scheduleOfValue?.LaborUnitCost ?? 0;
        }

        public static void UpdateWorkOrderScheduleOfValue(int id, int workOrderID, int scheduleOfValueID, decimal total, bool isOvertime, string otherDescription)
        {
            var entity = GetEntity(id);
            entity.ScheduleOfValueID = scheduleOfValueID;
            entity.Total = total;
            entity.IsOvertime = isOvertime;
            entity.OtherDescription = otherDescription;
            entity.LaborUnitCost = GetLaborUnitCostForScheduleOfValue(scheduleOfValueID);
            Update(entity);
        }
    }
}