using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.ClassExtensions.IQueryableExtensions;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class GasMonitor : EntityLookup, IEntity, IThingWithNotes, IThingWithDocuments
    {
        #region Fields

        private GasMonitorDisplayItem _display;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual MostRecentGasMonitorCalibration MostRecentPassingGasMonitorCalibration { get; set; }

        /// <summary>
        /// Gets/sets the Equipment that this GasMonitor record is for.
        /// The Equipment must have an EquipmentType of SAFGASDT and
        /// and EquipmentPurpose(Equipment Purpose) of "Personal Gas Detector".
        /// </summary>
        public virtual Equipment Equipment { get; set; }

        /// <summary>
        /// Must be greater than or equal to 1.
        /// </summary>
        [View("Calibration Frequency (days)")]
        public virtual int CalibrationFrequencyDays { get; set; }

        public virtual Employee AssignedEmployee { get; set; }
        public virtual ISet<GasMonitorCalibration> Calibrations { get; set; }

        public virtual string Display
        {
            get
            {
                if (Equipment == null)
                {
                    return null;
                }

                return (_display ?? (_display = new GasMonitorDisplayItem {   
                    Description = Equipment.Description,
                    Id = Id,
                    SerialNumber = Equipment.SerialNumber,
                    Status = Equipment.EquipmentStatus?.Description,
                    OperatingCenterCode = Equipment.OperatingCenter?.OperatingCenterCode,
                    OperatingCenterName = Equipment.OperatingCenter?.OperatingCenterName
                })).Display;
            }
        }

        public virtual OperatingCenter OperatingCenter => Equipment?.OperatingCenter;
        public virtual Department Department => Equipment.Facility.Department;
        public virtual string EquipmentDescription => Equipment.Description;
        public virtual EquipmentModel EquipmentModel => Equipment.EquipmentModel;
        public virtual EquipmentManufacturer Manufacturer => Equipment?.EquipmentManufacturer;
        public virtual EquipmentPurpose EquipmentPurpose => Equipment.EquipmentPurpose;
        public virtual EquipmentStatus EquipmentStatus => Equipment.EquipmentStatus;
        public virtual string SerialNumber => Equipment.SerialNumber;

        public virtual IList<Document<GasMonitor>> Documents { get; set; }
        public virtual IList<Note<GasMonitor>> Notes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        [DoesNotExport]
        public virtual string TableName => GasMonitorMap.TABLE_NAME;

        public virtual string RecordUrl { get; set; }

        [View("Equipment Owned By")]
        public virtual string OwnedBy
        {
            get
            {
                var ownedBy = Equipment?.ActiveCharacteristics?.FirstOrDefault(x => x.Field.FieldName == "OWNED_BY");
                return ownedBy != null ? ownedBy.DisplayValue : "Unknown";
            }
        }

        #endregion

        #region Constructors

        public GasMonitor()
        {
            Calibrations = new HashSet<GasMonitorCalibration>();
            Documents = new List<Document<GasMonitor>>();
            Notes = new List<Note<GasMonitor>>();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Display;
        }

        #endregion
    }

    [Serializable]
    public class GasMonitorDisplayItem : DisplayItem<GasMonitor>
    {
        [SelectDynamic("SerialNumber", Field = "Equipment")]
        public string SerialNumber { get; set; }

        [SelectDynamic("OperatingCenterName", Field = "Equipment.OperatingCenter")]
        public string OperatingCenterName { get; set; }

        [SelectDynamic("OperatingCenterCode", Field = "Equipment.OperatingCenter")]
        public string OperatingCenterCode { get; set; }

        [SelectDynamic("Description", Field = "Equipment")]
        public string Description { get; set; }

        [SelectDynamic("Description", Field = "Equipment.EquipmentStatus")]
        public string Status { get; set; }

        public override string Display => $"{SerialNumber} - {OperatingCenterCode} - {OperatingCenterName} - {Description} - {Status}";
    }

    [Serializable]
    public class MostRecentGasMonitorCalibration : IEntity
    {
        public virtual int Id { get; set; }
        public virtual GasMonitor GasMonitor { get; set; }
        public virtual bool DueCalibration { get; set; }

        [View("Last Passing Calibration Date")]
        public virtual DateTime? CalibrationDate { get; set; }

        public virtual DateTime? NextDueDate { get; set; }
    }
}
