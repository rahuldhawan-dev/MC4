using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public interface ISearchAuthenticationLog : ISearchSet<AuthenticationLog>
    {
        [SearchAlias("User", "user", "DefaultOperatingCenter.Id")]
        int? OperatingCenter { get; set; }

        int? User { get; set; }

        DateRange LoggedInAt { get; set; }
    }
}
