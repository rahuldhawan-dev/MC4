using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DriversLicenseEndorsement : IEntityLookup
    {
        public struct Indices
        {
            public const int
                DOUBLES_TRIPLES = 1,
                PASSENGER_TRANSPORTATION = 2,
                LIQUID_BULK_TANK_CARGO = 3,
                HAZARDOUS_MATERIAL = 4,
                HAZARDOUS_MATERIAL_AND_TANK_COMBINED = 5,
                SCHOOL_BUS = 6,
                STUDENT_TRANSPORTATION_VEHICLE = 7,
                ACTIVITY_VEHICLE = 8,
                TAXI_LIVERY_SERVICE_BUS_MOTOR_BUS_OR_MOTOR_COACH = 9;
        }

        #region Private Members

        private DriversLicenseEndorsementDisplayItem _display;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Letter { get; set; }
        public virtual string Title { get; set; }
        public virtual IList<DriversLicensesEndorsement> DriversLicensesEndorsements { get; set; }

        public virtual string Description => (_display ?? (_display = new DriversLicenseEndorsementDisplayItem {
            Letter = Letter,
            Title = Title
        })).Display;

        #endregion

        #region Constructors

        public DriversLicenseEndorsement()
        {
            DriversLicensesEndorsements = new List<DriversLicensesEndorsement>();
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
    public class DriversLicenseEndorsementDisplayItem : DisplayItem<DriversLicenseEndorsement>
    {
        public string Letter { get; set; }
        public string Title { get; set; }

        public override string Display => $"{Letter} - {Title}";
    }
}
