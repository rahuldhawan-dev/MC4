using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Metadata
{
    /// <summary>
    /// Attribute used in conjunction with the RoleAuthorizeFilter. This is used with controllers
    /// that implement IControllerOfRoles. 
    /// </summary>
    /// <remarks>
    /// 
    /// Just like RequiresRoleAttribute, do not use this at the class level.
    /// 
    /// </remarks>
    /// 
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class DynamicRequiresRoleAttribute : FilterAttribute
    {
        #region Properties

        public RoleActions Action { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a role requirement with a specific action. The module is set by the controller
        /// that implements IControllerOfRoles.
        /// </summary>
        /// <param name="action"></param>
        public DynamicRequiresRoleAttribute(RoleActions action)
        {
            Action = action;
        }

        #endregion
    }
}