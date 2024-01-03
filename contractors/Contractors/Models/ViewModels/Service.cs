using System.ComponentModel;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace Contractors.Models.ViewModels
{
    public class SearchService : SearchSet<Service>
    {
        #region Properties

        //Service Number:
        public long? ServiceNumber { get; set; }
        //Premise Number:
        public SearchString PremiseNumber { get; set; }
        //Operating Center:
        [DropDown]
        public int? OperatingCenter { get; set; }

        //Town:
        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }

        //Street Name:
        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? Street { get; set; }

        //Cross Street:
        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? CrossStreet { get; set; }

        //Town Section:
        [DropDown("", "TownSection", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? TownSection { get; set; }

        //Customer Name:
        [DisplayName("Customer Name")]
        public SearchString Name { get; set; }

        //Phone Number:
        public SearchString PhoneNumber { get; set; }
        //Street Number:
        public SearchString StreetNumber { get; set; }
        //Apt/Bldg:
        public SearchString ApartmentNumber { get; set; }
        //Development:
        public SearchString Development { get; set; }
        //Lot:
        public SearchString Lot { get; set; }
        //Block:
        public SearchString Block { get; set; }
        //Category of Service:
        [MultiSelect, DisplayName("Category of Service")]
        public int[] ServiceCategory { get; set; }

        //Service Size:\
        [DropDown, View(Service.DisplayNames.SERVICE_SIZE)]
        public int? ServiceSize { get; set; }

        //Installed Date:
        [DisplayName("Installed Date")]
        public DateRange DateInstalled { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial)), View(Service.DisplayNames.SERVICE_MATERIAL)]
        public int? ServiceMaterial { get; set; }

        #endregion
    }
}