using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace MMSINC.Metadata
{
    public class Int32ArrayModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var providerValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            // Dunno why this would be null, but it's what the internal
            // implementations of IModelBinder do so.
            if (providerValue == null || providerValue.AttemptedValue == string.Empty)
            {
                return null;
            }

            // We need to manually set the provider value in ModelState cause we're a binder
            // and that's part of our job. But we don't need to set it if the providerValue
            // is null.
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, providerValue);

            // If there's an int property that is secured, the secure form value provider
            // returns an int array as the raw value. We can return that as-is.
            if (providerValue.RawValue is int[])
            {
                return providerValue.RawValue;
            }

            // It's probably worth pointing out that the RawValue can also be a string array
            // which we could use to parse instead of splitting the string on a comma.
            return GetValue(providerValue.AttemptedValue, bindingContext);
        }

        private object GetValue(string attemptedValue, ModelBindingContext bindingContext)
        {
            var ret = new List<int>();

            foreach (var value in attemptedValue.Split(",".ToCharArray()))
            {
                int converted;

                if (!Int32.TryParse(value, out converted))
                {
                    // TODO: ModelBinder is supposed to set a ModelState error when it can't bind something. -Ross 2/7/2014
                    // TODO: And when we do that, we should then return null immediately instead of returning the the array. -Ross 1/3/2020
                    throw new ArgumentException(String.Format("Value '{0}' is not a valid Int32.", value),
                        bindingContext.ModelName);
                }

                ret.Add(converted);
            }

            return ret.ToArray();
        }
    }
}
