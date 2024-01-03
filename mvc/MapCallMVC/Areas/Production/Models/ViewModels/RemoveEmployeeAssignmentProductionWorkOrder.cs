using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels {
    public class RemoveEmployeeAssignmentProductionWorkOrder : ViewModel<ProductionWorkOrder>
    {
        #region Properties

        [DoesNotAutoMap("Mapped manually")]
        [Required, EntityMustExist(typeof(EmployeeAssignment))]
        public int? EmployeeAssignment { get; set; }

        [DoesNotAutoMap("Set in MapToEntity, used by controller")]
        public bool ProgressWorkOrder { get; set; }

        #endregion

        #region Constructors

        public RemoveEmployeeAssignmentProductionWorkOrder(IContainer container) : base(container) { }

        #endregion

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            var employeeAssignment = _container.GetInstance<IRepository<EmployeeAssignment>>().Find(EmployeeAssignment.Value);
            entity.EmployeeAssignments.Remove(employeeAssignment);

            // SAP Update??
            if (entity.ApprovedOn.HasValue || entity.DateCancelled.HasValue || !entity.OperatingCenter.CanSyncWithSAP)
                return entity;

            ProgressWorkOrder = true;
            return entity;
        }
    }
}