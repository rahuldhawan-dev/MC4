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
    public class ServiceFlushViewModel : ServiceFlushViewModelBase
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(ServiceFlushFlushType))]
        public int? FlushType { get; set; }

        [Required]
        public DateTime? SampleDate { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(ServiceFlushSampleType))]
        public int? SampleType { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(ServiceFlushSampleTakenByType))] 
        public int? TakenBy { get; set; }

        [Required]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceFlushReplacementType))]
        public int? ReplacementType { get; set; }

        #endregion

        #region Constructor

        public ServiceFlushViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateServiceFlushViewModel : ViewModel<Service>
    {
        #region Properties

        [Required, DoesNotAutoMap]
        public ServiceFlushViewModel ViewModel { get; set; }

        #endregion

        #region Constructor

        public CreateServiceFlushViewModel(IContainer container) : base(container) { }

        #endregion

        public override Service MapToEntity(Service entity)
        {
            // Don't call base method because this view model isn't inheriting
            // from ServiceFlushViewModel and has no mappable properties of its own.

            var flush = new ServiceFlush();
            ViewModel.MapToEntity(flush);
            flush.Service = entity;
            entity.Flushes.Add(flush);

            return entity;
        }
    }

    public class RemoveServiceFlushViewModel : ViewModel<Service>
    {
        #region Properties

        [Required, EntityMap(MapDirections.None), EntityMustExist(typeof(ServiceFlush))]
        public int? ServiceFlushId { get; set; }

        #endregion

        #region Constructor

        public RemoveServiceFlushViewModel(IContainer container) : base(container) { }

        #endregion

        public override Service MapToEntity(Service entity)
        {
            // Don't call base method.

            var flush = entity.Flushes.Single(x => x.Id == ServiceFlushId.Value);
            entity.Flushes.Remove(flush);

            return entity;
        }
    }

    public class ServiceFlushViewModelBase : ViewModel<ServiceFlush>
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(ServiceFlushSampleStatus))]
        public int? SampleStatus { get; set; }
        
        // This is called FlushContactMethod because otherwise there are conflicts on the
        // page due to ServicePremiseContact.ContactMethod existing.
        [DropDown, EntityMap(nameof(ServiceFlush.ContactMethod)), EntityMustExist(typeof(ServiceFlushPremiseContactMethod))]
        public int? FlushContactMethod { get; set; }
        
        public DateTime? PremiseContactDate { get; set; }

        // This is called FlushNotifiedCustomerServiceCenter because otherwise ther are conflicts on the page
        // due to the ServicePremiseContact.NotifiedCustomerServiceCenter existing.
        [View("Notified Customer Service Center"), AutoMap(SecondaryPropertyName = nameof(ServiceFlush.NotifiedCustomerServiceCenter))]
        public bool? FlushNotifiedCustomerServiceCenter { get; set; }

        [BoolFormat("Passed", "Failed", "n/a")]
        public virtual bool? SampleResultPassed { get; set; }

        public string FlushingNotes { get; set; }

        [View("Sample Id")]
        [RequiredWhen("SampleResultPassed", ComparisonType.NotEqualTo, null, ErrorMessage = "The Sample Id is required.")]
        public int? SampleId { get; set; }

        [DoesNotAutoMap]
        public ServiceFlush Display
        {
            // This is needed to render a cancel button that redirects back to the service rather than the flush.
            get { return _container.GetInstance<IRepository<ServiceFlush>>().Find(Id); }
        }

        #endregion

        #region Constructor

        public ServiceFlushViewModelBase(IContainer container) : base(container) { }

        #endregion
    }

}