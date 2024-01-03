using System;
using System.Web.Mvc;

namespace MMSINC.Metadata
{
    /// <summary>
    /// ModelMetadata class that includes a model accessor to the container object when possible.
    /// </summary>
    public class LinkedModelMetadata : ModelMetadata
    {
        #region Fields

        private readonly Func<object> _containerModelAccessor;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parent container of the Model for this metadata instance.
        /// </summary>
        public object ContainerModel
        {
            get
            {
                // Don't cache this result without researching if it's ok to 
                // do that. And I don't just mean writing tests, actual researching.
                // There's plenty of areas where ModelMetadata's used where we don't
                // know what effect it could have.
                return GetContainerModel();
            }
        }

        #endregion

        #region Constructor

        public LinkedModelMetadata(ModelMetadataProvider provider, Type containerType, Func<object> modelAccessor,
            Type modelType, string propertyName, Func<object> containerAccessor)
            : base(provider, containerType, modelAccessor, modelType, propertyName)
        {
            _containerModelAccessor = containerAccessor;
        }

        #endregion

        #region Private methods

        private object GetContainerModel()
        {
            // This field is allowed to be null, so return null if it is.
            if (_containerModelAccessor == null)
            {
                return null;
            }

            // See the note left inside the accessor about why it might return
            // null from time to time. This is entirely acceptable.
            var model = _containerModelAccessor();
            if (model == null)
            {
                return null;
            }

            if (model.GetType() != ContainerType)
            {
                throw new InvalidCastException(string.Format(
                    "Unable to get ContainerModel of type '{0}'. Actual type is '{1}'", ContainerType.FullName,
                    model.GetType().FullName));
            }

            return model;
        }

        #endregion
    }
}
