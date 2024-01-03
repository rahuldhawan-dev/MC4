using System;

namespace MMSINC.Authentication
{
    /// <summary>
    /// Place this attribute on a controller or action to indicate that it requires
    /// site admin access.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class RequiresAdminAttribute : Attribute { }
}
