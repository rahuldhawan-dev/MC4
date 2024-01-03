using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class CreateInstallationWorkOrder : ViewModel<WorkOrder>
    {
        #region Constants

        public const string WORK_ORDER_NOTES = "Please install {0} per NSI notification # {1}";
        public const string SAP_ERROR_CODE = "RETRY::API ORDER CREATED";
        public const string HYDRANT_ID_AND_VALVE_ID_BOTH_SET_ERROR_MESSAGE = "The Hydrant [{0}] and the Valve [{1}] properties cannot both be set.";

        #endregion

        #region API Properties
        
        [RequiredWhen(nameof(Valve), null), EntityMap, EntityMustExist(typeof(Hydrant))]
        public int? Hydrant { get; set; }

        [RequiredWhen(nameof(Hydrant), null), EntityMap, EntityMustExist(typeof(Valve))]
        public int? Valve { get; set; }

        [Required]
        public long? SAPNotificationNumber { get; set; }

        [Required]
        public string WBSNumber { get; set; }

        #endregion

        #region Prepopulated Properties

        [Required, EntityMap(MapDirections.ToEntity), EntityMustExist(typeof(WorkOrderRequester))]
        public int? RequestedBy => WorkOrderRequester.Indices.NSI;

        [Required, EntityMap(MapDirections.ToEntity), EntityMustExist(typeof(WorkOrderPurpose))]
        public virtual int? Purpose => (int) WorkOrderPurpose.Indices.CUSTOMER;

        [Required, EntityMap(MapDirections.ToEntity), EntityMustExist(typeof(WorkOrderPriority))]
        public virtual int? Priority => (int) WorkOrderPriority.Indices.ROUTINE;

        [Required, EntityMap(MapDirections.ToEntity), EntityMustExist(typeof(WorkDescription))]
        public int? WorkDescription => (int) (Hydrant != null ?
             MapCall.Common.Model.Entities.WorkDescription.Indices.HYDRANT_INSTALLATION :
             MapCall.Common.Model.Entities.WorkDescription.Indices.VALVE_INSTALLATION);

        [Required, EntityMap(MapDirections.ToEntity), EntityMustExist(typeof(MarkoutRequirement))]
        public int? MarkoutRequirement => (int) MapCall.Common.Model.Entities.MarkoutRequirement.Indices.ROUTINE;

        public bool DigitalAsBuiltRequired => true;

        public string SAPErrorCode => SAP_ERROR_CODE;

        [Required, EntityMap(MapDirections.ToEntity)]
        public SAPWorkOrderStep SAPWorkOrderStep => _container.GetInstance<IRepository<SAPWorkOrderStep>>().Find(SAPWorkOrderStep.Indices.CREATE);

        [Required, EntityMap(MapDirections.ToEntity), EntityMustExist(typeof(AssetType))]
        public int? AssetType => Hydrant != null ?
            MapCall.Common.Model.Entities.AssetType.Indices.HYDRANT :
            MapCall.Common.Model.Entities.AssetType.Indices.VALVE;

        [Required]
        public string Notes => string.Format(WORK_ORDER_NOTES, Hydrant != null ? "Hydrant" : "Valve", SAPNotificationNumber);
        
        #endregion

        #region Constructors

        public CreateInstallationWorkOrder(IContainer container) : base(container) {}
        
        #endregion

        #region Private Methods
        
        private IEnumerable<ValidationResult> ValidateOnlyOneAssetIdWasReceived()
        {
            if (Hydrant != null && Valve != null)
            {
                yield return new ValidationResult(string.Format(HYDRANT_ID_AND_VALVE_ID_BOTH_SET_ERROR_MESSAGE, 
                    Hydrant, 
                    Valve));
            }
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateOnlyOneAssetIdWasReceived());
        }

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            entity = base.MapToEntity(entity);
            
            if (Hydrant != null)
            {
                Hydrant hydrant = _container.GetInstance<IRepository<Hydrant>>().Find(Hydrant.Value);
                entity.OperatingCenter = hydrant.OperatingCenter;
                entity.Town = hydrant.Town;
                entity.Street = hydrant.Street;
                entity.StreetNumber = hydrant.StreetNumber;
            }
            else if(Valve != null)
            {
                Valve valve = _container.GetInstance<IRepository<Valve>>().Find(Valve.Value);
                entity.OperatingCenter = valve.OperatingCenter;
                entity.Town = valve.Town;
                entity.Street = valve.Street;
                entity.StreetNumber = valve.StreetNumber;
            }

            return entity;
        }

        #endregion
    }
}
