using System.Collections.Generic;
using Contractors.Data.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories
{
    public interface IMeterChangeOutRepository : IRepository<MeterChangeOut>
    {
        bool IsNewSerialNumberUnique(int meterChangeOutId, string newSerialNumber);
        IEnumerable<MeterChangeOut> GetScheduledReport();
        IEnumerable<MeterChangeOutCompletionReportItem> GetCompletionsReport(ISearchMeterChangeOutCompletions search);
    }
}