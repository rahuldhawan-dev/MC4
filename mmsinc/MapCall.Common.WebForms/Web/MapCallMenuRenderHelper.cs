using System;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace MapCall.Common.Web
{
    public class MapCallMenuRenderHelper : MenuRenderHelper
    {
        #region Constructors

        public MapCallMenuRenderHelper(StringOrTextWriter writer, IAuthenticationService<User> authServ,
            IBasicRoleService roleServ) : base(writer, authServ, roleServ) { }

        #endregion

        #region Private Methods

        private void RenderChild(IMenuPart part)
        {
            if (CanRender(part))
            {
                Writer.WriteLine("<li>");
                Render(part);
                Writer.WriteLine("</li>");
            }
        }

        private IResourceConfiguration GetSiteConfiguration()
        {
            return MapCallHttpApplication.Instance.SiteConfiguration;
        }

        private string FormatUrl(string url)
        {
            if (String.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }

            if (url.StartsWith("http"))
            {
                // external links
                return url;
            }

            if (url.StartsWith("~"))
            {
                throw new InvalidOperationException("Menu links must be absolute. They can't start with an ~");
            }

            if (!url.StartsWith("/"))
            {
                throw new InvalidOperationException("Menu links must start with /");
            }

#if DEBUG
            // If not release/staging.
            switch (GetSiteConfiguration().Site)
            {
                case Site.MapCall:
                case Site.WorkOrders:
                    if (MapCallHttpApplication.IsLocalhostMapCall)
                    {
                        url = "/mapcall" + url;
                    }

                    // otherwise don't append it because it's running as
                    // root site in localhost. 
                    break;
                default:

                    throw new NotSupportedException("");
            }
#endif

            // Returns as-is in release mode. 
            return url;
        }

        #endregion

        #region Exposed Methods

        public override void RenderMenu(Menu menu)
        {
            Writer.WriteLine("<div class=\"mainMenuWrap\"><div id=\"mainMenu\" class=\"accordion\">");

            menu.Groups.Each(RenderGroup);

            Writer.WriteLine("</div></div>");
        }

        public override void RenderGroup(MenuGroup group)
        {
            if (group.IsTextOnlyLink)
            {
                Writer.WriteLine("<div><a href=\"{0}\">{1}</a></div>", FormatUrl(group.Url), group.Title);
            }
            else if (group.Parts.Count == 0 || group.Url != null)
            {
                Writer.WriteLine("<div class=\"linkOnly\">");
                Writer.WriteLine("<div class=\"title ui-state-default\"><a href=\"{0}\">{1}</a></div>",
                    FormatUrl(group.Url), group.Title);
                Writer.WriteLine("</div>");
            }
            else
            {
                Writer.WriteLine("<div>");
                Writer.WriteLine("<div class=\"title ui-state-default\"><a>{0}</a></div>", group.Title);
                Writer.WriteLine("<div class=\"content\">");
                Writer.WriteLine("<ul>");

                group.Parts.Each(RenderChild);

                Writer.WriteLine("</ul>");
                Writer.WriteLine("</div>");
                Writer.WriteLine("</div>");
            }
        }

        public override void RenderLink(MenuLink link)
        {
            if (String.IsNullOrEmpty(link.Url))
            {
                Writer.WriteLine("<span class=\"item disabled\">{0}</span>", link.Title);
            }
            else if (link.OpenInNewWindow)
            {
                Writer.WriteLine("<span class=\"item\"><a href=\"{0}\" target=\"_blank\">{1}</a></span>",
                    FormatUrl(link.Url), link.Title);
            }
            else
            {
                Writer.WriteLine("<span class=\"item\"><a href=\"{0}\">{1}</a></span>", FormatUrl(link.Url),
                    link.Title);
            }
        }

        public override void RenderSection(MenuSection section)
        {
            Writer.WriteLine("<span>{0}</span>", section.Title);
            Writer.WriteLine("<ul>");

            section.Parts.Each(RenderChild);

            Writer.WriteLine("</ul>");
        }

        #endregion
    }
}
