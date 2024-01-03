using System;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public enum InspectionProductivityWeekSpan
    {
        OneWeek,
        TwoWeeks
    }

    public interface ISearchInspectionProductivity : ISearchSet<InspectionProductivityReportItem>
    {
        // Required
        [Search(CanMap = false)]
        DateTime? StartDate { get; set; }

        [Search(CanMap = false)]
        InspectionProductivityWeekSpan? Week { get; set; }

        [SearchAlias("asset.OperatingCenter", "opc", "Id")]
        int? OperatingCenter { get; set; }

        [SearchAlias("InspectedBy", "inspectedBy", "Id")]
        int? InspectedBy { get; set; }

        int GetDays();
    }

    public class InspectionProductivityReportItem
    {
        #region Fields

        private string _inspectionType;

        #endregion

        public string OperatingCenter { get; set; }
        public string InspectedBy { get; set; }
        public string AssetType { get; set; }

        public string InspectionType
        {
            get
            {
                // This is to make life easier when doing all the crazy grouping going on
                // in the Index view for this report. ValveInspections don't have an inspection type,
                // they just get a yes/no for some reason. Hydrants/BlowOffs should never
                // have ValveOperated.HasValue == true.
                if (ValveOperated.HasValue)
                {
                    return ValveOperated.Value ? "Yes" : "No";
                }

                return _inspectionType;
            }
            set { _inspectionType = value; }
        }

        public string ValveSize { get; set; }
        public bool? ValveOperated { get; set; }

        public DateTime DateInspected
        {
            get { return new DateTime(DateInspectedYear, DateInspectedMonth, DateInspectedDay); }
        }

        public int Count { get; set; }

        public int DateInspectedYear { get; set; }
        public int DateInspectedMonth { get; set; }
        public int DateInspectedDay { get; set; }
    }
}
