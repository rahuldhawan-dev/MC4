using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class CompleteMaterialPlanning : ViewModel<WorkOrder>
    {
        public CompleteMaterialPlanning(IContainer container) : base(container) { }

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            entity.MaterialPlanningCompletedOn = _container.GetInstance<DateTimeProvider>().GetCurrentDate();
            return entity;
        }
    }
}
