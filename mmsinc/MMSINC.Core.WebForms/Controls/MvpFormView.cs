using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;
using MMSINC.Interface;

namespace MMSINC.Controls
{
    public class MvpFormView : FormView, IDetailControl, IDataBoundControl
    {
        #region Properties

        public DetailViewMode CurrentMvpMode
        {
            get { return CurrentMode.ToMVPDetailViewMode(); }
        }

        #endregion

        #region Exposed Methods

        public void ChangeMvpMode(DetailViewMode newMode)
        {
            ChangeMode(newMode.ToFormViewMode());
        }

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            return ControlExtensions.FindControl<TControl>(this, id);
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            return ControlExtensions.FindIControl<TIControl>((IControl)this, id);
        }

        #endregion
    }
}
