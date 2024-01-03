using System;
using System.ComponentModel;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    public class ServiceDSICReportItem : IEntity
    {
        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual string Street { get; set; }
        public virtual string Town { get; set; }
        public virtual string OperatingCenterStr { get; set; }

        public virtual string StreetNumber { get; set; }
        public virtual long? ServiceNumber { get; set; }
        public virtual string TaskNumber1 { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? DateInstalled { get; set; }

        public virtual decimal? Latitude { get; set; }
        public virtual decimal? Longitude { get; set; }
    }
}
