using System;
using System.Web.Mvc;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Base attribute used to determine formatting for a value when it's displayed in MVC.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class ModelFormatterAttribute : Attribute, ICustomModelMetadataAttribute
    {
        #region Fields

        // This can be shared by all inheriting classes because only one
        // instance is allowed on a property.
        internal const string METADATA_KEY = "FormatAttribute";

        #endregion

        #region Constructor

        #endregion

        public void Process(ModelMetadata modelMetaData)
        {
            modelMetaData.AdditionalValues.Add(METADATA_KEY, this);
        }

        /// <summary>
        /// Method called when a value needs to be modified for display purposes.
        /// </summary>
        public abstract string FormatValue(object value);

        /// <summary>
        /// Returns the ModelFormatterAttribute for a given ModelMetadata instance. 
        /// This does NOT return a default registered formatter registered with ModelFormatterProviders.
        /// You should really be using ModelFormatterProviders.Current.TryGetModelFormatter().
        /// </summary>
        /// <param name="modelMetadata"></param>
        /// <returns></returns>
        public static ModelFormatterAttribute TryGetAttributeFromModelMetadata(ModelMetadata modelMetadata)
        {
            object attr;
            modelMetadata.AdditionalValues.TryGetValue(METADATA_KEY, out attr);
            return (ModelFormatterAttribute)attr;
        }
    }
}
