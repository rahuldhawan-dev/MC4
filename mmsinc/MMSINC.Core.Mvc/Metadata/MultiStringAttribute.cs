using System;
using System.Web.Mvc;

namespace MMSINC.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MultiStringAttribute : Attribute, ICustomModelMetadataAttribute
    {
        public void Process(ModelMetadata modelMetaData)
        {
            modelMetaData.TemplateHint = "MultiString";
        }
    }
}
