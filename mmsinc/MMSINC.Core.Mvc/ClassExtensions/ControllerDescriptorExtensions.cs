using System;
using System.Linq;
using System.Web.Mvc;

namespace MMSINC.ClassExtensions
{
    public static class ControllerDescriptorExtensions
    {
        /// <summary>
        /// Returns a ReflectedActionDescriptor for an action if it exists on the controller. Returns null otherwise.
        /// actionName is case-insensitive.
        /// </summary>
        /// <param name="controllerDescriptor"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static ReflectedActionDescriptor FindReflectedActionDescriptor(
            this ControllerDescriptor controllerDescriptor, string actionName)
        {
            return controllerDescriptor.GetCanonicalActions()
                                       .OfType<ReflectedActionDescriptor>()
                                       .SingleOrDefault(x =>
                                            x.ActionName.Equals(actionName,
                                                StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
