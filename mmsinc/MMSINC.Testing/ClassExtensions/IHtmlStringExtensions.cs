using System;
using System.Web;

namespace MMSINC.Testing.ClassExtensions
{
    public static class IHtmlStringExtensions
    {
        public static string ToStringWithoutLineBreaks(this IHtmlString str)
        {
            return str.ToString().Replace(Environment.NewLine, "");
        }
    }
}
