using System;
using System.Web.Mvc;
using System.Web.Routing;
using MMSINC.ClassExtensions;
using MMSINC.Data;

// ReSharper disable CheckNamespace
namespace MMSINC.Helpers
    // ReSharper restore CheckNamespace
{
    public interface IRazorTableColumnHeader<TModel>
    {
        #region Methods

        TagBuilder Render(HtmlHelper<TModel> helper, IRazorTableColumn<TModel> column, ISearchSet sortedSet);

        #endregion
    }

    internal class RazorTableColumnHeader<TModel> : IRazorTableColumnHeader<TModel>
    {
        #region Properties

        public string Text { get; set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Return the text for the header column cell with it html escaped.
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        protected virtual string GetHtmlEscapedText(HtmlHelper<TModel> helper)
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                return null;
            }

            return helper.Encode(Text);
        }

        protected virtual void RenderSortable(HtmlHelper<TModel> helper, string sortBy, ISearchSet sortedSet,
            TagBuilder tag)
        {
            // This allows us to copy all the existing routedata(controller, action, querystring junk)
            // and keep it as part of the link. Good for search results using query strings and stuff.
            var routeValues = new RouteValueDictionary(helper.ViewContext.RouteData.Values);
            foreach (var rv in helper.ViewData.ModelState.ToRouteValueDictionary())
            {
                // Don't use routeValues.Add as there might be a route value with a matching key.
                // ex: If you search for a specific record by id, "id" will already be in the RouteData.
                routeValues[rv.Key] = rv.Value;
            }

            var action = (string)helper.ViewContext.RouteData.Values["action"];
            var controller = (string)helper.ViewContext.RouteData.Values["controller"];

            Func<int, string, bool, int, string> getUrl = ((page, sort, sortAscending, pageSize) => {
                routeValues["PageNumber"] = page;
                routeValues["SortBy"] = sort;
                routeValues["SortAscending"] = sortAscending;
                routeValues["PageSize"] = pageSize;
                var urlHelper = new SecureUrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
                return urlHelper.Action(action, controller, routeValues);
            });

            tag.InnerHtml = helper.SortByLink(sortedSet, sortBy, GetHtmlEscapedText(helper), getUrl, false).ToString();
            if (sortBy == sortedSet.SortBy)
            {
                tag.AddCssClass(sortedSet.SortAscending ? "sort-asc" : "sort-desc");
            }

            tag.AddCssClass("sortable");
        }

        protected virtual void RenderAdditionalDataAttributes(TagBuilder thTag)
        {
            // noop for base method.
        }

        #endregion

        #region Public Methods

        public TagBuilder Render(HtmlHelper<TModel> helper, IRazorTableColumn<TModel> column, ISearchSet sortedSet)
        {
            var th = new TagBuilder("th");

            if (column.IsSortable)
            {
                RenderSortable(helper, column.SortBy, sortedSet, th);
            }
            else
            {
                th.InnerHtml = GetHtmlEscapedText(helper);
            }

            RenderAdditionalDataAttributes(th);

            return th;
        }

        #endregion
    }
}
