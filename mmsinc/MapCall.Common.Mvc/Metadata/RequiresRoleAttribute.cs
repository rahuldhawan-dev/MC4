using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Metadata
{
    /// <summary>
    /// Attribute used in conjunction with the RoleAuthorizeFilter. 
    /// </summary>
    /// <remarks>
    /// 
    /// Do not use this at class level anymore. Class-level usage was leading to
    /// everyone using AllowAnonymous to incorrectly bypass the class-level role requirement rather
    /// than setting up the controller actions correctly. -Ross 2/14/2018
    /// 
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequiresRoleAttribute : FilterAttribute
    {
        #region Properties

        public RoleModules Module { get; }
        public RoleActions Action { get; }

        #endregion

        #region Constructors

        public RequiresRoleAttribute(RoleModules module, RoleActions action)
        {
            Module = module;
            Action = action;
        }

        public RequiresRoleAttribute(RoleModules module) : this(module, RoleActions.Read) { }

        #endregion

        #region Exposed Methods

        public Module GetActualModule(IRepository<Module> moduleRepository)
        {
            return moduleRepository.Find((int)Module);
        }

        #endregion
    }
}
