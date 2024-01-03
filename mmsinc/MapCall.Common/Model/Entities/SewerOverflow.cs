using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities.Users;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SewerOverflow
        : IEntityWithCreationTracking<User>, IThingWithNotes, IThingWithDocuments, IThingWithCoordinate
    {
        #region Constants

        public struct DisplayNames
        {
            public const string IS_SYSTEM_UNDER_CONSENT_ORDER =
                                    "Did this occur while the system was under a consent order?",
                                IS_SYSTEM_NEWLY_ACQUIRED = "Did this occur within a year of acquisition of the system?",
                                SEWAGE_RECOVERED_GALLONS = "Sewage Recovered (gallons)",
                                DISCHARGE_LOCATION = "Discharge To",
                                DISCHARGE_LOCATION_OTHER = "Discharge To (Other)",
                                OVERFLOW_TYPE = "Overflow Type",
                                WASTEWATER_SYSTEM = "Wastewater System",
                                GALLONS_FLOWED_INTO_BODY_OF_WATER = "How Many Gallons Flowed Into Body Of Water",
                                OVERFLOW_CUSTOMERS = "Overflow on Customer Side";
        }

        public struct Descriptions
        {
            public const string MULTI_SELECT = "(select all that apply)";
        }

        public struct StringLengths
        {
            public const int TALKED_TO = 50,
                             STREET_NUMBER = 20,
                             ENFORCING_AGENCY_CASE_NUMBER = 50,
                             LOCATION_OF_STOPPAGE = 255,
                             TRUCK_NUMBER = 20;
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        
        [View(DisplayNames.WASTEWATER_SYSTEM)]
        public virtual WasteWaterSystem WasteWaterSystem { get; set; }

        public virtual DateTime? IncidentDate { get; set; }

        [StringLength(StringLengths.TALKED_TO)]
        public virtual string TalkedTo { get; set; }

        [StringLength(StringLengths.STREET_NUMBER)]
        public virtual string StreetNumber { get; set; }

        public virtual Town Town { get; set; }
        public virtual Street Street { get; set; }
        public virtual Street CrossStreet { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual int? GallonsOverflowedEstimated { get; set; }

        [View(DisplayNames.SEWAGE_RECOVERED_GALLONS)]
        public virtual int? SewageRecoveredGallons { get; set; }

        [View(DisplayNames.DISCHARGE_LOCATION)]
        public virtual SewerOverflowDischargeLocation DischargeLocation { get; set; }
        
        [View(DisplayNames.DISCHARGE_LOCATION_OTHER)]
        public virtual string DischargeLocationOther { get; set; }
        
        public virtual BodyOfWater BodyOfWater { get; set; }
        public virtual DischargeWeatherRelatedType WeatherType { get; set; }
        
        [View(DisplayNames.OVERFLOW_TYPE)]
        public virtual SewerOverflowType OverflowType { get; set; }
        
        public virtual SewerOverflowCause OverflowCause { get; set; }
        
        public virtual int? GallonsInContainedDrains { get; set; }
        
        [View(DisplayNames.GALLONS_FLOWED_INTO_BODY_OF_WATER)]
        public virtual int? GallonsFlowedIntoBodyOfWater { get; set; }

        [StringLength(StringLengths.ENFORCING_AGENCY_CASE_NUMBER)]
        public virtual string EnforcingAgencyCaseNumber { get; set; }

        public virtual DateTime? CallReceived { get; set; }
        public virtual DateTime? CrewArrivedOnSite { get; set; }
        public virtual DateTime? SewageContained { get; set; }
        public virtual DateTime? StoppageCleared { get; set; }
        public virtual DateTime? WorkCompleted { get; set; }

        [StringLength(StringLengths.LOCATION_OF_STOPPAGE)]
        public virtual string LocationOfStoppage { get; set; }

        [StringLength(StringLengths.TRUCK_NUMBER)]
        public virtual string TruckNumber { get; set; }
        
        public virtual SewerClearingMethod SewerClearingMethod { get; set; }

        [View(DisplayNames.OVERFLOW_CUSTOMERS), BoolFormat("Yes", "No")]
        public virtual bool? OverflowCustomers { get; set; }
        
        public virtual SewerOverflowArea AreaCleanedUpTo { get; set; }
        public virtual ZoneType ZoneType { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }

        [View(DisplayNames.IS_SYSTEM_UNDER_CONSENT_ORDER)]
        public virtual bool IsSystemUnderConsentOrder { get; set; }

        [View(DisplayNames.IS_SYSTEM_NEWLY_ACQUIRED)]
        public virtual bool IsSystemNewlyAcquired { get; set; }

        #endregion

        #region Logical Properties

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        #region Documents

        public virtual IList<SewerOverflowDocument> SewerOverflowDocuments { get; set; } = new List<SewerOverflowDocument>();

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return SewerOverflowDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return SewerOverflowDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<SewerOverflowNote> SewerOverflowNotes { get; set; } = new List<SewerOverflowNote>();

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return SewerOverflowNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return SewerOverflowNotes.Map(n => n.Note); }
        }

        #endregion

        [DoesNotExport]
        public virtual string TableName => nameof(SewerOverflow) + "s";

        [DoesNotExport]
        public virtual MapIcon Icon => Coordinate.Icon;

        #endregion

        #endregion
    }
}
