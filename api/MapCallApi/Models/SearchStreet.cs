using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallApi.Models
{
    public class SearchStreet : SearchSet<Street>
    {
        [Required]
        public int? Town { get; set; }
        public SearchString FullStName { get; set; }
    }
}
