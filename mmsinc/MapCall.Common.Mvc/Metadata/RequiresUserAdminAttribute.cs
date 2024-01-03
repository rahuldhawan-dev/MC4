using System;
using System.Web.Mvc;

namespace MapCall.Common.Metadata
{
    /// <summary>
    /// Attribute used in conjunction with RequiresUserAdminAuthorizer. This attribute should be used
    /// on controller actions that require that the current user's IsUserAdmin property to be true if
    /// they aren't already a site admin.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequiresUserAdminAttribute : FilterAttribute { }
}
