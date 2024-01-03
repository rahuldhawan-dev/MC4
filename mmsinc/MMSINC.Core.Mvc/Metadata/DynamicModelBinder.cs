using System.Web.Mvc;
using MMSINC.Validation;

namespace MMSINC.Metadata
{
    /// <summary>
    /// ModelBinder that has added support for models with IDynamicModel.
    /// </summary>
    public class DynamicModelBinder : DefaultModelBinder
    {
        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // This is the chain of events as DefaultModelBinder is implemented:
            // 1. BindModel is called, which sets all the property values on the model. 
            //    This includes calling out for another binder to create any complex
            //    child objects(ie binding to properties that are more than just value types or strings)
            // 2. At this point, we have a model with its values set, and any complex child objects
            //    have been bound *and fully validated*
            // 3. After that, the base OnModelUpdated method is called. This is the method that actually
            //    calls out to the validator providers to get the required validators. This is the one
            //    spot where we can hook in and set any values needed on the model before the DynamicModelValidator
            //    does its thing.
            // 4. Keep in mind that by "fully validated" I mean that the child objects have already gone through
            //    this entire cycle. 

            //Debug.Print("OnModelUpdated");

            var dynamoModel = (IDynamicModel)bindingContext.Model;
            dynamoModel.OnPreValidating();

            base.OnModelUpdated(controllerContext, bindingContext);
        }
    }
}
