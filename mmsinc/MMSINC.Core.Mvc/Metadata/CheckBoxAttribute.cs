using System;
using System.Web.Mvc;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Put this on a nullable boolean property if it should be a checkbox rather
    /// than a dropdown. This allows the view model to keep the nullable bool(and
    /// the required validator if it has one) without the annoyance of a dropdown.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckBoxAttribute : Attribute, ICustomModelMetadataAttribute
    {
        private const string ADDITIONAL_VALUES_KEY = "CheckBoxAttribute";

        public void Process(ModelMetadata modelMetaData)
        {
            modelMetaData.AdditionalValues.Add(ADDITIONAL_VALUES_KEY, this);
        }

        public static CheckBoxAttribute GetFromModelMetadata(ModelMetadata metadata)
        {
            object result;
            if (metadata.AdditionalValues.TryGetValue(ADDITIONAL_VALUES_KEY, out result))
            {
                return (CheckBoxAttribute)result;
            }

            return null;
        }
    }
}
