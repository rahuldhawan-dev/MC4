using System;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace Contractors.Data.Models.ViewModels
{
    public class MeterChangeOutCompletionReportItem
    {
        #region Properties

        public ContractorMeterCrew ContractorMeterCrew { get; set; }
        [View(DisplayFormat = CommonStringFormats.DATE)]
        public DateTime CompletionDate { get; set; }
        public int Changed { get; set; }

        #endregion
    }

    public interface ISearchMeterChangeOutCompletions : ISearchSet<MeterChangeOutCompletionReportItem>
    {
        int? CalledInByContractorMeterCrew { get; set; }
        DateRange DateStatusChanged { get; set; }
        int[] MeterChangeOutStatus { get; set; }
    }
}
