using System;

namespace MMSINC.DataPages
{
    public interface IDataPageRenderHelper
    {
        #region Methods

        string RenderBackToResultsButton();
        string RenderBackToSearchButton();
        string RenderExportToExcelButton();
        string RenderCreateNewRecordButton(string linkText);
        string RenderLinkButton(string url, string linkText);
        string RenderNewSearchButton();

        #endregion
    }

    // TODO: Make each individual render method its own object thats
    //       obtained from a property. That way it can be patchable
    //       and custom renderers can be put in on a per-page basis.

    public class DataPageRenderHelper : IDataPageRenderHelper
    {
        #region Constants

        internal const string LINKBUTTON_FORMAT = "<a href=\"{0}\" class=\"linkButton\">{1}</a>";

        #endregion

        #region Properties

        public IDataPageBase Owner { get; private set; }
        public IDataPagePath PathHelper { get; private set; }

        #endregion

        public DataPageRenderHelper(IDataPageBase owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            Owner = owner;
            PathHelper = Owner.PathHelper;
        }

        #region Public methods

        // linktext can be null.
        public string RenderLinkButton(string url, string linkText)
        {
            return String.Format(LINKBUTTON_FORMAT, url, linkText);
        }

        public string RenderBackToResultsButton()
        {
            // If there wasn't a valid search we want to hide the Back to Results button.
            // This is mainly for when a page is opened directly to a record from a non-search page.
            if (Owner.CachedFilterKey != Guid.Empty)
            {
                return RenderLinkButton(PathHelper.GetSearchResultsUrl(), "Back To Results");
            }

            return string.Empty;
        }

        public string RenderBackToSearchButton()
        {
            return RenderLinkButton(PathHelper.GetBaseUrl(), "Back to Search");
        }

        /// <summary>
        /// Renders a Create New Record button if a page is not readonly and the user has permission. 
        /// </summary>
        /// <param name="linkText"></param>
        /// <returns></returns>
        public string RenderCreateNewRecordButton(string linkText)
        {
            // Do I want to do this here? Easier to test I suppose. 
            if (!Owner.IsReadOnlyPage && Owner.Permissions.CreateAccess.IsAllowed)
            {
                return RenderLinkButton(PathHelper.GetCreateNewRecordUrl(), linkText);
            }

            return string.Empty;
        }

        public string RenderExportToExcelButton()
        {
            return RenderLinkButton(PathHelper.GetExportToExcelUrl(), "Export");
        }

        public string RenderNewSearchButton()
        {
            return RenderLinkButton(PathHelper.GetNewSearchUrl(), "New Search");
        }

        #endregion
    }
}
