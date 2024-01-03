using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MMSINC.Metadata
{
    // TODO: Is this class still needed? MVC5 fixed the container not being available.

    /// <summary>
    /// ModelMetadataProvider that returns LinkedModelMetadata instances.
    /// </summary>
    public class LinkedModelMetadataProvider : AssociatedMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType,
            Func<object> modelAccessor, Type modelType, string propertyName)
        {
            // Making an accessor for the container to match the modelAccessor pattern.
            Func<object> containerAccessor = null;

            // modelAccessor can be null in a lot of instances, so we need to check for that.
            if (modelAccessor != null)
            {
                containerAccessor = () => {
                    // This is incredibly hacky, and it breaks if you need to go more than one level deep.
                    // ie: If you have go EditorFor(x => x.SomeProp.SomeProperty), the Target ends up being
                    //     whatever x is and not whatever SomeProp is, even though containerType in the
                    //     constructor here is whatever SomeProp's type is.
                    var containerInfo = modelAccessor.Target.GetType().GetField("container");

                    // In some situations, we'll get a modelAccessor with a container that isn't
                    // registered with the LinkedModelMetadataProvider, but has child properties
                    // that are. In this case, we won't get the expected container, and we need
                    // to ignore that for the time being. So return null in this instance.
                    //
                    // Also, see notes in LinkedModelMetadataProviderTest.TestContainerModelThrowsExceptionIfNotNullAndTypeIsNotContainerType
                    // if this starts returning null when you don't expect it to. -Ross 1/5/2018
                    if (containerInfo == null)
                    {
                        return null;
                    }
                    else
                    {
                        return containerInfo.GetValue(modelAccessor.Target);
                    }
                };
            }

            // For some reason I have yet to find, containerType will be null sometimes, but
            // we have a container. So we can just use the container's type.
            if (containerType == null && containerAccessor != null)
            {
                var container = containerAccessor();
                containerType = (container != null ? container.GetType() : null);
            }

            // NOTE: This does NOT have the same functionality as DataAnnotationsMetadataProvider. That
            //       does some extra business in its CreateMetadata call. If something weird's happening,
            //       take a look in there to see if there's something we should be setting here.
            return new LinkedModelMetadata(this, containerType, modelAccessor, modelType, propertyName,
                containerAccessor);
        }
    }
}
