using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MMSINC.Metadata
{
    // This garbage class is needed because the querystring provider doesn't know how to handle
    // querystring keys with multiple comma separated values. It also doesn't know how to handle 
    // querystrings with multiples of the same key because it automatically creates a comma separated
    // string instead.
    public class StringListModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // The default model binder/value provider system doesn't know exactly how to handle binding
            // when there are multiple values for a key. By default, it only works if the querystring, 
            // post data, or whatever has duplicate keys for each value. It does not work at all when
            // that data has a single key with comma separated values.
            //
            // Consider these two cases:
            // Case 1: qs=A&qs=B,
            // Case 2: qs=A,B
            //
            // For both cases, the AttemptedValue will always come back as "A,B"
            // But for Case 1, the RawValue will be string[] { "A", "B" }
            // while for Case 2, the RawValue will be string[] { "A,B" }
            //
            // There's not a definitively clean way to allow for both scenarios. This will break
            // if any of the individual string values include a comma that isn't for value separation.
            // We can deal with this if it actually becomes an issue at some point.

            var providerValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            // Dunno why this would be null, but it's what the internal
            // implementations of IModelBinder do so.
            if (providerValue == null || providerValue.AttemptedValue == string.Empty
            ) // RawValue will be an string array with a single empty string in this case, too
            {
                return null;
            }

            // We need to manually set the provider value in ModelState cause we're a binder
            // and that's part of our job. But we don't need to set it if the providerValue
            // is null.
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, providerValue);
            return GetValue(providerValue.AttemptedValue, bindingContext);
        }

        private object GetValue(string attemptedValue, ModelBindingContext bindingContext)
        {
            return attemptedValue.Split(',').ToList();
        }
    }
}
