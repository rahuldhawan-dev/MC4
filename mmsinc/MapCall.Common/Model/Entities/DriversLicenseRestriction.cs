using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DriversLicenseRestriction : IEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int
                CORRECTIVE_LENSES = 1,
                MECHANICAL_AID = 2,
                PROSTHETIC_AID = 3,
                AUTOMATIC_TRANSMISSION = 4,
                OUTSIDE_MIRROR = 5,
                CDL_INTRASTATE_ONLY = 6,
                VEHICLES_WITHOUT_AIR_BRAKES = 7,
                HEARING_AIDE_REQUIRED = 8,
                MEDICAL_WAIVER_REQUIRED = 9;
        }

        #endregion

        #region Private Members

        private DriversLicenseRestrictionDisplayItem _display;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Letter { get; set; }
        public virtual string Title { get; set; }
        public virtual IList<DriversLicensesRestriction> DriversLicensesRestrictions { get; set; }

        public virtual string Description => (_display ?? (_display = new DriversLicenseRestrictionDisplayItem {
            Id = Id,
            Letter = Letter,
            Title = Title
        })).Display;

        #endregion

        #region Constructors

        public DriversLicenseRestriction()
        {
            DriversLicensesRestrictions = new List<DriversLicensesRestriction>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }

    [Serializable]
    public class DriversLicenseRestrictionDisplayItem : DisplayItem<DriversLicenseRestriction>
    {
        public string Letter { get; set; }
        public string Title { get; set; }

        public override string Display => $"{Letter} - {Title}";
    }
}
