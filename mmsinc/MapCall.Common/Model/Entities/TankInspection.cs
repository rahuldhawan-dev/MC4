using MapCall.Common.Model.Migrations;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using MMSINC.Utilities.ObjectMapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AjaxControlToolkit.HTMLEditor.ToolbarButton;
using FluentNHibernate.Conventions;
using FluentNHibernate.Utils;
using MMSINC.ClassExtensions.IQueryableExtensions;
using NHibernate.Engine;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TankInspection : IEntity, IThingWithCoordinate, IThingWithNotes, IThingWithDocuments
    {
        #region Consts

        public struct StringLengths
        {
            public const int COMMENT_STRING_LENGTH = int.MaxValue,
                             GENERAL_TAB_LENGTH = 255,
                             ZIP_CODE = 12;
        }

        public struct ValueRanges
        {
            public const string MIN_RANGE = "0.001";
            public const string MAX_RANGE = "15.00";
        }

        public struct Display
        {
            #region Generic Values for the Tabs

            public const string OBSERVATION_AND_COMMENTS = "Observation and Comments",
                                REPAIRS_NEEDED = "Repairs Needed - Y/N? If Yes, explain",
                                CORRECTIVE_WORKORDER_DATE_CREATED = "Corrective Work Order Date Created",
                                CORRECTIVE_WORKORDER_DATE_COMPLETED = "Corrective Work Order Date Completed",
                                TOWN = "Town",
                                ADDRESS = "Address",
                                COORDINATES = "Coordinates",
                                TANK_CAPACITY = "Tank Capacity (MG)",
                                LAST_OBSERVATION = "Last Observation / Inspection Date",
                                OBSERVATION_DATE = "Observation / Inspection Date",
                                TANK_STRUCTURE = "Type of Tank Structure",
                                TANK_INSPECTION_TYPE = "Observation / Inspection Type",
                                OBSERVATION_ID = "Observation Id",
                                OPERATING_CENTER = "Operating Center",
                                PRODUCTION_WORK_ORDER = "Production Work Order",
                                PUBLIC_WATER_SUPPLY = "Public Water Supply ID",
                                TABLE_NAME = "TankInspections";

            #endregion
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }
        [DoesNotExport]
        public virtual MapIcon Icon => Coordinate?.Icon;

        #region General Tab

        public virtual State State => Town?.State;
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual Employee TankObservedBy { get; set; }
        [View(Display.PUBLIC_WATER_SUPPLY)]
        [StringLength(StringLengths.GENERAL_TAB_LENGTH)]
        public virtual PublicWaterSupply PublicWaterSupply { get; set; }
        [View(Display.TANK_CAPACITY)]
        public virtual decimal TankCapacity { get; set; }
        [StringLength(StringLengths.GENERAL_TAB_LENGTH)]
        [View(Display.ADDRESS)]
        public virtual string TankAddress { get; set; }
        public virtual Town Town { get; set; }
        [StringLength(StringLengths.GENERAL_TAB_LENGTH)]
        public virtual Coordinate Coordinate { get; set; }
        public virtual decimal? Latitude => Coordinate?.Latitude;
        public virtual decimal? Longitude => Coordinate?.Longitude;
        [StringLength(StringLengths.ZIP_CODE)]
        public virtual string ZipCode { get; set; }
        [View(Display.OBSERVATION_DATE, FormatStyle.Date)]
        public virtual DateTime? ObservationDate { get; set; }
        [View(Display.LAST_OBSERVATION, FormatStyle.Date)]
        public virtual DateTime? LastObserved { get; set; }
        public virtual Facility Facility { get; set; }
        [View(Display.TANK_INSPECTION_TYPE)]
        public virtual TankInspectionType TankInspectionType { get; set; }

        #endregion

        #region TankInspection Questions

        public virtual IList<TankInspectionQuestion> TankInspectionQuestions { get; set; }

        #endregion

        #endregion

        #region Logical Properties

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }
        public override string ToString()
        {
            return $"{Id}-{Facility?.FacilityName}-{Equipment?.EquipmentType?.Display}";
        }

        public virtual string DisplayObservationInspectionType => Equipment != null ? String.Join(", ", Equipment.MaintenancePlans.Select(x => x.MaintenancePlan.Description)) : String.Empty;

        #endregion

        #region NotesDocs

        #region Documents

        [DoesNotExport]
        public virtual string TableName => TankInspection.Display.TABLE_NAME;
        public virtual IList<Document<TankInspection>> Documents { get; set; }
        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        #endregion

        #endregion

        #region Notes

        public virtual IList<Note<TankInspection>> Notes { get; set; }
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion

        #endregion

        #region Constructor

        public TankInspection()
        {
            TankInspectionQuestions = new List<TankInspectionQuestion>();
        }

        #endregion
    }
}