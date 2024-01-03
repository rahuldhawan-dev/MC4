using System;
using System.ComponentModel.DataAnnotations;

namespace Permits.Data.Client.Entities
{
    public class Bond : IBond
    {
        #region Consts

        public struct ValidationErrors
        {
            public const string STATE_IS_REQUIRED = "Invalid state. State must be set.",
                                STATE_NOT_FOUND = "Invalid state. State not found.",
                                COUNTY_NOT_FOUND = "Invalid county. County not found.",
                                MUNI_NOT_FOUND = "Invalid municipality. Municipality not found.",
                                COUNTY_REQUIRED_IF_MUNI_IS_SET = "County must be set when Municipality is set.",
                                BOND_DOES_NOT_EXIST_FORMAT = "The bond with id '{0}' does not exist.";
        }

        #endregion

        #region Properties

        public int Id { get; set; }

        [Required(ErrorMessage = ValidationErrors.STATE_IS_REQUIRED)]
        public string StateName { get; set; }

        // RequiredIf could really be use a "Required if this value is not null" argument.
        public string CountyName { get; set; }
        public string MunicipalityName { get; set; }
        public bool IsRecurring { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? ExpirationDate { get; set; }

        #endregion
    }

    public interface IBond
    {
        int Id { get; set; }

        [Required(ErrorMessage = Bond.ValidationErrors.STATE_IS_REQUIRED)]
        string StateName { get; set; }

        string CountyName { get; set; }
        string MunicipalityName { get; set; }

        bool IsRecurring { get; set; }

        [Required]
        DateTime? StartDate { get; set; }

        [Required]
        DateTime? ExpirationDate { get; set; }
    }
}
