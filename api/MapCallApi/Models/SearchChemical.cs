using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallApi.Models
{
    public class SearchChemical : SearchSet<Chemical>
    {
        public SearchString Name { get; set; }
        public SearchString PartNumber { get; set; }
    }
}