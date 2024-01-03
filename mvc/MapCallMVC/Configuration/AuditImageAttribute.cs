using System;
using System.Web.Mvc;

namespace MapCallMVC.Configuration
{
    // This filter's purpose is only to audit that a user has viewed
    // the pdf of an image that exists. It should not audit images
    // that do not exist, and it should not audit just viewing the show
    // page for a thing.
    [AttributeUsage(AttributeTargets.Method)]
    public class AuditImageAttribute : ActionFilterAttribute { }
}
