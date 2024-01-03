using System.Web.Mvc;

namespace MMSINC.Metadata
{
    public interface ICustomModelMetadataAttribute
    {
        #region Methods

        /// <summary>
        /// Method for processing custom attribute data.
        /// </summary>
        void Process(ModelMetadata modelMetaData);

        #endregion
    }
}
