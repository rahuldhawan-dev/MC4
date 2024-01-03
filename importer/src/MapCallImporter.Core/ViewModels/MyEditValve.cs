using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Validation;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyEditValve : ViewModel<Valve>
    {
        public virtual int InspectionFrequency { get; set; }

        [EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public virtual int? InspectionFrequencyUnit { get; set; }

        [EntityMap, EntityMustExist(typeof(ValveZone))]
        public virtual int? ValveZone { get; set; }

        public MyEditValve(IContainer container) : base(container) { }
    }
}
