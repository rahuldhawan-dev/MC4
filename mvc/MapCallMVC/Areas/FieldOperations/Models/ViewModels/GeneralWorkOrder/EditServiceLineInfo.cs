using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using WorkDescriptionEntity = MapCall.Common.Model.Entities.WorkDescription;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder
{
    public class EditServiceLineInfo : ViewModel<WorkOrder>, IServiceLineInfo
    {
        #region Constructor

        public EditServiceLineInfo(IContainer container) : base(container) { }

        #endregion

        #region Fields

        private WorkOrder _original;

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public WorkOrder WorkOrder
        {
            get
            {
                if (_original == null)
                {
                    _original = Original ?? _container.GetInstance<IRepository<WorkOrder>>().Find(Id);
                }
                return _original;
            }
        }

        [AutoMap(MapDirections.None)]
        public int? WorkDescription => WorkOrder?.WorkDescription?.Id;

        [RequiredWhen(nameof(WorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineInfoWorkDescriptions), typeof(EditServiceLineInfo))]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public int? PreviousServiceLineMaterial { get; set; }

        [RequiredWhen(nameof(WorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineInfoWorkDescriptions), typeof(EditServiceLineInfo))]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int? PreviousServiceLineSize { get; set; }

        [RequiredWhen(nameof(WorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineInfoWorkDescriptions), typeof(EditServiceLineInfo))]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public int? CompanyServiceLineMaterial { get; set; }

        [RequiredWhen(nameof(WorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineInfoWorkDescriptions), typeof(EditServiceLineInfo))]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int? CompanyServiceLineSize { get; set; }

        [RequiredWhen(nameof(WorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineInfoWorkDescriptions), typeof(EditServiceLineInfo))]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public int? CustomerServiceLineMaterial { get; set; }

        [RequiredWhen(nameof(WorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineInfoWorkDescriptions), typeof(EditServiceLineInfo))]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int? CustomerServiceLineSize { get; set; }

        [RequiredWhen(nameof(WorkDescription), ComparisonType.EqualToAny,
            nameof(ServiceLineInfoWorkDescriptions), typeof(EditServiceLineInfo))]
        public DateTime? DoorNoticeLeftDate { get; set; }

        #endregion

        #region Private Methods

        public static int[] ServiceLineInfoWorkDescriptions() => WorkDescriptionEntity.SERVICE_LINE_INFO_REQUIREMENT;

        #endregion

        #region Public Methods

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            base.MapToEntity(entity);

            if (entity.Service != null)
            {
                var service = entity.Service;
                if (entity.PreviousServiceLineMaterial != null)
                {
                    service.PreviousServiceMaterial = entity.PreviousServiceLineMaterial;
                }
                if (entity.PreviousServiceLineSize != null)
                {
                    service.PreviousServiceSize = entity.PreviousServiceLineSize;
                }
                if (entity.CustomerServiceLineMaterial != null)
                {
                    // set sync if values are different
                    if (service.Premise != null && service.CustomerSideMaterial?.Id != entity.CustomerServiceLineMaterial?.Id)
                    {
                        service.NeedsToSync = true;
                    }
                    service.CustomerSideMaterial = entity.CustomerServiceLineMaterial;
                }
                if (entity.CustomerServiceLineSize != null)
                {
                    service.CustomerSideSize = entity.CustomerServiceLineSize;
                }
                if (entity.CompanyServiceLineMaterial != null)
                {
                    // set sync if values are different
                    if (service.Premise != null && service.ServiceMaterial?.Id != entity.CompanyServiceLineMaterial?.Id)
                    {
                        service.NeedsToSync = true;
                    }
                    service.ServiceMaterial = entity.CompanyServiceLineMaterial;
                }
                if (entity.CompanyServiceLineSize != null)
                {
                    service.ServiceSize = entity.CompanyServiceLineSize;
                }

                var serviceRepo = _container.GetInstance<IServiceRepository>();
                serviceRepo.Save(service);
            }

            return entity;
        }

        #endregion
    }

    public interface IServiceLineInfo
    {
        int? PreviousServiceLineMaterial { get; set; }

        int? PreviousServiceLineSize { get; set; }

        int? CompanyServiceLineMaterial { get; set; }

        int? CompanyServiceLineSize { get; set; }

        int? CustomerServiceLineMaterial { get; set; }

        int? CustomerServiceLineSize { get; set; }

        DateTime? DoorNoticeLeftDate { get; set; }
    }
}