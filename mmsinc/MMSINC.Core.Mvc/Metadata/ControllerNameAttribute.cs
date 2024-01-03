using System;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Base attribute used on a controller class to get its dynamic controller name.
    /// This is used by RouteContext/CrumbBuilder/Stuff. This pretty much exists
    /// so generic controllers can have a way of giving out unique names.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ControllerNameAttribute : Attribute
    {
        public virtual string GetControllerName(Type controllerType)
        {
            var typeName = controllerType.Name;
            if (typeName.EndsWith("Controller", StringComparison.InvariantCultureIgnoreCase))
            {
                typeName = typeName.Substring(0, typeName.Length - "Controller".Length);
            }

            return typeName;
        }
    }
}
