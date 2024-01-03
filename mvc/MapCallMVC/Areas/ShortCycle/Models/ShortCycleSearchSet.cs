using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.ShortCycle.Models
{
    public abstract class ShortCycleSearchSet<T> : SearchSet<T>
    where T : IShortCycleWorkOrderEntity
    {
        [View("SAP Communication Status"), MultiSelect, EntityMap, EntityMustExist(typeof(SapCommunicationStatus))]
        public int[] SapCommunicationStatus { get; set; }
    }
}
