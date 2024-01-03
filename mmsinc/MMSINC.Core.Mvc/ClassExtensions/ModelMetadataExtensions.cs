using System;
using System.Linq;
using System.Web.Mvc;

namespace MMSINC.ClassExtensions
{
    public static class ModelMetadataExtensions
    {
        /// <summary>
        /// Returns the correct ModelMetadata from a string expression, unlike the static ModelMetadata.FromStringExpression
        /// method that doesn't do anything correctly when the model is null. Returns null if metadata can not be found.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static ModelMetadata TryFromStringExpression(this ModelMetadata parent, string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                // The rest of MVC uses an empty string to mean "give me the ModelMetadata of the current model"
                return parent;
            }

            if (expression.Contains("["))
            {
                throw new InvalidOperationException("Expressions with index access aren't supported. " + expression);
            }

            if (!expression.Contains('.'))
            {
                return parent.Properties.SingleOrDefault(x => x.PropertyName == expression);
            }

            var expSplit = expression.Split(new[] {'.'}, 2);
            var newParent = parent.TryFromStringExpression(expSplit[0]);
            if (newParent == null)
            {
                return null;
            }

            return newParent.TryFromStringExpression(expSplit[1]);
        }
    }
}
