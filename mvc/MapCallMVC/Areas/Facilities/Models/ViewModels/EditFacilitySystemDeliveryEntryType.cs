using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class EditFacilitySystemDeliveryEntryType : ViewModel<FacilitySystemDeliveryEntryType>
    {
        #region Constructors

        public EditFacilitySystemDeliveryEntryType(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [RequiredWhen("IsEnabled", ComparisonType.EqualTo, true)]
        public decimal? MinimumValue { get; set; }

        [RequiredWhen("IsEnabled", ComparisonType.EqualTo, true)]
        public decimal? MaximumValue { get; set; }

        [Required]
        public bool? IsEnabled { get; set; }
        [DoesNotAutoMap]
        public int SystemDeliveryType { get; set; }

        public int Facility { get; set; }

        [Required, DropDown("Production", "SystemDeliveryEntryType", "BySystemDeliveryTypeId", DependsOn = "SystemDeliveryType"), EntityMap, EntityMustExist(typeof(SystemDeliveryEntryType))]
        public int? SystemDeliveryEntryType { get; set; }
        [DoesNotAutoMap, DropDown, EntityMustExist(typeof(OperatingCenter))] 
        [RequiredWhen(nameof(SystemDeliveryEntryType), ComparisonType.EqualToAny, new[]{MapCall.Common.Model.Entities.SystemDeliveryEntryType.Indices.TRANSFERRED_FROM, MapCall.Common.Model.Entities.SystemDeliveryEntryType.Indices.TRANSFERRED_TO}, FieldOnlyVisibleWhenRequired = true)]
        public int? OperatingCenter { get; set; }
        [DropDown("","Facility", "GetActiveByOperatingCenterWithPointOfEntry", DependsOn = nameof(OperatingCenter), PromptText = "Please select an operating center above"), EntityMap, EntityMustExist(typeof(Facility))]
        [RequiredWhen(nameof(SystemDeliveryEntryType), ComparisonType.EqualToAny, new[]{MapCall.Common.Model.Entities.SystemDeliveryEntryType.Indices.TRANSFERRED_FROM, MapCall.Common.Model.Entities.SystemDeliveryEntryType.Indices.TRANSFERRED_TO}, FieldOnlyVisibleWhenRequired = true)]
        public int? SupplierFacility { get; set; }
        [StringLength(100)]
        [RequiredWhen(nameof(SystemDeliveryEntryType), ComparisonType.EqualTo, MapCall.Common.Model.Entities.SystemDeliveryEntryType.Indices.PURCHASED_WATER, FieldOnlyVisibleWhenRequired = true)]
        [View("Supplier")]
        public string PurchaseSupplier { get; set; }
        [Required, RegularExpression("^\\d{6}$", ErrorMessage = "Business unit must be 6 digits long")]
        public int? BusinessUnit { get; set; }
        
        [Required]
        public bool? IsInjectionSite { get; set; }
        
        [Required]
        public bool? IsAutomationEnabled { get; set; }

        #endregion
        
        #region Exposed Methods
        
        public override void Map(FacilitySystemDeliveryEntryType entity)
        {
            base.Map(entity);
            SystemDeliveryType = entity.SystemDeliveryEntryType.SystemDeliveryType.Id;
            Facility = entity.Facility.Id;
            OperatingCenter = entity.SupplierFacility?.OperatingCenter?.Id;
        }

        #endregion
    }
}
