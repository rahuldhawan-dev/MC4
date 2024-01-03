using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LINQTo271
{
    public partial class GC161 : System.Web.UI.Page
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            lblCurrentMem.Text = GC.GetTotalMemory(true) + " bytes";
            lblIs64Bit.Text = Environment.Is64BitProcess.ToString();
        }
    }
}