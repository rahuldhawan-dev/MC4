using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MMSINC.Metadata;

namespace MapCall.Common.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CoordinateAttribute : Attribute, ICustomModelMetadataAttribute
    {
        #region Consts

        public const string METADATA_KEY = "CoordinateAttribute",
                            DROPDOWN_TEMPLATE_HINT = "Coordinate";

        #endregion

        #region Properties

        /// <summary>
        /// The javascript method called to obtain an address value that
        /// the coordinate picker can use by default. Use this if you need
        /// to do some craziness with multiple fields to get an address.
        /// </summary>
        public string AddressCallback { get; set; }

        /// <summary>
        /// Gets/sets the field used to pre-populate the coordinate picker's address with.
        /// Use this if the address can be retrieved from a single field. 
        /// </summary>
        public string AddressField { get; set; }

        public IconSets IconSet { get; set; }

        #endregion

        #region Constructor

        public CoordinateAttribute()
        {
            IconSet = IconSets.All;
        }

        #endregion

        #region Public Methods

        public void Process(ModelMetadata modelMetaData)
        {
            modelMetaData.AdditionalValues.Add(METADATA_KEY, this);
            modelMetaData.TemplateHint = DROPDOWN_TEMPLATE_HINT;
        }

        /// <summary>
        /// Returns, if one exists, the CoordinateAttribute for some metadata. Returns null otherwise.
        /// </summary>
        /// <param name="modelMetadata"></param>
        /// <returns></returns>
        public static CoordinateAttribute TryGetAttributeFromModelMetadata(ModelMetadata modelMetadata)
        {
            object attr;
            modelMetadata.AdditionalValues.TryGetValue(METADATA_KEY, out attr);
            return (CoordinateAttribute)attr;
        }

        #endregion
    }
}
