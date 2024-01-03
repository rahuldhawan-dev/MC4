using MapCall.Common.Model.Entities;

namespace MapCall.Common.Controllers
{
    /// <summary>
    /// Interface for use with DynamicRequiresRoleAttribute
    /// </summary>
    public interface IControllerOfRoles
    {
        /// <summary>
        /// Returns the RoleModules value needed for a DynamicRequiresRoleAttribute
        /// placed on any action method in the controller.
        /// </summary>
        RoleModules GetDynamicRoleModuleForAction(string action);
    }
}