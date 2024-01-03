using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class AddEmployeeEmployeeAssignment : ViewModel<EmployeeAssignment>
    {
        #region Constructors

        public AddEmployeeEmployeeAssignment(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required, DropDown, EntityMap(MapDirections.None /* Mapped manually */), EntityMustExist(typeof(Employee))]
        public int? Employee { get; set; }

        [DoesNotAutoMap]
        public bool ProgressWorkOrder { get; set; }

        #endregion

        #region Exposed Methods

        public override EmployeeAssignment MapToEntity(EmployeeAssignment entity)
        {
            var employee = _container.GetInstance<IEmployeeRepository>()
                .Find(Employee.Value);
            entity.Employees.Add(employee);

            if (entity.ProductionWorkOrder.ApprovedOn.HasValue || entity.ProductionWorkOrder.DateCancelled.HasValue || !entity.ProductionWorkOrder.OperatingCenter.CanSyncWithSAP)
                return entity;

            ProgressWorkOrder = true;
            return entity;
        }

        #endregion
    }

    public class RemoveEmployeeEmployeeAssignment : ViewModel<EmployeeAssignment>
    {
        #region Properties

        [Required, EntityMustExist(typeof(Employee)), DoesNotAutoMap("Manually mapped")]
        public int? Employee { get; set; }

        [DoesNotAutoMap("Used by controller")]
        public bool ProgressWorkOrder { get; set; }

        #endregion

        #region Constructors

        public RemoveEmployeeEmployeeAssignment(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override EmployeeAssignment MapToEntity(EmployeeAssignment entity)
        {
            var employee = _container.GetInstance<IEmployeeRepository>().Find(Employee.Value);
            entity.Employees.Remove(employee);

            if (entity.ProductionWorkOrder.ApprovedOn.HasValue || entity.ProductionWorkOrder.DateCancelled.HasValue || !entity.ProductionWorkOrder.OperatingCenter.CanSyncWithSAP)
                return entity;

            ProgressWorkOrder = true;
            return entity;
        }

        #endregion
    }
}
