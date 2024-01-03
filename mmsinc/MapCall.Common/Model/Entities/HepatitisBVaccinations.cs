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
using NHibernate.Classic;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class HepatitisBVaccination : IEntity, IValidatableObject, IThingWithDocuments, IThingWithNotes,
        IThingWithOperatingCenter, IThingWithEmployee
    {
        #region Properties

        public virtual int Id { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime ResponseDate { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual HepatitisBVaccineStatus HepatitisBVaccineStatus { get; set; }

        public virtual IList<HepatitisBVaccinationDocument> HepatitisBVaccinationDocuments { get; set; }
        public virtual IList<HepatitisBVaccinationNote> HepatitisBVaccinationNotes { get; set; }

        #region Notes/Docs

        [DoesNotExport]
        public virtual string TableName => nameof(HepatitisBVaccination) + "s";

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return HepatitisBVaccinationDocuments.Map(d => (IDocumentLink)d); }
        }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return HepatitisBVaccinationNotes.Map(n => (INoteLink)n); }
        }

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
