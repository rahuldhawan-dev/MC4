﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MapCall.Common.Views.Shared.DisplayTemplates
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
    
    #line 8 "..\..\Views\Shared\DisplayTemplates\PhoneNumber.cshtml"
    using MMSINC.ClassExtensions;
    
    #line default
    #line hidden
    
    #line 9 "..\..\Views\Shared\DisplayTemplates\PhoneNumber.cshtml"
    using MMSINC.Utilities;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/PhoneNumber.cshtml")]
    public partial class PhoneNumber_ : MapCall.Common.Views.TemplateViewBase<dynamic>
    {
        public PhoneNumber_() : base() { }
        public override void Execute()
        {
WriteLiteral("\r\n\r\n");

            #line 10 "..\..\Views\Shared\DisplayTemplates\PhoneNumber.cshtml"
  
    var phone = ViewData.TemplateInfo.FormattedModelValue as string;
    var prettyPhone = PhoneNumberFormatter.TryFormat(phone);

            #line default
            #line hidden
WriteLiteral("\r\n");

            #line 14 "..\..\Views\Shared\DisplayTemplates\PhoneNumber.cshtml"
Write(RenderDisplayForTemplate(item => new System.Web.WebPages.HelperResult(__razor_template_writer => {
            #line default
            #line hidden
            
            #line 14 "..\..\Views\Shared\DisplayTemplates\PhoneNumber.cshtml"
 WriteTo(__razor_template_writer, prettyPhone);

            #line default
            #line hidden
            
            #line 14 "..\..\Views\Shared\DisplayTemplates\PhoneNumber.cshtml"
                                                  })));

            #line default
            #line hidden
WriteLiteral("\r\n");
        }
    }
}
#pragma warning restore 1591