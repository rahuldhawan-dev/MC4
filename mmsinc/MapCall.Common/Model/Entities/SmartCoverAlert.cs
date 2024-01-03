using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SmartCoverAlert : IEntity, IThingWithDocuments, IThingWithNotes, IThingWithOperatingCenter, IThingWithSyncing, IThingWithCoordinate
    {
        #region Consts

        public struct StringLengths
        {
            public const int SEWER_OPENING_NUMBER = 50,
                             APPLICATION_DESCRIPTION = 50,
                             POWER_PACK_VOLTAGE = 10,
                             WATER_LEVEL_ABOVE_BOTTOM = 10,
                             TEMPERATURE = 10,
                             SIGNAL_STRENGTH = 10,
                             SIGNAL_QUALITY = 10;
        }

        public struct Display
        {
            public const string ALERT_ID = "Alert Id",
                                SEWER_OPENING_NUMBER = "Manhole Number",
                                RECENT_WORK_ORDER = "Work Order Number";
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }
        [View(Display.ALERT_ID)]
        public virtual int AlertId { get; set; }
        [MaxLength(StringLengths.SEWER_OPENING_NUMBER)]
        public virtual string SewerOpeningNumber { get; set; }
        public virtual decimal? Latitude { get; set; }
        public virtual decimal? Longitude { get; set; }
        public virtual decimal? Elevation { get; set; }
        public virtual decimal? SensorToBottom { get; set; }
        public virtual decimal? ManholeDepth { get; set; }
        public virtual DateTime DateReceived { get; set; }
        public virtual bool Acknowledged { get; set; }
        [MaxLength(StringLengths.POWER_PACK_VOLTAGE)]
        public virtual string PowerPackVoltage{ get; set; }
        [MaxLength(StringLengths.WATER_LEVEL_ABOVE_BOTTOM)]
        public virtual string WaterLevelAboveBottom{ get; set; }
        [MaxLength(StringLengths.TEMPERATURE)]
        public virtual string Temperature{ get; set; }
        [MaxLength(StringLengths.SIGNAL_STRENGTH)]
        public virtual string SignalStrength{ get; set; }
        [MaxLength(StringLengths.SIGNAL_QUALITY)]
        public virtual string SignalQuality{ get; set; }
        public virtual DateTime? AcknowledgedOn { get; set; }
        public virtual decimal? HighAlarmThreshold { get; set; }
        public virtual bool NeedsToSync { get; set; }
        public virtual DateTime? LastSyncedAt { get; set; }

        #region References

        public virtual SewerOpening SewerOpening { get; set; }

        public virtual SmartCoverAlertApplicationDescriptionType ApplicationDescription { get; set; }

        public virtual User AcknowledgedBy { get; set; }

        public virtual IList<WorkOrder> WorkOrders { get; set; }

        public virtual IList<SmartCoverAlertAlarm> SmartCoverAlertAlarms { get; set; }

        public virtual IList<SmartCoverAlertSmartCoverAlertType> SmartCoverAlertSmartCoverAlertTypes { get; set; }

        #endregion

        #endregion

        #region Logical Properties

        [DoesNotExport]
        public virtual string TableName => nameof(SmartCoverAlert) + "s";

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        [DoesNotExport]
        [View(Display.RECENT_WORK_ORDER)]
        public virtual WorkOrder MostRecentWorkOrder => WorkOrders.OrderByDescending(x => x.Id).FirstOrDefault();

        public virtual OperatingCenter OperatingCenter => SewerOpening?.OperatingCenter;

        #region Documents

        public virtual IList<Document<SmartCoverAlert>> Documents { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        #endregion

        #region Notes

        public virtual IList<Note<SmartCoverAlert>> Notes { get; set; }

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public virtual Coordinate Coordinate
        {
            get => new Coordinate {
                Longitude = Longitude.GetValueOrDefault(),
                Latitude = Latitude.GetValueOrDefault()
            }; 
            set {}
        }

        public virtual MapIcon Icon => Coordinate.Icon;

        #endregion

        #endregion

        #endregion

        #region Constructors

        public SmartCoverAlert()
        {
            Documents = new List<Document<SmartCoverAlert>>();
            Notes = new List<Note<SmartCoverAlert>>();
            WorkOrders = new List<WorkOrder>();
            SmartCoverAlertAlarms = new List<SmartCoverAlertAlarm>();
            SmartCoverAlertSmartCoverAlertTypes = new List<SmartCoverAlertSmartCoverAlertType>();
        }

        #endregion
    }
}
