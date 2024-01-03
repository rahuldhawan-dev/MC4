using System;
using System.Web.UI.WebControls;
using MMSINC.Controls;

namespace MapCall.Common.Resources.Masters
{
    public class MapCallBase : MasterPage
    {
        #region Control declarations

        protected ContentPlaceHolder head;
        protected ContentPlaceHolder content;

        #endregion

        #region Event Handlers

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            IHeader.DataBind();
        }

        #endregion
    }
}
