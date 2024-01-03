using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class GasMonitorCalibration
        : IEntityWithCreationTracking<User>, IThingWithNotes, IThingWithDocuments
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual GasMonitor GasMonitor { get; set; }

        [View(MMSINC.Utilities.FormatStyle.Date)]
        public virtual DateTime CalibrationDate { get; set; }

        public virtual bool CalibrationPassed { get; set; }

        [Multiline]
        public virtual string CalibrationFailedNotes { get; set; }

        public virtual User CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        public virtual IList<Document<GasMonitorCalibration>> Documents { get; set; }
        public virtual IList<Note<GasMonitorCalibration>> Notes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        [DoesNotExport]
        public virtual string TableName => GasMonitorCalibrationMap.TABLE_NAME;

        #endregion

        #region Constructor

        public GasMonitorCalibration()
        {
            Notes = new List<Note<GasMonitorCalibration>>();
            Documents = new List<Document<GasMonitorCalibration>>();
        }

        #endregion
    }
}
