using System;
using System.Text;
using System.Web.Mvc;
using MMSINC.ClassExtensions.StringBuilderExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data;

namespace MMSINC.Helpers
{
    public static class SortHelper
    {
        public const string SORT_ASC_SYMBOL = "▴",
                            SORT_DESC_SYMBOL = "▾";

        public static MvcHtmlString SortByLink(this HtmlHelper html, ISearchSet set, string sortBy,
            Func<int, string, bool, int, string> getPageUrl)
        {
            return SortByLink(html, set, sortBy, sortBy.ToTitleCase(), getPageUrl);
        }

        /// <summary>
        /// Generates a link for sorting a column. Supports non-logical properties and 
        /// one level of association.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="set"></param>
        /// <param name="sortBy">
        /// e.g Name, Id, Street.FullStName, Town.Name
        /// </param>
        /// <param name="headerText"></param>
        /// <param name="getPageUrl"></param>
        /// <param name="htmlEscapeHeader">Set to false if the header text already has been html escaped</param>
        /// <returns></returns>
        public static MvcHtmlString SortByLink(this HtmlHelper html, ISearchSet set, string sortBy, string headerText,
            Func<int, string, bool, int, string> getPageUrl, bool htmlEscapeHeader = true)
        {
            var sb = new StringBuilder();

            AppendSortLink(set, sortBy, headerText, set.PageSize, getPageUrl, sb, htmlEscapeHeader);

            if (set.SortBy == sortBy)
            {
                var symbol = set.SortAscending ? SORT_DESC_SYMBOL : SORT_ASC_SYMBOL;
                sb.Append(CreateSortIcon(symbol));
            }

            return sb.ToMvcHtmlString();
        }

        private static TagBuilder CreateSortIcon(string iconSymbol)
        {
            var tag = new TagBuilder("span");
            tag.SetInnerText(iconSymbol);
            return tag;
        }

        private static void AppendSortLink(ISearchSet set, string sortBy, string headerText, int pageSize,
            Func<int, string, bool, int, string> getPageUrl, StringBuilder sb, bool htmlEscapeHeader)
        {
            headerText = headerText ?? sortBy.ToTitleCase();
            var tag = new TagBuilder("a");
            var sortAscending = true;
            if (set.SortBy == sortBy)
                sortAscending = !set.SortAscending;
            tag.MergeAttribute("href", getPageUrl(set.PageNumber, sortBy, sortAscending, pageSize));

            if (htmlEscapeHeader)
            {
                tag.SetInnerText(headerText);
            }
            else
            {
                tag.InnerHtml = headerText;
            }

            sb.Append(tag);
        }
    }
}
