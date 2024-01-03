using System.Web.Mvc;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Model binds a string and also trims the value of any leading/trailing whitespace.
    /// </summary>
    public class StringModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // TODO: Maybe at some point we'll need a DoNotTrimAttribute to put on a property so we can skip trimming.
            var bound = base.BindModel(controllerContext, bindingContext);
            if (bound != null)
            {
                return ((string)bound).Trim();
            }

            return null;
        }
    }
}
