using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility;
using MMSINC.Authentication;

namespace MapCall.Common.Helpers
{
    public class MvcMenuRenderHelper : MenuRenderHelper
    {
        #region Constructors

        public MvcMenuRenderHelper(StringOrTextWriter writer, IAuthenticationService<User> authServ,
            IRoleService roleServ) : base(writer, authServ, roleServ) { }

        #endregion

        #region Exposed Methods

        public override void RenderGroup(MenuGroup group)
        {
            if (!CanRender(group))
            {
                return;
            }

            if (group.Url != null)
            {
                // Note: Link-only menu groups can not have children.
                Writer.Write("<div class=\"linkOnly\">");
                Writer.Write("<div class=\"title ui-state-default\"><a href=\"{0}\">{1}</a></div>", group.Url,
                    group.Title);
                Writer.Write("</div>");
            }
            else
            {
                Writer.Write("<div>");
                Writer.Write("<div class=\"title ui-state-default\"><a>{0}</a></div>", group.Title);
                Writer.Write("<div class=\"content\">");
                Writer.Write("<ul>");

                foreach (var part in group.Parts)
                {
                    Render(part);
                }

                Writer.Write("</ul></div></div>");
            }
        }

        public override void RenderSection(MenuSection section)
        {
            Writer.Write("<li>");
            Writer.Write("<span>{0}</span>", section.Title);
            Writer.Write("<ul>");

            foreach (var part in section.Parts)
            {
                Render(part);
            }

            Writer.Write("</ul></li>");
        }

        public override void RenderLink(MenuLink link)
        {
            if (!link.OnlyRenderAnchorTag)
            {
                Writer.Write("<li><span class=\"item\">");
            }

            if (link.OpenInNewWindow)
            {
                Writer.Write("<a href=\"{0}\" target=\"_blank\" data-area=\"{2}\" data-controller=\"{3}\">{1}</a>",
                    link.Url, link.Title, link.Area, link.Controller);
            }
            else
            {
                Writer.Write("<a href=\"{0}\" data-area=\"{2}\" data-controller=\"{3}\">{1}</a>", link.Url, link.Title,
                    link.Area, link.Controller);
            }

            if (!link.OnlyRenderAnchorTag)
            {
                Writer.Write("</span></li>");
            }
        }

        #endregion
    }
}
