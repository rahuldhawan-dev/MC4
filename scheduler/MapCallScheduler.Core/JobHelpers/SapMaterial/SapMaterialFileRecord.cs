using System;

namespace MapCallScheduler.JobHelpers.SapMaterial
{
    public class SapMaterialFileRecord
    {
        public string PartNumber { get; set; }
        public string Plant { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ChangedDate { get; set; }
        public decimal? Cost { get; set; }
        public bool DeletionFlag { get; set; }
    }
}