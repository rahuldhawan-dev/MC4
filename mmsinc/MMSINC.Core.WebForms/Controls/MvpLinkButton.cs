using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public class MvpLinkButton : LinkButton, ILinkButton
    {
        #region Exposed Methods

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

    public interface ILinkButton : IControl
    {
        string PostBackUrl { get; set; }
    }
}
