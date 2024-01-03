using System;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class StartEmployeeAssignment : ViewModel<EmployeeAssignment>
    {
        public StartEmployeeAssignment(IContainer container) : base(container) { }

        [DateTimePicker]
        public DateTime? DateStarted { get; set; }

        [AutoMap(MapDirections.None)]
        public int ProductionWorkOrder { get; set; }

        public override void Map(EmployeeAssignment entity)
        {
            base.Map(entity);
            ProductionWorkOrder = entity.ProductionWorkOrder.Id;
        }

        public override EmployeeAssignment MapToEntity(EmployeeAssignment entity)
        {
            entity.DateStarted = DateStarted ?? _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            return entity;
        }
    }
}