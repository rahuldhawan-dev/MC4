using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallApi.Models
{
    public class SearchFacility : SearchSet<Facility>
    {
        [Required]
        [SearchAlias("OperatingCenter", "S", "State.Id")]
        public int? State { get; set; }

        [Required]
        public int? PublicWaterSupply { get; set; }

        [Required]
        public int? WasteWaterSystem { get; set; }
    }
}