using MMSINC.Data;
using System.ComponentModel.DataAnnotations;

namespace MapCall.Common.Model.ViewModels
{
    public class SewerMainCleaningFootageReportItem
    {
        #region Properties

        public virtual string OperatingCenter { get; set; }

        public virtual string Town { get; set; }

        public virtual string InspectionType { get; set; }

        public virtual int? Month { get; set; }

        public virtual int? Year { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public float Total { get; set; }

        #endregion
    }

    public class SewerMainCleaningFootageReport
    {
        public virtual string OperatingCenter { get; set; }

        public virtual string Town { get; set; }

        public virtual string InspectionType { get; set; }

        public virtual int? Year { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public virtual float Jan { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public virtual float Feb { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public virtual float Mar { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public virtual float Apr { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public virtual float May { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public virtual float Jun { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public virtual float Jul { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public virtual float Aug { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public virtual float Sep { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public virtual float Oct { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public virtual float Nov { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public virtual float Dec { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public virtual float Total { get; set; }
    }

    public interface ISearchSewerMainCleaningFootageReport : ISearchSet<SewerMainCleaningFootageReportItem>
    {
        #region Properties

        [SearchAlias("OperatingCenter", "opc", "Id")]
        int? OperatingCenter { get; set; }

        [SearchAlias("Town", "town", "Id")]
        int? Town { get; set; }

        [SearchAlias("InspectionType", "type", "Id")]
        int? InspectionType { get; set; }

        [Search(CanMap = false)]
        int? Year { get; set; }

        [Search(CanMap = false)]
        int? Month { get; set; }

        #endregion
    }
}