using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;
using MMSINC.Common;
using MMSINC.Interface;

namespace MMSINC.Controls
{
    public class MvpPanel : Panel, IPanel
    {
        private StyleWrapper _iStyleWrapper;

        #region Exposed Methods

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            return
                ControlExtensions.FindControl
                    <TControl>(this, id);
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            return
                ControlExtensions.FindIControl
                    <TIControl>((Control)this, id);
        }

        #endregion

        #region Implementation of IPanel

        public IStyle IStyle
        {
            get
            {
                if (_iStyleWrapper == null)
                    _iStyleWrapper = new StyleWrapper(Style);

                return _iStyleWrapper;
            }
        }

        #endregion
    }
}
