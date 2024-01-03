using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Areas.Admin.Models.ViewModels.RoleGroups
{
    /// <summary>
    /// This view model is only to be used as a child of RoleGroupViewModel.
    /// </summary>
    public class CreateRoleGroupRole : ViewModel<RoleGroupRole>
    {
        #region Properties

        // NOTE: There's no RoleGroup property here as this view model should only
        // ever be used as a child property of RoleGroupViewModel.

        [Required]
        public int? Action { get; set; }

        [Required]
        public int? Module { get; set; }
        public int? OperatingCenter { get; set; }

        #endregion

        #region Constructor

        public CreateRoleGroupRole(IContainer container) : base(container) { }

        #endregion
    }
}