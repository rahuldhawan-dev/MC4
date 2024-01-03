using MMSINC.Utilities.ObjectMapping;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using StructureMap;
using System.Collections.Generic;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class RemoveSchedulingEmployeeAssignments : ViewModelSet<ProductionWorkOrder>
    {
        #region Properties

        [DoesNotAutoMap]
        public int[] EmployeeAssignmentIds { get; set; }

        [DoesNotAutoMap]
        public IList<int> WorkOrdersToProgress { get; } = new List<int>();

        #endregion

        #region Exposed Methods

        private IEnumerable<ProductionWorkOrder> _items = null;

        public override IEnumerable<ProductionWorkOrder> Items
        {
            get
            {
                return _items ?? (_items = _container.GetInstance<IRepository<ProductionWorkOrder>>()
                                                      .Where(x => x.EmployeeAssignments.Any(y => EmployeeAssignmentIds.Contains(y.Id))));
            }
        }

        public override void OnSaving(IViewModelSet<ProductionWorkOrder> entity)
        {
            var assignments = _container.GetInstance<IRepository<EmployeeAssignment>>()
                                        .Where(x => EmployeeAssignmentIds.Contains(x.Id));

            foreach (var item in assignments)
            {
                item.ProductionWorkOrder.EmployeeAssignments.Remove(item);
                
                if (!item.ProductionWorkOrder.ApprovedOn.HasValue &&
                    !item.ProductionWorkOrder.DateCancelled.HasValue &&
                    item.ProductionWorkOrder.OperatingCenter.CanSyncWithSAP)
                {
                    WorkOrdersToProgress.Add(item.ProductionWorkOrder.Id);
                }
            }

            base.OnSaving(entity);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EmployeeAssignmentIds == null || !EmployeeAssignmentIds.Any())
            {
                yield return new ValidationResult("At least one assignment must be selected for removal.");
            }
        }

        #endregion

        #region Constructors

        public RemoveSchedulingEmployeeAssignments(IContainer container) : base(container) { }

        #endregion
    }
}