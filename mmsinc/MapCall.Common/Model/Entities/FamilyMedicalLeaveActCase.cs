using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FamilyMedicalLeaveActCase : IEntity, IValidatableObject, IThingWithNotes, IThingWithDocuments,
        IThingWithEmployee
    {
        #region Constants

        public const string TO_STRING_FORMAT = "FMLACaseID: {0}, {1}, {2}, {3}";

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual Employee Employee { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? StartDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? EndDate { get; set; }

        [DisplayName("Frequency")]
        public virtual string FrequencyDays { get; set; }

        [BoolFormat("Yes", "No")]
        public virtual bool CertificationExtended { get; set; }

        [DisplayName("SendFMLAPackage"), BoolFormat("Yes", "No")]
        public virtual bool SendPackage { get; set; }

        [DisplayName("FMLA Package Date Sent")]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? PackageDateSent { get; set; }

        [DisplayName("FMLA Package Date Received")]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? PackageDateReceived { get; set; }

        [DisplayName("FMLA Package Date Due")]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? PackageDateDue { get; set; }

        public virtual CompanyAbsenceCertification CompanyAbsenceCertification { get; set; }

        [BoolFormat("Yes", "No")]
        public virtual bool ChronicCondition { get; set; }

        [DisplayName("FMLA Absense Id")]
        public virtual string AbsenseId { get; set; }

        public virtual string Duration { get; set; }

        #region Logical Properties

        #region Documents

        public virtual IList<FamilyMedicalLeaveActCaseDocument> FamilyMedicalLeaveActCaseDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return FamilyMedicalLeaveActCaseDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return FamilyMedicalLeaveActCaseDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<FamilyMedicalLeaveActCaseNote> FamilyMedicalLeaveActCaseNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return FamilyMedicalLeaveActCaseNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return FamilyMedicalLeaveActCaseNotes.Map(n => n.Note); }
        }

        #endregion

        [DoesNotExport]
        public virtual string TableName => FamilyMedicalLeaveActCaseMap.TABLE_NAME;

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public virtual string Description =>
            String.Format(TO_STRING_FORMAT,
                Id,
                String.Format(CommonStringFormats.DATE, StartDate),
                String.Format(CommonStringFormats.DATE, EndDate),
                FrequencyDays);

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }

    [Serializable]
    public class CompanyAbsenceCertification : EntityLookup { }
}
