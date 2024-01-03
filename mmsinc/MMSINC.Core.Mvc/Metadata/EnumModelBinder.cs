using System;
using System.Web.Mvc;

namespace MMSINC.Metadata
{
    /// <summary>
    /// This is a custom model binder that makes JSON model binding properly convert ints to the enum type.
    /// By default, it only works if you send the enum name and not the value itself, which is kind of dumb.
    /// </summary>
    /// <remarks>
    /// 
    /// Everything I know about IModelBinder I learned from here:
    /// http://odetocode.com/blogs/scott/archive/2009/04/27/6-tips-for-asp-net-mvc-model-binding.aspx
    /// 
    /// NOTE: This isn't tested for use with FlagsAttribute.
    /// 
    /// </remarks>
    public class EnumModelBinder : IModelBinder
    {
        #region Consts

        internal const string ERROR_FORMAT = "{0} does not have a value that matches '{1}'";

        #endregion

        #region Fields

        private readonly Type _type;

        #endregion

        #region Constructors

        public EnumModelBinder(Type type)
        {
            if (!type.IsEnum)
            {
                throw new InvalidOperationException("EnumModelBinder<T> requires that T be an enum type.");
            }

            _type = type;
        }

        #endregion

        #region Private methods

        private object GetValue(string attemptedValue, ModelBindingContext bindingContext)
        {
            if (string.IsNullOrWhiteSpace(attemptedValue))
            {
                return null;
            }

            Exception possibleException = null;
            try
            {
                var parsed = Enum.Parse(_type, attemptedValue, true);
                if (Enum.IsDefined(_type, parsed))
                {
                    return parsed;
                }
            }
            catch (Exception ex)
            {
                possibleException = ex;
            }

            SetInvalidValueException(attemptedValue, possibleException, bindingContext);
            return null;
        }

        internal void SetInvalidValueException(object val, Exception inner, ModelBindingContext bindingContext)
        {
            var ex = new Exception(string.Format(ERROR_FORMAT, _type.FullName, val), inner);
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex);
        }

        #endregion

        #region Public methods

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
            var providerValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            // Dunno why this would be null, but it's what the internal
            // implementations of IModelBinder do so.
            if (providerValue == null)
            {
                return null;
            }

            // We need to manually set the provider value in ModelState cause we're a binder
            // and that's part of our job. But we don't need to set it if the providerValue
            // is null.
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, providerValue);
            return GetValue(providerValue.AttemptedValue, bindingContext);
        }

        #endregion
    }
}
