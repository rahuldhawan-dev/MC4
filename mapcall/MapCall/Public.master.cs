using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace MapCall
{
    public partial class Public : MasterPage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var link = new HtmlLink();
            link.Attributes.Add("rel", "stylesheet");
            link.Href = "~/includes/Login.css";
            cphHead.Controls.Add(link);
        }
    }
}
