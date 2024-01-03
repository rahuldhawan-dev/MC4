using System;
using System.Collections.Generic;
using System.Web.UI;
using MMSINC.Common;
using MMSINC.Utilities;

namespace MMSINC.DataPages
{
    /// <summary>
    /// This control is for making links to a specific data record on a specific DataPageBase derived
    /// page. This is so it'll be easier to change the querystring down the road if necessary. 
    /// 
    /// Example usage:
    /// var d = new DataPageViewRecordLink();
    /// d.LinkUrl = "~/DataPage.aspx";
    /// d.DataRecordId = "2946"
    /// 
    /// Renders href as http://whatever.com/DataPage.aspx?view=2946
    /// 
    /// </summary>
    public class DataPageViewRecordLink : MvpUserControl
    {
        #region Fields

        private string _linkText;
        private string _linkUrl;
        private string _dataRecordId;

        #endregion

        #region Properties

        public string DataRecordId
        {
            get { return (_dataRecordId ?? string.Empty); }
            set { _dataRecordId = value; }
        }

        public string LinkText
        {
            get { return (_linkText ?? string.Empty); }
            set { _linkText = value; }
        }

        public string LinkUrl
        {
            get
            {
                // Return string.Empty for null values to stay consistant
                // with other ASP controls returning the same thing.
                return (_linkUrl ?? string.Empty);
            }
            set { _linkUrl = value; }
        }

        public Guid ReturnSearchGuid { get; set; }

        public LinkTargetTypes Target { get; set; }

        #endregion

        #region Constructors

        public DataPageViewRecordLink()
        {
            base.EnableViewState = false;
        }

        #endregion

        #region Private methods

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute("href", GetLinkUrl());
            WriteTargetAttribute(writer);

            writer.RenderBeginTag("a");

            base.Render(writer);
            writer.Write(LinkText);
            writer.RenderEndTag();
        }

        internal void WriteTargetAttribute(HtmlTextWriter writer)
        {
            switch (Target)
            {
                case LinkTargetTypes.Blank:
                    writer.AddAttribute("target", "_blank");
                    break;
                default:
                    // Do nothing. Self is the default, which does not require a target attribute.
                    break;
            }
        }

        #endregion

        #region Exposed methods

        public string GetLinkUrl()
        {
            var query = new Dictionary<string, object>();
            query.Add(DataPageUtility.QUERY.VIEW, DataRecordId);
            if (ReturnSearchGuid != Guid.Empty)
            {
                query.Add(DataPageUtility.QUERY.SEARCH, ReturnSearchGuid.ToString());
            }

            var href = (!string.IsNullOrEmpty(LinkUrl) ? IPage.ResolveClientUrl(LinkUrl) : IRequest.Url);
            return QueryStringHelper.BuildFromDictionary(href, query);
        }

        #endregion
    }

    /// <summary>
    /// Enum representing values that go in an A tag's target attribute. 
    /// </summary>
    public enum LinkTargetTypes
    {
        /// <summary>
        /// Normal link behavior. Default.
        /// </summary>
        Self = 0,

        /// <summary>
        /// Opens the link in a new tab or window.
        /// </summary>
        Blank = 1,
    }
}
