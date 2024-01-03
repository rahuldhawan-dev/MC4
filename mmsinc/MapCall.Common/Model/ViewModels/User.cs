using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public interface ISearchUserTracking : ISearchSet<User>
    {
        int? DefaultOperatingCenter { get; set; }

        [Search(CanMap = false)] // Needs to be manually done in repo
        int? User { get; set; }

        DateRange LastLoggedInAt { get; set; }

        [Search(CanMap = false)] // Needs to be manually done in repo
        DateRange NotLoggedInAt { get; set; }
    }
}
