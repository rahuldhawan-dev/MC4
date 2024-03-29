﻿using StructureMap;

#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MapCall.Common.Views.Shared
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/PdfBaseLayout.cshtml")]
    public partial class PdfBaseLayout : MapCall.Common.Views.PdfViewBase<dynamic>
    {
        public PdfBaseLayout() :  base() { }
        public override void Execute()
        {
WriteLiteral("\r\n<!DOCTYPE html>\r\n<html>\r\n    <head>\r\n        <title>");

            #line 13 "..\..\Views\Shared\PdfBaseLayout.cshtml"
          Write(ViewBag.Title);

            #line default
            #line hidden
WriteLiteral("</title>\r\n");

WriteLiteral("        ");

            #line 14 "..\..\Views\Shared\PdfBaseLayout.cshtml"
   Write(RenderSection("styles", required: false));

            #line default
            #line hidden
WriteLiteral("\r\n        <style");

WriteLiteral(" type=\"text/css\"");

WriteLiteral(@">
            * {
                margin: 0px;
                padding: 0px;
                position: relative;
            }

            html {
                background: white;
                height: 100%;
                overflow: hidden;
                padding: 0.1in;
            }

            body {
                display: block;
            }

			/*This table css stuff is for making the pages automatically break into 
			  new pages without cutting elements in half. */
            table {
                width: 100%;
                border-collapse: collapse;
				page-break-inside: auto;
			}

			tr {
				page-break-inside: avoid;
				page-break-after: auto;
			}

			thead {
				display: table-header-group;
			}

			tfoot {
				display: table-footer-group;
			}
        </style>
    
");

WriteLiteral("        ");

            #line 56 "..\..\Views\Shared\PdfBaseLayout.cshtml"
   Write(RenderSection("head", required: false));

            #line default
            #line hidden
WriteLiteral("\r\n\r\n    </head>\r\n<body>\r\n");

WriteLiteral("    ");

            #line 60 "..\..\Views\Shared\PdfBaseLayout.cshtml"
Write(RenderSection("body", required: true));

            #line default
            #line hidden
WriteLiteral("\r\n</body>\r\n</html>");
        }
    }
}
#pragma warning restore 1591
