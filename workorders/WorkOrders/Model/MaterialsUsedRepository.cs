using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Exceptions;
using StructureMap;

namespace WorkOrders.Model
{
    public class MaterialsUsedRepository : SapWorkOrdersBaseRepository<MaterialsUsed>
    {
        #region Private Static Methods

        private static void EnsureWorkOrderMaterialsNotApproved(int workOrderId)
        {
            var order = DependencyResolver
                       .Current
                       .GetService<IWorkOrdersWorkOrderRepository>()
                       .Get(workOrderId);

            if (order.MaterialsApproved)
            {
                throw new DomainLogicException("Cannot alter materials used records for a work order which has already had materials approved.");
            }
        }

        #endregion

        public override void InsertNewEntity(MaterialsUsed entity)
        {
            EnsureWorkOrderMaterialsNotApproved(entity.WorkOrderID);
            base.InsertNewEntity(entity);
        }

        public override void UpdateCurrentEntity(MaterialsUsed entity)
        {
            EnsureWorkOrderMaterialsNotApproved(entity.WorkOrderID);
            base.UpdateCurrentEntity(entity);
        }

        public override void UpdateCurrentEntityLiterally(MaterialsUsed entity)
        {
            EnsureWorkOrderMaterialsNotApproved(entity.WorkOrderID);
            base.UpdateCurrentEntity(entity);
        }

        public override void DeleteEntity(MaterialsUsed entity)
        {
            EnsureWorkOrderMaterialsNotApproved(entity.WorkOrderID);
            base.DeleteEntity(entity);
        }

        #region Exposed Static Methods

        public static IEnumerable<MaterialsUsed> GetMaterialsUsedByWorkOrder(int workOrderID)
        {
            return
                (from mu in DataTable
                 where mu.WorkOrderID == workOrderID
                 orderby mu.Material.PartNumber
                 select mu);
        }

        public static void InsertMaterialUsed(int workOrderID, int? materialID, string description, int quantity, int? stockLocationID)
        {
            EnsureWorkOrderMaterialsNotApproved(workOrderID);
            var materialUsed = new MaterialsUsed {
                WorkOrderID = workOrderID,
                Quantity = (short)quantity
            };
            if (materialID == null)
                materialUsed.NonStockDescription = description;
            else
            {
                materialUsed.MaterialID = materialID;
                materialUsed.StockLocationID = stockLocationID;
            }

            Insert(materialUsed);
            //UpdateSAPWorkOrder(WorkOrderRepository.GetEntity(workOrderID));
        }

        public static void UpdateMaterialUsed(int materialsUsedID, int? materialID, string description, int quantity, int? stockLocationID)
        {
            var item = GetEntity(materialsUsedID);
            item.MaterialID = materialID;
            item.Description = description;
            item.Quantity = (short)quantity;
            item.StockLocationID = stockLocationID;
            EnsureWorkOrderMaterialsNotApproved(item.WorkOrderID);
            Update(item);
            //UpdateSAPWorkOrder(item.WorkOrder);
        }

        public static void DeleteMaterialUsed(int materialsUsedID)
        {
            var materialUsed = GetEntity(materialsUsedID);
            var workOrder = materialUsed.WorkOrder;
            EnsureWorkOrderMaterialsNotApproved(materialUsed.WorkOrderID);
            Delete(materialUsed);
            //UpdateSAPWorkOrder(workOrder);
        }

        #endregion
    }
}
