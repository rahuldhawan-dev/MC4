using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DriversLicense : IEntity, IValidatableObject, IThingWithDocuments, IThingWithNotes,
        IThingWithOperatingCenter, IThingWithEmployee
    {
        #region Private Members

        private IList<DriversLicenseEndorsement> _endorsements;
        private IList<DriversLicenseRestriction> _restrictions;

        #endregion

        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }
        public virtual string LicenseNumber { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? IssuedDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? RenewalDate { get; set; }

        #endregion

        #region References

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual State State { get; set; }
        public virtual DriversLicenseClass DriversLicenseClass { get; set; }
        public virtual IList<DriversLicensesEndorsement> Endorsements { get; set; }
        public virtual IList<DriversLicensesRestriction> Restrictions { get; set; }
        public virtual IList<DriversLicenseDocument> DriversLicenseDocuments { get; set; }
        public virtual IList<DriversLicenseNote> DriversLicenseNotes { get; set; }

        #endregion

        #region Logical Properties

        public virtual bool Expired { get; set; }

        public virtual string TableName => nameof(DriversLicense) + "s";

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return DriversLicenseDocuments.Map(d => (IDocumentLink)d); }
        }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return DriversLicenseNotes.Map(n => (INoteLink)n); }
        }

        public virtual bool? HasHazardMaterialEndorsement { get; set; }
        public virtual bool? HasLiquidBulkTankCargoEndorsement { get; set; }
        public virtual bool? HasHazardMaterialAndTankCombinedEndorsement { get; set; }
        public virtual bool? HasMedicalWavierRequiredRestriction { get; set; }

        #endregion

        #endregion

        #region Constructors

        public DriversLicense()
        {
            Endorsements = new List<DriversLicensesEndorsement>();
            Restrictions = new List<DriversLicensesRestriction>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
