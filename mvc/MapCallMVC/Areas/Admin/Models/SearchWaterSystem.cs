using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallMVC.Areas.Admin.Models {
    public class SearchWaterSystem : SearchSet<WaterSystem>
    {
        public string Description { get; set; }
        public string LongDescription { get; set; }
    }
}