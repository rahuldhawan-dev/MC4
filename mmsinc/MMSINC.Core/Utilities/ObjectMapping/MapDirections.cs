// ReSharper disable CheckNamespace

using System;

namespace MMSINC.Utilities.ObjectMapping
    // ReSharper restore CheckNamespace
{
    // TODO: Maybe add an Auto value as the default. That would say, "Hey, 
    //       mapper, figure out if this can be mapped or not and don't throw about it"

    /// <summary>
    /// Enum that describes the direction an ObjectMapper should map values.
    /// </summary>
    public enum MapDirections
    {
        /// <summary>
        /// Automapping will set values on both the primary and secondary 
        /// </summary>
        BothWays = 0, // Default value

        /// <summary>
        /// Automapping will attempt to copy values from the entity to the model.
        /// </summary>
        ToPrimary,

        /// <summary>
        /// Synoymous with ToPrimary value.
        /// </summary>
        ToViewModel = ToPrimary,

        /// <summary>
        /// Automapping will attempt to copy values from the model to the entity.
        /// </summary>
        ToSecondary,

        /// <summary>
        /// Synonymous with ToSecondary value.
        /// </summary>
        ToEntity = ToSecondary,

        /// <summary>
        /// Automapping will not occur. Values will have to be manually copied.
        /// </summary>
        None
    }
}
