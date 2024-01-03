using System;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class GrievanceDisplayItem : DisplayItem<Grievance>
    {
        public override int Id { get; set; }
        public override string Display => Id.ToString();
    }
}
