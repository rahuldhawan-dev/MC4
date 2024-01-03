using MMSINC.Data;
using MapCall.Common.Model.Entities.Users;

namespace MapCall.Common.Model.ViewModels.Users
{
    public interface ISearchUser : ISearchSet<User>
    {
        [Search(CanMap = false)] // needs to be done manually in the repo
        string EmployeeId { get; set; }
    }
}
