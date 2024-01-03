using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MMSINC.ClassExtensions;
using MMSINC.Data;

// ReSharper disable CheckNamespace
namespace MMSINC.Helpers
    // ReSharper restore CheckNamespace
{
    internal class RazorTablePaginatedFooter : RazorTableFooter
    {
        #region Consts

        // This could easily be made configurable at some point.
        public const string DIV_LINKS_CLASS_NAME = "page-links",
                            DIV_FOOTER_RESULTS_PER_PAGE_CLASS_NAME = "page-links-results-per-page";

        #endregion

        #region Properties

        public ISearchSet SearchSetModel { get; private set; }

        /// <summary>
        /// If true, the rendered table will include options to change the page size. True by default.
        /// </summary>
        public bool ShowPageSizeOptions { get; set; } = true;

        #endregion

        #region Constructors

        public RazorTablePaginatedFooter(ISearchSet model)
        {
            SearchSetModel = model;
        }

        #endregion

        private static Func<int, string, bool, int, string> GetAnonymousGetUrlFunc(HtmlHelper htmlHelper)
        {
            var viewContext = htmlHelper.ViewContext;
            var action = (string)viewContext.RouteData.Values["action"];
            var controller = (string)viewContext.RouteData.Values["controller"];

            // This allows us to copy all the existing routedata(controller, action, querystring junk)
            // and keep it as part of the link. Good for search results using query strings and stuff.
            var routeValues = new RouteValueDictionary(viewContext.RouteData.Values);
            foreach (var rv in htmlHelper.ViewData.ModelState.ToRouteValueDictionary())
            {
                // Don't use routeValues.Add as there might be a route value with a matching key.
                // ex: If you search for a specific record by id, "id" will already be in the RouteData.
                routeValues[rv.Key] = rv.Value;
            }

            var urlHelper = new SecureUrlHelper(viewContext.RequestContext, htmlHelper.RouteCollection);

            return ((page, sort, sortAscending, pageSize) => {
                // This has the potential to be potentially buggy because the same RouteValueDictionary
                // will get passed to urlHelper.Action multiple times, only with these values changed.
                // At the moment though it's fine. Be wary if other values need to be added down the road.
                routeValues["PageNumber"] = page;
                routeValues["SortBy"] = sort;
                routeValues["SortAscending"] = sortAscending;
                routeValues["PageSize"] = pageSize;
                return urlHelper.Action(action, controller, routeValues);
            });
        }

        protected override void Render(HtmlHelper htmlHelper, TagBuilder footerTag, int colSpan)
        {
            if (SearchSetModel == null)
            {
                // PaginationHelper throws an exception if the model is null. Rather than
                // throw an exception here, we just won't render anything. 
                return;
            }

            var getUrlFunc = GetAnonymousGetUrlFunc(htmlHelper);

            // We don't wanna render empty divs all over the place if we don't have to,
            // so only actually add the tag to the StringBuilder if the PaginationHelper
            // methods actually return a non-empty value.
            var sb = new StringBuilder();
            var paginationLinks = htmlHelper.PaginationLinks(SearchSetModel, getUrlFunc).ToHtmlString();
            if (!string.IsNullOrWhiteSpace(paginationLinks))
            {
                var div = new TagBuilder("div");
                div.AddCssClass(DIV_LINKS_CLASS_NAME);
                div.InnerHtml = paginationLinks;
                sb.Append(div);
            }

            if (ShowPageSizeOptions)
            {
                var resultsPerPageLinks = htmlHelper.PaginationFooter(SearchSetModel, getUrlFunc).ToHtmlString();
                if (!string.IsNullOrWhiteSpace(resultsPerPageLinks))
                {
                    var div = new TagBuilder("div");
                    div.AddCssClass(DIV_FOOTER_RESULTS_PER_PAGE_CLASS_NAME);
                    div.InnerHtml = resultsPerPageLinks;
                    sb.Append(div);
                }
            }

            footerTag.InnerHtml = sb.ToString();
        }
    }
}
