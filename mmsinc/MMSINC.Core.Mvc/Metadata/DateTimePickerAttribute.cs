using System;
using System.Web.Mvc;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Put this on a model property if its editor also requires a time picker.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateTimePickerAttribute : Attribute, ICustomModelMetadataAttribute
    {
        private const string ADDITIONAL_VALUES_KEY = "DateTimePickerAttribute";

        public void Process(ModelMetadata modelMetaData)
        {
            modelMetaData.AdditionalValues.Add(ADDITIONAL_VALUES_KEY, this);
            modelMetaData.TemplateHint = "DateTime";
        }

        public static DateTimePickerAttribute GetFromModelMetadata(ModelMetadata metadata)
        {
            object result;
            if (metadata.AdditionalValues.TryGetValue(ADDITIONAL_VALUES_KEY, out result))
            {
                return (DateTimePickerAttribute)result;
            }

            return null;
        }
    }
}
