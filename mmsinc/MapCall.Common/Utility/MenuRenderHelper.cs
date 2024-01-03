using System;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using StructureMap;

namespace MapCall.Common.Utility
{
    public abstract class MenuRenderHelper
    {
        #region Properties

        public IAuthenticationService<User> AuthenticationService { get; set; }
        public IBasicRoleService RoleService { get; set; }
        public StringOrTextWriter Writer { get; set; }

        #endregion

        #region Constructors

        public MenuRenderHelper(StringOrTextWriter writer, IAuthenticationService<User> authServ,
            IBasicRoleService roleServ)
        {
            Writer = writer;
            AuthenticationService = authServ;
            RoleService = roleServ;
        }

        #endregion

        #region Abstract Methods

        public abstract void RenderGroup(MenuGroup menuGroup);
        public abstract void RenderLink(MenuLink menuLink);
        public abstract void RenderSection(MenuSection menuSection);

        #endregion

        #region Exposed Methods

        public virtual void RenderMenu(Menu menu)
        {
            foreach (var group in menu.Groups)
            {
                RenderGroup(group);
            }
        }

        public void Render(IMenuPart part)
        {
            if (!CanRender(part))
            {
                return;
            }

            if (part is MenuSection)
            {
                RenderSection(part as MenuSection);
            }
            else if (part is MenuLink)
            {
                RenderLink(part as MenuLink);
            }
        }

        public bool CanRender(IMenuPart menuPart)
        {
            if (menuPart.MustBeSiteAdmin)
            {
                // Don't care about roles if it requires a SiteAdmin.
                return AuthenticationService.CurrentUserIsAdmin;
            }

            if (!menuPart.Role.HasValue)
            {
                return true;
            }

            var roleAction = (menuPart.MustBeAdminInRole ? RoleActions.UserAdministrator : RoleActions.Read);
            return RoleService.CanAccessRole(menuPart.Role.Value, roleAction);
        }

        public static void Render<THelper>(Menu menu, StringOrTextWriter sw) where THelper : MenuRenderHelper
        {
            var authServ = DependencyResolver.Current.GetService<IAuthenticationService<User>>();
            var roleServ = DependencyResolver.Current.GetService<IBasicRoleService>();
            var helper = (MenuRenderHelper)Activator.CreateInstance(typeof(THelper), sw, authServ, roleServ);

            helper.RenderMenu(menu);

            sw.Flush();
        }

        #endregion
    }
}
