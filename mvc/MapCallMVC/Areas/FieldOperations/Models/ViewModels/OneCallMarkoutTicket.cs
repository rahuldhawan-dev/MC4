using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchOneCallMarkoutTicket : SearchSet<OneCallMarkoutTicket>
    {
        #region Properties

        public DateRange DateReceived { get; set; }
        public DateRange DateTransmitted { get; set; }
        [DropDown]
        public int? OperatingCenter { get; set; }
        [MultiSelect, View("CDC Code")]
        public List<string> CDCCode { get; set; }
        [DropDown]
        public int? MessageType { get; set; }
        public int? RequestNumber { get; set; }
        [View("County"), DropDown("FieldOperations", "OneCallMarkoutTicket", "GetCounties", DependsOn = "OperatingCenter")]
        public string CountyText { get; set; }
        [View("Town"), DropDown("FieldOperations", "OneCallMarkoutTicket", "GetTowns", DependsOn = "OperatingCenter,CountyText")]
        public string TownText { get; set; }
        [View("Street")]
        public string StreetText { get; set; }
        [View("Nearest Cross Street")]
        public string NearestCrossStreetText { get; set; }
        public bool? HasResponse { get; set; }
        public bool? ExcavatorIsAmericanWater { get; set; }

        public string Excavator { get; set; }

        #endregion
    }
}