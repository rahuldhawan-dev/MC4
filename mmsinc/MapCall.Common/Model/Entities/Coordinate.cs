using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Coordinate : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual decimal Latitude { get; set; }
        public virtual decimal Longitude { get; set; }

        public virtual MapIcon Icon { get; set; }
        //public virtual int AddressId { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        #endregion
    }
}

