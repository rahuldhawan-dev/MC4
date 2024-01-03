using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public class MvpImageButton : ImageButton, IImageButton
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

    public interface IImageButton : IControl
    {
        #region Properties

        string ImageUrl { get; set; }

        #endregion
    }
}
