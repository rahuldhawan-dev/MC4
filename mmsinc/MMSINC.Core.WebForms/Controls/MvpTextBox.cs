using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;

namespace MMSINC.Controls
{
    public class MvpTextBox : TextBox, ITextBox
    {
        #region Exposed Methods

        public int GetIntValue()
        {
            return
                ControlExtensions.GetIntValue(
                    this);
        }

        public int? TryGetIntValue()
        {
            return
                ControlExtensions.TryGetIntValue(
                    this);
        }

        public double GetDoubleValue()
        {
            return
                ControlExtensions.GetDoubleValue(
                    this);
        }

        public double? TryGetDoubleValue()
        {
            return
                ControlExtensions.TryGetDoubleValue(
                    this);
        }

        public DateTime GetDateTimeValue()
        {
            return
                ControlExtensions.GetDateTimeValue(this);
        }

        public DateTime? TryGetDateTimeValue()
        {
            return
                ControlExtensions.TryGetDateTimeValue(this);
        }

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
    }
}
