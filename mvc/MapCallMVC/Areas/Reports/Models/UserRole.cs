using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchUserRole : SearchSet<Role>
    {
        #region Properties

        [DropDown]
        public int? OperatingCenter { get; set; }

        [DropDown, Required]
        public int? Module { get; set; }

        [DropDown, DisplayName("Action")]
        public int? RoleAction { get; set; }

        public bool? UserHasAccess { get; set; }

        #endregion

    }
}