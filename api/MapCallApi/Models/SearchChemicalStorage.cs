using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallApi.Models
{
    public class SearchChemicalStorage : SearchSet<ChemicalStorage>
    {
        [Required]
        [SearchAlias("Facility", "F", "Id")]
        public int? Facility { get; set; }
        [Required]
        [SearchAlias("F.OperatingCenter", "S", "State.Id")]
        public int? State { get; set; }
        [Required]
        [SearchAlias("F.PublicWaterSupply", "PWS", "Id")]
        public int? PublicWaterSupply { get; set; }
    }
}