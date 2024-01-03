using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Contractors.Models.ViewModels
{
    public class CreateContractorContact : ViewModel<Contractor>
    {
        #region Constructors

        public CreateContractorContact(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [EntityMustExist(typeof(Contact)), Required, AutoMap(MapDirections.None)]
        [AutoComplete("Contact", "ByPartialNameMatch")]
        public int? Contact { get; set; }

        [EntityMustExist(typeof(ContactType)), Required, DropDown, AutoMap(MapDirections.None)]
        public int? ContactType { get; set; }

        #endregion

        #region Mapping

        public override Contractor MapToEntity(Contractor entity)
        {
            var cc = new ContractorContact {
                Contractor = entity,
                Contact = _container.GetInstance<IContactRepository>().Find(Contact.GetValueOrDefault()),
                ContactType =
                    _container.GetInstance<IRepository<ContactType>>().Find(ContactType.GetValueOrDefault())
            };
            entity.Contacts.Add(cc);
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Union(Validate());
        }

        private IEnumerable<ValidationResult> Validate()
        {
            var contractor = _container.GetInstance<IRepository<Contractor>>().Find(Id);
            if (contractor == null)
                yield break;
            if (contractor.Contacts.Any(x => x.ContactType.Id == ContactType && x.Contact.Id == Contact))
            {
                var error = "A contact already exists for this contractor and contact type.";
                yield return new ValidationResult(error, new[] { "ContactId" });
            }
        }

        #endregion
    }
}