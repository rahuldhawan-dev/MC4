using System;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MeterReadingRouteReadingDate : IEntity
    {
        public virtual int Id { get; set; }
        public virtual MeterReadingRoute MeterReadingRoute { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime ReadingDate { get; set; }
    }
}
