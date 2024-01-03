using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallApi.Models {
    public class SearchTailgateTalk : SearchSet<TailgateTalk>
    {
        public DateRange HeldOn { get; set; }
    }
}