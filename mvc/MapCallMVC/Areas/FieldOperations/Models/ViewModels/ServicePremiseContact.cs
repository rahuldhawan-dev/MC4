using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    // NOTE: If you add a property to this model, you need to add the field to the
    // ServicePremiseContact/Edit view as well as the Service/_AddPremiseContact view.
    public class ServicePremiseContactViewModel : ViewModel<ServicePremiseContact>
    {
        #region Properties
        
        [Required]
        [DropDown, EntityMap, EntityMustExist(typeof(ServicePremiseContactMethod))]
        public int? ContactMethod { get; set; }

        [Required]
        [DropDown, EntityMap, EntityMustExist(typeof(ServicePremiseContactType))]
        public int? ContactType { get; set; }

        [Required]
        public bool? NotifiedCustomerServiceCenter { get; set; }

        [Required]
        public DateTime? ContactDate { get; set; }

        [Required]
        public bool? CertifiedLetterSent { get; set; }

        [Multiline]
        public string ContactInformation { get; set; }

        [Multiline]
        public string CommunicationResults { get; set; }

        [DoesNotAutoMap]
        public ServicePremiseContact Display
        {
            // This is needed to render a cancel button that redirects back to the service rather than the premise contact.
            get { return _container.GetInstance<IRepository<ServicePremiseContact>>().Find(Id); }
        }

        #endregion

        #region Constructors

        public ServicePremiseContactViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateServicePremiseContactViewModel : ViewModel<Service>
    {
        #region Properties

        [Required, DoesNotAutoMap]
        public ServicePremiseContactViewModel ViewModel { get; set; }

        #endregion

        #region Constructor

        public CreateServicePremiseContactViewModel(IContainer container) : base(container) { }

        #endregion

        public override Service MapToEntity(Service entity)
        {
            // Don't call base method because this view model isn't inheriting
            // from ServicePremiseContactViewModel and has no mappable properties of its own.

            var contact = new ServicePremiseContact();
            ViewModel.MapToEntity(contact);
            contact.Service = entity;
            entity.PremiseContacts.Add(contact);

            return entity;
        }
    }

    public class RemoveServicePremiseContactViewModel : ViewModel<Service>
    {
        #region Properties

        [Required, EntityMap(MapDirections.None), EntityMustExist(typeof(ServicePremiseContact))]
        public int? PremiseContactId { get; set; }

        #endregion

        #region Constructor

        public RemoveServicePremiseContactViewModel(IContainer container) : base(container) { }

        #endregion

        public override Service MapToEntity(Service entity)
        {
            // Don't call base method.

            var contact = entity.PremiseContacts.Single(x => x.Id == PremiseContactId.Value);
            entity.PremiseContacts.Remove(contact);

            return entity;
        }
    }
}