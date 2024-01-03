using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Contractors.Models.ViewModels
{
    public class DestroyContractorContact : ViewModel<Contractor>
    {
        #region Properties

        [Required, DoesNotAutoMap, EntityMustExist(typeof(ContractorContact))]
        public int? ContractorContactId { get; set; }

        #endregion

        #region Constructors

        public DestroyContractorContact(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override Contractor MapToEntity(Contractor entity)
        {
            var actualContractorContact = entity.Contacts.Single(x => x.Id == ContractorContactId);
            entity.Contacts.Remove(actualContractorContact);

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var contractor = _container.GetInstance<IRepository<Contractor>>().Find(Id);
            if (contractor == null)
            {
                yield break;
            }
            var contractorContact = contractor.Contacts.SingleOrDefault(x => x.Id == ContractorContactId);
            if (contractorContact == null)
            {
                yield return new ValidationResult("Contact does not exist for the contractor", new[] { "ContractorContactId" });
            }
        }

        #endregion
    }
}