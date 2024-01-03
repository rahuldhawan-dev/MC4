using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class AddFacilitySystemDeliveryEntryType : ViewModel<Facility>
    {
        #region Properties

        [RequiredWhen("IsEnabled", ComparisonType.EqualTo, true), DoesNotAutoMap()]
        public decimal? MinimumValue { get; set; }
        [RequiredWhen("IsEnabled", ComparisonType.EqualTo, true), DoesNotAutoMap()]
        public decimal? MaximumValue { get; set; }
        [Required, DoesNotAutoMap()] 
        public bool? IsEnabled { get; set; }
        [EntityMap(MapDirections.ToPrimary)]
        public int SystemDeliveryType { get; set; }
        [Required, DropDown("Production", "SystemDeliveryEntryType", "BySystemDeliveryTypeId", DependsOn = "SystemDeliveryType"), DoesNotAutoMap(), EntityMustExist(typeof(SystemDeliveryEntryType)) ] 
        public int? SystemDeliveryEntryType { get; set; }
        [DoesNotAutoMap(), DropDown, EntityMustExist(typeof(OperatingCenter))] 
        [RequiredWhen(nameof(SystemDeliveryEntryType), ComparisonType.EqualToAny, new[]{MapCall.Common.Model.Entities.SystemDeliveryEntryType.Indices.TRANSFERRED_FROM, MapCall.Common.Model.Entities.SystemDeliveryEntryType.Indices.TRANSFERRED_TO}, FieldOnlyVisibleWhenRequired = true)]
        public int? OperatingCenter { get; set; }
        [DoesNotAutoMap(), DropDown("","Facility", "GetActiveByOperatingCenterWithPointOfEntry", DependsOn = nameof(OperatingCenter), PromptText = "Please select an operating center above"), EntityMustExist(typeof(Facility))]
        [RequiredWhen(nameof(SystemDeliveryEntryType), ComparisonType.EqualToAny, new[]{MapCall.Common.Model.Entities.SystemDeliveryEntryType.Indices.TRANSFERRED_FROM, MapCall.Common.Model.Entities.SystemDeliveryEntryType.Indices.TRANSFERRED_TO}, FieldOnlyVisibleWhenRequired = true)]
        public int? SupplierFacility { get; set; }
        [DoesNotAutoMap, StringLength(100)]
        [RequiredWhen(nameof(SystemDeliveryEntryType), ComparisonType.EqualTo, MapCall.Common.Model.Entities.SystemDeliveryEntryType.Indices.PURCHASED_WATER, FieldOnlyVisibleWhenRequired = true)]
        [View("Supplier")]
        public string PurchaseSupplier { get; set; }
        [DoesNotAutoMap, Required, RegularExpression("^\\d{6}$", ErrorMessage = "Business unit must be 6 digits long.")]
        public int? BusinessUnit { get; set; }
        
        [Required, DoesNotAutoMap]
        public bool? IsInjectionSite { get; set; } = false;
        
        [Required, DoesNotAutoMap]
        public bool? IsAutomationEnabled { get; set; } = false;

        #endregion

        #region Constructors

        public AddFacilitySystemDeliveryEntryType(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override Facility MapToEntity(Facility entity)
        {
            var entry = new FacilitySystemDeliveryEntryType {
                Facility = entity,
                MinimumValue = MinimumValue,
                MaximumValue = MaximumValue,
                IsEnabled = IsEnabled == true,
                PurchaseSupplier = PurchaseSupplier,
                SystemDeliveryEntryType = _container.GetInstance<IRepository<SystemDeliveryEntryType>>().Find(SystemDeliveryEntryType.Value),
                BusinessUnit = BusinessUnit,
                IsInjectionSite = IsInjectionSite == true,
                IsAutomationEnabled = IsAutomationEnabled == true
            };

            if (SupplierFacility != null && (SystemDeliveryEntryType == MapCall.Common.Model.Entities.SystemDeliveryEntryType.Indices.TRANSFERRED_TO || SystemDeliveryEntryType == MapCall.Common.Model.Entities.SystemDeliveryEntryType.Indices.TRANSFERRED_FROM))
            {
                entry.SupplierFacility = _container.GetInstance<IFacilityRepository>().Find(SupplierFacility.Value);
            }

            entity.FacilitySystemDeliveryEntryTypes.Add(entry);

            return entity;
        }

        #endregion
    }
}
