using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MedicalCertificate : IEntity, IValidatableObject, IThingWithDocuments, IThingWithNotes,
        IThingWithOperatingCenter, IThingWithEmployee
    {
        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime CertificationDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime ExpirationDate { get; set; }

        public virtual string Comments { get; set; }

        #endregion

        #region References

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual IList<MedicalCertificateDocument> MedicalCertificateDocuments { get; set; }
        public virtual IList<MedicalCertificateNote> MedicalCertificateNotes { get; set; }

        #endregion

        #region Logical Properties

        [DoesNotExport]
        public virtual string TableName => nameof(MedicalCertificate) + "s";

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return MedicalCertificateDocuments.Map(d => (IDocumentLink)d); }
        }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return MedicalCertificateNotes.Map(n => (INoteLink)n); }
        }

        public virtual bool Expired { get; set; }

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
