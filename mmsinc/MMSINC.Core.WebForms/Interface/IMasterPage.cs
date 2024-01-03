using System.Web.UI;

namespace MMSINC.Interface
{
    public interface IMasterPage
    {
        Control FindControl(string ctrl);
    }
}
