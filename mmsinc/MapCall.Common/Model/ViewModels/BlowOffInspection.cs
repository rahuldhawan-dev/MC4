using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Utilities;

namespace MapCall.Common.Model.ViewModels
{
    public class BlowOffInspectionSearchResultViewModel
    {
        #region Properties

        public int Id { get; set; }
        public int ValveId { get; set; }
        public string ValveNumber { get; set; }

        public string OperatingCenter { get; set; }
        public string Town { get; set; }
        public string FunctionalLocation { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS)]
        public DateTime DateInspected { get; set; }

        [DisplayName("Inspection Type")]
        public string HydrantInspectionType { get; set; }

        public bool? FullFlow { get; set; }
        public int? GallonsFlowed { get; set; }
        public decimal? GPM { get; set; }
        public decimal? MinutesFlowed { get; set; }
        public decimal? StaticPressure { get; set; }

        public decimal? PreResidualChlorine { get; set; }

        public decimal? ResidualChlorine { get; set; }

        public decimal? PreTotalChlorine { get; set; }

        public decimal? TotalChlorine { get; set; }

        public string WorkOrderRequestOne { get; set; }
        public string Remarks { get; set; }
        public string InspectedBy { get; set; }
        public DateTime DateAdded { get; set; }
        public string SAPErrorCode { get; set; }
        public string FreeNoReadReason { get; set; }
        public string TotalNoReadReason { get; set; }

        #endregion
    }

    public interface ISearchBlowOffInspection : ISearchSet<BlowOffInspectionSearchResultViewModel>
    {
        #region Abstract Properties

        [SearchAlias("val.OperatingCenter", "opc", "Id")]
        int[] OperatingCenter { get; set; }

        [SearchAlias("val.Town", "town", "Id")]
        int? Town { get; set; }

        [SearchAlias("Valve", "val", "ValveSuffix")]
        int? ValveSuffix { get; set; }

        [SearchAlias("InspectedBy", "inspectedBy", "Id")]
        int? InspectedBy { get; set; }

        // TODO: DateRange props are missing. 

        [Search(CanMap = false)]
        bool? WorkOrderRequired { get; set; }

        // Needed for ModifyValues and so that the search mapper
        // has a property for this. Not part of view.
        [SearchAlias("WorkOrderRequestOne", "wor1", "Id")]
        int? WorkOrderRequestOne { get; set; }

        [SearchAlias("FreeNoReadReason", "freeNoRead", "Id")]
        int? FreeNoReadReason { get; set; }

        [SearchAlias("TotalNoReadReason", "freeNoRead", "Id")]
        int? TotalNoReadReason { get; set; }

        [SearchAlias("HydrantInspectionType", "hit", "Id")]
        int? HydrantInspectionType { get; set; }

        #endregion
    }
}
