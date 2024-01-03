using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.DictionaryExtensions;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Base attribute for adding arbitrary html attributes to a model's editor.
    /// </summary>
    public abstract class HtmlAttribute : Attribute, ICustomModelMetadataAttribute
    {
        #region Consts

        public const string HTML_ATTRIBUTES_KEY = "HtmlAttribute's html attributes";

        #endregion

        #region Private methods

        /// <summary>
        /// Add any custom html attributes that should be rendered by a template.
        /// </summary>
        protected abstract void AddAttributes(ModelMetadata modelMetadata, IDictionary<string, object> attrDict);

        #endregion

        #region Public Methods

        public void Process(ModelMetadata modelMetadata)
        {
            var attrs = new Dictionary<string, object>();
            AddAttributes(modelMetadata, attrs);
            // No need to do anything if AddAttributes added nothing.
            if (!attrs.Any())
            {
                return;
            }

            // We merge all the values in to a new dictionary so we don't
            // mess with dictionary instances that might be being held 
            // on to elsewhere.
            var merged = new Dictionary<string, object>();
            var existing = GetHtmlAttributesFromMetadata(modelMetadata);

            if (existing != null)
            {
                merged.MergeIn(existing);
            }

            merged.MergeIn(attrs);

            modelMetadata.AdditionalValues[HTML_ATTRIBUTES_KEY] = merged;
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Returns the current collection of html attributes that have been combined from all HtmlAttribute
        /// derived instances on a model. Returns null if there isn't anything.
        /// </summary>
        public static IDictionary<string, object> GetHtmlAttributesFromMetadata(ModelMetadata modelMetadata)
        {
            if (modelMetadata.AdditionalValues.ContainsKey(HTML_ATTRIBUTES_KEY))
            {
                return (IDictionary<string, object>)modelMetadata.AdditionalValues[HTML_ATTRIBUTES_KEY];
            }

            return null;
        }

        #endregion
    }
}
