using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMSINC.Helpers
{
    /// <summary>
    /// Configures settings for static PaginationHelper methods to use. 
    /// Register this with ObjectFactory if you need to override it!
    /// </summary>
    public class PaginationHelperConfiguration
    {
        #region Consts

        private const int DEFAULT_BOOKEND_LINK_COUNT = 5,
                          DEFAULT_MIDDLE_LINK_COUNT = 5;

        private const string DEFAULT_PREVIOUS_LINK_TEXT = "<<",
                             DEFAULT_NEXT_LINK_TEXT = ">>",
                             DEFAULT_MIDDLE_LINK_SEPARATOR_TEXT = "...";

        #endregion

        #region Properties

        /// <summary>
        /// The default number of links to display at the beginning and end of the list of
        /// page links when there are too many to display.
        /// </summary>
        public int BookEndLinkCount { get; set; }

        /// <summary>
        /// The default number of links to display from the middle of a huge list of page links
        /// when there are too many to display.
        /// </summary>
        public int MiddleLinkCount { get; set; }

        public string MiddleLinkSeparatorText { get; set; }
        public string NextLinkText { get; set; }
        public string PreviousLinkText { get; set; }

        #endregion

        #region Constructor

        public PaginationHelperConfiguration()
        {
            BookEndLinkCount = DEFAULT_BOOKEND_LINK_COUNT;
            MiddleLinkCount = DEFAULT_MIDDLE_LINK_COUNT;
            MiddleLinkSeparatorText = DEFAULT_MIDDLE_LINK_SEPARATOR_TEXT;
            NextLinkText = DEFAULT_NEXT_LINK_TEXT;
            PreviousLinkText = DEFAULT_PREVIOUS_LINK_TEXT;
        }

        #endregion
    }
}
