using System;
using MMSINC.Common;
using WorkOrders;

namespace LINQTo271.Views.WorkOrders.Input
{
    public partial class WorkOrderInputRPCPage : MvpPage
    {
        protected override void OnLoad(EventArgs e)
        {
            Response.Redirect("../General/WorkOrderGeneralResourceRPCPage.aspx?cmd=update&arg=" + Request.QueryString["arg"]);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Header.DataBind();
        }
    }
}
