using System.Web.UI;
using MMSINC.Interface;

namespace MMSINC.Controls
{
    public interface IPanel : IControl
    {
        #region Properties

        IStyle IStyle { get; }
        ControlCollection Controls { get; }

        #endregion
    }
}
