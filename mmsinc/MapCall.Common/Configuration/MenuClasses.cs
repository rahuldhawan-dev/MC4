using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility;
using StructureMap;

namespace MapCall.Common.Configuration
{
    public class Menu
    {
        #region Properties

        public IList<MenuGroup> Groups { get; private set; }

        #endregion

        #region Constructors

        public Menu()
        {
            Groups = new List<MenuGroup>();
        }

        #endregion

        #region Exposed Methods

        public MenuGroup Group(string title, string url = null, bool mustBeSiteAdmin = false,
            bool mustBeAdminInRole = false, RoleModules? role = null, bool isTextOnlyLink = false)
        {
            var group = new MenuGroup(this, title, url, mustBeSiteAdmin, mustBeAdminInRole, role, isTextOnlyLink);
            Groups.Add(group);
            return group;
        }

        #endregion
    }

    public class MenuGroup : IMenuPart, IMenuGroupOrSection
    {
        public static explicit operator Menu(MenuGroup group)
        {
            return group.ParentMenu;
        }

        #region Properties

        public Menu ParentMenu { get; private set; }
        public IList<IMenuPart> Parts { get; private set; }
        public string Title { get; private set; }
        public string Url { get; private set; }
        public bool MustBeSiteAdmin { get; private set; }
        public bool MustBeAdminInRole { get; private set; }
        public RoleModules? Role { get; private set; }
        public bool IsTextOnlyLink { get; private set; }
        public MenuSection CurrentSection { get; private set; }

        #endregion

        #region Constructors

        public MenuGroup(Menu parentMenu, string title, string url = null, bool mustBeSiteAdmin = false,
            bool mustBeAdminInRole = false, RoleModules? role = null, bool isTextOnlyLink = false)
        {
            ParentMenu = parentMenu;
            Title = title;
            Url = url;
            MustBeSiteAdmin = mustBeSiteAdmin;
            MustBeAdminInRole = mustBeAdminInRole;
            Role = role;
            IsTextOnlyLink = isTextOnlyLink;
            Parts = new List<IMenuPart>();
        }

        #endregion

        #region Exposed Methods

        public MenuGroup Group(string title, string url = null, bool mustBeSiteAdmin = false,
            bool mustBeAdminInRole = false, RoleModules? role = null, bool isTextOnlyLink = false)
        {
            return ParentMenu.Group(title, url, mustBeSiteAdmin, mustBeAdminInRole, role, isTextOnlyLink);
        }

        public IMenuGroupOrSection Link(string title, string url, RoleModules? role = null,
            bool mustBeAdminInRole = false, bool openInNewWindow = false, bool mustBeSiteAdmin = false)
        {
            Parts.Add(new MenuLink(title, url, role, openInNewWindow: openInNewWindow, mustBeSiteAdmin: mustBeSiteAdmin,
                mustBeAdminInRole: mustBeAdminInRole));
            return this;
        }

        public IMenuGroupOrSection Link(string title, string action, string controller, string area = "",
            RoleModules? role = null, bool mustBeAdminInRole = false, bool openInNewWindow = false,
            bool mustBeSiteAdmin = false)
        {
            var url = DependencyResolver.Current.GetService<IUrlHelper>().Action(action, controller, new {area});
            Parts.Add(new MenuLink(title, url, role, openInNewWindow: openInNewWindow, mustBeSiteAdmin: mustBeSiteAdmin,
                mustBeAdminInRole: mustBeAdminInRole) {
                Controller = controller,
                Area = area
            });
            return this;
        }

        public IMenuGroupOrSection BeginSection(string title, RoleModules? role = null, bool mustBeAdminInRole = false,
            bool mustBeSiteAdmin = false)
        {
            CurrentSection = new MenuSection(this, title, role: role, mustBeSiteAdmin: mustBeSiteAdmin,
                mustBeAdminInRole: mustBeAdminInRole);
            Parts.Add(CurrentSection);
            return CurrentSection;
        }

        public IMenuGroupOrSection EndSection()
        {
            CurrentSection = null;
            return this;
        }

        #endregion
    }

    public class MenuSection : IMenuPart, IMenuGroupOrSection
    {
        #region Properties

        public MenuGroup ParentGroup { get; private set; }
        public MenuSection ParentSection { get; private set; }
        public string Title { get; private set; }
        public bool MustBeSiteAdmin { get; private set; }
        public bool MustBeAdminInRole { get; private set; }
        public RoleModules? Role { get; private set; }
        public IList<IMenuPart> Parts { get; private set; }

        #endregion

        #region Constructors

        private MenuSection(string title, bool mustBeSiteAdmin = false, bool mustBeAdminInRole = false,
            RoleModules? role = null)
        {
            Title = title;
            Role = role;
            MustBeSiteAdmin = mustBeSiteAdmin;
            MustBeAdminInRole = mustBeAdminInRole;
            Parts = new List<IMenuPart>();
        }

        public MenuSection(MenuGroup parentGroup, string title, bool mustBeSiteAdmin = false,
            bool mustBeAdminInRole = false, RoleModules? role = null) : this(title, mustBeSiteAdmin, mustBeAdminInRole,
            role)
        {
            ParentGroup = parentGroup;
        }

        public MenuSection(MenuSection parentSection, string title, bool mustBeSiteAdmin = false,
            bool mustBeAdminInRole = false, RoleModules? role = null) : this(title, mustBeSiteAdmin, mustBeAdminInRole,
            role)
        {
            ParentSection = parentSection;
        }

        #endregion

        #region Exposed Methods

        public IMenuGroupOrSection Link(string title, string url, RoleModules? role = null,
            bool mustBeAdminInRole = false, bool openInNewWindow = false, bool mustBeSiteAdmin = false)
        {
            Parts.Add(new MenuLink(title, url, role, mustBeAdminInRole, openInNewWindow,
                mustBeSiteAdmin: mustBeSiteAdmin));
            return this;
        }

        public IMenuGroupOrSection Link(string title, string action, string controller, string area = "",
            RoleModules? role = null, bool mustBeAdminInRole = false, bool openInNewWindow = false,
            bool mustBeSiteAdmin = false)
        {
            var url = DependencyResolver.Current.GetService<IUrlHelper>().Action(action, controller, new {area});

            Parts.Add(new MenuLink(title, url, role, mustBeAdminInRole, openInNewWindow,
                mustBeSiteAdmin: mustBeSiteAdmin) {
                Controller = controller,
                Area = area
            });
            return this;
            //return Link(title, url, role, mustBeAdminInRole, openInNewWindow, mustBeSiteAdmin: mustBeSiteAdmin);
        }

        public IMenuGroupOrSection BeginSection(string title, RoleModules? role = null, bool mustBeAdminInRole = false,
            bool mustBeSiteAdmin = false)
        {
            var section = new MenuSection(this, title, role: role, mustBeSiteAdmin: mustBeSiteAdmin,
                mustBeAdminInRole: mustBeAdminInRole);
            Parts.Add(section);
            return section;
        }

        public IMenuGroupOrSection EndSection()
        {
            return ParentSection ?? (ParentGroup as IMenuGroupOrSection);
        }

        public MenuGroup Group(string title, string url = null, bool mustBeSiteAdmin = false,
            bool mustBeAdminInRole = false, RoleModules? role = null, bool isTextOnlyLink = false)
        {
            throw new InvalidOperationException(
                "Cannot start new group with section still open.  Call .EndSection() first.");
        }

        #endregion
    }

    public class MenuLink : IMenuPart
    {
        #region Properties

        public string Title { get; private set; }
        public string Url { get; private set; }
        public bool MustBeSiteAdmin { get; private set; }
        public bool MustBeAdminInRole { get; private set; }
        public bool OpenInNewWindow { get; private set; }
        public bool OnlyRenderAnchorTag { get; private set; }
        public RoleModules? Role { get; private set; }
        public string Controller { get; set; }
        public string Area { get; set; }

        #endregion

        #region Constructors

        public MenuLink(string title, string url, RoleModules? role = null, bool mustBeAdminInRole = false,
            bool openInNewWindow = false, bool mustBeSiteAdmin = false)
        {
            Title = title;
            Url = url;
            Role = role;
            MustBeAdminInRole = mustBeAdminInRole;
            OpenInNewWindow = openInNewWindow;
            MustBeSiteAdmin = mustBeSiteAdmin;
        }

        #endregion
    }

    public interface IMenuPart
    {
        #region Abstract Properties

        bool MustBeSiteAdmin { get; }
        bool MustBeAdminInRole { get; }
        RoleModules? Role { get; }

        #endregion
    }

    public interface IMenuGroupOrSection
    {
        IMenuGroupOrSection BeginSection(string title, RoleModules? role = null, bool mustBeAdminInRole = false,
            bool mustBeSiteAdmin = false);

        IMenuGroupOrSection EndSection();

        IMenuGroupOrSection Link(string title, string url, RoleModules? role = null, bool mustBeAdminInRole = false,
            bool openInNewWindow = false, bool mustBeSiteAdmin = false);

        IMenuGroupOrSection Link(string title, string action, string controller, string area = "",
            RoleModules? role = null, bool mustBeAdminInRole = false, bool openInNewWindow = false,
            bool mustBeSiteAdmin = false);

        MenuGroup Group(string title, string url = null, bool mustBeSiteAdmin = false, bool mustBeAdminInRole = false,
            RoleModules? role = null, bool isTextOnlyLink = false);
    }
}
