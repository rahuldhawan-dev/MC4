using MMSINC.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.ViewModels
{
    public class SewerOpeningInspectionSearchResultViewModel
    {
        #region Properties

        public int Id { get; set; }
        public int SewerOpeningId { get; set; }
        public string OperatingCenter { get; set; }
        public string Town { get; set; }
        public string FunctionalLocation { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime DateInspected { get; set; }
        public decimal AmountOfDebrisGritCubicFeet { get; set; }
        public string Remarks { get; set; }
        public string InspectedBy { get; set; }
        public string EmployeeId { get; set; }
        public DateTime DateAdded { get; set; }
        public string OpeningNumber { get; set; }
        public int OpeningSuffix { get; set; }
        public int? Route { get; set; }

        #endregion
    }

    public interface ISearchSewerOpeningInspection : ISearchSet<SewerOpeningInspectionSearchResultViewModel>
    {
        [SearchAlias("sm.OperatingCenter", "opc", "Id")]
        int? OperatingCenter { get; set; }

        [SearchAlias("sm.Town", "town", "Id")]
        int? Town { get; set; }

        [SearchAlias("SewerOpening", "sm", "OpeningSuffix")]
        int? OpeningSuffix { get; set; }

        [SearchAlias("InspectedBy", "inspectedBy", "Id")]
        int? InspectedBy { get; set; }

        [SearchAlias("SewerOpening", "sm", "Route")]
        int? Route { get; set; }
    }
}
