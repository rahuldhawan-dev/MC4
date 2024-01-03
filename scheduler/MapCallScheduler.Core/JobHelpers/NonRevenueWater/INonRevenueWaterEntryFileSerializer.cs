using System.Linq;
using MapCall.Common.Model.ViewModels;

namespace MapCallScheduler.JobHelpers.NonRevenueWater
{
    public interface INonRevenueWaterEntryFileSerializer
    {
        #region Abstract Methods

        string Serialize(IQueryable<NonRevenueWaterEntryFileDumpViewModel> viewModels);

        #endregion
    }
}
