using System;
using System.ComponentModel;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Customer.Models.ViewModels
{
    public class SearchPremise : SearchPremiseBase<Premise>
    {
        public static implicit operator SearchPremiseForMap(SearchPremise search)
            => new SearchPremiseForMap {
                AreaCode = search.AreaCode,
                CriticalCareType = search.CriticalCareType,
                DeviceSerialNumber = search.DeviceSerialNumber,
                EntityId = search.EntityId,
                Equipment = search.Equipment,
                FullStreetAddress = search.FullStreetAddress,
                HasMeter = search.HasMeter,
                IsMajorAccount = search.IsMajorAccount,
                MeterLocationFreeText = search.MeterLocationFreeText,
                MeterSerialNumber = search.MeterSerialNumber,
                MeterSize = search.MeterSize,
                OperatingCenter = search.OperatingCenter,
                PremiseNumber = search.PremiseNumber,
                PremiseType = search.PremiseType,
                PublicWaterSupply = search.PublicWaterSupply,
                RegionCode = search.RegionCode,
                RouteNumber = search.RouteNumber,
                ServiceAddressApartment = search.ServiceAddressApartment,
                ServiceAddressHouseNumber = search.ServiceAddressHouseNumber,
                ServiceAddressStreet = search.ServiceAddressStreet,
                ServiceCity = search.ServiceCity,
                ServiceDistrict = search.ServiceDistrict,
                ServiceUtilityType = search.ServiceUtilityType,
                State = search.State,
                StatusCode = search.StatusCode,
            };
    }

    public class SearchPremiseForMap : SearchPremiseBase<PremiseCoordinate>, ISearchPremiseForMap
    {
        #region Constants

        public const int MAX_MAP_RESULT_COUNT = 10000; 
        
        #endregion
        
        #region Properties

        /// <remarks>
        /// Returns false and is not settable, because map coordinates shouldn't be paged.
        /// </remarks>
        public override bool EnablePaging
        {
            get => false;
            set { }
        }

        #endregion
    }

    public abstract class SearchPremiseBase<TSearchSet> : SearchSet<TSearchSet>
    {
        [DisplayName("Id")]
        public int? EntityId { get; set; }

        public SearchString PremiseNumber { get; set; }
        public SearchString DeviceSerialNumber { get; set; }

        [DropDown("", "OperatingCenter", "ByStateIdForFieldServicesWorkManagement", DependsOn = "State", DependentsRequired = DependentRequirement.None), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [View(DisplayName = "Status")]
        [DropDown, EntityMap, EntityMustExist(typeof(PremiseStatusCode))]
        public int? StatusCode { get; set; }

        public bool? IsMajorAccount { get; set; }

        [View(DisplayName = "CriticalCareType")]
        [DropDown, EntityMap, EntityMustExist(typeof(PremiseCriticalCareType))]
        public int? CriticalCareType { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Select an Operating Center above.")]
        [EntityMap, EntityMustExist(typeof(Town))]
        public int? ServiceCity { get; set; }

        [View(DisplayName = "House #")]
        public SearchString ServiceAddressHouseNumber { get; set; }
        [View("Apartment Addtl")]
        public SearchString ServiceAddressApartment { get; set; }

        [View(DisplayName = "Street")]
        public SearchString ServiceAddressStreet { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(PremiseDistrict))]
        public int? ServiceDistrict { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(PremiseAreaCode))]
        public int? AreaCode { get; set; }

        [MultiSelect("Customer", "Premise", "RegionCodesByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Select an Operating Center above.")]
        [View(DisplayName = "City"), EntityMap, EntityMustExist(typeof(RegionCode))]
        public int[] RegionCode { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MeterReadingRoute))]
        public int? RouteNumber { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(ServiceUtilityType))]
        [View(DisplayName = "Installation Type")]
        public int[] ServiceUtilityType { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int[] MeterSize { get; set; }
        //public SearchString Owner { get; set; }
        public SearchString FullStreetAddress { get; set; }
        public SearchString MeterSerialNumber { get; set; }
        public SearchString MeterLocationFreeText { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("OperatingCenter", "State.Id")]
        public int? State { get; set; }
        public SearchString Equipment { get; set; }
        [Search(CanMap = false)]
        public bool? HasMeter { get; set; }
        [DropDown("", nameof(PublicWaterSupply), "ByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        public int? PublicWaterSupply { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(PremiseType))]
        public int? PremiseType { get; set; }

        #region Public Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);
            if (HasMeter.HasValue)
            {
                if (HasMeter.Value)
                {
                    mapper.MappedProperties["Equipment"].Value = SearchMapperSpecialValues.IsNotNullOrEmpty;
                    mapper.MappedProperties["MeterSerialNumber"].Value = SearchMapperSpecialValues.IsNotNullOrEmpty;
                }
                else
                {
                    mapper.MappedProperties["Equipment"].Value = SearchMapperSpecialValues.IsNullOrEmpty;
                    mapper.MappedProperties["MeterSerialNumber"].Value = SearchMapperSpecialValues.IsNullOrEmpty;
                }
            }
        }

        #endregion
    }
}