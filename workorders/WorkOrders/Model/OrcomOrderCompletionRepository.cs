using System;
using System.Linq;

namespace WorkOrders.Model
{
    public class OrcomOrderCompletionRepository : WorkOrdersRepository<OrcomOrderCompletion>
    {
        public static void InsertOrcomOrderCompletion(int workOrderID, int completedByID, DateTime completedOn)
        {
            var orcomOrderCompletion = new OrcomOrderCompletion {
                WorkOrderID = workOrderID,
                CompletedByID = completedByID,
                CompletedOn = completedOn
            };

            Insert(orcomOrderCompletion);
        }

        public static void DoNothing(int workOrderID)
        {
            
        }

        public static IQueryable<OrcomOrderCompletion> GetCompletionsByOpCenterAndCompany(int? operatingCenterID, string company)
        {
            return (from c in DataTable
                    where
                        (operatingCenterID == null ||
                         c.WorkOrder.OperatingCenterID == operatingCenterID) &&
                        (company == null || company == String.Empty ||
                         (c.CompletedBy.Company != null &&
                          c.CompletedBy.Company == company))
                    select c);
        }
    }
}
