using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FireDistrict : IEntity, IValidatableObject
    {
        #region Constants

        public struct StringLengths
        {
            public const int ADDRESS = 255,
                             ADDRESS_CITY = 50,
                             ADDRESS_ZIP = 10,
                             CONTACT = 50,
                             DISTRICT_NAME = 50,
                             FAX = 20,
                             PHONE = 20,
                             ABBREVIATION = 10,
                             UTILITY_NAME = 50,
                             PREMISE_NUMBER = ChangeFireDistrictPremiseNumberToVarchar.PREMISE_NUMBER_LENGTH;
        }

        #endregion

        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }

        public virtual string Address { get; set; }

        public virtual string AddressCity { get; set; }

        public virtual string AddressZip { get; set; }

        public virtual string Contact { get; set; }

        public virtual string DistrictName { get; set; }

        public virtual string Fax { get; set; }

        public virtual string Phone { get; set; }

        public virtual string Abbreviation { get; set; }

        public virtual string UtilityName { get; set; }

        public virtual string PremiseNumber { get; set; }

        public virtual int? UtilityDistrict { get; set; }

        #endregion

        #region References

        public virtual State State { get; set; }

        public virtual ISet<FireDistrictTown> TownFireDistricts { get; set; } =
            new HashSet<FireDistrictTown>();

        public virtual ISet<Hydrant> Hydrants { get; set; } =
            new HashSet<Hydrant>();

        #endregion

        #region Logical Properties

        public virtual bool CanBeDeleted =>
            !TownFireDistricts.Any() && !Hydrants.Any();

        #endregion

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return DistrictName;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
