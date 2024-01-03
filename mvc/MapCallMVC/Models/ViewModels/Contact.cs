using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Metadata;
using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class ContactViewModel : ViewModel<Contact>
    {
        #region Properties

        [StringLength(Contact.StringLengths.BUSINESS_PHONE)]
        public string BusinessPhoneNumber { get; set; }

        [MMSINC.Validation.EmailAddress]
        [StringLength(Contact.StringLengths.EMAIL)]
        public string Email { get; set; }

        [StringLength(Contact.StringLengths.FAX)]
        public string FaxNumber { get; set; }

        [Required]
        [StringLength(Contact.StringLengths.FIRST_NAME)]
        public string FirstName { get; set; }

        [StringLength(Contact.StringLengths.HOME_PHONE)]
        public string HomePhoneNumber { get; set; }

        [Required]
        [StringLength(Contact.StringLengths.LAST_NAME)]
        public string LastName { get; set; }

        [StringLength(Contact.StringLengths.MIDDLE_INITIAL)]
        public string MiddleInitial { get; set; }

        [StringLength(Contact.StringLengths.MOBILE)]
        public string MobilePhoneNumber { get; set; }

        public AddressViewModel Address { get; set; }

        #endregion

        #region Constructor

        public ContactViewModel(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void Map(Contact entity)
        {
            base.Map(entity);
            if (entity.Address != null)
            {
                Address = _container.GetInstance<IViewModelFactory>().Build<AddressViewModel, Address>(entity.Address);
            }
            else
            {
                Address = null;
            }
        }

        public override Contact MapToEntity(Contact entity)
        {
            base.MapToEntity(entity);
            if (Address == null || !Address.HasRequiredValues())
            {
                entity.Address = null;
            }
            else
            {
                if (entity.Address == null)
                {
                    entity.Address = new Address();
                }
                Address.MapToEntity(entity.Address);
            }
            return entity;
        }

        #endregion
    }

    public class SearchContact : SearchSet<Contact>
    {
        #region Properties

        // This is a required alias needed for State/County/Town
        // It does not appear on the page.
        [SearchAlias("Address", "A", "Id", Required = true)]
        public int? Address { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DropDown]
        [SearchAlias("C.State", "S","Id", Required = true)]
        public int? State { get; set; }

        [DropDown("County", "ByStateId", DependsOn = "State", PromptText = "Please select a state above")]
        [SearchAlias("T.County", "C", "Id", Required = true)]
        public int? County { get; set; }

        [DropDown("Town", "ByCountyId", DependsOn = "County", PromptText = "Please select a county above")]
        [SearchAlias("A.Town", "T", "Id", Required = true)]
        public int? Town { get; set; }

        #endregion
    }
}