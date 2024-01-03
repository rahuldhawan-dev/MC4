using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Repositories
{
    public interface INonRevenueWaterEntryRepository : IRepository<NonRevenueWaterEntry>
    {
        IQueryable<NonRevenueWaterEntryFileDumpViewModel> GetDataForNonRevenueWaterEntryFileDump(DateTime startDate);
    }
}
