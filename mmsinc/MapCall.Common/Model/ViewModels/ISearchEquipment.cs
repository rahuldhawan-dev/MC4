using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public interface ISearchEquipment : ISearchSet<Equipment>
    {
        int? Facility { get; set; }
        int[] EquipmentPurpose { get; set; }
        int? OriginalEquipmentId { get; set; } 
        int? NotEqualEntityId { get; set; }
    }
}
