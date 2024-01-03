using System.Web.UI;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class StyleWrapper : IStyle
    {
        private CssStyleCollection _innerStyle;

        public StyleWrapper(CssStyleCollection innerStyle)
        {
            _innerStyle = innerStyle;
        }

        public void Remove(string key)
        {
            _innerStyle.Remove(key);
        }
    }
}
