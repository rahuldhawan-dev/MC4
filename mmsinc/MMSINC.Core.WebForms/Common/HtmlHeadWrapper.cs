using System.Web.UI.HtmlControls;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class HtmlHeadWrapper : IHtmlHead
    {
        #region Fields

        private HtmlHead _header;

        #endregion

        #region Constructors

        public HtmlHeadWrapper(HtmlHead header)
        {
            _header = header;
        }

        #endregion

        #region Methods

        public void DataBind()
        {
            _header.DataBind();
        }

        #endregion
    }
}
