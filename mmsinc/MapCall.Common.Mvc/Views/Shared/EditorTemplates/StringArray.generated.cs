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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/StringArray.cshtml")]
    public partial class StringArray_ : MapCall.Common.Views.TemplateViewBase<dynamic>
    {
        public StringArray_() : base() { }
        public override void Execute()
        {
            #line 1 "..\..\Views\Shared\EditorTemplates\StringArray.cshtml"
Write(RenderEditorForTemplate(item => new System.Web.WebPages.HelperResult(__razor_template_writer => {
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n                              <div");

WriteAttributeTo(__razor_template_writer, "id", Tuple.Create(" id=\"", 68), Tuple.Create("\"", 85)
            
            #line 2 "..\..\Views\Shared\EditorTemplates\StringArray.cshtml"
, Tuple.Create(Tuple.Create("", 73), Tuple.Create<System.Object, System.Int32>(Html.Id("")
            
            #line default
            #line hidden
, 73), false)
);

WriteAttributeTo(__razor_template_writer, "name", Tuple.Create(" name=\"", 86), Tuple.Create("\"", 107)
            
            #line 2 "..\..\Views\Shared\EditorTemplates\StringArray.cshtml"
, Tuple.Create(Tuple.Create("", 93), Tuple.Create<System.Object, System.Int32>(Html.Name("")
            
            #line default
            #line hidden
, 93), false)
);

WriteLiteralTo(__razor_template_writer, " class=\"multiinput\"");

WriteLiteralTo(__razor_template_writer, "></div>\r\n                          ");

            #line 3 "..\..\Views\Shared\EditorTemplates\StringArray.cshtml"
                               })));

            #line default
            #line hidden
WriteLiteral("\r\n");
        }
    }
}
#pragma warning restore 1591
