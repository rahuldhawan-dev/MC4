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

namespace MapCall.Common.Views.Shared.EditorTemplates
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
    
    #line 1 "..\..\Views\Shared\EditorTemplates\Boolean.cshtml"
    using MMSINC.Metadata;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/Boolean.cshtml")]
    public partial class Boolean_ : MapCall.Common.Views.TemplateViewBase<bool?>
    {
        public Boolean_() : base() { }
        public override void Execute()
        {
WriteLiteral("\r\n");

            #line 18 "..\..\Views\Shared\EditorTemplates\Boolean.cshtml"
  
    // Use a checkbox only if a nullable boolean property has the CheckBoxAttribute affixed to it.
    var useBoolDrop = ViewData.ModelMetadata.IsNullableValueType && CheckBoxAttribute.GetFromModelMetadata(ViewData.ModelMetadata) == null;
    var control = (useBoolDrop ? (IHtmlString)Control.BoolDropDown("") : (IHtmlString)Control.CheckBox("").With(ViewTemplateHelper.HtmlAttributes));

            #line default
            #line hidden
WriteLiteral("\r\n");

            #line 23 "..\..\Views\Shared\EditorTemplates\Boolean.cshtml"
Write(RenderEditorForTemplate(item => new System.Web.WebPages.HelperResult(__razor_template_writer => {
            #line default
            #line hidden
            
            #line 23 "..\..\Views\Shared\EditorTemplates\Boolean.cshtml"
WriteTo(__razor_template_writer, control);

            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, " ");

            #line 23 "..\..\Views\Shared\EditorTemplates\Boolean.cshtml"
         WriteTo(__razor_template_writer, Html.ValidationMessage(""));

            #line default
            #line hidden
            
            #line 23 "..\..\Views\Shared\EditorTemplates\Boolean.cshtml"
                                                                         })));

            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
