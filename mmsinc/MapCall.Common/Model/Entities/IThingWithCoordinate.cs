using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// Represents an entity that has <see cref="MapCall.Common.Model.Entities.Coordinate"/> data.
    /// </summary>
    public interface IThingWithCoordinate : IEntity
    {
        #region Properties

        Coordinate Coordinate { get; set; }

        /// <summary>
        /// Gets the icon that should be displayed. Should normally return 
        /// Coordinate.Icon but can be overridden by implementor. 
        /// An icon must not be returned if Coordinate is null.
        /// </summary>
        MapIcon Icon { get; }

        // TODO: This will probably need some way of attaching extra data.
        
        #endregion
    }
}
