using System;
using System.Linq;
using MMSINC.Metadata;

namespace MapCall.Common.Metadata
{
    /// <summary>
    /// If you have a generic controller class whose type parameter should be the name
    /// of the controller, use this attribute. This is for EntityLookupControllerBase
    /// primarily. 
    /// </summary>
    public class GenericControllerNameAttribute : ControllerNameAttribute
    {
        public override string GetControllerName(Type controllerType)
        {
            return controllerType.GetGenericArguments().Last().Name;
        }
    }
}
