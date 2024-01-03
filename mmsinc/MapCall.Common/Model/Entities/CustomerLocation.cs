using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CustomerLocation : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }

        [Required]
        public virtual string PremiseNumber { get; set; }

        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string Zip { get; set; }

        public virtual IList<CustomerCoordinate> CustomerCoordinates { get; set; }

        #region Association

        //public virtual CustomerCoordinate ActiveCoordinate { get; set; }

        #endregion

        #region Logical Properties

        public virtual bool HasVerifiedCoordinate
        {
            get { return (CustomerCoordinates.Any(x => x.Verified)); }
        }

        public virtual float? Latitude => (VerifiedCoordinate != null) ? VerifiedCoordinate.Latitude : (float?)null;

        public virtual float? Longitude => (VerifiedCoordinate != null) ? VerifiedCoordinate.Longitude : (float?)null;

        public virtual CustomerCoordinate VerifiedCoordinate
        {
            get { return (CustomerCoordinates.FirstOrDefault(x => x.Verified)); }
        }

        #endregion

        #endregion

        #region Constructors

        public CustomerLocation()
        {
            CustomerCoordinates = new List<CustomerCoordinate>();
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
