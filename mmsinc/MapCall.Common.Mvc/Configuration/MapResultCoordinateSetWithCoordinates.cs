using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.Common.Configuration
{
    public class MapResultCoordinateSetWithCoordinates : MapResultCoordinateSet
    {
        public MapResultCoordinateSetWithCoordinates(IIconSetRepository iconSetRepository,
            IEnumerable<IThingWithCoordinate> coords) : base(iconSetRepository)
        {
            coords.ThrowIfNull("coords");
            Coordinates = coords;
        }
    }
}
