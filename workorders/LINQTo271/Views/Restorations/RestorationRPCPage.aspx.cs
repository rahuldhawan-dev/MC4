using System;
using MMSINC.Common;
using WorkOrders;

namespace LINQTo271.Views.Restorations
{
    public partial class RestorationRPCPage : MvpPage
    {
        protected override void OnInit(EventArgs e)
        {
            Response.Redirect("/Modules/Mvc/FieldOperations/Restoration/New/" +
                              IRequest.IQueryString.GetValue("arg"));
            base.OnInit(e);
        }
    }
}
