﻿using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;

namespace MMSINC.Controls
{
    public class MvpLabel : Label, ILabel
    {
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
    }
}
