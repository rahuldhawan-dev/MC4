using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using StructureMap;

namespace MMSINC.Helpers
{
    public static class PaginationHelper
    {
        public const string PAGINATION_LINK_CLASS = "paginationLink";

        #region Fields

        private static readonly PaginationHelperConfiguration _defaultConfig = new PaginationHelperConfiguration();

        #endregion

        #region Properties

        /// <summary>
        /// Returns the PaginationHelperConfiguration instance registered with
        /// ObjectFactory or a default instance if one can not be found.
        /// </summary>
        internal static PaginationHelperConfiguration Configuration
        {
            get { return DependencyResolver.Current.GetService<PaginationHelperConfiguration>() ?? _defaultConfig; }
        }

        #endregion

        #region Private Methods

        private static void ApplyAjaxOptions(TagBuilder tag, AjaxOptions ajaxOptions)
        {
            if (ajaxOptions != null)
            {
                tag.MergeAttributes(ajaxOptions.ToUnobtrusiveHtmlAttributes());
            }
        }

        private static void AppendLinkForPageSize(ISearchSet set, Func<int, string, bool, int, string> getPageUrl,
            StringBuilder sb, int size)
        {
            AppendLinkForPageSize(set, getPageUrl, sb, size, size.ToString());
        }

        private static void AppendLinkForPageSize(ISearchSet set, Func<int, string, bool, int, string> getPageUrl,
            StringBuilder sb, int size, string linkText)
        {
            sb.AppendLine(
                CreatePaginationlinkTag(
                    getPageUrl(1, set.SortBy, set.SortAscending, size), linkText).ToString());
        }

        private static void AppendNextLink(ISearchSet set, Func<int, string, bool, int, string> getPageUrl,
            StringBuilder sb, AjaxOptions ajaxOptions = null)
        {
            var tag =
                CreatePaginationlinkTag(getPageUrl(set.PageNumber + 1, set.SortBy, set.SortAscending, set.PageSize),
                    Configuration.NextLinkText);
            ApplyAjaxOptions(tag, ajaxOptions);
            sb.AppendLine(tag.ToString());
        }

        private static void AppendNumberedLinks(ISearchSet set, Func<int, string, bool, int, string> getPageUrl,
            StringBuilder sb, AjaxOptions ajaxOptions = null)
        {
            for (var i = 1; i <= set.PageCount; ++i)
            {
                AppendNumberedLink(set, i, getPageUrl, sb, ajaxOptions);
            }
        }

        private static void AppendNumberedLinks(ISearchSet set, IEnumerable<int> range,
            Func<int, string, bool, int, string> getPageUrl, StringBuilder sb,
            AjaxOptions ajaxOptions = null)
        {
            foreach (var i in range)
            {
                AppendNumberedLink(set, i, getPageUrl, sb, ajaxOptions);
            }
        }

        private static void AppendNumberedLink(ISearchSet set, int pageNumber,
            Func<int, string, bool, int, string> getPageUrl, StringBuilder sb, AjaxOptions ajaxOptions = null)
        {
            var tag = pageNumber == set.PageNumber
                ? new TagBuilder("span")
                : CreatePaginationlinkTag(getPageUrl(pageNumber, set.SortBy, set.SortAscending, set.PageSize));
            tag.SetInnerText(pageNumber.ToString(CultureInfo.InvariantCulture));
            ApplyAjaxOptions(tag, ajaxOptions);
            sb.AppendLine(tag.ToString());
        }

        private static void AppendPreviousLink(ISearchSet set, Func<int, string, bool, int, string> getPageUrl,
            StringBuilder sb, AjaxOptions ajaxOptions = null)
        {
            var tag =
                CreatePaginationlinkTag(getPageUrl(set.PageNumber - 1, set.SortBy, set.SortAscending, set.PageSize),
                    Configuration.PreviousLinkText);
            ApplyAjaxOptions(tag, ajaxOptions);
            sb.AppendLine(tag.ToString());
        }

        private static TagBuilder CreatePaginationlinkTag(string url, string text = null)
        {
            var tag = new TagBuilder("a");
            tag.MergeAttribute("href", url);
            if (!string.IsNullOrWhiteSpace(text))
            {
                tag.SetInnerText(text);
            }

            tag.MergeAttribute("class", PAGINATION_LINK_CLASS);
            return tag;
        }

        /// <summary>
        /// Returns an object of just the valid page numbers worth displaying.
        /// </summary>
        /// <param name="beginningAndEndNumberOfPages">The number of pages that should be displayed for both the beginning and ending groups of links.</param>
        /// <param name="middleNumberOfPages">The number of pages to display for the middle group of links</param>
        /// <returns></returns>
        internal static LinkablePageNumbers GetLinkablePageNumbers(int beginningAndEndNumberOfPages,
            int middleNumberOfPages, ISearchSet set)
        {
            return LinkablePageNumbers.Get(beginningAndEndNumberOfPages, middleNumberOfPages, set);
        }

        private static IHtmlString GeneratePaginationLinks(ISearchSet set,
            Func<int, string, bool, int, string> getPageUrl, AjaxOptions ajaxOptions = null)
        {
            var sb = new StringBuilder();

            if (set.PageCount > 1)
            {
                // previous link
                if (set.PageNumber > 1)
                {
                    AppendPreviousLink(set, getPageUrl, sb);
                }

                var config = Configuration;

                var linkablePageNumbers = GetLinkablePageNumbers(config.BookEndLinkCount, config.MiddleLinkCount, set);
                if (linkablePageNumbers.AllPagesAreValid)
                {
                    // numbered links
                    AppendNumberedLinks(set, getPageUrl, sb);
                }
                else
                {
                    AppendNumberedLinks(set, linkablePageNumbers.Beginning, getPageUrl, sb, ajaxOptions);

                    if (linkablePageNumbers.Middle.Any())
                    {
                        sb.AppendFormat("<span>{0}</span>", Configuration.MiddleLinkSeparatorText);
                        AppendNumberedLinks(set, linkablePageNumbers.Middle, getPageUrl, sb, ajaxOptions);
                        sb.AppendFormat("<span>{0}</span>", Configuration.MiddleLinkSeparatorText);
                    }

                    AppendNumberedLinks(set, linkablePageNumbers.End, getPageUrl, sb, ajaxOptions);
                }

                // next link
                if (set.PageNumber < set.PageCount)
                {
                    AppendNextLink(set, getPageUrl, sb);
                }
            }

            return new HtmlString(sb.ToString());
        }

        #endregion

        #region Exposed Methods

        public static IHtmlString PaginationLinks(this HtmlHelper html, ISearchSet set, SecureUrlHelper url)
        {
            return html.PaginationLinks(set,
                // ReSharper disable once Mvc.ActionNotResolved
                (PageNumber, SortBy, SortDir, PageSize) => url.Action("Index", new {
                    PageNumber,
                    SortBy,
                    SortDir,
                    PageSize
                }));
        }

        public static IHtmlString PaginationLinks(this HtmlHelper html, ISearchSet set,
            Func<int, string, bool, int, string> getPageUrl)
        {
            return GeneratePaginationLinks(set, getPageUrl, null);
        }

        public static IHtmlString PaginationLinks(this AjaxHelper ajax, ISearchSet set, AjaxOptions ajaxOptions,
            Func<int, string, bool, int, string> getPageUrl)
        {
            return GeneratePaginationLinks(set, getPageUrl, ajaxOptions);
        }

        public static IHtmlString PaginationFooter(this HtmlHelper html, ISearchSet set,
            Func<int, string, bool, int, string> getPageUrl)
        {
            var sb = new StringBuilder();
            if (set.Count > 1)
            {
                sb.AppendLine("Results Per Page: ( ");
                AppendLinkForPageSize(set, getPageUrl, sb, 1);
                if (set.Count > 5)
                    AppendLinkForPageSize(set, getPageUrl, sb, 5);
                if (set.Count > 10)
                    AppendLinkForPageSize(set, getPageUrl, sb, 10);
                if (set.Count > 25)
                    AppendLinkForPageSize(set, getPageUrl, sb, 25);
                if (set.Count > 500)
                    AppendLinkForPageSize(set, getPageUrl, sb, 500);
                else
                    AppendLinkForPageSize(set, getPageUrl, sb, set.Count, "All");
                sb.AppendLine(" )");
            }

            return new HtmlString(sb.ToString());
        }

        #endregion

        #region Helper Class

        internal class LinkablePageNumbers
        {
            public IEnumerable<int> Beginning { get; private set; }
            public IEnumerable<int> Middle { get; private set; }
            public IEnumerable<int> End { get; private set; }

            /// <summary>
            /// Returns all the page numbers possible for a set. Not necessarily valid! Check AllPagesAreValid first.
            /// </summary>
            public IEnumerable<int> All { get; private set; }

            public bool AllPagesAreValid { get; private set; }

            private LinkablePageNumbers(int beginStart, int beginEnd, int middleStart, int middleEnd, int endStart,
                int endEnd)
            {
                Beginning = IEnumerableExtensions.Range(beginStart, beginEnd);
                End = IEnumerableExtensions.Range(endStart, endEnd);
                All = Enumerable.Empty<int>();
                if (middleStart > 0 && middleEnd > 0)
                {
                    Middle = IEnumerableExtensions.Range(middleStart, middleEnd);
                }
                else
                {
                    Middle = Enumerable.Empty<int>();
                }
            }

            private LinkablePageNumbers(int maxPageNumber)
            {
                Beginning = Middle = End = Enumerable.Empty<int>();
                All = IEnumerableExtensions.Range(1, maxPageNumber);
                AllPagesAreValid = true;
            }

            public static LinkablePageNumbers Get(int beginningAndEndNumberOfPages, int middleNumberOfPages,
                ISearchSet set)
            {
                // TODO: Refactor this method. I hate math. -Ross
                //       Maybe just move all this to the LinkablePageNumbers class.

                var actualNumberOfPages = set.PageCount;
                var currentPageNumber = set.PageNumber;

                if (actualNumberOfPages <= 0)
                {
                    throw new InvalidOperationException(
                        "This method should not be called if pagination is not possible.");
                }

                if ((beginningAndEndNumberOfPages * 2) >= actualNumberOfPages)
                {
                    return new LinkablePageNumbers(actualNumberOfPages);
                }

                if (((beginningAndEndNumberOfPages * 2) + middleNumberOfPages) >= actualNumberOfPages)
                {
                    return new LinkablePageNumbers(actualNumberOfPages);
                }

                var beginStart = 1;
                var beginEnd = beginningAndEndNumberOfPages;
                var endStart = actualNumberOfPages - beginningAndEndNumberOfPages + 1;
                var endEnd = actualNumberOfPages;

                // The middle page should actually be the current page if the current page
                // does not fall within the range of the beginning or end groups.
                var middlePage = (actualNumberOfPages / 2);
                if ((beginEnd + 1) <= currentPageNumber && currentPageNumber <= (endStart - 1))
                {
                    middlePage = currentPageNumber;
                }

                var midStart = 0;
                var midEnd = 0;

                // This isn't setup to correctly divide and adjust things based on
                // whether the page range is odd/even or the middleCount is odd/even. 
                // It's not worth the hassle(yet). -Ross 4/5/2013

                if (middleNumberOfPages <= 0)
                {
                    midStart = 0;
                    midEnd = 0;
                }
                else if (middleNumberOfPages == 1)
                {
                    midStart = middlePage;
                    midEnd = middlePage;
                }
                else if (currentPageNumber == beginEnd)
                {
                    midStart = beginEnd + 1;
                    midEnd = beginEnd + middleNumberOfPages;
                }
                else if (currentPageNumber == endStart)
                {
                    midStart = endStart - middleNumberOfPages;
                    midEnd = endStart - 1;
                }
                else
                {
                    var middleCountDiff = ((double)middleNumberOfPages / 2);
                    midStart = Convert.ToInt32(Math.Ceiling(middlePage - middleCountDiff));
                    // Need to subtract 1 or else we'll always end up with a range
                    // with one more additional link than asked for.
                    midEnd = midStart + middleNumberOfPages - 1;
                }

                // Now we need to fix possible collisions.
                if (midStart > 0 && midEnd > 0)
                {
                    if (midStart <= beginEnd)
                    {
                        var diff = (beginEnd - midStart) + 1;
                        midStart += diff;
                        midEnd += diff;
                    }
                    else if (midEnd >= endStart)
                    {
                        var diff = (midEnd - endStart) + 1;
                        midStart -= diff;
                        midEnd -= diff;
                    }
                }

                return new LinkablePageNumbers(beginStart, beginEnd, midStart, midEnd, endStart, endEnd);
            }
        }

        #endregion
    }
}
