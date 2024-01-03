using System;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public class DynamicBoundField : BoundField
    {
        #region Constants

        private struct ViewStateKeys
        {
            public const string TEXT_MODE = "TextMode";
        }

        #endregion

        #region Properties

        public TextBoxMode TextMode
        {
            get
            {
                var ret = ViewState[ViewStateKeys.TEXT_MODE];
                return (ret == null) ? TextBoxMode.SingleLine : (TextBoxMode)ret;
            }
            set { ViewState[ViewStateKeys.TEXT_MODE] = value; }
        }

        #endregion

        #region Private Methods

        protected override void OnDataBindField(object sender, EventArgs e)
        {
            base.OnDataBindField(sender, e);
            var txt = sender as TextBox;
            if (txt != null)
                txt.TextMode = TextMode;
        }

        #endregion
    }
}
