using MMSINC.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMSINC.Utilities.Excel;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class InterconnectionTest : IEntity, IThingWithNotes, IThingWithDocuments, IThingWithCoordinate
    {
        #region Constants

        public struct StringLengths
        {
            #region Constants

            public const int REPRESENTATIVE_ON_SITE = 50,
                             INSPECTION_COMMENTS = 250;

            #endregion
        }

        public struct Display
        {
            public const string INTERCONNECTION_INSPECTION_RATING = "Inspection Rating";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual Facility Facility { get; set; }

        public virtual DateTime? InspectionDate { get; set; }

        [View(Display.INTERCONNECTION_INSPECTION_RATING)]
        public virtual InterconnectionInspectionRating InterconnectionInspectionRating { get; set; }

        public virtual float? MaxFlowMGDAchieved { get; set; }

        public virtual bool? AllValvesOperational { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Contractor Contractor { get; set; }

        public virtual WorkOrder WorkOrder { get; set; }

        [StringLength(StringLengths.INSPECTION_COMMENTS)]
        public virtual string InspectionComments { get; set; }

        [StringLength(StringLengths.REPRESENTATIVE_ON_SITE)]
        public virtual string RepresentativeOnSite { get; set; }

        #region Notes

        public virtual IList<Note<InterconnectionTest>> Notes { get; set; } = new List<Note<InterconnectionTest>>();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        [DoesNotExport]
        public virtual string TableName => nameof(InterconnectionTest) + "s";

        #endregion

        #region Documents

        public virtual IList<Document<InterconnectionTest>> Documents { get; set; } = new List<Document<InterconnectionTest>>();

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        #endregion

        #region Coordinates

        public virtual Coordinate Coordinate
        {
            get => Facility?.Coordinate;
            set { }
        }

        public virtual MapIcon Icon => Coordinate?.Icon;

        #endregion

        #endregion
    }
}
