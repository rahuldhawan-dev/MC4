using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderSupervisorApproval
{
    public class CreateRevisitWorkOrderViewModel : ViewModel<WorkOrder>
    {
        public CreateRevisitWorkOrderViewModel(IContainer container) : base(container) { }

        [Required, EntityMap, EntityMustExist(typeof(WorkOrderPurpose))]
        public int? Purpose { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(WorkOrderPriority))]
        public int? Priority { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(WorkDescription))]
        public int? WorkDescription { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(MarkoutRequirement))]
        public int? MarkoutRequirement { get; set; }

        [Multiline, StringLength(int.MaxValue)]
        public string Notes { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(WorkOrderRequester))]
        public int? RequestedBy { get; set; }

        public bool DigitalAsBuiltRequired { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(AssetType))]
        public int? AssetType { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? OriginalOrderNumber { get; set; }

        public long? DeviceLocation { get; set; }

        public long? SAPEquipmentNumber { get; set; }

        public long? Installation { get; set; }

        public string PremiseNumber { get; set; }

        public string ServiceNumber { get; set; }

        public string SAPErrorCode { get; set; }
        
        [Required, EntityMap, EntityMustExist(typeof(SAPWorkOrderStep))]
        public int? SAPWorkOrderStep { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }
 
        [Required, EntityMap, EntityMustExist(typeof(TownSection))]
        public int? TownSection { get; set; }

        public string StreetNumber { get; set; }
        
        [Required, EntityMap, EntityMustExist(typeof(Street))]
        public int? Street { get; set; }

        public string ApartmentAddtl { get; set; }
        
        [Required, EntityMap, EntityMustExist(typeof(Street))]
        public int? NearestCrossStreet { get; set; }
        
        public string ZipCode { get; set; }
        
        [Required, EntityMap, EntityMustExist(typeof(PlantMaintenanceActivityType))]
        public int? PlantMaintenanceActivityTypeOverride { get; set; }

        public string AccountCharged { get; set; }
        
        [Required, EntityMap, EntityMustExist(typeof(Service))]
        public int? Service { get; set; }

        public virtual DateTime? DateReceived { get; set; }
    }
}