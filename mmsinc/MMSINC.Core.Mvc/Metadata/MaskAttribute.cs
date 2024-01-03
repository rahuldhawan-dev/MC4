using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Attribute for use with the jquery unobtrusive masked input library.
    /// http://digitalbush.com/projects/masked-input-plugin/
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaskAttribute : HtmlAttribute
    {
        #region Consts

        public const string DATA_MASK_ATTRIBUTE_KEY = "data-mask";

        #endregion

        #region Properties

        public string Mask { get; set; }

        #endregion

        #region Constructor

        public MaskAttribute(string mask)
        {
            Mask = mask;
        }

        #endregion

        #region Protected Methods

        protected override void AddAttributes(ModelMetadata modelMetaData, IDictionary<string, object> attrDict)
        {
            if (!string.IsNullOrWhiteSpace(Mask))
            {
                attrDict.Add(DATA_MASK_ATTRIBUTE_KEY, Mask);
            }
        }

        #endregion
    }
}
