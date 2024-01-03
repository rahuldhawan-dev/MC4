using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallApi.Models
{
    public class SearchFireDistrict : SearchSet<FireDistrict>
    {
        [Required]
        public int? Town { get; set; }
    }
}
