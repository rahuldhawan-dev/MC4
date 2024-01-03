using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels.FireDistricts
{
    public class FireDistrictViewModel : ViewModel<FireDistrict>
    {
        #region Properties

        [StringLength(FireDistrict.StringLengths.ADDRESS)]
        public virtual string Address { get; set; }

        [StringLength(FireDistrict.StringLengths.ADDRESS_CITY)]
        public virtual string AddressCity { get; set; }

        [StringLength(FireDistrict.StringLengths.ADDRESS_ZIP)]
        public virtual string AddressZip { get; set; }

        [StringLength(FireDistrict.StringLengths.CONTACT)]
        public virtual string Contact { get; set; }

        [Required]
        [StringLength(FireDistrict.StringLengths.DISTRICT_NAME)]
        public virtual string DistrictName { get; set; }

        [StringLength(FireDistrict.StringLengths.FAX)]
        public virtual string Fax { get; set; }

        [StringLength(FireDistrict.StringLengths.PHONE)]
        public virtual string Phone { get; set; }

        [StringLength(FireDistrict.StringLengths.ABBREVIATION)]
        public virtual string Abbreviation { get; set; }

        [StringLength(FireDistrict.StringLengths.UTILITY_NAME)]
        public virtual string UtilityName { get; set; }

        [StringLength(FireDistrict.StringLengths.PREMISE_NUMBER)]
        public virtual string PremiseNumber { get; set; }

        public virtual int? UtilityDistrict { get; set; }

        [EntityMap, EntityMustExist(typeof(State)), DropDown, Required]
        public virtual int? State { get; set; }

        #endregion

        #region Constructors

        public FireDistrictViewModel(IContainer container) : base(container) { }

        #endregion
    }
}
