using System.Web.UI;

namespace MMSINC.Controls
{
    public interface IPlaceHolder : IControl
    {
        ControlCollection Controls { get; }
    }
}
