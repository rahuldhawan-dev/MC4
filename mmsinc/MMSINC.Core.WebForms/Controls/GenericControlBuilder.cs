using System;
using System.Collections;
using System.Web.UI;

namespace MMSINC.Controls
{
    public class GenericControlBuilder<TChildControl> : ControlBuilder
    {
        public override Type GetChildControlType(string tagName, IDictionary attribs)
        {
            var ret = typeof(TChildControl);
            if (ret.Name.ToLower() == tagName.ToLower())
                return ret;
            return null;
        }
    }
}
