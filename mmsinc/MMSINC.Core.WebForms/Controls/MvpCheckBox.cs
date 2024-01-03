using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public class MvpCheckBox : CheckBox, ICheckBox
    {
        #region Properties

        public bool? NullablyChecked
        {
            get => Checked;
            set => Checked = value ?? false;
        }

        #endregion

        #region IControl Members

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            throw new NotImplementedException();
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
