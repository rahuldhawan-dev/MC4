using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ical.Net.DataTypes;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Customer.Models.ViewModels
{
    public class SearchPremiseFind : SearchSet<Premise>
    {
        [DisplayName("Id")]
        public int? EntityId { get; set; }

        public SearchString PremiseNumber { get; set; }
        public SearchString DeviceSerialNumber { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter",
             PromptText = "Select an Operating Center above.")]
        [EntityMap, EntityMustExist(typeof(Town))]
        public int? ServiceCity { get; set; }

        [View(DisplayName = "House #")]
        public SearchString ServiceAddressHouseNumber { get; set; }

        [View(DisplayName = "Street")]
        public SearchString ServiceAddressStreet { get; set; }

        public string ServiceDistrict { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(PremiseAreaCode))]
        public int? AreaCode { get; set; }

        [MultiSelect]
        [View(DisplayName = "City"), EntityMap, EntityMustExist(typeof(RegionCode))]
        public int[] RegionCode { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MeterReadingRoute))]
        public int? RouteNumber { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(ServiceUtilityType))]
        [View(DisplayName = "Installation Type")]
        public int[] ServiceUtilityType { get; set; }

        public string MeterSize { get; set; }
        //public string AccountName { get; set; }
        public SearchString FullStreetAddress { get; set; }
        public SearchString MeterSerialNumber { get; set; }
        public SearchString MeterLocationFreeText { get; set; }
    }
}