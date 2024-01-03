using System;
using System.ComponentModel;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    public class ValveDSICReportItem : IEntity
    {
        public virtual int Id { get; set; }

        [DoesNotExport]
        public virtual Coordinate Coordinate { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Street Street { get; set; }
        public virtual Town Town { get; set; }

        public virtual string StreetNumber { get; set; }
        public virtual string ValveNumber { get; set; }
        public virtual string WBSNumber { get; set; }

        [DisplayName("SAP Equipment ID")]
        public virtual int? SAPEquipmentId { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? DateInstalled { get; set; }

        public virtual decimal? Latitude => Coordinate?.Latitude;
        public virtual decimal? Longitude => Coordinate?.Longitude;

        [DoesNotExport]
        public virtual bool MatchesWBSPattern { get; set; }

        [DoesNotExport]
        public virtual bool IsInstalled { get; set; }
    }
}
