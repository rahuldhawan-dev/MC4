using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SewerOpeningInspection : IEntityWithCreationTimeTracking, ISAPInspection, IThingWithDocuments
    {
        #region Constants

        public struct StringLengths
        {
            public const int PIPES = 50,
                             REMARKS = 100;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual SewerOpening SewerOpening { get; set; }

        [View(FormatStyle.Date, ApplyFormatInEditMode = true)]
        public virtual DateTime DateInspected { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        [View("Rim To Water Level Depth (FT)")]
        public virtual decimal RimToWaterLevelDepth { get; set; }

        [View("Rim Height Above / Below Grade (IN)")]
        public virtual decimal RimHeightAboveBelowGrade { get; set; }

        [View("Pipe(s) In:")]
        public virtual string PipesIn { get; set; }

        [View("Pipe(s) Out:")]
        public virtual string PipesOut { get; set; }

        public virtual User InspectedBy { get; set; }

        [View("AMOUNT OF DEBRIS/GRIT REMOVED (Cubic Ft):")]
        public virtual decimal? AmountOfDebrisGritCubicFeet { get; set; }

        [Multiline]
        public virtual string Remarks { get; set; }

        public virtual string SAPErrorCode { get; set; }
        public virtual string SAPNotificationNumber { get; set; }

        [DoesNotExport]
        public virtual bool SendToSAP =>
            SewerOpening != null && !SewerOpening.OperatingCenter.SAPEnabled
                                 && !SewerOpening.OperatingCenter.IsContractedOperations
                                 && string.IsNullOrEmpty(SAPNotificationNumber);

        #endregion

        #region Documents

        public virtual IList<Document<SewerOpeningInspection>> Documents { get; set; } = new List<Document<SewerOpeningInspection>>();

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        #region Logical Properties

        [DoesNotExport]
        public virtual int SewerOpeningType => SewerOpening != null ? SewerOpening.SewerOpeningType.Id : 0;

        [DoesNotExport]
        public virtual string TableName => nameof(SewerOpeningInspection) + "s";

        #endregion

        #endregion
    }
}
