using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Easement : IEntity, IThingWithNotes, IThingWithDocuments
    {
        #region Constants

        public struct Display
        {
            public const string CATEGORY = "Asset Category",
                                DATE_RECORDED = "Recorded Date",
                                EASEMENT_DESCRIPTION = "Description",
                                OWNER_NAME = "Grantor Name",
                                OWNER_ADDRESS = "Grantor Address",
                                OWNER_PHONE = "Grantor Phone",
                                REASON_FOR_EASEMENT = "Easement Reason",
                                TYPE_OF_EASEMENT = "Easement Type",
                                WBS = "WBS";
        }

        public struct StringLengths
        {
            public const int RECORD_NUMBER = 50;
        }

        #endregion

        #region Constructors

        public Easement()
        {
            EasementNotes = new List<EasementNote>();
            EasementDocuments = new List<EasementDocument>();
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        [View(Display.CATEGORY)]
        public virtual EasementCategory Category { get; set; }
        [View(Display.REASON_FOR_EASEMENT)]
        public virtual EasementReason Reason { get; set; }
        [View(Display.TYPE_OF_EASEMENT)]
        public virtual EasementType Type { get; set; }
        [View(Display.DATE_RECORDED)]
        public virtual DateTime? DateRecorded { get; set; }
        public virtual string DeedBook { get; set; }
        public virtual string DeedPage { get; set; }
        [View(Display.WBS)]
        public virtual string Wbs { get; set; }
        public virtual DateTime? WorkOrderCompletionDate { get; set; }
        public virtual PayMonth PayMonth { get; set; }
        public virtual string PayFrequency { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        [View(Display.EASEMENT_DESCRIPTION)]
        public virtual string EasementDescription { get; set; }
        public virtual string EasementRequirements { get; set; }
        public virtual string PropertyAddress { get; set; }
        public virtual Town Town { get; set; }
        public virtual State State { get; set; }
        public virtual string PropertyZip { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual string BlockLot { get; set; }
        public virtual decimal? Fee { get; set; }
        public virtual FeeFrequency FeeFrequency { get; set; }
        public virtual string OwnerVendorName { get; set; }
        [View(Display.OWNER_NAME)]
        public virtual string OwnerName { get; set; }
        [View(Display.OWNER_ADDRESS)]
        public virtual string OwnerAddress { get; set; }
        public virtual Town OwnerTown { get; set; }
        public virtual State OwnerState { get; set; }
        public virtual string OwnerZip { get; set; }
        [View(Display.OWNER_PHONE)]
        public virtual string OwnerPhone { get; set; }
        public virtual GrantorType GrantorType { get; set; }
        public virtual bool? Gsp { get; set; }
        public virtual TownSection TownSection { get; set; }
        [MaxLength(StringLengths.RECORD_NUMBER)]
        public virtual string RecordNumber { get; set; }
        public virtual string StreetNumber { get; set; }
        public virtual Street Street { get; set; }
        public virtual Street CrossStreet { get; set; }
        public virtual EasementStatus Status { get; set; }

        #region Documents

        public virtual IList<EasementDocument> EasementDocuments { get; set; }
        public virtual IList<IDocumentLink> LinkedDocuments => EasementDocuments.Cast<IDocumentLink>().ToList();

        #endregion

        #region Notes

        public virtual IList<EasementNote> EasementNotes { get; set; }
        public virtual IList<INoteLink> LinkedNotes => EasementNotes.Cast<INoteLink>().ToList();

        #endregion

        #endregion

        #region Logical Properties

        [DoesNotExport]
        public virtual string TableName => nameof(Easement) + "s";

        #endregion

        #endregion
    }
}
