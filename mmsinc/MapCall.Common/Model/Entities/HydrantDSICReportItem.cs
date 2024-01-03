using System;
using System.ComponentModel;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    public class HydrantDSICReportItem : IEntity
    {
        public virtual int Id { get; set; }
        public virtual int? OperatingCenterId { get; set; }

        [View("Operating Center")]
        public virtual string OperatingCenterStr { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual string Street { get; set; }
        public virtual string Town { get; set; }

        public virtual string StreetNumber { get; set; }
        public virtual string HydrantNumber { get; set; }
        public virtual string WBSNumber { get; set; }

        [DisplayName("SAP Equipment ID")]
        public virtual int? SAPEquipmentId { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? DateInstalled { get; set; }

        public virtual decimal? Latitude { get; set; }
        public virtual decimal? Longitude { get; set; }
        public virtual string PremiseNumber { get; set; }
    }
}
