using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentNHibernate.Automapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class BillingPartyViewModel : ViewModel<BillingParty>
    {
        #region Properties

        [Required, StringLength(CreateTrafficControlTicketsForBug2341.StringLengths.BillingParties.DESCRIPTION)]
        public string Description { get; set; }

        [Range(0.00, 999.99)]
        public decimal? EstimatedHourlyRate { get; set; }

        [StringLength(255)]
        public string Payee { get; set; }

        #endregion

        #region Constructors

        public BillingPartyViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateBillingParty : BillingPartyViewModel
    {
        #region Constructors

		public CreateBillingParty(IContainer container) : base(container) {}

        #endregion
	}

    public class EditBillingParty : BillingPartyViewModel
    {
        #region Constructors

		public EditBillingParty(IContainer container) : base(container) {}

        #endregion
	}

    public class SearchBillingParty : SearchSet<BillingParty>
    {
        #region Properties

        public string Description { get; set; }

        #endregion
	}

    public class CreateBillingPartyContact : ViewModel<BillingParty>
    {
        #region Properties

        [EntityMustExist(typeof(Contact)), Required]
        [AutoComplete("Contact", "ByPartialNameMatch")]
        [AutoMap(MapDirections.None)]
        public int? Contact { get; set; }

        [EntityMustExist(typeof(ContactType))]
        [Required]
        [DropDown]
        [AutoMap(MapDirections.None)]
        public int? ContactType { get; set; }

        #endregion
        
        #region Constructors

        public CreateBillingPartyContact(IContainer container) : base(container) {}

        #endregion

        public override BillingParty MapToEntity(BillingParty entity)
        {
            var bpc = new BillingPartyContact {
                BillingParty = entity,
                Contact = _container.GetInstance<IContactRepository>().Find(Contact.GetValueOrDefault()),
                ContactType =
                    _container.GetInstance<IRepository<ContactType>>().Find(ContactType.GetValueOrDefault())
            };
            entity.BillingPartyContacts.Add(bpc);
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Union(Validate());
        }

        private IEnumerable<ValidationResult> Validate()
        {
            var billingParty = _container.GetInstance<IRepository<BillingParty>>().Find(Id);
            if (billingParty == null)
                yield break;
            if (billingParty.BillingPartyContacts.Any(x => x.ContactType.Id == ContactType && x.Contact.Id == Contact))
            {
                const string errFormat = "A contact already exists for this billing party and contact type for {0}";
                var err = string.Format(errFormat, billingParty.Description);
                yield return new ValidationResult(err, new[] {"ContactId"});
            }
        } 
    }

    public class DestroyBillingPartyContact : ViewModel<BillingParty>
    {
        #region Properties

        [Required, DoesNotAutoMap]
        [EntityMustExist(typeof(BillingPartyContact))]
        public int? BillingPartyContactId { get; set; }

        #endregion

        #region Constructors

        public DestroyBillingPartyContact(IContainer container) : base(container) {}

        #endregion

        #region Public Methods

        public override BillingParty MapToEntity(BillingParty entity)
        {
            var actualBillingPartyContact = entity.BillingPartyContacts.Single(x => x.Id == BillingPartyContactId);
            entity.BillingPartyContacts.Remove(actualBillingPartyContact);

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var billingParty = _container.GetInstance<IRepository<BillingParty>>().Find(Id);
            if (billingParty == null)
            {
                yield break;
            }
            var billingPartyContact =
                billingParty.BillingPartyContacts.SingleOrDefault(x => x.Id == BillingPartyContactId);
            if (billingPartyContact == null)
            {
                yield return new ValidationResult("Contact does not exist for the billing party", new[] {"BillingPartyContactId"});
            }
        } 

        #endregion
    }
}