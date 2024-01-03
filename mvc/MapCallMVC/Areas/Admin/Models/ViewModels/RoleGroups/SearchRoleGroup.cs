using MMSINC.Data;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Areas.Admin.Models.ViewModels.RoleGroups
{
    public class SearchRoleGroup : SearchSet<RoleGroup>
    {
        #region Properties

        public SearchString Name { get; set; }

        #endregion
    }
}