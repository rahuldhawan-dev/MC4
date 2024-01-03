using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OneCallTicket : IEntity, IValidatableObject
    {
        #region Consts

        public struct StringLengths
        {
            public const int REQUEST_NUMBER = 18,
                             // This is supposed to be 9 but the database is 18 so blame Alex.
                             COUNTY = 35,
                             TOWN = 35,
                             STREET = 45,
                             STATE = 5,
                             NEAREST_CROSS_STREET = 40,
                             // Why this isn't 45 like Street is I do not know.
                             EXCAVATOR = 50,
                             EXCAVATOR_PHONE = 18,
                             EXCAVATOR_ADDRESS = 120;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string RequestNumber { get; set; }
        public virtual string State { get; set; }
        public virtual string County { get; set; }
        public virtual string Town { get; set; }
        public virtual string Street { get; set; }
        public virtual string NearestCrossStreet { get; set; }
        public virtual string Excavator { get; set; }
        public virtual string ExcavatorPhone { get; set; }
        public virtual string ExcavatorAddress { get; set; }

        #endregion

        #region Public Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
