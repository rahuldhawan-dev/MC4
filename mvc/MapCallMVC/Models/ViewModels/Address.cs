using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class AddressViewModel : ViewModel<Address>
    {
        #region Properties

        [Required, StringLength(Address.StringLengths.ADDRESS_1)]
        public string Address1 { get; set; }
        [StringLength(Address.StringLengths.ADDRESS_2)]
        public string Address2 { get; set; }
        [Required, StringLength(Address.StringLengths.ZIP_CODE)]
        public string ZipCode { get; set; }

        [DropDown]
        [Required]
        [EntityMap(MapDirections.ToPrimary)]
        public int? State { get; set; }

        [Required]
        [DropDown("County", "ByStateId", DependsOn = "State", PromptText = "Please select a state above")]
        [EntityMap(MapDirections.ToPrimary)]
        public int? County { get; set; }

        [Required]
        [EntityMustExist(typeof(Town))]
        [EntityMap(RepositoryType = typeof(ITownRepository))]
        [DropDown("Town", "ByCountyId", DependsOn = "County", PromptText = "Please select a county above")]
        public int? Town { get; set; }

        #endregion

        #region Constructor

        public AddressViewModel(IContainer container) : base(container) {}

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Returns true if all of the required properties have values. 
        /// Only use this *after* ensuring the model's valid through whatever
        /// validation means are being used. This is a smelly hack and will go away hopefully.
        /// </summary>
        /// <returns></returns>
        public bool HasRequiredValues()
        {
            return (!string.IsNullOrWhiteSpace(Address1) &&
                    !string.IsNullOrWhiteSpace(ZipCode) &&
                    Town.HasValue);
        }

        #endregion
    }
}