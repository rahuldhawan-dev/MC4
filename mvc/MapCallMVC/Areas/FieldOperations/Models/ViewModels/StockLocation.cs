using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class StockLocationViewModel : ViewModel<StockLocation>
    {
        #region Properties

        [Required, StringLength(StockLocation.StringLengths.DESCRIPTION)]
        public string Description { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [StringLength(StockLocation.StringLengths.SAP_STOCK_LOCATION)]
        public string SAPStockLocation { get; set; }

        public bool IsActive { get; set; }

        #endregion

        #region Constructors

        public StockLocationViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchStockLocation : SearchSet<StockLocation>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        public string Description { get; set; }

        #endregion
    }
}