using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;
using MMSINC.Controls;

namespace LINQTo271.Common
{
    public class MvpHiddenField : HiddenField, IHiddenField
    {
        #region Exposed Methods

        public TControl FindControl<TControl>(string id)
           where TControl : Control 
        {
            return ControlExtensions.FindControl<TControl>(this, id);
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
