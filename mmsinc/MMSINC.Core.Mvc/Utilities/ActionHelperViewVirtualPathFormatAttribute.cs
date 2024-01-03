using System;
using MMSINC.ClassExtensions.StringExtensions;

namespace MMSINC.Utilities
{
    /// <summary>
    /// When placed on a controller class, ActionHelper will use this path to 
    /// automatically generate view names that go to a specific directory rather
    /// than whatever the current request dictates. You'll probably never need this.
    /// Except in MapCallMVC. Because I made it for that. For one thing. -Ross
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ActionHelperViewVirtualPathFormatAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Gets/sets the string format for the virtual path to a directory
        /// that includes a view. ie: "~/Views/SomeDir/{0}.cshtml"
        /// </summary>
        public string VirtualPathFormat { get; set; }

        #endregion

        #region Constructor

        public ActionHelperViewVirtualPathFormatAttribute(string virtualPathFormat)
        {
            virtualPathFormat.ThrowIfNullOrWhiteSpace("virtualPathFormat");
            VirtualPathFormat = virtualPathFormat;
        }

        #endregion
    }
}
