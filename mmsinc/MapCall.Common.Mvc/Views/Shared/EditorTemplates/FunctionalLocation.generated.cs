﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/FunctionalLocation.cshtml")]
    public partial class _Views_Shared_EditorTemplates_FunctionalLocation_cshtml_ : MapCall.Common.Views.TemplateViewBase<string>
    {
        public _Views_Shared_EditorTemplates_FunctionalLocation_cshtml_() { }
        public override void Execute()
        {
            #line 3 "..\..\Views\Shared\EditorTemplates\FunctionalLocation.cshtml"
Write(RenderEditorForTemplate(item => new System.Web.WebPages.HelperResult(__razor_template_writer => {
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n    <div");

WriteLiteralTo(__razor_template_writer, " id=\"wbsElementLookupLink\"");

WriteLiteralTo(__razor_template_writer, ">\r\n        ");

WriteLiteralTo(__razor_template_writer, "\r\n");

WriteTo(__razor_template_writer, 
            
            #line 6 "..\..\Views\Shared\EditorTemplates\FunctionalLocation.cshtml"
        
            #line default
            #line hidden
            
            #line 6 "..\..\Views\Shared\EditorTemplates\FunctionalLocation.cshtml"
         Ajax.ActionLink("Click here to Lookup and Verify the Functional Location", "Find", "SAPFunctionalLocation", new { area = "SAP" }, new AjaxOptions { HttpMethod = "GET" }, new { data_ajax_table = "#functionalLocationTable" }));

            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n    </div>\r\n\r\n");

WriteTo(__razor_template_writer, 
            
            #line 9 "..\..\Views\Shared\EditorTemplates\FunctionalLocation.cshtml"
    
            #line default
            #line hidden
            
            #line 9 "..\..\Views\Shared\EditorTemplates\FunctionalLocation.cshtml"
     Control.TextBox(""));

            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n");

WriteTo(__razor_template_writer, 
            
            #line 10 "..\..\Views\Shared\EditorTemplates\FunctionalLocation.cshtml"
    
            #line default
            #line hidden
            
            #line 10 "..\..\Views\Shared\EditorTemplates\FunctionalLocation.cshtml"
     Html.ValidationMessage(""));

            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n");

            #line 11 "..\..\Views\Shared\EditorTemplates\FunctionalLocation.cshtml"
     })));

            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591