using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Operations.Models.ViewModels
{
    public class SearchAuthenticationLog : SearchSet<AuthenticationLog>, ISearchAuthenticationLog
    {
        #region Properties

        [DropDown]
        public int? OperatingCenter { get; set; }

        [DropDown("", "User", "GetAllByOperatingCenterId", DependsOn="OperatingCenter")]
        public int? User { get; set; }

        public DateRange LoggedInAt { get; set; }

        #endregion
    }
}