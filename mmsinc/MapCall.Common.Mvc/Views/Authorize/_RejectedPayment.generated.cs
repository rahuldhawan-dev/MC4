﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MapCall.Common.Views.Authorize
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Authorize/_RejectedPayment.cshtml")]
    public partial class RejectedPayment : MapCall.Common.Views.TemplateViewBase<AuthorizeNet.IGatewayResponse>
    {
        public RejectedPayment() : base() { }
        public override void Execute()
        {
WriteLiteral("<h2>Payment Rejected</h2>\r\n\r\nYour payment has been rejected, and the selected per" +
"mit will not be submitted.\r\n\r\n");

            #line 7 "..\..\Views\Authorize\_RejectedPayment.cshtml"
Write(Html.DisplayFor(m => m.Message));

            #line default
            #line hidden
WriteLiteral("\r\n");
        }
    }
}
#pragma warning restore 1591
