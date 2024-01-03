using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Production.Models.ViewModels.SystemDeliveryEntries
{
    public class CreateSystemDeliveryFacilityEntry : SystemDeliveryFacilityEntryViewModel
    {
        public CreateSystemDeliveryFacilityEntry(IContainer container) : base(container) { }
    }

    public class EditSystemDeliveryFacilityEntry : SystemDeliveryFacilityEntryViewModel
    {
        public EditSystemDeliveryFacilityEntry(IContainer container) : base(container) { }

        public SystemDeliveryEntryType SystemDeliveryEntryType { get; set;}

        [DoesNotAutoMap]
        public virtual SystemDeliveryFacilityEntry DisplaySystemDeliveryFacilityEntry => _container.GetInstance<IRepository<SystemDeliveryFacilityEntry>>().Find(Id);
        
        public virtual bool IsBeingAdjusted { get; set; }

        public virtual decimal? AdjustedEntryValue { get; set; }

        public virtual string AdjustmentComment { get; set; }
    }

    public class SystemDeliveryFacilityEntryViewModel : ViewModel<SystemDeliveryFacilityEntry>
    {
        [DoesNotAutoMap]
        public int SystemDeliveryType { get; set; }

        [EntityMustExist(typeof(SystemDeliveryEntry))]
        public int? SystemDeliveryEntry { get; set; }

        [EntityMustExist(typeof(SystemDeliveryEntryType))]
        public int SystemDeliveryEntryType { get; set; }

        [DoesNotAutoMap]
        public string SystemDeliveryEntryTypeDesc { get; set; }

        [EntityMustExist(typeof(Employee))]
        public int? EnteredBy { get; set; }

        [DoesNotAutoMap]
        public int FacilityId { get; set; }

        [DoesNotAutoMap]
        public string FacilityName { get; set; }

        [DoesNotAutoMap]
        public string FacilityIdWithFacilityName { get; set; }

        [DoesNotAutoMap]
        public int? OperatingCenterId { get; set; }

        [DoesNotAutoMap]
        public string OperatingCenterDescription { get; set; }

        [DoesNotAutoMap]
        public int? SupplierFacility { get; set; }

        [DoesNotAutoMap]
        public string SupplierFacilityDesc { get; set; }

        [DoesNotAutoMap]
        public string PurchaseSupplier { get; set; }

        [View(FormatStyle.Date)]
        public DateTime EntryDate { get; set; }

        [Required]
        public decimal? EntryValue { get; set; }

        [CheckBox]
        public bool IsInjection { get; set; }

        [DoesNotAutoMap]
        public decimal? WeeklyTotal { get; set; }

        [DoesNotAutoMap]
        public decimal? MaxWeeklyTotal { get; set; }

        [DoesNotAutoMap]
        public bool SystemDeliveryEntryTypeIsInjectionSite { get; set; }

        #region Constructors

        public SystemDeliveryFacilityEntryViewModel(IContainer container) : base(container)
        {
            // We are setting entries to zero here. I'm sure you wondering why because system delivery is kinda confusing. 
            // Well they didn't want to allow null there and it's prepopulating zero because if they made a mistake and didn't select all the facilities they needed OR selected to many,
            // they can adjust save and it'll repull the correct meters for the entry w/o forcing them to enter values into entries that are just gonna be cleared out anyway
            EntryValue = decimal.Zero;
        }

        #endregion
    }
}
