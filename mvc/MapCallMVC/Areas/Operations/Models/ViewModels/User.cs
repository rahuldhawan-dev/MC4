using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Operations.Models.ViewModels
{
    public class SearchUserTracking : SearchSet<User>, ISearchUserTracking
    {
        #region Properties

        [DropDown]
        public int? DefaultOperatingCenter { get; set; }

        [DropDown("", "User", "GetAllByOperatingCenterId", DependsOn = "DefaultOperatingCenter")]
        public int? User { get; set; }

        public DateRange LastLoggedInAt { get; set; }
        public DateRange NotLoggedInAt { get; set; }

        #endregion
    }
}