using System;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// Represents an entity that has <see cref="MapCall.Common.Model.Entities.Coordinate"/> data just like
    /// <see cref="IThingWithCoordinate"/>, only the data has been loaded as flat properties on that entity
    /// from which a representation of the <see cref="MapCall.Common.Model.Entities.Coordinate"/> and its
    /// children can be constructed.  This is useful for generating efficient map queries in repositories
    /// where it would be difficult to construct models which aren't flat (see implementors of this
    /// interface for examples).
    /// </summary>
    public interface IThingWithFlattenedCoordinate : IThingWithCoordinate
    {
        decimal Latitude { get; }
        decimal Longitude { get; }
        int? MapIconId { get; }
        string MapIconFileName { get; }
        int? MapIconHeight { get; }
        int? MapIconWidth { get; }
        int? MapIconOffsetId { get; }
        string MapIconOffsetDescription { get; }
    }

    public static class ThingWithFlattenedCoordinateExtensions
    {
        public static Coordinate ToCoordinate(this IThingWithFlattenedCoordinate that)
        {
            return new Coordinate {
                Latitude = that.Latitude,
                Longitude = that.Longitude,
                Icon = !that.MapIconId.HasValue
                    ? null
                    : new MapIcon {
                        Id = that.MapIconId.Value,
                        FileName = that.MapIconFileName,
                        Height = that.MapIconHeight ?? default,
                        Width = that.MapIconWidth ?? default,
                        Offset = !that.MapIconOffsetId.HasValue
                            ? null
                            : new MapIconOffset {
                                Description = that.MapIconOffsetDescription
                            }
                    }
            };
        }
    }

    /// <inheritdoc />
    /// <remarks>
    /// This implementation is provided for convenience as well as a reference for classes which might not
    /// be able to inherit from it directly.
    /// </remarks>
    public abstract class ThingWithFlattenedCoordinateBase : IThingWithFlattenedCoordinate
    {
        #region Private Members

        private Coordinate _coordinate;

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual decimal Latitude { get; set; }
        public virtual decimal Longitude { get; set; }

        public virtual int? MapIconId { get; set; }
        public virtual string MapIconFileName { get; set; }
        public virtual int? MapIconHeight { get; set; }
        public virtual int? MapIconWidth { get; set; }

        public virtual int? MapIconOffsetId { get; set; }
        public virtual string MapIconOffsetDescription { get; set; }

        public virtual Coordinate Coordinate
        {
            get => _coordinate ?? (_coordinate = this.ToCoordinate());
            set => throw new InvalidOperationException();
        }

        public virtual MapIcon Icon => Coordinate.Icon;

        #endregion        
    }
}
